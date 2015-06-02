namespace uScoober.Hardware.Display
{
    public interface IDisplayScrollingText : IDisplayText
    {
        //todo: bool AutoScroll { get; set; }

        void ScrollLeft();

        void ScrollReset();

        void ScrollRight();
    }
}