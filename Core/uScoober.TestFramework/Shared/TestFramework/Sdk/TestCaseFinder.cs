using System;
using System.Collections;
using System.Reflection;
using uScoober.Extensions;
using uScoober.Threading;

namespace uScoober.TestFramework.Sdk
{
    internal class TestCaseFinder
    {
        public static void FindAndRunTestCases(Assembly assemblyUnderTest, TestRun testRun) {
            var random = new Random();

            var types = assemblyUnderTest.GetTypes();
            for (int typeIndex = 0; typeIndex < types.Length; typeIndex++) {
                // NB: randomize run order
                int typeOffsetMax = types.Length - typeIndex - 1;
                int typeOffset = typeOffsetMax > 0 ? random.Next(typeOffsetMax) : 0;
                var type = types[typeIndex + typeOffset];
                types[typeIndex + typeOffset] = types[typeIndex];
                types[typeIndex] = type;

                // NB: the convention is:
                //      all classes ending with "Test(s)" or "Spec(s)" 
                //      and they must have a default constructor
                string typeName = type.Name;
                if (!typeName.EndsWithAny("Test", "Tests", "Spec", "Specs")) {
                    continue;
                }
                var constructor = type.GetConstructor(new Type[0]);
                if (constructor == null) {
                    continue;
                }

                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                for (int methodIndex = 0; methodIndex < methods.Length; methodIndex++) {
                    // NB: randomize run order
                    int methodOffsetMax = methods.Length - methodIndex - 1;
                    int methodOffset = methodOffsetMax > 0 ? random.Next(methodOffsetMax) : 0;
                    var method = methods[methodIndex + methodOffset];
                    methods[methodIndex + methodOffset] = methods[methodIndex];
                    methods[methodIndex] = method;
                    var methodName = method.Name;

                    // NB: ALL facts and theories must return void for synchronous execution or Task for async execution
                    if (method.ReturnType != typeof(void) && method.ReturnType.IsSubclassOf(typeof(Task))) {
                        continue;
                    }

                    // Find and run all Facts
                    if ((methodName.Length > 5) && (methodName.EndsWith("_Fact"))) {
                        var test = new TestCase.Fact(typeName, constructor, method);
                        testRun.RunAndRecord(test);
                    }

                    // Find and run all Theories
                    if ((methodName.Length > 7) && (methodName.EndsWith("_Theory"))) {
                        //check for theory data provider method by name
                        var theoryDataName = methodName.Substring(0, methodName.Length - 7) + "_Data";
                        for (int theoryDataIndex = 0; theoryDataIndex < methods.Length; theoryDataIndex++) {
                            if (methods[theoryDataIndex].Name == theoryDataName) {
                                var argsInstance = constructor.Invoke(new object[0]);
                                var theoryData = (IEnumerable)methods[theoryDataIndex].Invoke(argsInstance, new object[0]);
                                foreach (var theoryArgs in theoryData) {
                                    var theory = new TestCase.Theory(typeName, constructor, method, theoryArgs);
                                    testRun.RunAndRecord(theory);
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}