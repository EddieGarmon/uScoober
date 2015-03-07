using System;

namespace uScoober.TestFramework
{
    public class FailureTests
    {
        public void FactName_Fact() {
            throw new Exception("expected failure!");
        }
    }
}