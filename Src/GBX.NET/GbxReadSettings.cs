using Microsoft.Extensions.Logging;

namespace GBX.NET;

public readonly record struct GbxReadSettings
{
    /// <summary>
    /// Maximum allowed size for arrays, lists, and other data structures. Default is 512 MB, but for backend it is recommended to set it to a lower value, like 16 MB, to prevent potential DoS attacks with maliciously crafted Gbx files.
    /// </summary>
    /// <remarks>Null uses the default limit of 536870912. Set a larger value, up to <see cref="int.MaxValue"/>, to allow larger data structures.</remarks>
    public int? MaxDataSize { get; init; }
    
    /// <summary>
    /// Maximum allowed size for user data. Default is 16 MB, but for backend it is recommended to set it to a lower value, like 2 MB, to prevent potential DoS attacks with maliciously crafted Gbx files.
    /// </summary>
    public int? MaxUserDataSize { get; init; }

    /// <summary>
    /// Maximum allowed size for uncompressed body. Default is 512 MB, but for backend it is recommended to set it to a lower value, like 64 MB, to prevent potential DoS attacks with maliciously crafted Gbx files.
    /// </summary>
    public int? MaxUncompressedBodySize { get; init; }

    /// <summary>
    /// Maximum allowed size for compressed body. Default is 512 MB, but for backend it is recommended to set it to a lower value, like 32 MB, to prevent potential DoS attacks with maliciously crafted Gbx files.
    /// </summary>
    public int? MaxCompressedBodySize { get; init; }

    /// <summary>
    /// Maximum allowed size for skippable chunks. Default is 256 MB, but for backend it is recommended to set it to a lower value, like 4 MB, to prevent potential DoS attacks with maliciously crafted Gbx files.
    /// </summary>
    public int? MaxSkippableChunkSize { get; init; }

    public bool SkipUserData { get; init; }
    public SerializationMode DeserializationMode { get; init; }

    /// <summary>
    /// Optional game context for legacy payloads that cannot be distinguished from their newer format by their bytes alone.
    /// </summary>
    public GameVersion GameVersion { get; init; }

    /// <summary>
    /// If to store the raw body as a byte array data in <see cref="Gbx"/>, allowing to serialize the Gbx back with <see cref="Gbx"/>'s ParseHeader methods or Parse methods of unknown nodes. Do NOT use if you have a stream with more data after the Gbx data. On uncompressed Gbx bodies, this will cause the stream to be read until the end, including the irrelevant data.
    /// </summary>
    public bool ReadRawBody { get; init; }
    public bool IgnoreExceptionsInBody { get; init; }

    /// <summary>
    /// Closes the stream after reading is finished. Default is <see langword="false"/>.
    /// </summary>
    public bool CloseStream { get; init; }

    public ILogger? Logger { get; init; }

    /// <summary>
    /// Solves the occasional bug with OpenPlanet extraction where the header chunks are not properly written into the Gbx, while the length of this data section is still set to a non-zero value.
    /// </summary>
    public bool OpenPlanetHookExtractMode { get; init; }

    public HashSet<uint>? SkipChunkIds { get; init; }

    /// <summary>
    /// Skippable chunks will be read less efficiently, but they will be ignored if the read will fail, with (usually) no corruption once saved.
    /// </summary>
    public bool SafeSkippableChunks { get; init; }

    public IEncryptionInitializer? EncryptionInitializer { get; init; }
}
