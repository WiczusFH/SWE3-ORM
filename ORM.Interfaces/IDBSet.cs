using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Interfaces
{
    public interface IDBSet
    {
        public void setContext(IContext context);
    }
}
