namespace uScoober.Threading
{
    public delegate void CancellableAction(CancellationToken token);

    public delegate object CancellableFunc(CancellationToken token);

    public delegate void UnobservedTaskExceptionHandler(AggregateException exception);
}