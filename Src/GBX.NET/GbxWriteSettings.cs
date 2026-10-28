using Microsoft.Extensions.Logging;

namespace GBX.NET;

public readonly record struct GbxWriteSettings
{
    /// <summary>
    /// Maximum allowed size for arrays, lists, and other data structures. Default is 512 MB, but for backend it is recommended to set it to a lower value, like 16 MB, to prevent potential DoS attacks with maliciously crafted Gbx objects.
    /// </summary>
    /// <remarks>Null uses the default limit of 536870912. Set a larger value, up to <see cref="int.MaxValue"/>, to allow larger data structures.</remarks>
    public int? MaxDataSize { get; init; }

    public byte? PackDescVersion { get; init; }
    public ClassIdRemapMode? ClassIdRemapMode { get; init; }
    /// <summary>
    /// Closes the stream after writing is finished. Default is <see langword="false"/>.
    /// </summary>
    public bool CloseStream { get; init; }

    public ILogger? Logger { get; init; }
}
