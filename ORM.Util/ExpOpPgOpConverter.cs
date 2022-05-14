using System;
using System.Linq.Expressions;

namespace ORM.Util
{
    public class ExpOpPgOpConverter
    {
        public static string PgOpFromExpOp(ExpressionType expressionOperator)
        {
            return expressionOperator switch
            {
                ExpressionType.Equal => "=",
                ExpressionType.LessThanOrEqual => "<=",
                ExpressionType.GreaterThan => ">",
                ExpressionType.And => "AND",
                ExpressionType.Or => "OR",
                ExpressionType.NotEqual => "<>",
                ExpressionType.OrElse => "OR",
                ExpressionType.AndAlso => "AND",
                ExpressionType.LessThan => "<",
                ExpressionType.GreaterThanOrEqual => ">=",
                _ => null
            };
        }
    }
}
