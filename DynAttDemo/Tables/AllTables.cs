using SqExpress;

namespace DynAttDemo.Tables
{
    public static class AllTables
    {
        public static TableBase[] BuildAllTableList() => new TableBase[]{GetAttribute(Alias.Empty), GetAttributeItem(Alias.Empty), GetProduct(Alias.Empty), GetProductAttribute(Alias.Empty), GetProductAttributeItem(Alias.Empty)};
        public static TblAttribute GetAttribute(Alias alias) => new TblAttribute(alias);
        public static TblAttribute GetAttribute() => new TblAttribute(Alias.Auto);
        public static TblAttributeItem GetAttributeItem(Alias alias) => new TblAttributeItem(alias);
        public static TblAttributeItem GetAttributeItem() => new TblAttributeItem(Alias.Auto);
        public static TblProduct GetProduct(Alias alias) => new TblProduct(alias);
        public static TblProduct GetProduct() => new TblProduct(Alias.Auto);
        public static TblProductAttribute GetProductAttribute(Alias alias) => new TblProductAttribute(alias);
        public static TblProductAttribute GetProductAttribute() => new TblProductAttribute(Alias.Auto);
        public static TblProductAttributeItem GetProductAttributeItem(Alias alias) => new TblProductAttributeItem(alias);
        public static TblProductAttributeItem GetProductAttributeItem() => new TblProductAttributeItem(Alias.Auto);
    }
}