using System.Collections;
using uScoober.DataStructures.Typed;
using uScoober.TestFramework.Assert;

namespace uScoober.DataStructures.Typed
{
    public class StringListTests
    {
        public void AddItem_Fact() {
            var list = new StringList();
            list.ShouldHaveState(0, 0);
            list.Add("Hello");
            list.ShouldHaveState(1, 1, "Hello");
            list.Add("Good-bye");
            list.ShouldHaveState(2, 2, "Hello", "Good-bye");
        }

        public void AddItemRange_Fact() {
            var list = new StringList();
            list.ShouldHaveState(0, 0);
            list.AddRange("Hello", "Good-bye");
            list.ShouldHaveState(2, 2, "Hello", "Good-bye");
        }

        public void ContainsHit_Fact() {
            var list = new StringList("aaa", "bbb");
            list.Contains("aaa")
                .ShouldBeTrue();
        }

        public void ContainsMiss_Fact() {
            var list = new StringList("aaa", "bbb", "ccc");
            list.Contains("$#!*")
                .ShouldBeFalse();
        }

        public void IndexedItem_Fact() {
            var list = new StringList("a", "b", "c");
            list.ShouldHaveState(3, 0, "a", "b", "c");
            list[1].ShouldEqual("b");
            list[1] = "#";
            list.ShouldHaveState(3, 1, "a", "#", "c");
        }

        public void IndexOfItem_Fact() {
            var list = new StringList("1", "2", "3");
            list.IndexOf("0")
                .ShouldEqual(-1);
            list.IndexOf("2")
                .ShouldEqual(1);
        }

        public void InsertItem_Fact() {
            var list = new StringList();
            list.Insert(0, "hello");
            list.ShouldHaveState(1, 1, "hello");
            list.Insert(1, "world");
            list.ShouldHaveState(2, 2, "hello", "world");
            list.Insert(1, "micro");
            list.ShouldHaveState(3, 3, "hello", "micro", "world");
            list.Insert(0, "awesome:");
            list.ShouldHaveState(4, 4, "awesome:", "hello", "micro", "world");
        }

        public void InsertItemRange_Fact() {
            var list = new StringList();
            list.InsertRange(0, "world");
            list.ShouldHaveState(1, 1, "world");
            list.InsertRange(0, "hello");
            list.ShouldHaveState(2, 2, "hello", "world");
            list.InsertRange(1, "micro");
            list.ShouldHaveState(3, 3, "hello", "micro", "world");
            list.InsertRange(0, "super", "awesome", "fun", "time");
            list.ShouldHaveState(7, 7, "super", "awesome", "fun", "time", "hello", "micro", "world");
        }
    }
}

namespace uScoober.TestFramework.Assert
{
    internal static partial class AssertExtensions
    {
        public static void ShouldHaveState(this StringList list, int count, int editVersion, params string[] items) {
            list.Count.ShouldEqual(count);
            list.EditVersion.ShouldEqual(editVersion);
            if (items.Length > 0) {
                list.ShouldEnumerateEqual((IEnumerable)items);
            }
        }
    }
}