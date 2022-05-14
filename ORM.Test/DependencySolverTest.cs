using NUnit.Framework;
using ORM.Attributes;
using ORM.DTO;
using ORM.Interfaces;
using ORM.Repository;
using ORM.Showcase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Test
{
    public class DependencySolverTest
    {
        public class Author
        {
            public string name;
            [ForeignKey("title")]
            public List<Book> books;
        }
        public class Book
        {
            [ForeignKey("name")]
            public Author author;
            public string title;
            public Book()
            {

            }
            public Book(string title)
            {
                this.title = title;
            }
        }
        public class Repository : Context
        {
            public DBSet<Book> books = new DBSet<Book>();
            public DBSet<Author> authors = new DBSet<Author>();
        }
        public class CRUDMock : ICRUD
        {

            public void createStatements(ITable table)
            {
            }

            public void deleteStatement(ITable table, string whereStatement)
            {
            }

            public void insertStatement(string tableName, List<IParam> param)
            {
            }

            public void insertStatement(string tableName, IInsert insert)
            {
            }

            public void insertStatements(string tableName, List<IInsert> insert)
            {
            }

            public void selectStatement(string statement)
            {
            }

            public void selectStatement(ITable table, string whereStatement)
            {
            }

            public List<T> selectStatement<T>(ITable table, string whereStatement)
            {
                return new List<T>();
            }
        }

        [Test]
        public void test1() {
            Repository rep = new Repository();
            rep.crud = new CRUDMock();
            rep.books.Add(new Book("hello"));
            rep.build();
            ITableMap _TableMap = rep._TableMap;

            Assert.AreEqual(2, _TableMap.MapType.Count);
            Assert.AreEqual(1, _TableMap.MapMI.Count);
            Assert.AreEqual(2, _TableMap.getTable(typeof(Book)).columnMap.MapMI.Count);
            Assert.AreEqual("Author", _TableMap.getTable(typeof(Author)).name);
            Assert.AreEqual(true, _TableMap.getTable(typeof(Author)).columnMap.getColumn(typeof(Author).GetMember("books")[0]).hidden);

            Assert.AreEqual(2, _TableMap.getTable(typeof(Author).GetMember("books")[0]).columnMap.MapMI.Count);
        }

    }
}
