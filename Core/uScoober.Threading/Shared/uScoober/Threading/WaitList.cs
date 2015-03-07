using System;
using System.Threading;
using uScoober.DataStructures;

namespace uScoober.Threading
{
    internal class WaitList : IDisposable
    {
        private bool _isStorageRing;
        private object _storage;

        public WaitList() {
            _storage = null;
            _isStorageRing = false;
        }

        public WaitList(Task task) {
            if (task == null) {
                throw new ArgumentNullException();
            }
            _storage = task;
            _isStorageRing = false;
        }

        public WaitList(Task task1, Task task2) {
            if (task1 == null) {
                throw new ArgumentNullException("task1");
            }
            if (task2 == null) {
                throw new ArgumentNullException("task2");
            }
            _storage = new Ring(task1, task2);
            _isStorageRing = true;
        }

        public WaitList(Task task1, Task task2, Task task3) {
            if (task1 == null) {
                throw new ArgumentNullException("task1");
            }
            if (task2 == null) {
                throw new ArgumentNullException("task2");
            }
            if (task3 == null) {
                throw new ArgumentNullException("task3");
            }
            _storage = new Ring(task1, task2, task3);
            _isStorageRing = true;
        }

        public WaitList(params Task[] tasks) {
            if (tasks == null) {
                throw new ArgumentNullException("tasks");
            }
            _storage = new Ring();
            foreach (Task task in tasks) {
                if (task == null) {
                    throw new ArgumentNullException("task");
                }
                StorageAsRing.InsertTail(task);
            }
        }

        private Ring StorageAsRing {
            get { return (Ring)_storage; }
        }

        private Task StorageAsTask {
            get { return (Task)_storage; }
        }

        public void Add(Task task) {
            if (task == null) {
                throw new ArgumentNullException();
            }
            lock (this) {
                if (_storage == null) {
                    _storage = task;
                }
                else if (!_isStorageRing) {
                    _storage = new Ring(_storage, task);
                    _isStorageRing = true;
                }
                else {
                    StorageAsRing.InsertTail(task);
                }
            }
        }

        public void Dispose() {
            if (_isStorageRing && _storage != null) {
                StorageAsRing.Clear();
            }
        }

        public void ScheduleAll() {
            if (!_isStorageRing) {
                if (_storage != null) {
                    ((Task)_storage).Schedule();
                }
            }
            else {
                foreach (Task task in ((Ring)_storage)) {
                    task.Schedule();
                }
            }
        }

        public void WaitAll() {
            if (_storage == null) {
                return;
            }
            if (!_isStorageRing) {
                StorageAsTask.Wait();
            }
            var handles = new WaitHandle[StorageAsRing.Count];
            int i = 0;
            foreach (Task task in StorageAsRing) {
                if (task == null) {
                    throw new ArgumentNullException();
                }
                if (!TaskContinuationManager.ScheduleAntecedent(task)) {
                    task.Schedule();
                }
                //BUG?: we need to handle completed tasks
                handles[i++] = task.CompletionHandle;
            }
            WaitHandle.WaitAll(handles);
        }

        public Task WaitAny() {
            if (_storage == null) {
                return Task.Completed();
            }
            if (!_isStorageRing) {
                StorageAsTask.Wait();
            }
            var handles = new WaitHandle[StorageAsRing.Count];
            int i = 0;
            foreach (Task task in StorageAsRing) {
                if (task == null) {
                    throw new ArgumentNullException();
                }
                if (task.IsComplete) {
                    return task;
                }
                if (!TaskContinuationManager.ScheduleAntecedent(task)) {
                    task.Schedule();
                }
                //BUG?: we need to handle completed tasks
                handles[i++] = task.CompletionHandle;
            }
            var index = WaitHandle.WaitAny(handles);
            return (Task)StorageAsRing.FindAtIndex(index)
                                      .Value;
        }
    }
}