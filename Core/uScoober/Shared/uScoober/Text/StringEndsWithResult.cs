namespace uScoober.Text
{
    public class StringEndsWithResult
    {
        private readonly bool _isMatch;
        private readonly string _option;

        public StringEndsWithResult(string source, string option) {
            _option = option;
            if (option.Length > source.Length) {
                return;
            }
            int sourceIndex = source.Length - 1;
            int optionIndex = option.Length - 1;
            while (optionIndex >= 0) {
                if (source[sourceIndex] != option[optionIndex]) {
                    return;
                }
                if (optionIndex == 0) {
                    _isMatch = true;
                }
                sourceIndex--;
                optionIndex--;
            }
        }

        public bool IsMatch {
            get { return _isMatch; }
        }

        public string Option {
            get { return _option; }
        }
    }
}