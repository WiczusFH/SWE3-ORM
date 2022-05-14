using ORM.DTO;
using ORM.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Showcase
{
    public class Repository : Context
    {
        public DBSet<Book> books = new DBSet<Book>();
        public DBSet<Author> authors = new DBSet<Author>();
    }
}
