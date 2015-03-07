namespace uScoober.DataStructures.Typed
{
    public class StringListEnumerator : Ring.Enumerator
    {
        internal StringListEnumerator(Ring storage, bool isReversed = false)
            : base(storage, isReversed) { }

        public new string Current {
            get { return (string)base.Current; }
        }
    }
}