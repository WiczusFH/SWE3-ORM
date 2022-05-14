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
    /// Contains informations about Columns and their mapped members.
    /// </summary>
    public class ColumnMap : IColumnMap
    {
        public Dictionary<MemberInfo, IColumn> MapMI { get; set; } = new Dictionary<MemberInfo, IColumn>();

        public void addLink(MemberInfo memberInfo, IColumn column)
        {
            MapMI.Add(memberInfo, column);
        }

        public MemberInfo getMemberInfo(IColumn column)
        {
            foreach (MemberInfo info in MapMI.Keys) {
                if (MapMI[info] == column) {
                    return info;
                }
            }
            throw new Exception("MemberInfo not found. ");
        }

        public IColumn getColumn(MemberInfo info) {
            return MapMI[info];
        }

    }
}
