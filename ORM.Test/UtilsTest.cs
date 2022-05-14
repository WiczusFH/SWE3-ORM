using NUnit.Framework;
using ORM.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Test
{
    public class UtilsTest
    {
        public class Book {
            public string name;
            public List<int> someList;
            public int[] someArray;
        }
        [Test]
        public void GetValueTest() {
            Book newBook = new Book();
            newBook.name = "test";
            MemberInfo info = typeof(Book).GetMember("name")[0];

            Assert.AreEqual(Reflection.GetValue(info,newBook),newBook.name);
        }
        [Test]
        public void getTypeFromMember()
        {
            Book newBook = new Book();
            newBook.name = "test";
            MemberInfo info = typeof(Book).GetMember("name")[0];

            Assert.AreEqual(typeof(string), Reflection.getTypeFromMember(info));
        }
        [Test]
        public void setValueTest()
        {
            Book newBook = new Book();
            MemberInfo info = typeof(Book).GetMember("name")[0];
            Reflection.setValue(info, newBook, "abcd");
            Assert.AreEqual("abcd",newBook.name);
        }
        [Test]
        public void isListTest()
        {
            Book newBook = new Book();
            MemberInfo info1 = typeof(Book).GetMember("name")[0];
            MemberInfo info2 = typeof(Book).GetMember("someList")[0];
            MemberInfo info3 = typeof(Book).GetMember("someArray")[0];

            Assert.False(Reflection.isList(info1));
            Assert.True(Reflection.isList(info2));
            Assert.True(Reflection.isList(info3));
        }
        [Test]
        public void isPrimitiveTest()
        {
            Book newBook = new Book();
            MemberInfo info1 = typeof(Book).GetMember("name")[0];
            MemberInfo info2 = typeof(Book).GetMember("someList")[0];
            MemberInfo info3 = typeof(Book).GetMember("someArray")[0];

            Assert.True(Reflection.isPrimitive(info1));
            Assert.False(Reflection.isPrimitive(info2));
            Assert.False(Reflection.isPrimitive(info3));
        }
    }
}
