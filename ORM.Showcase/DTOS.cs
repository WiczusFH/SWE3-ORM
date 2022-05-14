using ORM.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Showcase
{
    public class Author {
        [PrimaryKey]
        public int AuthorId;
        [ForeignKey("lastName")]
        public Person person;
        [CascaseDelete]
        public List<Book> books = new List<Book>();
        public Author() { }
        public Author(Person person) { this.person = person; }
    }
    public class Person{
        public string firstName;
        public string lastName;
        public Person() { }
        public Person(string firstName, string lastName) { this.firstName = firstName; this.lastName = lastName;}

    }
    public class Book {
        [UniqueKey]
        public int bookId;
        [ColumnName("title")]
        public string name;
        public Book() { }
        public Book(int id,string name) { this.bookId = id; this.name = name; }
    }
}
