using SqExpress;
using SqExpress.Syntax.Type;

namespace DynAttDemo.Tables
{
    public class TblAttribute : TableBase
    {
        public TblAttribute(): this(alias: SqExpress.Alias.Auto)
        {
        }

        public TblAttribute(Alias alias): base(schema: "dbo", name: "Attribute", alias: alias)
        {
            this.AttributeId = this.CreateInt32Column("AttributeId", ColumnMeta.PrimaryKey());
            this.AttributeName = this.CreateStringColumn(name: "AttributeName", size: 255, isUnicode: true, isText: false, columnMeta: null);
            this.AttributeType = this.CreateInt32Column("AttributeType", null);
        }

        [SqModel("AttributeData", PropertyName = "Id")]
        public Int32TableColumn AttributeId { get; }

        [SqModel("AttributeData", PropertyName = "Name")]
        public StringTableColumn AttributeName { get; }

        [SqModel("AttributeData", PropertyName = "Type", CastType = typeof(AttributeType))]
        public Int32TableColumn AttributeType { get; }
    }
}