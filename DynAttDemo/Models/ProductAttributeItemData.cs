using System;
using SqExpress;
using SqExpress.QueryBuilders.RecordSetter;
using DynAttDemo.Tables;
using SqExpress.Syntax.Names;
using System.Collections.Generic;

namespace DynAttDemo.Models
{
    public class ProductAttributeItemData
    {
        public ProductAttributeItemData(int productId, int attributeId, int attributeItemId)
        {
            this.ProductId = productId;
            this.AttributeId = attributeId;
            this.AttributeItemId = attributeItemId;
        }

        public static ProductAttributeItemData Read(ISqDataRecordReader record, TblProductAttributeItem table)
        {
            return new ProductAttributeItemData(productId: table.ProductId.Read(record), attributeId: table.AttributeId.Read(record), attributeItemId: table.AttributeItemId.Read(record));
        }

        public static ProductAttributeItemData ReadOrdinal(ISqDataRecordReader record, TblProductAttributeItem table, int offset)
        {
            return new ProductAttributeItemData(productId: table.ProductId.Read(record, offset), attributeId: table.AttributeId.Read(record, offset + 1), attributeItemId: table.AttributeItemId.Read(record, offset + 2));
        }

        public int ProductId { get; }

        public int AttributeId { get; }

        public int AttributeItemId { get; }

        public ProductAttributeItemData WithProductId(int productId)
        {
            return new ProductAttributeItemData(productId: productId, attributeId: this.AttributeId, attributeItemId: this.AttributeItemId);
        }

        public ProductAttributeItemData WithAttributeId(int attributeId)
        {
            return new ProductAttributeItemData(productId: this.ProductId, attributeId: attributeId, attributeItemId: this.AttributeItemId);
        }

        public ProductAttributeItemData WithAttributeItemId(int attributeItemId)
        {
            return new ProductAttributeItemData(productId: this.ProductId, attributeId: this.AttributeId, attributeItemId: attributeItemId);
        }

        public static TableColumn[] GetColumns(TblProductAttributeItem table)
        {
            return new TableColumn[]{table.ProductId, table.AttributeId, table.AttributeItemId};
        }

        public static IRecordSetterNext GetMapping(IDataMapSetter<TblProductAttributeItem, ProductAttributeItemData> s)
        {
            return s.Set(s.Target.ProductId, s.Source.ProductId).Set(s.Target.AttributeId, s.Source.AttributeId).Set(s.Target.AttributeItemId, s.Source.AttributeItemId);
        }

        public static ISqModelReader<ProductAttributeItemData, TblProductAttributeItem> GetReader()
        {
            return ProductAttributeItemDataReader.Instance;
        }

        private class ProductAttributeItemDataReader : ISqModelReader<ProductAttributeItemData, TblProductAttributeItem>
        {
            public static ProductAttributeItemDataReader Instance { get; } = new ProductAttributeItemDataReader();
            IReadOnlyList<ExprColumn> ISqModelReader<ProductAttributeItemData, TblProductAttributeItem>.GetColumns(TblProductAttributeItem table)
            {
                return ProductAttributeItemData.GetColumns(table);
            }

            ProductAttributeItemData ISqModelReader<ProductAttributeItemData, TblProductAttributeItem>.Read(ISqDataRecordReader record, TblProductAttributeItem table)
            {
                return ProductAttributeItemData.Read(record, table);
            }

            ProductAttributeItemData ISqModelReader<ProductAttributeItemData, TblProductAttributeItem>.ReadOrdinal(ISqDataRecordReader record, TblProductAttributeItem table, int offset)
            {
                return ProductAttributeItemData.ReadOrdinal(record, table, offset);
            }
        }

        public static ISqModelUpdater<ProductAttributeItemData, TblProductAttributeItem> GetUpdater()
        {
            return ProductAttributeItemDataUpdater.Instance;
        }

        private class ProductAttributeItemDataUpdater : ISqModelUpdater<ProductAttributeItemData, TblProductAttributeItem>
        {
            public static ProductAttributeItemDataUpdater Instance { get; } = new ProductAttributeItemDataUpdater();
            IRecordSetterNext ISqModelUpdater<ProductAttributeItemData, TblProductAttributeItem>.GetMapping(IDataMapSetter<TblProductAttributeItem, ProductAttributeItemData> dataMapSetter)
            {
                return ProductAttributeItemData.GetMapping(dataMapSetter);
            }
        }
    }
}