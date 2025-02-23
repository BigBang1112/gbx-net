using Microsoft.Extensions.Logging;

namespace GBX.NET;

public readonly record struct GbxReadSettings
{
    public bool SkipUserData { get; init; }
    public bool SkipUnclearedUserData { get; init; }
    public bool SkipUnclearedHeaderChunkBuffers { get; init; }
    public bool SkipUnclearedSkippableChunkBuffers { get; init; }
    public SerializationMode DeserializationMode { get; init; }

    /// <summary>
    /// If to store the raw body as a byte array data in <see cref="Gbx"/>, allowing to serialize the Gbx back with <see cref="Gbx"/>'s ParseHeader methods or Parse methods of unknown nodes. Do NOT use if you have a stream with more data after the Gbx data. On uncompressed Gbx bodies, this will cause the stream to be read until the end, including the irrelevant data.
    /// </summary>
    public bool ReadRawBody { get; init; }
    public bool IgnoreExceptionsInBody { get; init; }

    /// <summary>
    /// Closes the stream after reading is finished. Default is <see langword="false"/>.
    /// </summary>
    public bool CloseStream { get; init; }

    public int? MaxUncompressedBodySize { get; init; }
    public int? MaxUserDataSize { get; init; }
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