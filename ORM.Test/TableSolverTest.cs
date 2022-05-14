using NpgsqlTypes;
using NUnit.Framework;
using ORM.DTO;
using ORM.Solver;
using OZCore.Attributes;

namespace ORM.Test
{
    public class TableSolverTest
    {
        [TableName("A_Class")]
        public class TestClass{
            public int id;
            public string nameN;
            public NestedTestClass nestedClass;
        }
        public class NestedTestClass {
            public int id;
            public string nameN;
        }


        [Test]
        public void Test1()
        {
            TableSolver solver = new TableSolver();
            solver.solveForTable(typeof(TestClass));

            TableMap map = solver.map;

            Assert.AreEqual("A_Class",map.MapType[typeof(TestClass)].name);
            Assert.AreEqual("id",map.MapType[typeof(TestClass)].columnMap.getColumn(typeof(TestClass).GetMember("id")[0]).name);
            Assert.AreEqual(NpgsqlDbType.Integer, map.MapType[typeof(TestClass)].columnMap.getColumn(typeof(TestClass).GetMember("id")[0]).type);
            Assert.AreEqual("NestedTestClass",map.MapType[typeof(NestedTestClass)].name);
        }
    }
}