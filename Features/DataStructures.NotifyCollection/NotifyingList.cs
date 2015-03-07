using System;
using System.Collections;
using System.Threading;

namespace uScoober.DataStructures
{
    public class NotifyingList : List,
                                 INotifyCollectionChanged
    {
        private IDisposable _stopNotifications;
        private int _suppressionCount;

        public NotifyingList() { }

        public NotifyingList(IEnumerable values)
            : base(values) { }

        public bool IsNotifying {
            get { return _suppressionCount == 0; }
            set {
                bool notifying = _stopNotifications == null;
                if (notifying == value) {
                    return;
                }
                if (notifying) {
                    _stopNotifications = SuppressChangeNotifications();
                }
                else {
                    _stopNotifications.Dispose();
                    _stopNotifications = null;
                }
            }
        }

        public override object this[int index] {
            get { return base[index]; }
            set {
                base[index] = value;
                NotifyOfCollectionChange(NotifyCollectionChangedAction.Reset);
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public override int Add(object value) {
            var index = base.Add(value);
            NotifyOfCollectionChange(NotifyCollectionChangedAction.Add);
            return index;
        }

        public override void AddRange(IEnumerable values) {
            base.AddRange(values);
            NotifyOfCollectionChange(NotifyCollectionChangedAction.Add);
        }

        public override void Clear() {
            if (Count == 0) {
                return;
            }
            base.Clear();
            NotifyOfCollectionChange(NotifyCollectionChangedAction.Reset);
        }

        public override void Insert(int index, object value) {
            base.Insert(index, value);
            NotifyOfCollectionChange(NotifyCollectionChangedAction.Add);
        }

        public void Refresh() {
            NotifyOfCollectionChange(NotifyCollectionChangedAction.Reset);
        }

        public override void Remove(object value) {
            base.Remove(value);
            NotifyOfCollectionChange(NotifyCollectionChangedAction.Remove);
        }

        public override void RemoveAt(int index) {
            base.RemoveAt(index);
            NotifyOfCollectionChange(NotifyCollectionChangedAction.Remove);
        }

        public IDisposable SuppressChangeNotifications() {
            Interlocked.Increment(ref _suppressionCount);
            return Disposable.Create(() => Interlocked.Decrement(ref _suppressionCount));
        }

        protected virtual void NotifyOfCollectionChange(NotifyCollectionChangedAction action) {
            if (!IsNotifying) {
                return;
            }
            var handler = CollectionChanged;
            if (handler != null) {
                handler(this, new NotifyCollectionChangedEventArgs(action));
            }
        }

        public static implicit operator object[](NotifyingList notifyingList) {
            return (List)notifyingList;
        }

        public static implicit operator NotifyingList(object[] array) {
            return new NotifyingList(array);
        }
    }
}