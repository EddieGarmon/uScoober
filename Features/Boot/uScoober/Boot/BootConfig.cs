using System;
using uScoober.Storage;

namespace uScoober.Boot
{
    public class BootConfig
    {
        private IFolder _bootRoot;

        private BootConfig(IFolder bootRoot) {
            _bootRoot = bootRoot;
        }

        public bool IsDefault { get; private set; }

        public ApplicationConfig[] ListApplications() {
            // explore directories - all child directories are individual apps - we will run only one at a time.
            // what application is configured?
            // what convention do we use for configurationless deployment?

            //how do we put a file watcher on the boot config to "Reboot" after write is done?

            throw new NotImplementedException();
        }

        public static BootConfig Get(IFolder bootRoot) {
            var config = new BootConfig(bootRoot);

            var file = bootRoot.GetFile("boot.stone");
            if (file != null) {
                //from disk?
            }
            else {
                //or default
                config.IsDefault = true;
            }
            return config;
        }
    }
}