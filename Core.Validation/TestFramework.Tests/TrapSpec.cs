using System;
using uScoober.TestFramework.Assert;

namespace uScoober.TestFramework
{
    public class TrapSpec
    {
        public void TrapException_Fact() {
            Exception exception = Trap.Exception(() => { throw new Exception("failure will be caught!"); });
            exception.ShouldNotBeNull();
        }
    }
}