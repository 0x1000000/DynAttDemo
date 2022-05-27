using System;
using SqExpress;
using SqExpress.QueryBuilders.RecordSetter;
using DynAttDemo.Tables;
using SqExpress.Syntax.Names;
using System.Collections.Generic;

namespace DynAttDemo.Models
{
    public class ProductData
    {
        public ProductData(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public static ProductData Read(ISqDataRecordReader record, TblProduct table)
        {
            return new ProductData(id: table.ProductId.Read(record), name: table.ProductName.Read(record));
        }

        public static ProductData ReadOrdinal(ISqDataRecordReader record, TblProduct table, int offset)
        {
            return new ProductData(id: table.ProductId.Read(record, offset), name: table.ProductName.Read(record, offset + 1));
        }

        public int Id { get; }

        public string Name { get; }

        public ProductData WithId(int id)
        {
            return new ProductData(id: id, name: this.Name);
        }

        public ProductData WithName(string name)
        {
            return new ProductData(id: this.Id, name: name);
        }

        public static TableColumn[] GetColumns(TblProduct table)
        {
            return new TableColumn[]{table.ProductId, table.ProductName};
        }

        public static IRecordSetterNext GetMapping(IDataMapSetter<TblProduct, ProductData> s)
        {
            return s.Set(s.Target.ProductId, s.Source.Id).Set(s.Target.ProductName, s.Source.Name);
        }

        public static IRecordSetterNext GetUpdateKeyMapping(IDataMapSetter<TblProduct, ProductData> s)
        {
            return s.Set(s.Target.ProductId, s.Source.Id);
        }

        public static IRecordSetterNext GetUpdateMapping(IDataMapSetter<TblProduct, ProductData> s)
        {
            return s.Set(s.Target.ProductName, s.Source.Name);
        }

        public static ISqModelReader<ProductData, TblProduct> GetReader()
        {
            return ProductDataReader.Instance;
        }

        private class ProductDataReader : ISqModelReader<ProductData, TblProduct>
        {
            public static ProductDataReader Instance { get; } = new ProductDataReader();
            IReadOnlyList<ExprColumn> ISqModelReader<ProductData, TblProduct>.GetColumns(TblProduct table)
            {
                return ProductData.GetColumns(table);
            }

            ProductData ISqModelReader<ProductData, TblProduct>.Read(ISqDataRecordReader record, TblProduct table)
            {
                return ProductData.Read(record, table);
            }

            ProductData ISqModelReader<ProductData, TblProduct>.ReadOrdinal(ISqDataRecordReader record, TblProduct table, int offset)
            {
                return ProductData.ReadOrdinal(record, table, offset);
            }
        }

        public static ISqModelUpdaterKey<ProductData, TblProduct> GetUpdater()
        {
            return ProductDataUpdater.Instance;
        }

        private class ProductDataUpdater : ISqModelUpdaterKey<ProductData, TblProduct>
        {
            public static ProductDataUpdater Instance { get; } = new ProductDataUpdater();
            IRecordSetterNext ISqModelUpdater<ProductData, TblProduct>.GetMapping(IDataMapSetter<TblProduct, ProductData> dataMapSetter)
            {
                return ProductData.GetMapping(dataMapSetter);
            }

            IRecordSetterNext ISqModelUpdaterKey<ProductData, TblProduct>.GetUpdateKeyMapping(IDataMapSetter<TblProduct, ProductData> dataMapSetter)
            {
                return ProductData.GetUpdateKeyMapping(dataMapSetter);
            }

            IRecordSetterNext ISqModelUpdaterKey<ProductData, TblProduct>.GetUpdateMapping(IDataMapSetter<TblProduct, ProductData> dataMapSetter)
            {
                return ProductData.GetUpdateMapping(dataMapSetter);
            }
        }
    }
}