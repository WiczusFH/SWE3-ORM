using ORM.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Solver
{
    /// <summary>
    /// Makes creates inserts by listing user defined classes. 
    /// Not implemented, analogous, recursive method is contained in Context class. 
    /// </summary>
    public class InsertSolver
    {
        HashSet<object> toInsert = new HashSet<object>();
        public List<Insert> inserts = new List<Insert>();
        public void solve(List<IList> sets) {
            throw new NotImplementedException();
            foreach(IList set in sets) {
                foreach (var value in set) {
                    toInsert.Add(value);
                }   
            }
        }
    }
}
