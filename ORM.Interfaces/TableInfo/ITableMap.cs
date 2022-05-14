using System;
using System.Collections.Generic;
using System.Reflection;

namespace ORM.Interfaces
{
    public interface ITableMap
    {
        public Dictionary<MemberInfo, ITable> MapMI {get;}
        public Dictionary<Type, ITable> MapType{get;}
        public void addTableLink(Type type, ITable table);
        public void addTableLink(MemberInfo memberInfo, ITable table);


        public ITable getTable(string name);
        public ITable getTable(Type type);
        public ITable getTable(MemberInfo info);
        public MemberInfo getMemberInfo(ITable table);
        public Type getType(ITable table);
        public List<ITable> getTableList();
        public void merge(ITableMap tableTypeMap);
    }
}
