namespace uScoober
{
    public delegate void Action();

    public delegate void Action1(object value);

    public delegate void Action2(object value1, object value2);

    public delegate object Func();

    public delegate object Func1(object value);

    public delegate object Func2(object value, object value2);

    public delegate bool Predicate(object value);

    public delegate bool ProvideBool();

    public delegate double ProvideDouble();
}