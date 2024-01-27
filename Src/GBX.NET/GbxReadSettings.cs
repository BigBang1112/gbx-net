namespace GBX.NET;

public readonly record struct GbxReadSettings
{
    public bool SkipUserData { get; init; }
    public bool SkipLengthValidation { get; init; }
    public bool SkipDataUntilLengthMatches { get; init; }
    public SerializationMode DeserializationMode { get; init; }

    /// <summary>
    /// If to store the raw body as a byte array data in <see cref="Gbx"/>, allowing to serialize the Gbx back with <see cref="Gbx"/>'s ParseHeader methods or Parse methods of unknown nodes.
    /// </summary>
    public bool ReadRawBody { get; init; }
    public bool SkipExceptionsInBody { get; init; }
    public bool LeaveOpen { get; init; }
    public int? MaxUncompressedBodySize { get; init; }
    public int? MaxUserDataSize { get; init; }
}