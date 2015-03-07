namespace uScoober.Threading
{
    public enum TaskStatus
    {
        Unknown = 0,

        //unstarted
        WaitingForAcivation,
        WaitingForAnticedent,

        //started
        Scheduled,
        UserWorkStarted,
        UserWorkFinished,
        CancelPending,
        FaultPending,

        //completed
        RanToCompletion,
        Canceled,
        Faulted,

        // all done
        Disposed
    }
}