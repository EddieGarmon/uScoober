using uScoober.Storage.Spot;

namespace uScoober.Hardware.Boards
{
    public class NetduinoPlus2
    {
        private SpotFileSystem _microSD;

        public SpotFileSystem MicroSD {
            get { return _microSD ?? (_microSD = new SpotFileSystem()); }
        }
    }
}