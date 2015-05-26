using System;
using System.Reflection;
using uScoober.Threading;

namespace uScoober.TestFramework.Core
{
    internal class Fact : ITestCase
    {
        private readonly ConstructorInfo _constructor;
        private readonly MethodInfo _fact;
        private readonly string _name;
        private object _instance;

        public Fact(string typeName, ConstructorInfo constructor, MethodInfo fact) {
            _name = typeName + "\n" + fact.Name.Substring(0, fact.Name.Length - 5);
            _constructor = constructor;
            _fact = fact;
        }

        public string Name {
            get { return _name; }
        }

        public void RunSetup() {
            _instance = _constructor.Invoke(Empty.Objects);
        }

        public void RunTeardown() {
            var disposable = _instance as IDisposable;
            if (disposable != null) {
                disposable.Dispose();
            }
        }

        public void RunTest() {
            if (_fact.ReturnType == typeof(void)) {
                _fact.Invoke(_instance, Empty.Objects);
            }
            else {
                var task = (Task)_fact.Invoke(_instance, Empty.Objects);
                task.Wait();
            }
        }
    }
}