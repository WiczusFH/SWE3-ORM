using NpgsqlTypes;
using System;
using System.Reflection;

namespace ORM.Interfaces
{
    public interface IColumn
    {
        public string name { get; set; }
        public NpgsqlDbType type {get;set;}
        public bool unique { get; set; }
        public bool primary { get; set; }
        public bool nullable { get; set; }
        public int length { get; set; }
        public bool hidden { get; set; }
        public bool serial { get; set; }
        public ITable dependencyTable { get; set; }

        public IColumn dependencyColumn { get; set; }
    }
}
