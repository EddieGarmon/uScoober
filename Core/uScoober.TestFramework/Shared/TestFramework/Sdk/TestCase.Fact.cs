using System;
using System.Reflection;
using uScoober.Threading;

namespace uScoober.TestFramework.Sdk
{
    internal partial class TestCase
    {
        internal class Fact : TestCase
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

            public override string Name {
                get { return _name; }
            }

            protected override void RunSetup() {
                _instance = _constructor.Invoke(EmptyObjects);
            }

            protected override void RunTeardown() {
                var disposable = _instance as IDisposable;
                if (disposable != null) {
                    disposable.Dispose();
                }
            }

            protected override void RunTest() {
                if (_fact.ReturnType == typeof(void)) {
                    _fact.Invoke(_instance, EmptyObjects);
                }
                else {
                    var task = (Task)_fact.Invoke(_instance, EmptyObjects);
                    task.Wait();
                }
            }
        }
    }
}