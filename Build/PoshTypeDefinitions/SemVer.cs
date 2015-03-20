using System;

public class SemVer : IComparable
{
    public SemVer(int major, int minor, int revision) {
        Major = major;
        Minor = minor;
        Revision = revision;
    }

    public SemVer(int major, int minor, int revision, string preRelease, string buildMetadata) {
        Major = major;
        Minor = minor;
        Revision = revision;
        PreRelease = preRelease;
        BuildMetadata = buildMetadata;
    }

    public string BuildMetadata { get; private set; }

    public int Major { get; private set; }

    public int Minor { get; private set; }

    public string PreRelease { get; private set; }

    public int Revision { get; private set; }

    public int CompareTo(object obj) {
        // todo: support pre-release 
        var other = obj as SemVer;
        if (other == null) {
            return 1;
        }
        int delta = Major - other.Major;
        if (delta != 0) {
            return delta;
        }
        delta = Minor - other.Minor;
        if (delta != 0) {
            return delta;
        }
        return Revision - Revision;
    }

    public SemVer GetNextNonGuarenteedCompatibleVersion() {
        return Major == 0 ? new SemVer(0, Minor + 1, 0) : new SemVer(Major + 1, 0, 0);
    }

    public string ToAssemblyVersion() {
        return string.Format("{0}.{1}.{2}.0", Major, Minor, Revision);
    }

    public override string ToString() {
        return string.Format("{0}.{1}.{2}{3}{4}{5}{6}",
                             Major,
                             Minor,
                             Revision,
                             PreRelease != null ? "-" : null,
                             PreRelease,
                             BuildMetadata != null ? "+" : null,
                             BuildMetadata);
    }

    public static readonly SemVer Zero = new SemVer(0, 0, 0);

    public static SemVer Parse(string input) {
        int major,
            minor,
            revision;
        string preRelease = null,
               buildMetadata = null;

        int start = 0;
        int end = input.IndexOf('.');
        major = int.Parse(input.Substring(start, end));
        start = end + 1;
        end = input.IndexOf('.', start);
        minor = int.Parse(input.Substring(start, end - start));
        start = end + 1;
        int preReleaseStart = input.IndexOf('-', start);
        int buildMetadataStart = input.IndexOf('+', start);
        if (preReleaseStart < 0 && buildMetadataStart < 0) {
            revision = int.Parse(input.Substring(start));
        }
        else if (preReleaseStart > 0 && buildMetadataStart < 0) {
            revision = int.Parse(input.Substring(start, preReleaseStart - start));
            preRelease = input.Substring(preReleaseStart + 1);
        }
        else if (preReleaseStart < 0 && buildMetadataStart > 0) {
            revision = int.Parse(input.Substring(start, buildMetadataStart - start));
            buildMetadata = input.Substring(buildMetadataStart + 1);
        }
        else {
            revision = int.Parse(input.Substring(start, preReleaseStart - start));
            preReleaseStart++; //eat the '-'
            preRelease = input.Substring(preReleaseStart, buildMetadataStart - preReleaseStart);
            buildMetadata = input.Substring(buildMetadataStart + 1);
        }
        return new SemVer(major, minor, revision, preRelease, buildMetadata);
    }
}