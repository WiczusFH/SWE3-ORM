using NpgsqlTypes;
using ORM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZCore.DTOs
{
    public class Param : IParam
    {
        public Param(string columnName, object columnValue, NpgsqlDbType columnType)
        {
            this.columnName = columnName;
            this.columnValue = columnValue ?? DBNull.Value;
            this.columnType = columnType;
        }

        public string columnName { get; set; }
        public object columnValue { get; set; }
        public NpgsqlDbType columnType { get; set; }
    }
}
