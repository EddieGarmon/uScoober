using System.IO;

namespace uScoober.Boot
{
    public static class ApplicationLoader
    {
        public static void BootFrom(string rootDirectory)
        {
            return;
            string pePath = rootDirectory + @"\appOne.pe";

            //Open a PE file from SD card
            //call a method in it
            using (FileStream stream = new FileStream(pePath, FileMode.Open)) {
                
            }
        }

        public static void BootFromDefault() { }

    }
}