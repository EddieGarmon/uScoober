using System.Collections;

namespace uScoober.TestFramework.Sdk
{
    public class ExecuteTheoriesSpec
    {
        public IEnumerable Synchronous_Data() {
            return new[] {
                new object()
            };
        }

        public void Synchronous_Theory(object data) { }
    }
}