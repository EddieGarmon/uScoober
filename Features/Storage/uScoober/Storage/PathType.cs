using System;

namespace uScoober.Storage
{
    [Flags]
    public enum PathType
    {
        Unknown = 0,

        Relative = 0x1000,
        Absolute = 0x2000,

        RamDrive = 0x2001, // \ram\<path>
        DriveLetter = 0x2002, // C:\<path>
        NetworkShare = 0x2004, // \\server\share\<path>
        IsolatedStorage = 0x2008, // <path>
        SdDrive = 0x2010, // \sd\<path>
    }
}