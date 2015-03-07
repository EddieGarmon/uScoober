namespace uScoober.DataStructures
{
    public class NotifyCollectionChangedEventArgs
    {
        public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action) {
            Action = action;
        }

        public NotifyCollectionChangedAction Action { get; private set; }
    }
}