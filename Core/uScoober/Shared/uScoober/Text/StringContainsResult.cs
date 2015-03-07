namespace uScoober.Text
{
    public class StringContainsResult
    {
        private readonly string _option;
        private readonly int _startIndex;

        public StringContainsResult(string source, string option) {
            _option = option;
            if ((source == null) || (option == null) || (option.Length == 0 || option.Length > source.Length)) {
                _startIndex = -1;
                return;
            }
            _startIndex = 0;
            int optionIndex = 0;
            while (optionIndex < option.Length) {
                if (source[_startIndex + optionIndex] == option[optionIndex]) {
                    optionIndex++;
                    if (optionIndex == option.Length) {
                        return;
                    }
                    continue;
                }
                _startIndex++;
                if (_startIndex + option.Length > source.Length) {
                    _startIndex = -1;
                    return;
                }
                optionIndex = 0;
            }
        }

        public bool IsMatch {
            get { return _startIndex > -1; }
        }

        public string Option {
            get { return _option; }
        }

        public int StartIndex {
            get { return _startIndex; }
        }
    }
}