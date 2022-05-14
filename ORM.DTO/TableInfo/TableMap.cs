using ORM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORM.DTO
{
    /// <summary>
    /// Contains informations on Tables and their mapped Classes or Members. 
    /// </summary>
    public class TableMap : ITableMap
    {
        public Dictionary<Type, ITable> MapType { get; private set; } = new Dictionary<Type, ITable>();
        public Dictionary<MemberInfo, ITable> MapMI { get; private set; } = new Dictionary<MemberInfo, ITable>();
        public ITable getTable(string name)
        {
            foreach (MemberInfo info in MapMI.Keys)
            {
                if (info.Name == name)
                {
                    return MapMI[info];
                }
            }
            foreach (Type type in MapType.Keys)
            {
                if (type.Name == name)
                {
                    return MapType[type];
                }
            }
            return null;
        }
        public List<ITable> getTableList() {
            List<ITable> result = new List<ITable>();
            foreach (ITable table in MapType.Values) {
                result.Add(table);
            }
            foreach (ITable table in MapMI.Values)
            {
                result.Add(table);
            }
            return result;
        }
        public void addTableLink(Type type, ITable table)
        {
            if (!MapType.ContainsKey(type)) { 
                MapType.Add(type, table);
            }
        }

        public void addTableLink(MemberInfo memberInfo, ITable table)
        {
            if (!MapMI.ContainsKey(memberInfo))
            {
               MapMI.Add(memberInfo, table);
            }
        }

        public MemberInfo getMemberInfo(ITable table)
        {
            foreach (MemberInfo info in MapMI.Keys) {
                if (MapMI[info] == table) {
                    return info;
                }
            }
            return null;
        }

        public ITable getTable(Type type)
        {
           return MapType.ContainsKey(type) ? MapType[type] : null;
        }
        public ITable getTable(MemberInfo info) {
            return MapMI.ContainsKey(info) ? MapMI[info] : null;
        }
        public Type getType(ITable table)
        {
            foreach (Type type in MapType.Keys)
            {
                if (MapType[type] == table)
                {
                    return type;
                }
            }
            return null;
        }

        public void merge(ITableMap tableTypeMap)
        {
            foreach (MemberInfo key in tableTypeMap.MapMI.Keys) {
                MapMI.Add(key, tableTypeMap.MapMI[key]);
            }
            foreach (Type key in tableTypeMap.MapType.Keys)
            {
                MapMI.Add(key, tableTypeMap.MapType[key]);
            }
        }
    }
}
