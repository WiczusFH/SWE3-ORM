using NUnit.Framework;
using ORM.Attributes;
using ORM.Interfaces;
using ORM.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Test
{

    public class Author
    {
        [PrimaryKey]
        public int AuthorId;
        [ForeignKey("lastName")]
        public Person person;
        [CascaseDelete]
        public List<Book> books = new List<Book>();
        public Author() { }
        public Author(Person person) { this.person = person; }
    }
    public class Person
    {
        public string firstName;
        public string lastName;
        public Person() { }
        public Person(string firstName, string lastName) { this.firstName = firstName; this.lastName = lastName; }

    }
    public class Book
    {
        [UniqueKey]
        public int bookId;
        [ColumnName("title")]
        public string name;
        public Book() { }
        public Book(int id, string name) { this.bookId = id; this.name = name; }
    }
    class CRUDMock : ICRUD
    {
        public void createStatements(ITable table)
        {
        }

        public void deleteStatement(ITable table, string whereStatement)
        {
        }

        public void insertStatement(string tableName, IInsert insert)
        {
        }

        public void insertStatements(string tableName, List<IInsert> insert)
        {
        }

        public List<T> selectStatement<T>(ITable table, string whereStatement)
        {
            throw new NotImplementedException();
        }
    }
    public class Repository : Context
    {
        public DBSet<Book> books = new DBSet<Book>();
        public DBSet<Author> authors = new DBSet<Author>();
    }
    public class ContextTest
    {
        [Test]
        public void integrationTest()
        {
            Repository rep = new Repository();
            rep.crud = new CRUDMock();
            rep.build();
            
            //Create some data
            Person person1 = new Person("John", "Tolkien");
            Author author1 = new Author(person1);
            Book book1 = new Book(0, "Lord of the rings");
            Book book2 = new Book(1, "Hobbit");
            author1.books.Add(book1);
            author1.books.Add(book2);

            Person person2 = new Person("George", "Martin");
            Author author2 = new Author(person2);
            Book book3 = new Book(2, "Song of Ice and Fire");
            author2.books.Add(book3);

            rep.authors.Add(author1);
            rep.authors.Add(author2);
            rep.books.Add(book1);

            //Insert all DBsets
            rep.insert();

            Assert.AreEqual(rep.tableOrder.Count(), 4);
            Assert.AreEqual(rep.tableOrder[0].name,"Book");
            Assert.AreEqual(rep.tableOrder[1].name,"Person");
            Assert.AreEqual(rep.tableOrder[2].name,"Author");
            Assert.AreEqual(rep.tableOrder[3].name,"RT_Book_bookId_Author_AuthorId");

            Assert.AreEqual(rep.tableOrder[0].outstandingInserts.Count, 3);
            Assert.AreEqual(rep.tableOrder[1].outstandingInserts.Count, 2);
            Assert.AreEqual(rep.tableOrder[2].outstandingInserts.Count, 2);
            Assert.AreEqual(rep.tableOrder[3].outstandingInserts.Count, 3);
        }
    }
}
