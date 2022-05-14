using ORM.Attributes;
using ORM.DTO;
using ORM.Util;
using OZCore.Attributes;
using OZCore.Converters;
using System;
using System.Reflection;

namespace ORM.Solver
{
    public class TableSolver
    {

        public TableMap map = new TableMap();
        /**
         * Fills the Table map. Returns last table result.
         */
        public Table solveForTable(Type type)
        {
            Table result = new Table();

            result.name = getTableName(type);
            //InheritanceSolver inhSolver = new InheritanceSolver();
            //inhSolver.solve(type);
            result.columnMap = createColumnMap(type);
            //if (inhSolver.baseClass != null)
            //{
            //    solveForTable(inhSolver.baseClass);
            //}

            map.addTableLink(type, result);
            return result;
        }
        /**
         * Creates Column Map. If 1:1 dependency is found, it solves for that table and links the dependency.
         */
        public ColumnMap createColumnMap(Type type)
        {
            ColumnMap result = new ColumnMap();
            foreach (MemberInfo info in type.GetMembers())
            {
                if ((info.MemberType == MemberTypes.Property || info.MemberType == MemberTypes.Field))
                {
                    Column column = new Column();
                    column.name = getColumnName(type,info);

                    if (Reflection.isPrimitive(info) && !Reflection.isList(info))
                    {
                        column.type = NPGTypeConverter.NPGTypeFromCSType(Reflection.getTypeFromMember(info));
                    }
                    if (!Reflection.isPrimitive(info) && !Reflection.isList(info))
                    {
                        column.dependencyTable=solveForTable(Reflection.getTypeFromMember(info));
                        column.dependencyColumn = DependencySolver.getFK(column.dependencyTable,info);
                        column.type = column.dependencyColumn.type;
                    }
                    PrimaryKey serialAttribute = (PrimaryKey)Attribute.GetCustomAttribute(info, typeof(PrimaryKey));
                    if (serialAttribute!=null) {
                        column.serial = true;
                    }
                    UniqueKey uniqueKey = (UniqueKey)Attribute.GetCustomAttribute(type, typeof(UniqueKey));
                    if (serialAttribute != null)
                    {
                        column.unique = true;
                    }
                    result.addLink(info, column);
                }
            }
            return result;
        }
        /*
         *Set the name of the table. If attribute is present use it, if not use standard name. 
         */
        public static string getTableName(Type type)
        {
            TableName nameAttribute = (TableName)Attribute.GetCustomAttribute(type, typeof(TableName));
            if (nameAttribute != null)
            {
                return nameAttribute.name;
            }
            return type.Name;

        }
        public static string getColumnName(Type type, MemberInfo info)
        {
            ColumnName nameAttribute = (ColumnName)Attribute.GetCustomAttribute(type, typeof(ColumnName));
            if (nameAttribute != null)
            {
                return nameAttribute.name;
            }
            return info.Name;
        }

    }
}
