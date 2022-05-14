using ORM.DTO;
using ORM.Repository;
using ORM.Solver;
using ORM.Util;
using OZCore.Attributes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ORM.Showcase
{

    class Program
    {
        static void Main(string[] args)
        {
            //Build Repository and data model
            Repository rep = new Repository();
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

            Expression<Func<Book, bool>> isHobbit = s => s.name == "Hobbit";
            Console.WriteLine(rep.books.get(isHobbit)[0].name);

            //Read from List
            Expression<Func<Book, bool>> isLong = s => s.name.Length > 5;
            Console.WriteLine(rep.books.get(isLong)[0].name);
            rep.books.Clear();
            //Read from Cache
            Console.WriteLine(rep.books.get(isLong)[0].name);
            rep.books.clearCache();
            //Read From DB
            Console.WriteLine(rep.books.get(isLong)[0].name);

            Expression<Func<Book, bool>> allBooks = s => true;
            rep.books.delete(allBooks);
            Expression<Func<Author, bool>> allAuthors = s => true;
            rep.authors.delete(allAuthors);

        }
    }
}
