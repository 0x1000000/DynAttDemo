using System.Data.SqlClient;
using DynAttDemo.Tables;
using DynAttDemo.Models;
using DynAttDemo;
using SqExpress;
using SqExpress.DataAccess;
using SqExpress.SqlExport;
using SqExpress.Syntax.Boolean;
using SqExpress.Syntax.Names;
using SqExpress.Syntax.Select;
using SqExpress.Syntax.Boolean.Predicate;

// Specify MS SQL server address and database name.
// If the specified database does not exist if will be created and then destroyed.
using var dbData = DatatbaseManager.CreateDatabaseFile("(local)", "DynAttDemo");
if(dbData.ErrorText != null)
{
    Console.WriteLine(dbData.ErrorText);
    return;
}

using var database = new SqDatabase<SqlConnection>(
    connection: new SqlConnection(dbData.ConnectionString),
    commandFactory: (conn, sql) => new SqlCommand(cmdText: sql, connection: conn),
    sqlExporter: TSqlExporter.Default,
    disposeConnection: true);

await CreateData(database);

ExprBoolean filter = BuildFilter();

Console.WriteLine("Filter as SQL:");
Console.WriteLine(TSqlExporter.Default.ToSql(filter));
Console.WriteLine();

List<int> filterAttributes = FindAllFilterAttributes(filter);

Console.WriteLine("Unique attributes in the filter:");
foreach (var filterAttribute in filterAttributes)
{
    Console.WriteLine(filterAttribute);
}
Console.WriteLine();

Dictionary<int, AttributeType> attTypesDict = await LoadAttributeTypes(database, filterAttributes);

var tblProduct = AllTables.GetProduct();

var subQuery = PrepareSubQuery(tblProduct, attTypesDict);

var query = SqQueryBuilder
    .Select(tblProduct.ProductName)
    .From(tblProduct)
    .InnerJoin(
        subQuery, 
        on: tblProduct.ProductId == tblProduct.ProductId.WithSource(subQuery.Alias))
    .Where(filter)
    .Done();

Console.WriteLine("Products that match the filter:");
await query.Query(database, r => Console.WriteLine("-" + tblProduct.ProductName.Read(r)));
Console.WriteLine();

//Filter by "Set" attribute

var cProtocols = SqQueryBuilder.Column("3");

filter = cProtocols.In(3, 5);

Console.WriteLine("Filter by \"Set\" attribute:");
Console.WriteLine(TSqlExporter.Default.ToSql(filter));
Console.WriteLine();

filter = ModifyFilterForSetAttributes(tblProduct, filter);

Console.WriteLine("Filter by \"Set\" attribute after modification:");
Console.WriteLine(TSqlExporter.Default.ToSql(filter));
Console.WriteLine();

Console.WriteLine("Products that match the filter:");
await SqQueryBuilder
    .Select(tblProduct.ProductName)
    .From(tblProduct)
    .Where(filter)
    .Query(database, r => Console.WriteLine("-" + tblProduct.ProductName.Read(r)));
Console.WriteLine();

Console.WriteLine("Finish");

static ExprBoolean BuildFilter()
{
    var vendor = SqQueryBuilder.Column("1");
    var internalMemory = SqQueryBuilder.Column("2");

    ExprBoolean filter = vendor == 1;
    filter = filter & internalMemory >= 64 & internalMemory <= 256;
    filter |= vendor == 2;

    return filter;
}

static List<int> FindAllFilterAttributes(ExprBoolean filter)
{
    return filter
        .SyntaxTree()
        .DescendantsAndSelf()
        .OfType<ExprColumn>()
        .Select(c => int.Parse(c.ColumnName.Name))
        .Distinct()
        .ToList();
}

static Task<Dictionary<int, AttributeType>> LoadAttributeTypes(ISqDatabase database, IReadOnlyList<int> filterAttributes)
{
    var tblAttribute = AllTables.GetAttribute();

    return SqQueryBuilder
        .Select(tblAttribute.AttributeId, tblAttribute.AttributeType)
        .From(tblAttribute)
        .Where(tblAttribute.AttributeId.In(filterAttributes))
        .QueryDictionary(
            database,
            r => tblAttribute.AttributeId.Read(r),
            r => (AttributeType)tblAttribute.AttributeType.Read(r));
}

static ExprDerivedTableQuery PrepareSubQuery(TblProduct tblProduct, Dictionary<int, AttributeType> typesDict)
{
    var subQueryColumns = new List<IExprSelecting> { tblProduct.ProductId };

    var subQuerySelect = SqQueryBuilder
        .Select(subQueryColumns)
        .From(tblProduct);

    foreach (var filterAttributeId in typesDict.Keys)
    {
        var tblProductAttribute = AllTables.GetProductAttribute();

        subQuerySelect = subQuerySelect.LeftJoin(
            tblProductAttribute,
            on: tblProductAttribute.ProductId == tblProduct.ProductId
                & tblProductAttribute.AttributeId == filterAttributeId);

        switch (typesDict[filterAttributeId])
        {
            case AttributeType.Integer:
                subQueryColumns.Add(tblProductAttribute.ValueInt.As(filterAttributeId.ToString()));
                break;
            case AttributeType.Set:
                subQueryColumns.Add(tblProductAttribute.ValueItem.As(filterAttributeId.ToString()));
                break;
            case AttributeType.Date:
                subQueryColumns.Add(tblProductAttribute.ValueDate.As(filterAttributeId.ToString()));
                break;
        }
    }

    return subQuerySelect.As(SqQueryBuilder.TableAlias("ATTRIBUTES"));
}

static ExprBoolean ModifyFilterForSetAttributes(TblProduct tblProduct, ExprBoolean filter)
{
    return (ExprBoolean)filter.SyntaxTree().Modify<ExprInValues>(exprInValues =>
    {
        var column = (ExprColumn)exprInValues.TestExpression;
        var columId = int.Parse(column.ColumnName.Name);

        var t = AllTables.GetProductAttributeItem();
        return SqQueryBuilder.Exists(
            SqQueryBuilder
                .SelectOne()
                .From(t)
                .Where(
                    t.AttributeId == columId
                    & t.ProductId == tblProduct.ProductId
                    & t.AttributeItemId.In(exprInValues.Items)));
    })!;
}

static async Task CreateData(ISqDatabase database)
{
    var tables = AllTables.BuildAllTableList();

    foreach (var table in tables.Reverse())
    {
        await database.Statement(table.Script.DropIfExist());
    }

    foreach (var table in tables)
    {
        await database.Statement(table.Script.Create());
    }

    var products = new[] { new ProductData(1, "iPhone 11 128"), new ProductData(2, "Galaxy Note 20 Ultra") };
    await SqQueryBuilder.InsertDataInto(AllTables.GetProduct(), products).MapData(ProductData.GetMapping).Exec(database);

    var attributes = new[] {
        new AttributeData(1, "Vendor", AttributeType.Set),
        new AttributeData(2, "Internal Memory (Gb)", AttributeType.Integer),
        new AttributeData(3, "Cellular Protocols", AttributeType.MultiSet),
        new AttributeData(4, "Release Date", AttributeType.Date),
    };
    await SqQueryBuilder
        .InsertDataInto(AllTables.GetAttribute(), attributes)
        .MapData(AttributeData.GetMapping)
        .Exec(database);

    var attributeItems = new[] {
        new AttributeItemData(1, 1, "Apple"),
        new AttributeItemData(2, 1, "Samsung"),
        new AttributeItemData(3, 3, "LTE"),
        new AttributeItemData(4, 3, "4G"),
        new AttributeItemData(5, 3, "5G"),
    };
    await SqQueryBuilder
        .InsertDataInto(AllTables.GetAttributeItem(), attributeItems)
        .MapData(AttributeItemData.GetMapping)
        .Exec(database);

    var productAttributes = new[] {
        new ProductAttributeData(1, 1, 1, null, null),
        new ProductAttributeData(1, 2, null, 128, null),
        new ProductAttributeData(1, 4, null, null, new DateTime(2019, 9, 20)),
        new ProductAttributeData(2, 1, 2, null, null),
        new ProductAttributeData(2, 2, null, 512, null),
        new ProductAttributeData(2, 4, null, null, new DateTime(2022, 5, 29)),
    };
    await SqQueryBuilder
        .InsertDataInto(AllTables.GetProductAttribute(), productAttributes)
        .MapData(ProductAttributeData.GetMapping)
        .Exec(database);

    var productAttributeItems = new[] {
        new ProductAttributeItemData(1, 3, 3),
        new ProductAttributeItemData(1, 3, 4),
        new ProductAttributeItemData(1, 3, 5),
        new ProductAttributeItemData(2, 3, 3),
        new ProductAttributeItemData(2, 3, 4)
    };
    await SqQueryBuilder
        .InsertDataInto(AllTables.GetProductAttributeItem(), productAttributeItems)
        .MapData(ProductAttributeItemData.GetMapping)
        .Exec(database);
}
