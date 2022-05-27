using System;
using SqExpress;
using SqExpress.QueryBuilders.RecordSetter;
using DynAttDemo.Tables;
using SqExpress.Syntax.Names;
using System.Collections.Generic;

namespace DynAttDemo.Models
{
    public class ProductAttributeData
    {
        public ProductAttributeData(int productId, int attributeId, int? valueItem, int? valueInt, DateTime? valueDate)
        {
            this.ProductId = productId;
            this.AttributeId = attributeId;
            this.ValueItem = valueItem;
            this.ValueInt = valueInt;
            this.ValueDate = valueDate;
        }

        public static ProductAttributeData Read(ISqDataRecordReader record, TblProductAttribute table)
        {
            return new ProductAttributeData(productId: table.ProductId.Read(record), attributeId: table.AttributeId.Read(record), valueItem: table.ValueItem.Read(record), valueInt: table.ValueInt.Read(record), valueDate: table.ValueDate.Read(record));
        }

        public static ProductAttributeData ReadOrdinal(ISqDataRecordReader record, TblProductAttribute table, int offset)
        {
            return new ProductAttributeData(productId: table.ProductId.Read(record, offset), attributeId: table.AttributeId.Read(record, offset + 1), valueItem: table.ValueItem.Read(record, offset + 2), valueInt: table.ValueInt.Read(record, offset + 3), valueDate: table.ValueDate.Read(record, offset + 4));
        }

        public int ProductId { get; }

        public int AttributeId { get; }

        public int? ValueItem { get; }

        public int? ValueInt { get; }

        public DateTime? ValueDate { get; }

        public ProductAttributeData WithProductId(int productId)
        {
            return new ProductAttributeData(productId: productId, attributeId: this.AttributeId, valueItem: this.ValueItem, valueInt: this.ValueInt, valueDate: this.ValueDate);
        }

        public ProductAttributeData WithAttributeId(int attributeId)
        {
            return new ProductAttributeData(productId: this.ProductId, attributeId: attributeId, valueItem: this.ValueItem, valueInt: this.ValueInt, valueDate: this.ValueDate);
        }

        public ProductAttributeData WithValueItem(int? valueItem)
        {
            return new ProductAttributeData(productId: this.ProductId, attributeId: this.AttributeId, valueItem: valueItem, valueInt: this.ValueInt, valueDate: this.ValueDate);
        }

        public ProductAttributeData WithValueInt(int? valueInt)
        {
            return new ProductAttributeData(productId: this.ProductId, attributeId: this.AttributeId, valueItem: this.ValueItem, valueInt: valueInt, valueDate: this.ValueDate);
        }

        public ProductAttributeData WithValueDate(DateTime? valueDate)
        {
            return new ProductAttributeData(productId: this.ProductId, attributeId: this.AttributeId, valueItem: this.ValueItem, valueInt: this.ValueInt, valueDate: valueDate);
        }

        public static TableColumn[] GetColumns(TblProductAttribute table)
        {
            return new TableColumn[]{table.ProductId, table.AttributeId, table.ValueItem, table.ValueInt, table.ValueDate};
        }

        public static IRecordSetterNext GetMapping(IDataMapSetter<TblProductAttribute, ProductAttributeData> s)
        {
            return s.Set(s.Target.ProductId, s.Source.ProductId).Set(s.Target.AttributeId, s.Source.AttributeId).Set(s.Target.ValueItem, s.Source.ValueItem).Set(s.Target.ValueInt, s.Source.ValueInt).Set(s.Target.ValueDate, s.Source.ValueDate);
        }

        public static IRecordSetterNext GetUpdateKeyMapping(IDataMapSetter<TblProductAttribute, ProductAttributeData> s)
        {
            return s.Set(s.Target.ProductId, s.Source.ProductId).Set(s.Target.AttributeId, s.Source.AttributeId);
        }

        public static IRecordSetterNext GetUpdateMapping(IDataMapSetter<TblProductAttribute, ProductAttributeData> s)
        {
            return s.Set(s.Target.ValueItem, s.Source.ValueItem).Set(s.Target.ValueInt, s.Source.ValueInt).Set(s.Target.ValueDate, s.Source.ValueDate);
        }

        public static ISqModelReader<ProductAttributeData, TblProductAttribute> GetReader()
        {
            return ProductAttributeDataReader.Instance;
        }

        private class ProductAttributeDataReader : ISqModelReader<ProductAttributeData, TblProductAttribute>
        {
            public static ProductAttributeDataReader Instance { get; } = new ProductAttributeDataReader();
            IReadOnlyList<ExprColumn> ISqModelReader<ProductAttributeData, TblProductAttribute>.GetColumns(TblProductAttribute table)
            {
                return ProductAttributeData.GetColumns(table);
            }

            ProductAttributeData ISqModelReader<ProductAttributeData, TblProductAttribute>.Read(ISqDataRecordReader record, TblProductAttribute table)
            {
                return ProductAttributeData.Read(record, table);
            }

            ProductAttributeData ISqModelReader<ProductAttributeData, TblProductAttribute>.ReadOrdinal(ISqDataRecordReader record, TblProductAttribute table, int offset)
            {
                return ProductAttributeData.ReadOrdinal(record, table, offset);
            }
        }

        public static ISqModelUpdaterKey<ProductAttributeData, TblProductAttribute> GetUpdater()
        {
            return ProductAttributeDataUpdater.Instance;
        }

        private class ProductAttributeDataUpdater : ISqModelUpdaterKey<ProductAttributeData, TblProductAttribute>
        {
            public static ProductAttributeDataUpdater Instance { get; } = new ProductAttributeDataUpdater();
            IRecordSetterNext ISqModelUpdater<ProductAttributeData, TblProductAttribute>.GetMapping(IDataMapSetter<TblProductAttribute, ProductAttributeData> dataMapSetter)
            {
                return ProductAttributeData.GetMapping(dataMapSetter);
            }

            IRecordSetterNext ISqModelUpdaterKey<ProductAttributeData, TblProductAttribute>.GetUpdateKeyMapping(IDataMapSetter<TblProductAttribute, ProductAttributeData> dataMapSetter)
            {
                return ProductAttributeData.GetUpdateKeyMapping(dataMapSetter);
            }

            IRecordSetterNext ISqModelUpdaterKey<ProductAttributeData, TblProductAttribute>.GetUpdateMapping(IDataMapSetter<TblProductAttribute, ProductAttributeData> dataMapSetter)
            {
                return ProductAttributeData.GetUpdateMapping(dataMapSetter);
            }
        }
    }
}