using System;
using System.Collections.Generic;

namespace ORM.Interfaces
{
    public interface ITable
    {
        public string name { get; set; }
        public IColumnMap columnMap { get; set; }
        public List<IInsert> outstandingInserts { get; set; }

    }
}
