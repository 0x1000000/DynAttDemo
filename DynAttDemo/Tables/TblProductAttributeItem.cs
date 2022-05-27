using SqExpress;
using SqExpress.Syntax.Type;

namespace DynAttDemo.Tables
{
    public class TblProductAttributeItem : TableBase
    {
        public TblProductAttributeItem(): this(alias: SqExpress.Alias.Auto)
        {
        }

        public TblProductAttributeItem(Alias alias): base(schema: "dbo", name: "ProductAttributeItem", alias: alias)
        {
            this.ProductId = this.CreateInt32Column("ProductId", ColumnMeta.PrimaryKey().ForeignKey<TblProduct>(t => t.ProductId));
            this.AttributeId = this.CreateInt32Column("AttributeId", ColumnMeta.PrimaryKey().ForeignKey<TblAttribute>(t => t.AttributeId));
            this.AttributeItemId = this.CreateInt32Column("AttributeItemId", ColumnMeta.PrimaryKey().ForeignKey<TblAttributeItem>(t => t.AttributeItemId));
        }

        [SqModel("ProductAttributeItemData")]
        public Int32TableColumn ProductId { get; }

        [SqModel("ProductAttributeItemData")]
        public Int32TableColumn AttributeId { get; }

        [SqModel("ProductAttributeItemData")]
        public Int32TableColumn AttributeItemId { get; }
    }
}