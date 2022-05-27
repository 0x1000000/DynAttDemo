using System;
using SqExpress;
using SqExpress.QueryBuilders.RecordSetter;
using DynAttDemo.Tables;
using SqExpress.Syntax.Names;
using System.Collections.Generic;

namespace DynAttDemo.Models
{
    public class AttributeData
    {
        public AttributeData(int id, string name, AttributeType type)
        {
            this.Id = id;
            this.Name = name;
            this.Type = type;
        }

        public static AttributeData Read(ISqDataRecordReader record, TblAttribute table)
        {
            return new AttributeData(id: table.AttributeId.Read(record), name: table.AttributeName.Read(record), type: (AttributeType)table.AttributeType.Read(record));
        }

        public static AttributeData ReadOrdinal(ISqDataRecordReader record, TblAttribute table, int offset)
        {
            return new AttributeData(id: table.AttributeId.Read(record, offset), name: table.AttributeName.Read(record, offset + 1), type: (AttributeType)table.AttributeType.Read(record, offset + 2));
        }

        public int Id { get; }

        public string Name { get; }

        public AttributeType Type { get; }

        public AttributeData WithId(int id)
        {
            return new AttributeData(id: id, name: this.Name, type: this.Type);
        }

        public AttributeData WithName(string name)
        {
            return new AttributeData(id: this.Id, name: name, type: this.Type);
        }

        public AttributeData WithType(AttributeType type)
        {
            return new AttributeData(id: this.Id, name: this.Name, type: type);
        }

        public static TableColumn[] GetColumns(TblAttribute table)
        {
            return new TableColumn[]{table.AttributeId, table.AttributeName, table.AttributeType};
        }

        public static IRecordSetterNext GetMapping(IDataMapSetter<TblAttribute, AttributeData> s)
        {
            return s.Set(s.Target.AttributeId, s.Source.Id).Set(s.Target.AttributeName, s.Source.Name).Set(s.Target.AttributeType, (int)s.Source.Type);
        }

        public static IRecordSetterNext GetUpdateKeyMapping(IDataMapSetter<TblAttribute, AttributeData> s)
        {
            return s.Set(s.Target.AttributeId, s.Source.Id);
        }

        public static IRecordSetterNext GetUpdateMapping(IDataMapSetter<TblAttribute, AttributeData> s)
        {
            return s.Set(s.Target.AttributeName, s.Source.Name).Set(s.Target.AttributeType, (int)s.Source.Type);
        }

        public static ISqModelReader<AttributeData, TblAttribute> GetReader()
        {
            return AttributeDataReader.Instance;
        }

        private class AttributeDataReader : ISqModelReader<AttributeData, TblAttribute>
        {
            public static AttributeDataReader Instance { get; } = new AttributeDataReader();
            IReadOnlyList<ExprColumn> ISqModelReader<AttributeData, TblAttribute>.GetColumns(TblAttribute table)
            {
                return AttributeData.GetColumns(table);
            }

            AttributeData ISqModelReader<AttributeData, TblAttribute>.Read(ISqDataRecordReader record, TblAttribute table)
            {
                return AttributeData.Read(record, table);
            }

            AttributeData ISqModelReader<AttributeData, TblAttribute>.ReadOrdinal(ISqDataRecordReader record, TblAttribute table, int offset)
            {
                return AttributeData.ReadOrdinal(record, table, offset);
            }
        }

        public static ISqModelUpdaterKey<AttributeData, TblAttribute> GetUpdater()
        {
            return AttributeDataUpdater.Instance;
        }

        private class AttributeDataUpdater : ISqModelUpdaterKey<AttributeData, TblAttribute>
        {
            public static AttributeDataUpdater Instance { get; } = new AttributeDataUpdater();
            IRecordSetterNext ISqModelUpdater<AttributeData, TblAttribute>.GetMapping(IDataMapSetter<TblAttribute, AttributeData> dataMapSetter)
            {
                return AttributeData.GetMapping(dataMapSetter);
            }

            IRecordSetterNext ISqModelUpdaterKey<AttributeData, TblAttribute>.GetUpdateKeyMapping(IDataMapSetter<TblAttribute, AttributeData> dataMapSetter)
            {
                return AttributeData.GetUpdateKeyMapping(dataMapSetter);
            }

            IRecordSetterNext ISqModelUpdaterKey<AttributeData, TblAttribute>.GetUpdateMapping(IDataMapSetter<TblAttribute, AttributeData> dataMapSetter)
            {
                return AttributeData.GetUpdateMapping(dataMapSetter);
            }
        }
    }
}