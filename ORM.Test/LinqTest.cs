using NUnit.Framework;
using ORM.DTO;
using ORM.Postgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Test
{
    public class LinqTest
    {
        class Book{
            public string title;
        }
        [Test]
        public void test1() {


            TableMap tableMap = new TableMap();

            Table book = new Table();
            book.name = "Book";
            book.columnMap = new ColumnMap();
            Column title = new Column();
            title.name = "title";
            book.columnMap.addLink(typeof(Book).GetMember("title")[0],title);

            tableMap.addTableLink(typeof(Book),book);

            Expression<Func<Book, bool>> isFantasy = s => (s.title == "Lord of the Rings") || (s.title == "Game of Thrones");
            LinqQueryBuilder builder = new LinqQueryBuilder(isFantasy,tableMap);

            Assert.AreEqual("WHERE Book.title = 'Lord of the Rings' OR Book.title = 'Game of Thrones'", builder.whereQuery);
        }
        [Test]
        public void test2()
        {


            TableMap tableMap = new TableMap();

            Table book = new Table();
            book.name = "Book";
            book.columnMap = new ColumnMap();
            Column title = new Column();
            title.name = "title";
            book.columnMap.addLink(typeof(Book).GetMember("title")[0], title);

            tableMap.addTableLink(typeof(Book), book);

            Expression<Func<Book, bool>> isFantasy = s => (s.title.Length == 10);
            LinqQueryBuilder builder = new LinqQueryBuilder(isFantasy, tableMap);

            Assert.AreEqual("WHERE length(Book.title) = 10", builder.whereQuery);
        }

    }
}
