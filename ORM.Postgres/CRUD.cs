using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Configuration;
using ORM.Interfaces;
using ORM.DTO;
using System.Linq.Expressions;
using ORM.Util;
using System.Reflection;
using NpgsqlTypes;

namespace ORM.Postgres
{
    public class CRUD : ICRUD
    {

        static string connString = $"Host={ConfigurationManager.AppSettings["host"]};Username={ConfigurationManager.AppSettings["username"]};" +
            $"Password={ConfigurationManager.AppSettings["password"]};Database={ConfigurationManager.AppSettings["database"]};Port={ConfigurationManager.AppSettings["port"]}";


        public void insertStatements(string tableName, List<IInsert> inserts) {
            foreach (Insert insert in inserts) {
                insertStatement(tableName, insert);
            }
        }
        public void insertStatement(string tableName, IInsert insert)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string statement = "INSERT INTO " + tableName + " (";
                foreach (IColumn col in insert.data.Keys)
                {
                    if (col.serial) {
                        continue;
                    }
                    statement += col.name;
                    statement += ", ";
                }
                if (statement.EndsWith(", "))
                {
                    statement = statement.Substring(0, statement.Length - 2);
                }
                statement += ") VALUES (";
                foreach (IColumn col in insert.data.Keys)
                {
                    if (col.serial)
                    {
                        continue;
                    }
                    statement += "@" + col.name;
                    statement += ", ";
                }
                if (statement.EndsWith(", "))
                {
                    statement = statement.Substring(0, statement.Length - 2);
                }
                statement += ")";
                NpgsqlCommand cmd = new NpgsqlCommand(statement, conn);
                foreach (IColumn col in insert.data.Keys)
                {
                    if (col.serial)
                    {
                        continue;
                    }
                    if (col.type != NpgsqlDbType.Integer)
                    {
                        cmd.Parameters.AddWithValue(col.name, col.type, insert.data[col] ?? DBNull.Value);
                    }
                    else { 
                        cmd.Parameters.AddWithValue(col.name, col.type, insert.data[col] ?? 0);
                    }
                }
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

        }

        public List<string> tableColumns(string name)
        {
            string statement = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'" + name + "'";
            List<string> columns = new List<string>();
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(statement, conn);
                NpgsqlDataReader reader = npgsqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    columns.Add(reader.GetString(0));
                }
                conn.Close();
            }
            return columns;
        }

        public void alterTable(List<string> colNames, ITable table) {
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                foreach (IColumn col in table.columnMap.MapMI.Values) {
                    if (!colNames.Contains(col.name))
                    {
                        if (col.hidden)
                        {
                            continue;
                        }
                        string statement = "ALTER TABLE " + table.name + " ADD COLUMN ";
                        statement += col.name;
                        if (col.serial)
                        {
                            statement += " serial primary key";
                        }
                        else
                        {
                            statement += " " + col.type.ToString();
                        }
                        statement += ";";
                        NpgsqlCommand npgsqlCommand = new NpgsqlCommand(statement, conn);
                        npgsqlCommand.ExecuteNonQuery();
                    }
                }
                foreach (string name in colNames)
                {
                    if (table.columnMap.MapMI.Values.AsQueryable().Where(s=>s.name==name).Count()==0)
                    {
                        string statement = "ALTER TABLE " + table.name + " DROP COLUMN "+name+"; ";
                        NpgsqlCommand npgsqlCommand = new NpgsqlCommand(statement, conn);
                        npgsqlCommand.ExecuteNonQuery();
                    }
                }
                conn.Close();
            }
        }
        public void createStatements(ITable table)
        {
            List<string> colNames = tableColumns(table.name);
            if (colNames.Count > 0 ) {
                alterTable(colNames, table);
            }
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string statement = "CREATE TABLE IF NOT EXISTS " + table.name + " (";
                foreach (IColumn column in table.columnMap.MapMI.Values)
                {
                    if (column.hidden)
                    {
                        continue;
                    }

                    statement += column.name;
                    if (column.serial)
                    {
                        statement += " serial primary key";
                    }
                    else { 
                        statement += " " + column.type.ToString();
                    }
                    statement += ", ";
                }
                if (statement.EndsWith(", "))
                {
                    statement = statement.Substring(0, statement.Length - 2);
                }
                statement += ");";
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(statement, conn);
                npgsqlCommand.ExecuteNonQuery();
                conn.Close();
            }

        }
        public void deleteStatement(ITable table, string whereStatement)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string statement = "DELETE FROM " + table.name + " " + whereStatement + ";";
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(statement, conn);
                npgsqlCommand.ExecuteNonQuery();
                conn.Close();
            }

        }



        public List<T> selectStatement<T>(ITable table, string whereStatement)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string statement = "SELECT ";
                foreach (IColumn col in table.columnMap.MapMI.Values)
                {
                    if (!col.hidden)
                    {
                        statement += col.name;
                        statement += ", ";
                    }
                }
                if (statement.EndsWith(", "))
                {
                    statement = statement.Substring(0, statement.Length - 2);
                }
                statement += " FROM " + table.name;
                if (whereStatement != null) {
                    statement += " " + whereStatement;
                }
                statement += ";";
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(statement, conn);
                NpgsqlDataReader reader = npgsqlCommand.ExecuteReader();

                List<T> fromDB = new List<T>();
                while (reader.Read())
                {
                    T newT = Activator.CreateInstance<T>();
                    int i = 0;
                    foreach (MemberInfo info in table.columnMap.MapMI.Keys)
                    {
                        IColumn col = table.columnMap.MapMI[info];
                        if (col.dependencyTable == null)
                        {
                            object val = reader.GetValue(i);
                            if(val == DBNull.Value) {
                                Reflection.setValue(info, newT, null);
                            } else {
                                Reflection.setValue(info, newT, val);
                            }
                        }
                        //TODO dependency
                        i++;
                    }
                    fromDB.Add(newT);
                }
                conn.Close();
                return fromDB;
            }
        }
    }
}
