using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ORM.DTO
{
    public class Cache<T>
    {
        public Dictionary<Expression<Func<T, bool>>, List<T>> formerqueries = new Dictionary<Expression<Func<T, bool>>, List<T>>();
    }
}
