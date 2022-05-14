using System;
using System.Collections.Generic;
using System.Reflection;

namespace ORM.Interfaces
{
    public interface IColumnMap
    {
        public Dictionary<MemberInfo, IColumn> MapMI{get;}
        public void addLink(MemberInfo memberInfo, IColumn column);

        public IColumn getColumn(MemberInfo info);
        public MemberInfo getMemberInfo(IColumn column);
    }
}
