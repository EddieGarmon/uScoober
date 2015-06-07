using System;
using System.Collections;
using System.Reflection;
using Microsoft.SPOT;
using uScoober.Extensions;
using uScoober.Threading;

namespace uScoober.TestFramework.Core
{
    // [DebuggerStepThrough]
    public class TestRunner
    {
        private readonly Assembly _assemblyUnderTest;
        private readonly IRunnerResultProcessor _output;
        private TestRunResult _currentRun;
        private ActionTask _workload;

        public TestRunner(Assembly assemblyUnderTest, IRunnerUserInput input, IRunnerResultProcessor output) {
            _assemblyUnderTest = assemblyUnderTest;
            _output = output;

            //hook input events if defined
            if (input != null) {
                if (input.StartTests != null) {
                    input.StartTests.OnInterrupt += (source, state, time) => {
                                                        Debug.Print("Attempt Restart");
                                                        ExecuteTests();
                                                    };
                }
            }
        }

        public ActionTask ExecuteTests() {
            return _workload ?? (_workload = Task.Run(() => {
                                                          if (_currentRun != null) {
                                                              _currentRun.Dispose();
                                                          }
                                                          Debug.GC(true);
                                                          _currentRun = new TestRunResult();
                                                          _output.TestsStarting(_currentRun);
                                                          FindAndRunTests();
                                                          _currentRun.TestingComplete();
                                                          _output.TestsCompleted(_currentRun);
                                                          Debug.GC(false);
                                                          _workload = null;
                                                      }));
        }

        private void FindAndRunTests() {
            var random = new Random();

            Type[] types = _assemblyUnderTest.GetTypes();
            for (int typeIndex = 0; typeIndex < types.Length; typeIndex++) {
                // NB: randomize run order
                int typeOffsetMax = types.Length - typeIndex - 1;
                int typeOffset = typeOffsetMax > 0 ? random.Next(typeOffsetMax) : 0;
                Type type = types[typeIndex + typeOffset];
                types[typeIndex + typeOffset] = types[typeIndex];
                types[typeIndex] = type;

                // NB: the convention is:
                //      all classes ending with "Test(s)" or "Spec(s)" 
                //      and they must have a default constructor
                string typeName = type.Name;
                if (!typeName.EndsWithAny("Test", "Tests", "Spec", "Specs")) {
                    continue;
                }
                ConstructorInfo constructor = type.GetConstructor(new Type[0]);
                if (constructor == null) {
                    continue;
                }

                MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                for (int methodIndex = 0; methodIndex < methods.Length; methodIndex++) {
                    // NB: randomize run order
                    int methodOffsetMax = methods.Length - methodIndex - 1;
                    int methodOffset = methodOffsetMax > 0 ? random.Next(methodOffsetMax) : 0;
                    MethodInfo method = methods[methodIndex + methodOffset];
                    methods[methodIndex + methodOffset] = methods[methodIndex];
                    methods[methodIndex] = method;
                    string methodName = method.Name;

                    // NB: ALL facts and theories must return void for synchronous execution 
                    if (method.ReturnType != typeof(void)) {
                        // or Task for async execution
                        if (!method.ReturnType.IsSubclassOf(typeof(Task))) {
                            continue;
                        }
                    }

                    // Find and run all Facts
                    if ((methodName.Length > 5) && (methodName.EndsWith("_Fact"))) {
                        var test = new Fact(typeName, constructor, method);
                        RunAndRecordTestCase(test);
                    }

                    // Find and run all Theories
                    if ((methodName.Length > 7) && (methodName.EndsWith("_Theory"))) {
                        //check for theory data provider method by name
                        string theoryDataName = methodName.Substring(0, methodName.Length - 7) + "_Data";
                        for (int theoryDataIndex = 0; theoryDataIndex < methods.Length; theoryDataIndex++) {
                            if (methods[theoryDataIndex].Name == theoryDataName) {
                                object argsInstance = constructor.Invoke(new object[0]);
                                var theoryData = (IEnumerable)methods[theoryDataIndex].Invoke(argsInstance, new object[0]);
                                foreach (object theoryArgs in theoryData) {
                                    var theory = new Theory(typeName, constructor, method, theoryArgs);
                                    RunAndRecordTestCase(theory);
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void RunAndRecordTestCase(ITestCase testCase) {
            _output.TestCaseStarting(testCase.Name);
            var result = new TestCaseResult(testCase.Name);
            Stopwatch stopwatch = Stopwatch.StartNew();
            try {
                testCase.RunSetup();
                result.DurationOfSetup = stopwatch.Split();
                testCase.RunTest();
                result.Passed = true;
                result.DurationOfTest = stopwatch.Split();
            }
            catch (Exception ex) {
                result.ExceptionMessage = ex.ToString();
                result.Passed = false;
                result.DurationOfTest = stopwatch.Split();
            }
            finally {
                testCase.RunTeardown();
                result.DurationOfTeardown = stopwatch.Split();
            }
            _currentRun.Record(result);
            _output.TestCaseCompleted(result);
        }
    }
}