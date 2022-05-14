using NUnit.Framework;
using ORM.DTO;
using ORM.Interfaces;
using ORM.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Test
{
    public class OrderSolverTest
    {
        public class Book{
            public int id;
        }
        public class Person { 
            public int id;
        }
        public class Author {
            public int id;
        }

        [Test]
        public void test1 (){
            TableMap map = new TableMap();
            Table table3 = new Table();
            Table table2 = new Table();
            Table table1 = new Table();
            map.addTableLink(typeof(Book), table1);
            map.addTableLink(typeof(Person), table2);
            map.addTableLink(typeof(Author), table3);

            MemberInfo info = typeof(Book).GetMember("id")[0];
            Column column3 = new Column();
            column3.dependencyTable = table2;
            table3.columnMap.addLink(info, column3);
            Column column2 = new Column();
            column2.dependencyTable = table1;
            table2.columnMap.addLink(info, column2);


            OrderSolver orderSolver = new OrderSolver();
            orderSolver.solve(map);
            List<ITable> tables = orderSolver.tablesOrdered;
            Assert.AreEqual(tables[0], table1);
            Assert.AreEqual(tables[1], table2);
            Assert.AreEqual(tables[2], table3);
        }
    }
}
