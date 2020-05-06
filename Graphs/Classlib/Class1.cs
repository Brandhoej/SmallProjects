using Graphs.Bla;

namespace Graphs {
    public class Class1 {
        public Class2 Test {
            get; private set;
        }

        public Class1(Class2 test) {
            Test = test;
        }
    }
}

namespace Graphs.Bla {
    public class Class2 {
        public string Val {
            get; private set;
        }

        public Class2(string test) {
            Val = test;
        }
    }
}

namespace Graphs.FooBar {
    public class Iron {
        public bool test => true;
    }
}