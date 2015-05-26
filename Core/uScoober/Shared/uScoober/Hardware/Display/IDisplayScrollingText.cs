namespace uScoober.Hardware.Display
{
    public interface IDisplayScrollingText : IDisplayText
    {
        bool AutoScroll { get; set; }

        void ScrollLeft();

        void ScrollRight();
    }
}