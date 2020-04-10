using Microsoft.VisualStudio.TestTools.UnitTesting;
using Graphs;
using Graphs.Bla;
using Graphs.FooBar;

namespace Tests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            Class1 test = new Class1(new Class2("FooBar"));
            Assert.AreEqual(test.Test.Val, "FooBar");
            Assert.AreEqual(true, true);

            Iron iron = new Iron();
            Assert.AreEqual(iron.test, true);
        }
    }
}
