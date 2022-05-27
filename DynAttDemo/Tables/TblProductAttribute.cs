using SqExpress;
using SqExpress.Syntax.Type;

namespace DynAttDemo.Tables
{
    public class TblProductAttribute : TableBase
    {
        public TblProductAttribute(): this(alias: SqExpress.Alias.Auto)
        {
        }

        public TblProductAttribute(Alias alias): base(schema: "dbo", name: "ProductAttribute", alias: alias)
        {
            this.ProductId = this.CreateInt32Column("ProductId", ColumnMeta.PrimaryKey().ForeignKey<TblProduct>(t => t.ProductId));
            this.AttributeId = this.CreateInt32Column("AttributeId", ColumnMeta.PrimaryKey().ForeignKey<TblAttribute>(t => t.AttributeId));
            this.ValueItem = this.CreateNullableInt32Column("ValueItem", null);
            this.ValueInt = this.CreateNullableInt32Column("ValueInt", null);
            this.ValueDate = this.CreateNullableDateTimeColumn("ValueDate", true, null);
        }

        [SqModel("ProductAttributeData")]
        public Int32TableColumn ProductId { get; }

        [SqModel("ProductAttributeData")]
        public Int32TableColumn AttributeId { get; }

        [SqModel("ProductAttributeData")]
        public NullableInt32TableColumn ValueItem { get; }

        [SqModel("ProductAttributeData")]
        public NullableInt32TableColumn ValueInt { get; }

        [SqModel("ProductAttributeData")]
        public NullableDateTimeTableColumn ValueDate { get; }
    }
}