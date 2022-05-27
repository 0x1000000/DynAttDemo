using SqExpress;
using SqExpress.Syntax.Type;

namespace DynAttDemo.Tables
{
    public class TblAttributeItem : TableBase
    {
        public TblAttributeItem(): this(alias: SqExpress.Alias.Auto)
        {
        }

        public TblAttributeItem(Alias alias): base(schema: "dbo", name: "AttributeItem", alias: alias)
        {
            this.AttributeItemId = this.CreateInt32Column("AttributeItemId", ColumnMeta.PrimaryKey());
            this.AttributeId = this.CreateInt32Column("AttributeId", ColumnMeta.ForeignKey<TblAttribute>(t => t.AttributeId));
            this.AttributeItemName = this.CreateStringColumn(name: "AttributeItemName", size: 255, isUnicode: true, isText: false, columnMeta: null);
        }

        [SqModel("AttributeItemData", PropertyName = "Id")]
        public Int32TableColumn AttributeItemId { get; }

        [SqModel("AttributeItemData")]
        public Int32TableColumn AttributeId { get; }

        [SqModel("AttributeItemData", PropertyName = "Name")]
        public StringTableColumn AttributeItemName { get; }
    }
}