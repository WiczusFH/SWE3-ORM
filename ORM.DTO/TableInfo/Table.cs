using ORM.Interfaces;
using System;
using System.Collections.Generic;

namespace ORM.DTO
{
    public class Table : ITable
    {
        public string name { get; set; }
        public IColumnMap columnMap { get; set; } = new ColumnMap();
        public List<IInsert> outstandingInserts { get; set; } = new List<IInsert>();
    }
}
