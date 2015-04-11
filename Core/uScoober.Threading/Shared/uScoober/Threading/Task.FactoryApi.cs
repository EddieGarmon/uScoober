using System;
using System.Threading;

namespace uScoober.Threading
{
    public partial class Task
    {
        private static readonly ActionTask __canceled = new ActionTask(true);
        private static readonly ActionTask __completed = new ActionTask(false);

        public static ActionTask Canceled() {
            return __canceled;
        }

        public static ActionTask Completed() {
            return __completed;
        }

        public static FuncTask Completed(object result) {
            return new FuncTask(result);
        }

        public static ActionTask Delay(TimeSpan duration, CancellationToken cancellationToken = null) {
            return Delay(duration.Milliseconds, cancellationToken);
        }

        public static ActionTask Delay(int milliseconds, CancellationToken cancellationToken = null) {
            return new ActionTask(token => {
                token.ThrowIfCancellationRequested();
                Thread.Sleep(milliseconds);
            },
                                  cancellationToken);
        }

        public static ActionTask Faulted(string message) {
            return (Faulted(new Exception(message)));
        }

        public static ActionTask Faulted(Exception exception) {
            return new ActionTask(exception);
        }

        public static ActionTask New(Action doThis, TaskScheduler scheduler = null) {
            return new ActionTask(doThis, scheduler);
        }

        public static ActionTask New(CancellableAction doThis, CancellationToken cancellationToken = null, TaskScheduler scheduler = null) {
            return new ActionTask(doThis, cancellationToken, scheduler);
        }

        public static FuncTask New(Func doThis, TaskScheduler scheduler = null) {
            return new FuncTask(doThis, scheduler);
        }

        public static FuncTask New(CancellableFunc doThis, CancellationToken cancellationToken = null, TaskScheduler scheduler = null) {
            return new FuncTask(doThis, cancellationToken, scheduler);
        }

        public static ActionTask Run(Action doThis, TaskScheduler scheduler = null) {
            var task = new ActionTask(doThis, scheduler);
            task.Start();
            return task;
        }

        public static ActionTask Run(CancellableAction doThis, CancellationToken cancellationToken = null, TaskScheduler scheduler = null) {
            var task = new ActionTask(doThis, cancellationToken, scheduler);
            task.Start();
            return task;
        }

        public static FuncTask Run(Func doThis, TaskScheduler scheduler = null) {
            var task = new FuncTask(doThis, scheduler);
            task.Start();
            return task;
        }

        public static FuncTask Run(CancellableFunc doThis, CancellationToken cancellationToken = null, TaskScheduler scheduler = null) {
            var task = new FuncTask(doThis, cancellationToken, scheduler);
            task.Start();
            return task;
        }

        public static WaitTask WaitAll(Task a, Task b, CancellationToken cancellationToken = null) {
            return new WaitTask(new WaitList(a, b), WaitOn.All, cancellationToken);
        }

        public static WaitTask WaitAll(Task a, Task b, Task c, CancellationToken cancellationToken = null) {
            return new WaitTask(new WaitList(a, b, c), WaitOn.All, cancellationToken);
        }

        public static WaitTask WaitAll(params Task[] tasks) {
            return new WaitTask(new WaitList(tasks), WaitOn.All, CancellationToken.None);
        }

        public static WaitTask WaitAll(Task[] tasks, CancellationToken cancellationToken) {
            return new WaitTask(new WaitList(tasks), WaitOn.All, cancellationToken);
        }

        public static WaitTask WaitAny(Task a, Task b, CancellationToken cancellationToken = null) {
            return new WaitTask(new WaitList(a, b), WaitOn.Single, cancellationToken);
        }

        public static WaitTask WaitAny(Task a, Task b, Task c, CancellationToken cancellationToken = null) {
            return new WaitTask(new WaitList(a, b, c), WaitOn.Single, cancellationToken);
        }

        public static WaitTask WaitAny(params Task[] tasks) {
            return new WaitTask(new WaitList(tasks), WaitOn.Single, CancellationToken.None);
        }

        public static WaitTask WaitAny(Task[] tasks, CancellationToken cancellationToken) {
            return new WaitTask(new WaitList(tasks), WaitOn.Single, cancellationToken);
        }

        public static void WarmUp() {
            Run(() => { })
                .Wait();
        }
    }
}