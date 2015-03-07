using System.Collections;
using uScoober.DataStructures;
using uScoober.TestFramework.Assert;

namespace uScoober.DataStructures
{
    public class ListTests
    {
        private List _list;

        public void AddItemRange_Fact() {
            _list = new List();
            _list.AddRange("hello", "micro", "world");
            _list.ShouldHaveState(3, 3, "hello", "micro", "world");
        }

        public void AddItem_Fact() {
            _list = new List();
            _list.Add("hello");
            _list.ShouldHaveState(1, 1, "hello");
            _list.Add("micro");
            _list.ShouldHaveState(2, 2, "hello", "micro");
            _list.Add(4);
            _list.ShouldHaveState(3, 3, "hello", "micro", 4);
        }

        public void ContainsItem_Fact() {
            _list = new List(1, 2, 3);
            _list.Contains(2)
                 .ShouldBeTrue();
            _list.Contains(42)
                 .ShouldBeFalse();
        }

        public void IndexItem_Fact() {
            _list = new List(1, 2, 3);
            _list[1].ShouldEqual(2);
            _list[1] = 100;
            _list.ShouldHaveState(3, 1, 1, 100, 3);
        }

        public void IndexOfItem_Fact() {
            _list = new List(1, 2, 3, 'a');
            _list.IndexOf(0)
                 .ShouldEqual(-1);
            _list.IndexOf(2)
                 .ShouldEqual(1);
            _list.IndexOf('a')
                 .ShouldEqual(3);
        }

        public void InsertItemRange_Fact() {
            _list = new List();
            _list.InsertRange(0, "world");
            _list.ShouldHaveState(1, 1, "world");
            _list.InsertRange(0, "hello");
            _list.ShouldHaveState(2, 2, "hello", "world");
            _list.InsertRange(1, "micro");
            _list.ShouldHaveState(3, 3, "hello", "micro", "world");
            _list.InsertRange(0, "super", "awesome", "fun", "time");
            _list.ShouldHaveState(7, 7, "super", "awesome", "fun", "time", "hello", "micro", "world");
        }

        public void InsertItem_Fact() {
            _list = new List();
            _list.Insert(0, "hello");
            _list.ShouldHaveState(1, 1, "hello");
            _list.Insert(1, "world");
            _list.ShouldHaveState(2, 2, "hello", "world");
            _list.Insert(1, "micro");
            _list.ShouldHaveState(3, 3, "hello", "micro", "world");
            _list.Insert(0, "awesome:");
            _list.ShouldHaveState(4, 4, "awesome:", "hello", "micro", "world");
        }

        public void MoveItems_Fact() {
            _list = new List("2", "3", "1");
            _list.Move(2, 0);
            _list.ShouldHaveState(3, 1, "1", "2", "3");
        }

        public void NewEmpty_Fact() {
            _list = new List();
            _list.ShouldHaveState(0, 0);
        }

        public void NewFromEnumerable_Fact() {
            _list = new List('a', "b", 3);
            _list.ShouldHaveState(3, 0, 'a', "b", 3);
        }

        public void RemoveItems_Fact() {
            _list = new List('a', "b", "c", 1, 2.3);
            _list.Remove("c");
            _list.ShouldHaveState(4, 1, 'a', "b", 1, 2.3);
            _list.Remove(1);
            _list.ShouldHaveState(3, 2, 'a', "b", 2.3);
            _list.RemoveAt(1);
            _list.ShouldHaveState(2, 3, 'a', 2.3);
        }

        public void SwapItems_Fact() {
            _list = new List("1", "2", "3", "4", "5");
            _list.Swap(1, 3);
            _list.ShouldHaveState(5, 1, "1", "4", "3", "2", "5");
        }
    }
}

namespace uScoober.TestFramework.Assert
{
    internal static partial class AssertExtensions
    {
        public static void ShouldHaveState(this List list, int count, int editVersion, params object[] items) {
            list.Count.ShouldEqual(count);
            list.EditVersion.ShouldEqual(editVersion);
            if (items.Length > 0) {
                list.ShouldEnumerateEqual((IEnumerable)items);
            }
        }
    }
}