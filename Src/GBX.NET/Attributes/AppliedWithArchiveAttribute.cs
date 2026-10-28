namespace GBX.NET.Attributes;

/// <summary>
/// Tells that the class archive is used to write a certain node member, used when the node is serialized without chunks.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class AppliedWithArchiveAttribute : Attribute
{
    /// <summary>
    /// Since which archive version this member is written.
    /// </summary>
    public int SinceVersion { get; }

    /// <summary>
    /// Up to which archive version the member is written. Null means there's no known version limit.
    /// </summary>
    public int? UpToVersion { get; }

    /// <summary>
    /// Creates the <see cref="AppliedWithArchiveAttribute"/>.
    /// </summary>
    /// <param name="sinceVersion">Since which archive version this member is written.</param>
    public AppliedWithArchiveAttribute(int sinceVersion = 0)
    {
        SinceVersion = sinceVersion;
    }

    /// <summary>
    /// Creates the <see cref="AppliedWithArchiveAttribute"/>.
    /// </summary>
    /// <param name="sinceVersion">Since which archive version this member is written.</param>
    /// <param name="upToVersion">Up to which archive version the member is written.</param>
    public AppliedWithArchiveAttribute(int sinceVersion, int upToVersion) : this(sinceVersion)
    {
        UpToVersion = upToVersion;
    }

    public bool Applies(int version)
    {
        return version >= SinceVersion && (UpToVersion is null || version <= UpToVersion);
    }
}
