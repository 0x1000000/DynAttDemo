using System;
using SqExpress;
using SqExpress.QueryBuilders.RecordSetter;
using DynAttDemo.Tables;
using SqExpress.Syntax.Names;
using System.Collections.Generic;

namespace DynAttDemo.Models
{
    public class AttributeItemData
    {
        public AttributeItemData(int id, int attributeId, string name)
        {
            this.Id = id;
            this.AttributeId = attributeId;
            this.Name = name;
        }

        public static AttributeItemData Read(ISqDataRecordReader record, TblAttributeItem table)
        {
            return new AttributeItemData(id: table.AttributeItemId.Read(record), attributeId: table.AttributeId.Read(record), name: table.AttributeItemName.Read(record));
        }

        public static AttributeItemData ReadOrdinal(ISqDataRecordReader record, TblAttributeItem table, int offset)
        {
            return new AttributeItemData(id: table.AttributeItemId.Read(record, offset), attributeId: table.AttributeId.Read(record, offset + 1), name: table.AttributeItemName.Read(record, offset + 2));
        }

        public int Id { get; }

        public int AttributeId { get; }

        public string Name { get; }

        public AttributeItemData WithId(int id)
        {
            return new AttributeItemData(id: id, attributeId: this.AttributeId, name: this.Name);
        }

        public AttributeItemData WithAttributeId(int attributeId)
        {
            return new AttributeItemData(id: this.Id, attributeId: attributeId, name: this.Name);
        }

        public AttributeItemData WithName(string name)
        {
            return new AttributeItemData(id: this.Id, attributeId: this.AttributeId, name: name);
        }

        public static TableColumn[] GetColumns(TblAttributeItem table)
        {
            return new TableColumn[]{table.AttributeItemId, table.AttributeId, table.AttributeItemName};
        }

        public static IRecordSetterNext GetMapping(IDataMapSetter<TblAttributeItem, AttributeItemData> s)
        {
            return s.Set(s.Target.AttributeItemId, s.Source.Id).Set(s.Target.AttributeId, s.Source.AttributeId).Set(s.Target.AttributeItemName, s.Source.Name);
        }

        public static IRecordSetterNext GetUpdateKeyMapping(IDataMapSetter<TblAttributeItem, AttributeItemData> s)
        {
            return s.Set(s.Target.AttributeItemId, s.Source.Id);
        }

        public static IRecordSetterNext GetUpdateMapping(IDataMapSetter<TblAttributeItem, AttributeItemData> s)
        {
            return s.Set(s.Target.AttributeId, s.Source.AttributeId).Set(s.Target.AttributeItemName, s.Source.Name);
        }

        public static ISqModelReader<AttributeItemData, TblAttributeItem> GetReader()
        {
            return AttributeItemDataReader.Instance;
        }

        private class AttributeItemDataReader : ISqModelReader<AttributeItemData, TblAttributeItem>
        {
            public static AttributeItemDataReader Instance { get; } = new AttributeItemDataReader();
            IReadOnlyList<ExprColumn> ISqModelReader<AttributeItemData, TblAttributeItem>.GetColumns(TblAttributeItem table)
            {
                return AttributeItemData.GetColumns(table);
            }

            AttributeItemData ISqModelReader<AttributeItemData, TblAttributeItem>.Read(ISqDataRecordReader record, TblAttributeItem table)
            {
                return AttributeItemData.Read(record, table);
            }

            AttributeItemData ISqModelReader<AttributeItemData, TblAttributeItem>.ReadOrdinal(ISqDataRecordReader record, TblAttributeItem table, int offset)
            {
                return AttributeItemData.ReadOrdinal(record, table, offset);
            }
        }

        public static ISqModelUpdaterKey<AttributeItemData, TblAttributeItem> GetUpdater()
        {
            return AttributeItemDataUpdater.Instance;
        }

        private class AttributeItemDataUpdater : ISqModelUpdaterKey<AttributeItemData, TblAttributeItem>
        {
            public static AttributeItemDataUpdater Instance { get; } = new AttributeItemDataUpdater();
            IRecordSetterNext ISqModelUpdater<AttributeItemData, TblAttributeItem>.GetMapping(IDataMapSetter<TblAttributeItem, AttributeItemData> dataMapSetter)
            {
                return AttributeItemData.GetMapping(dataMapSetter);
            }

            IRecordSetterNext ISqModelUpdaterKey<AttributeItemData, TblAttributeItem>.GetUpdateKeyMapping(IDataMapSetter<TblAttributeItem, AttributeItemData> dataMapSetter)
            {
                return AttributeItemData.GetUpdateKeyMapping(dataMapSetter);
            }

            IRecordSetterNext ISqModelUpdaterKey<AttributeItemData, TblAttributeItem>.GetUpdateMapping(IDataMapSetter<TblAttributeItem, AttributeItemData> dataMapSetter)
            {
                return AttributeItemData.GetUpdateMapping(dataMapSetter);
            }
        }
    }
}