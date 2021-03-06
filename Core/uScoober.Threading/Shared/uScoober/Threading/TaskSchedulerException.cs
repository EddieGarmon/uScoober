﻿using System;

namespace uScoober.Threading
{
    public class TaskSchedulerException : Exception
    {
        private const string DefaultMessage = "Task Scheduler Exception";

        public TaskSchedulerException()
            : base(DefaultMessage) { }

        public TaskSchedulerException(string message)
            : base(message) { }

        public TaskSchedulerException(Exception innerException)
            : base(DefaultMessage, innerException) { }

        public TaskSchedulerException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}