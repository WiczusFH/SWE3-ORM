using ORM.DTO;
using System;
using System.Collections.Generic;
using ORM.Util;
using System.Reflection;
using ORM.Interfaces;
using ORM.Attributes;
using System.Linq;
using OZCore.Converters;

namespace ORM.Solver
{
    public class DependencySolver
    {
        public TableMap rt_map = new TableMap();
        public void solveMNDependencies(Type type, ITableMap map) {
            //Not implemented
            TableMap result = new TableMap();
            foreach(MemberInfo info in type.GetMembers()) {
                if (Reflection.isList(info)) {
                    Table newTable = new Table();
                    Type targetType = Reflection.getTypeFromMember(info).GetProperty("Item").PropertyType;
                    
                    ITable targetTable =  map.getTable(targetType);
                    IColumn targetColumn = getFK(targetTable, info);

                    ITable sourceTable = map.getTable(type);
                    IColumn sourceColumn = getFK(sourceTable, targetTable.columnMap.getMemberInfo(targetColumn));

                    newTable.name = "RT_" + targetTable.name + "_" + targetColumn.name + "_" + sourceTable.name + "_" + sourceColumn.name;

                    Column pointingSource = new Column();
                    pointingSource.dependencyColumn = sourceColumn;
                    pointingSource.dependencyTable = sourceTable;
                    pointingSource.name = sourceColumn.name;
                    pointingSource.type = sourceColumn.type;

                    Column pointingTarget = new Column();
                    pointingTarget.dependencyColumn = targetColumn;
                    pointingTarget.dependencyTable = targetTable;
                    pointingTarget.name= targetColumn.name;
                    pointingTarget.type = targetColumn.type;

                    newTable.columnMap = new ColumnMap();
                    newTable.columnMap.addLink(sourceTable.columnMap.getMemberInfo(sourceColumn), pointingSource);
                    newTable.columnMap.addLink(targetTable.columnMap.getMemberInfo(targetColumn), pointingTarget);

                    map.getTable(type).columnMap.getColumn(info).hidden = true;
                    map.getTable(type).columnMap.getColumn(info).dependencyTable = newTable;
                    rt_map.addTableLink(info,newTable);
                }
            }
        }

        /**
         * Get FK based on table fields or foreignkey contained in column info. 
         */
        public static IColumn getFK(ITable table, MemberInfo sourceColumnInfo) {
            ForeignKey fkAttribute = (ForeignKey)Attribute.GetCustomAttribute(sourceColumnInfo, typeof(ForeignKey));
            if (fkAttribute != null) {
                foreach(MemberInfo key in table.columnMap.MapMI.Keys)
                {
                    if (table.columnMap.MapMI[key].name == fkAttribute.column) {
                        return table.columnMap.MapMI[key];
                    }
                }
            }
            foreach (MemberInfo key in table.columnMap.MapMI.Keys)
            {
                if (table.columnMap.MapMI[key].name.ToUpper().Contains("ID"))
                {
                    return table.columnMap.MapMI[key];
                }
            }
            return table.columnMap.MapMI[table.columnMap.MapMI.Keys.First()];
        }
    }
}
