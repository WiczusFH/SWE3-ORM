using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ColumnName : Attribute
    {
        public string name { get; }
        public ColumnName(string name)
        {
            this.name = name;
        }
    }
}
