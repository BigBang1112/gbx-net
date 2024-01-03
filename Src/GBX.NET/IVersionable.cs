namespace GBX.NET;

/// <summary>
/// Defines an interface for objects that are versionable.
/// </summary>
public interface IVersionable
{
    /// <summary>
    /// Version to use for backwards compatibility.
    /// </summary>
    int Version { get; set; }
}