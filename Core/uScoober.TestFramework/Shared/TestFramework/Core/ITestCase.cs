namespace uScoober.TestFramework.Core
{
    public interface ITestCase {
        string Name { get; }

        void RunSetup();

        void RunTeardown();

        void RunTest();
    }
}