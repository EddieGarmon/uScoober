using uScoober.DataStructures;

namespace uScoober.Threading
{
    internal static class TaskContinuationManager
    {
        private static readonly Ring __continuationRecords = new Ring();

        internal static void ContinueFrom(Task task) {
            // NB: This method will always and only be called from Task.Finish()
            // The task will be marked completed before entering here, so all future continuations should callback immediatly.
            // todo: handle canceled or failed task?
            var indexedLink = __continuationRecords.Find(obj => ((ContinuationRecord)obj).Task == task);
            if (indexedLink != null) {
                var continuations = ((ContinuationRecord)indexedLink.Value).Continuations;
                lock (__continuationRecords) {
                    __continuationRecords.RemoveLink(indexedLink);
                }
                if (continuations != null) {
                    continuations.ScheduleAll();
                    continuations.Dispose();
                }
            }
        }

        internal static void RegisterContinuation(Task task, Task continuation) {
            if (task.IsComplete) {
                //todo do we need try/catch here?
                continuation.Schedule();
                return;
            }
            lock (__continuationRecords) {
                ContinuationRecord continuationRecord;
                var indexedLink = __continuationRecords.Find(obj => ((ContinuationRecord)obj).Task == task);
                if (indexedLink != null) {
                    continuationRecord = ((ContinuationRecord)indexedLink.Value);
                }
                else {
                    continuationRecord = new ContinuationRecord(task);
                    __continuationRecords.InsertTail(continuationRecord);
                }
                continuationRecord.AddContinuation(continuation);

                //register the continuations antecedent
                indexedLink = __continuationRecords.Find(obj => ((ContinuationRecord)obj).Task == continuation);
                if (indexedLink != null) {
                    continuationRecord = ((ContinuationRecord)indexedLink.Value);
                }
                else {
                    continuationRecord = new ContinuationRecord(continuation);
                    __continuationRecords.InsertTail(continuationRecord);
                }
                continuationRecord.Antecedent = task;
            }
        }

        internal static bool ScheduleAntecedent(Task task) {
            Task antecedent = GetAntecedent(task);
            while (antecedent != null) {
                if (antecedent.HasStarted) {
                    return true;
                }
                var temp = GetAntecedent(antecedent);
                if (temp == null) {
                    antecedent.Schedule();
                    return true;
                }
                antecedent = temp;
            }
            return false;
        }

        private static Task GetAntecedent(Task task) {
            var indexedLink = __continuationRecords.Find(value => ((ContinuationRecord)value).Task == task);
            return indexedLink == null ? null : ((ContinuationRecord)indexedLink.Value).Antecedent;
        }

        private class ContinuationRecord
        {
            public ContinuationRecord(Task task) {
                Task = task;
            }

            public Task Antecedent { get; set; }

            public WaitList Continuations { get; private set; }

            public Task Task { get; private set; }

            public void AddContinuation(Task continuation) {
                if (Continuations == null) {
                    Continuations = new WaitList(continuation);
                }
                else {
                    Continuations.Add(continuation);
                }
            }
        }
    }
}