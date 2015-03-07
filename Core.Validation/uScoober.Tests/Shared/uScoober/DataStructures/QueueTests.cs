using uScoober.TestFramework.Assert;

namespace uScoober.DataStructures
{
    public class QueueTests
    {
        private Queue _queue;

        public void ConstructWithItems_Fact() {
            _queue = new Queue("first", "second");
            _queue.IsEmpty.ShouldBeFalse();
            _queue.Count.ShouldEqual(2);
            _queue.ShouldEnumerateEqual("first", "second");
        }

        public void Construct_Fact() {
            _queue = new Queue();
            _queue.IsEmpty.ShouldBeTrue();
            _queue.Count.ShouldEqual(0);
            _queue.ShouldEnumerateEqual(new object[0]);
        }

        public void Dequeue_Fact() {
            _queue = new Queue("a", "b", "c");
            _queue.Peek(0)
                  .ToString()
                  .ShouldEqual("a");
            var value = (string)_queue.Dequeue();
            value.ShouldEqual("a");
            _queue.Count.ShouldEqual(2);
            _queue.ShouldEnumerateEqual("b", "c");
            _queue.Peek(0)
                  .ToString()
                  .ShouldEqual("b");
            value = (string)_queue.Dequeue();
            value.ShouldEqual("b");
            _queue.Count.ShouldEqual(1);
            _queue.ShouldEnumerateEqual("c");
            _queue.Peek(0)
                  .ToString()
                  .ShouldEqual("c");
            value = (string)_queue.Dequeue();
            value.ShouldEqual("c");
            _queue.Count.ShouldEqual(0);
            _queue.IsEmpty.ShouldBeTrue();
            _queue.ShouldEnumerateEqual(new object[0]);
        }

        public void Enqueue_Fact() {
            _queue = new Queue();
            _queue.Enqueue("Hyzer");
            _queue.IsEmpty.ShouldBeFalse();
            _queue.Count.ShouldEqual(1);
            _queue.Peek(0)
                  .ToString()
                  .ShouldEqual("Hyzer");
            _queue.ShouldEnumerateEqual("Hyzer");
            _queue.Enqueue("Thumber");
            _queue.Count.ShouldEqual(2);
            _queue.Peek(0)
                  .ToString()
                  .ShouldEqual("Hyzer");
            _queue.Peek(1)
                  .ToString()
                  .ShouldEqual("Thumber");
            _queue.ShouldEnumerateEqual("Hyzer", "Thumber");
        }
    }
}