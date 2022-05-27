using SqExpress;
using SqExpress.Syntax.Type;

namespace DynAttDemo.Tables
{
    public class TblProduct : TableBase
    {
        public TblProduct(): this(alias: SqExpress.Alias.Auto)
        {
        }

        public TblProduct(Alias alias): base(schema: "dbo", name: "Product", alias: alias)
        {
            this.ProductId = this.CreateInt32Column("ProductId", ColumnMeta.PrimaryKey());
            this.ProductName = this.CreateStringColumn(name: "ProductName", size: 255, isUnicode: true, isText: false, columnMeta: null);
        }

        [SqModel("ProductData", PropertyName = "Id")]
        public Int32TableColumn ProductId { get; }

        [SqModel("ProductData", PropertyName = "Name")]
        public StringTableColumn ProductName { get; }
    }
}