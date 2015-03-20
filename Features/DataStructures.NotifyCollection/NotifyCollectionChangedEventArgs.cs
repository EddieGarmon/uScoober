namespace uScoober.DataStructures
{
    internal class NotifyCollectionChangedEventArgs
    {
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action) {
            Action = action;
        }

        public NotifyCollectionChangedAction Action { get; private set; }
    }
}