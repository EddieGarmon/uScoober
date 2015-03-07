using System.Text;

namespace uScoober.TestFramework
{
    public class PrettyArgs
    {
        private readonly StringBuilder _builder = new StringBuilder(40); //todo verify good starting size
        private bool _first = true;

        public PrettyArgs Add(string name, string value) {
            if (_first) {
                _first = false;
            }
            else {
                _builder.Append('\n');
            }
            _builder.Append(name);
            _builder.Append(": ");
            if (value == null) {
                _builder.Append("{null}");
            }
            else {
                _builder.Append('\'');
                _builder.Append(value);
                _builder.Append('\'');
            }
            return this;
        }

        public override string ToString() {
            return _builder.ToString();
        }
    }
}