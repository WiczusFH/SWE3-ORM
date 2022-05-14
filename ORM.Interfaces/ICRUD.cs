using Npgsql;
using ORM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ORM.Interfaces
{
    public interface ICRUD
    {
        void createStatements(ITable table);
        void insertStatements(string tableName, List<IInsert> insert);
        void insertStatement(string tableName, IInsert insert);
        void deleteStatement(ITable table, string whereStatement);
        List<T> selectStatement<T>(ITable table, string whereStatement);
    }
}