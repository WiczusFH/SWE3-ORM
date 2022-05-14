using Npgsql;
using ORM.Attributes;
using ORM.DTO;
using ORM.Interfaces;
using ORM.Postgres;
using ORM.Repository;
using ORM.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Repository
{
    [DBSetAttribute]
    public class DBSet<T> :  List<T>, IDBSet
    {
        public Type getTableType() {
            return typeof(T);
        }
        public Cache<T> cache = new Cache<T>();
        public ICRUD crud = new CRUD();
        IContext context;
        public void clearCache() {
            cache = new Cache<T>();
        }
        public void delete(Expression<Func<T, bool>> exp) {
            ITable table = context._TableMap.getTable(typeof(T));
            LinqQueryBuilder builder = new LinqQueryBuilder(exp, context._TableMap);
            foreach (MemberInfo info in typeof(T).GetMembers()) {

                ITable rt_Table = context._TableMap.getTable(info);
                if (rt_Table != null) { 
                    CascaseDelete attribute = (CascaseDelete)info.GetCustomAttribute(typeof(CascaseDelete));
                    if (attribute != null) {
                        List<T> toDelete = get(exp);
                        foreach (MemberInfo rt_info in rt_Table.columnMap.MapMI.Keys) {
                            IColumn col = rt_Table.columnMap.MapMI[rt_info];
                            if (col.dependencyTable == table) { 
                                foreach(T tD in toDelete)
                                {
                                    string statement = "WHERE " + col.dependencyColumn.name+"='"+Reflection.GetValue(rt_info,tD)+"'";
                                    crud.deleteStatement(rt_Table, statement);
                                }
                            }
                        }
                    }
                }
            }
            crud.deleteStatement(table, builder.whereQuery);

        }
        public List<T> get(Expression<Func<T,bool>> exp)
        {
            List<T> result = new List<T>();
            Func<T,bool> compiledExpression = exp.Compile();
            result.AddRange(this.Where(compiledExpression));

            if (cache.formerqueries.ContainsKey(exp))
            {
                List<T> cachedData = cache.formerqueries[exp];
                result.AddRange(cachedData);
            }
            else {
                ITable table = context._TableMap.getTable(typeof(T));
                LinqQueryBuilder builder = new LinqQueryBuilder(exp,context._TableMap);
                List<T> fromDB = crud.selectStatement<T>(table, builder.whereQuery);
                result.AddRange(fromDB);
            }


            return result;
        }

        public void setContext(IContext context)
        {
            this.context = context;
        }
    }
}
