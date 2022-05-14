using ORM.DTO;
using ORM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Solver
{   
    /// <summary>
    /// Solves for create, insert, delete order. 
    /// </summary>
    public class OrderSolver
    {
        TableMap newMap = new TableMap();
        public List<ITable> tablesOrdered = new List<ITable>();

        public void solve(ITableMap map)
        {
            foreach (ITable table in map.getTableList())
            {
                tablesOrdered.Add(table);
                addDependencyTablesRecursive(table, tablesOrdered, null);
            }
            tablesOrdered.Reverse();
            HashSet<ITable> setOrdered = new HashSet<ITable>(tablesOrdered);
            tablesOrdered = setOrdered.ToList<ITable>();
        }

        public static void addDependencyTablesRecursive(ITable table, List<ITable> tables, ITable initialTable)
        {
            if (initialTable == table)
            {
                throw new Exception("Circular Dependency at " + initialTable.name);
            }
            if (initialTable == null)
            {
                initialTable = table;
            }
            foreach (IColumn column in table.columnMap.MapMI.Values)
            {
                if (column.dependencyTable != null && !column.hidden)
                {
                    tables.Add(column.dependencyTable);
                    addDependencyTablesRecursive(column.dependencyTable, tables, initialTable);
                }
            }
        }
    }
}
 