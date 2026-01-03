using Swashbuckle.AspNetCore.Annotations;

namespace Qx.Core;

public sealed class Version(int major, int minor, int build, int revision)
{
    [SwaggerSchema(Description = "Major version number")]
    public int Major { get; init; } = major;

    [SwaggerSchema(Description = "Minor version number")]
    public int Minor { get; init; } = minor;

    [SwaggerSchema(Description = "Build number for this {major}.{minor} version")]
    public int Build { get; init; } = build;

    [SwaggerSchema(Description = "Revision number for this build")]
    public int Revision { get; init; } = revision;

    public override string ToString()
    {
        return $"{Major}.{Minor}.{Build}.{Revision}";
    }

    public static Version Parse(string version)
    {
        string[] parts = version.Split('.');
        if (parts.Length != 4) throw new FormatException($"Invalid version ({version}) format. Expected major.minor.build.revision.");
        var intParts = parts.Select(p => int.TryParse(p, out int _) ? int.Parse(p) : throw new FormatException($"Invalid version ({version}) format. Expected an integer for {p}")).ToList();
        return new Version(intParts[0], intParts[1], intParts[2], intParts[3]);
    }
}