using System;
using System.Reflection;

namespace uScoober.TestFramework.Core
{
    internal class Theory : ITestCase
    {
        private readonly ConstructorInfo _constructor;
        private readonly string _name;
        private readonly MethodInfo _theory;
        private readonly object _theoryArgs;
        private object _instance;

        public Theory(string typeName, ConstructorInfo constructor, MethodInfo theory, object args) {
            _name = typeName + "\n" + theory.Name.Substring(0, theory.Name.Length - 7) + "\n" + args;
            _constructor = constructor;
            _theory = theory;
            _theoryArgs = args;
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
            _theory.Invoke(_instance,
                           new[] {
                               _theoryArgs
                           });
        }
    }
}