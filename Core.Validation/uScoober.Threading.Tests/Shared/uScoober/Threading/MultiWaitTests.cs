//using System;
//using uScoober.TestFramework;
//using uScoober.TestFramework.Assert;

//namespace uScoober.Threading
//{
//    public class MultiWaitTests
//    {
//        public void WaitAllOnMultipleComplete_Fact() {
//            Task.WaitAll(Task.Completed(), Task.Completed(true), Task.Completed(42));
//        }

//        public void WaitAllOnMultipleNotStarted_Fact() {
//            bool first = false;
//            bool second = false;
//            Task.WaitAll(new ActionTask(() => first = true), new ActionTask(() => second = true));
//            first.ShouldBeTrue();
//            second.ShouldBeTrue();
//        }

//        public void WaitAllOnMultipleStarted_Fact() { }

//        public void WaitAllOnNoTasksThrows_Fact() {
//            var ex = Trap.Exception(() => Task.WaitAll(null)) as ArgumentNullException;
//            ex.ShouldNotBeNull();
//        }

//        public void WaitAllOnNullTasksThrows_Fact() {
//            var ex = Trap.Exception(() => Task.WaitAll(null, null)) as ArgumentNullException;
//            ex.ShouldNotBeNull();
//        }

//        public void WaitAllOnOneCompleted_Fact() { }

//        public void WaitAllOnOneNotStarted_Fact() { }

//        public void WaitAllOnOneStarted_Fact() { }

//        public void WaitAllOnUnstartedContinuation_Fact() {
//            bool pass = false;
//            var task = new ActionTask(() => { }).ContinueWith(previous => { pass = true; });
//            Task.WaitAll(task);
//            pass.ShouldBeTrue();
//        }

//        public void WaitAnyOnMultipleComplete_Fact() { }

//        public void WaitAnyOnMultipleNotStarted_Fact() { }

//        public void WaitAnyOnMultipleStarted_Fact() { }

//        public void WaitAnyOnNoneThrows_Fact() { }

//        public void WaitAnyOnOneCompleted_Fact() { }

//        public void WaitAnyOnOneNotStarted_Fact() { }

//        public void WaitAnyOnOneStarted_Fact() { }
//    }
//}

