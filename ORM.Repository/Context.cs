using ORM.Attributes;
using ORM.DTO;
using ORM.Interfaces;
using ORM.Postgres;
using ORM.Solver;
using ORM.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ORM.Repository
{
    public class Context : IContext
    {

        public ITableMap _TableMap { get; set; } = new TableMap();
        public ICRUD crud = new CRUD();
        public List<ITable> tableOrder = new List<ITable>();
        public List<IList> sets = new List<IList>();




        public Context(){
            MemberInfo[] infos = this.GetType().GetMembers();
            foreach (MemberInfo info in infos)
            {
                if (Reflection.getTypeFromMember(info)?.GetCustomAttribute(typeof(DBSetAttribute)) != null)
                {
                    ((IDBSet)Reflection.GetValue(info, this)).setContext(this);


                }
            }
        }

        public void build()
        {
            TableSolver tSolver = new TableSolver();
            DependencySolver dSolver = new DependencySolver();
            OrderSolver oSolver = new OrderSolver();

            MemberInfo[] infos = this.GetType().GetMembers();
            foreach (MemberInfo info in infos)
            {
                if (Reflection.getTypeFromMember(info)?.GetCustomAttribute(typeof(DBSetAttribute)) != null)
                {
                    Type type = Reflection.getTypeFromMember(info).GetProperty("Item").PropertyType;
                    sets.Add((IList)((FieldInfo)info).GetValue(this));
                    tSolver.solveForTable(type);
                }
            }
            _TableMap = tSolver.map;
            foreach (Type type in _TableMap.MapType.Keys)
            {
                dSolver.solveMNDependencies(type, _TableMap);
            }
            _TableMap.merge(dSolver.rt_map);
            oSolver.solve(_TableMap);
            tableOrder = oSolver.tablesOrdered;
            foreach (ITable table in tableOrder)
            {
                crud.createStatements(table);
            }
        }

        public void insert()
        {
            //InsertSolver solver = new InsertSolver();
            //solver.solve(sets);

            foreach (IList set in sets)
            {
                Type type = set.GetType().GenericTypeArguments[0];
                ITable table = _TableMap.getTable(type);

                //Foreach table entry
                foreach (var entry in set)
                {
                    insertFromInstance(table, entry, null);
                }
            }
            foreach (Table table in tableOrder) {
                crud.insertStatements(table.name,table.outstandingInserts);
            }
        }
        List<object> alreadyInserted = new List<object>();
        object insertFromInstance(ITable table, object source, IColumn key) {
            Insert insert = new Insert();
            if (source == null) {
                return null;
            }
            if (alreadyInserted.Contains(source)) {
                Console.WriteLine("Warning circular dependency. Cutting. ");
                return null;
            }
            alreadyInserted.Add(source);
            foreach (MemberInfo info in table.columnMap.MapMI.Keys)
            {
                IColumn col = table.columnMap.MapMI[info];
                if (!col.hidden && col.dependencyTable == null)
                {
                    var value = Reflection.GetValue(info, source);
                    insert.data.Add(col, value);
                }
                else if (!col.hidden && col.dependencyTable != null)
                {
                    //TODO: Check if value is already there. In which case do not use recursion. 

                    var value = insertFromInstance(col.dependencyTable, Reflection.GetValue(info, source), col.dependencyColumn);
                    insert.data.Add(col, value);
                }

            }
            foreach (MemberInfo info in table.columnMap.MapMI.Keys)
            {
                IColumn col = table.columnMap.MapMI[info];
                if (col.hidden && col.dependencyTable != null)
                {
                    IList targetValues = (IList) Reflection.GetValue(info, source);
                    if (targetValues == null) {
                        continue;
                    }
                    foreach (var targetValue in targetValues)
                    {
                        insertmtnDependency(col.dependencyTable, table, insert, targetValue);
                    }
                }
            }
            table.outstandingInserts.Add(insert);
            if (key != null)
            {
                return insert.data[key];
            }
            return null;
        }
        object insertmtnDependency(ITable MNTable, ITable sourceTable, Insert insert,object targetValue)
        {
            //MN Table
            object sourceVal=null;
            object targetVal;

            Insert newInsert = new Insert();
            //2 Info Max
            foreach (MemberInfo info in MNTable.columnMap.MapMI.Keys) {
                IColumn col = MNTable.columnMap.MapMI[info];
                if (col.dependencyTable == sourceTable)
                {
                    sourceVal = insert.data[col.dependencyColumn];
                    newInsert.data.Add(col, sourceVal);
                }
                else {
                    targetVal = insertFromInstance(col.dependencyTable, targetValue, col.dependencyColumn);
                    newInsert.data.Add(col, targetVal);
                }
            }

            MNTable.outstandingInserts.Add(newInsert);
            
            //return not strictly necessary
            return sourceVal;
        }
    }
}
