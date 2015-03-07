#Task Execution Flows:

##Create and Start ALL Tasks
1. **User** creates the Task
2. **User** starts the Task
3. Scheduler schedules the Task, marks it scheduled
4. TaskScheduler.Execute(Task) is called 
		if (task.CancellationToken.IsCancellationRequested) {
			// If cancellation is requested before execution, do not run.
		    task.Finish(); 
		    return;
		}
		if (task.Status != TaskStatus.Scheduled) {
			// this be bad
		    throw new Exception("Unscheduled task executing!!!");
		}
		try {
		    task.ExecuteNow();
		}
		catch (Exception ex) {
			// record the exception on the task
		    task.AddException(ex);
		}
		finally {
		    task.Finish();
		}

##Normal Execution
1. Task finishes user work without throwing any exceptions.
1. Any registered continuations are started and passed the completed task.

##Canceled Execution
1. The Tasks user action throws an OperationCanceledException.
1. TaskScheduler.Execute() catches the exception, transitions the task to Canceled.
1. ?Continuations are called 
2. 
##Exceptional Execution
1. If the Task's user action throws any exception other than an OperationCanceledException.
1. TaskScheduler.Execute() catches the exception, and stores it for later.
1. Attempt to observe the exception in a continuation.
1. Observe the exception and throw from Wait()/Result. 
1. If the exception is not observed before disposal, TaskScheduler.UnobservedException is fired.


---------------------
###TPL Reading:
http://www.ademiller.com/blogs/tech/2010/10/exception-handling-with-the-task-parallel-library/
http://msdn.microsoft.com/en-us/library/ff963549.aspx

How to: Handle Exceptions Thrown by Tasks -- http://msdn.microsoft.com/en-us/library/dd537614(v=vs.110).aspx
How to: Cancel a Task and Its Children -- http://msdn.microsoft.com/en-us/library/dd537607(v=vs.110).aspx

http://www.codeproject.com/Articles/159533/Task-Parallel-Library-of-n