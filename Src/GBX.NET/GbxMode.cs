namespace GBX.NET;

/// <summary>
/// Editor or release serialization mode, encoded in Gbx headers from version 4 onward.
/// </summary>
public enum GbxMode : byte
{
    /// <summary>
    /// Unspecified.
    /// </summary>
    Unspecified,
    /// <summary>
    /// Editor serialization. Includes editor-only data and excludes chunks marked NotInEditor.
    /// Also used implicitly by headers before version 4, which omit this byte.
    /// </summary>
    Editor = (byte)'E',
    /// <summary>
    /// Release serialization. Excludes editor-only data and chunks marked NotInRelease.
    /// </summary>
    Release = (byte)'R'
}
