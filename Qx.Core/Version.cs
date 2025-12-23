namespace Qx.Core;

public struct Version
{
    public int Major { get; set; }
    public int Minor { get; set; }
    public int Build { get; set; }
    public int Revision { get; set; }

    public override string ToString()
    {
        return $"{Major}.{Minor}.{Build}.{Revision}";
    }

    public static Version Parse(string version)
    {
        string[] parts = version.Split('.');
        if (parts.Length != 4) throw new FormatException($"Invalid version ({version}) format. Expected major.minor.build.revision.");
        var intParts = parts.Select(p => int.TryParse(p, out int _) ? int.Parse(p) : throw new FormatException($"Invalid version ({version}) format. Expected an integer for {p}")).ToList();
        return new Version
        {
            Major = intParts[0],
            Minor = intParts[1],
            Build = intParts[2],
            Revision = intParts[3]
        };
    }
}