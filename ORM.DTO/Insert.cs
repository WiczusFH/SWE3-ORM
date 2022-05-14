using ORM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.DTO
{
    public class Insert : IInsert
    {
        public Dictionary<IColumn, object> data { get; set; } = new Dictionary<IColumn, object>();
    }
}
