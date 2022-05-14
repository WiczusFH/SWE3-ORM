using ORM.Interfaces;
using ORM.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Postgres
{
    public class LinqQueryBuilder : ExpressionVisitor
    {
        ITableMap tableMap;
        public string baseQuery{ get; private set; }
        public string whereQuery { get; private set; } = "WHERE ";
        public string orderQuery { get; private set; }
        public string selectQuery { get; private set; }
        public LinqQueryBuilder(Expression exprTree, ITableMap tableMap) {
            this.tableMap = tableMap;
            Visit(exprTree);
        }

        private static readonly StringBuilder _sql = new StringBuilder();
        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);
            whereQuery += " "+ExpOpPgOpConverter.PgOpFromExpOp(node.NodeType)+" ";
            Visit(node.Right);
            return node;
        }
        bool inCountRecursion=false;
        protected override Expression VisitMember(MemberExpression node)
        {
            if (inCountRecursion) {
                ITable declaringTable = tableMap.getTable(node.Member.DeclaringType);
                IColumn column = declaringTable.columnMap.getColumn(node.Member);
                whereQuery += declaringTable.name + "." + column.name;
                whereQuery += ")";
                inCountRecursion = false;
                return node;

            }
            if (node.ToString().Contains("Length") || node.ToString().Contains("Count")) {
                whereQuery += "length(";
                inCountRecursion = true;
                Visit(node.Expression);

            } else {
            ITable declaringTable = tableMap.getTable(node.Member.DeclaringType);
            IColumn column = declaringTable.columnMap.getColumn(node.Member);
            whereQuery += declaringTable.name + "." + column.name;
            }
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Type == typeof(string))
            {
                whereQuery += "'" + node.Value + "'" ?? "null";
            }
            else {
                whereQuery += node.Value ?? "null";
            }
            return node;
        }
    }
}
