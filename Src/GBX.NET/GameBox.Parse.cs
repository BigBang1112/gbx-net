namespace GBX.NET;

public partial class GameBox
{
    /// <exception cref="NotAGbxException">The stream is not a Gbx stream nor starting at the correct position.</exception>
    /// <exception cref="VersionNotSupportedException">Version of Gbx below 3 is not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted Gbx is not supported.</exception>
    /// <exception cref="CompressedRefTableException">Compressed reference is not supported.</exception>
    /// <exception cref="InvalidDataException">Number of reference files is below zero.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    private static GameBox ParseHeaderWithoutProcessing(GameBoxReader reader,
                                                        IProgress<GameBoxReadProgress>? progress,
                                                        ILogger? logger,
                                                        out Type? classType,
                                                        out bool isRefTableCompressed)
    {
        var fileName = reader.BaseStream is FileStream fs ? fs.Name : null;

        var header = Header.Parse(reader, logger);

        //progress?.Report(new GameBoxReadProgress(header));

        var refTable = default(RefTable);

        try
        {
            refTable = RefTable.Parse(header, reader);
            isRefTableCompressed = false;
        }
        catch (CompressedRefTableException)
        {
            isRefTableCompressed = true;
        }

        classType = NodeCacheManager.GetClassTypeById(header.Id);

        if (classType is null)
        {
            return new GameBox(header, refTable, fileName);
        }

        var genericGameBoxType = typeof(GameBox<>).MakeGenericType(classType);

        var args = new object?[] { header, refTable, fileName };

        if (Activator.CreateInstance(genericGameBoxType, args) is not GameBox gbx)
        {
            throw new ThisShouldNotHappenException();
        }

        return gbx;
    }

    /// <summary>
    /// Parses only the header of the GBX.
    /// </summary>
    /// <param name="stream">Stream to read GBX format from.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readRawBody">If the body should be read in raw bytes. True allows modification (write abilities) of GBX headers.</param>
    /// <param name="logger">Logger.</param>
    /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast.</returns>
    /// <exception cref="NotAGbxException">The stream is not a Gbx stream nor starting at the correct position.</exception>
    /// <exception cref="VersionNotSupportedException">Version of Gbx below 3 is not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted Gbx is not supported.</exception>
    /// <exception cref="CompressedRefTableException">Compressed reference is not supported.</exception>
    /// <exception cref="InvalidDataException">Number of reference files is below zero.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public static GameBox ParseHeader(Stream stream,
                                      IProgress<GameBoxReadProgress>? progress = null,
                                      bool readRawBody = false,
                                      ILogger? logger = null)
    {
        using var r = new GameBoxReader(stream, logger: logger);

        var gbx = ParseHeaderWithoutProcessing(r, progress, logger, out Type? classType, out bool isRefTableCompressed);

        // I didn't see a compressed reference table yet
        // if anyone did, let me know!
        if (isRefTableCompressed)
        {
            return gbx;
        }

        var header = gbx.header;

        // Raw body can be read at pretty much any time
        if (readRawBody)
        {
            gbx.RawBody = Body.ParseRaw(header.CompressionOfBody, r);
        }

        if (classType is null)
        {
            return gbx;
        }

        gbx.Node = NodeCacheManager.GetClassConstructor(header.Id)();
        gbx.Node.SetGbx(gbx);

        if (header.UserData.Length == 0)
        {
            return gbx;
        }

        var stateGuid = StateManager.Shared.CreateState(gbx.refTable);
        ((IState)gbx.Node).StateGuid = stateGuid;

        using var ms = new MemoryStream(header.UserData);
        var headerR = new GameBoxReader(ms, stateGuid, logger: logger);

        Header.ProcessUserData(gbx.Node, classType, headerR, logger);

        return gbx;
    }

    /// <summary>
    /// Parses only the header of the GBX.
    /// </summary>
    /// <param name="fileName">Relative or absolute file path.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readRawBody">If the body should be read in raw bytes. True allows modification (write abilities) of GBX headers.</param>
    /// <param name="logger">Logger.</param>
    /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast.</returns>
    /// <exception cref="NotAGbxException">The stream is not a Gbx stream nor starting at the correct position.</exception>
    /// <exception cref="VersionNotSupportedException">Version of Gbx below 3 is not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted Gbx is not supported.</exception>
    /// <exception cref="CompressedRefTableException">Compressed reference is not supported.</exception>
    /// <exception cref="InvalidDataException">Number of reference files is below zero.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public static GameBox ParseHeader(string fileName,
                                      IProgress<GameBoxReadProgress>? progress = null,
                                      bool readRawBody = false,
                                      ILogger? logger = null)
    {
        using var fs = File.OpenRead(fileName);
        return ParseHeader(fs, progress, readRawBody, logger);
    }

    /// <summary>
    /// Parses only the header of the GBX.
    /// </summary>
    /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an <see cref="InvalidCastException"/>. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
    /// <param name="stream">Stream to read GBX format from.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readRawBody">If the body should be read in raw bytes. True allows modification (write abilities) of GBX headers.</param>
    /// <param name="logger">Logger.</param>
    /// <returns>A GameBox with specified main node type.</returns>
    /// <exception cref="NotAGbxException">The stream is not a Gbx stream nor starting at the correct position.</exception>
    /// <exception cref="VersionNotSupportedException">Version of Gbx below 3 is not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted Gbx is not supported.</exception>
    /// <exception cref="CompressedRefTableException">Compressed reference is not supported.</exception>
    /// <exception cref="InvalidDataException">Number of reference files is below zero.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="InvalidCastException">Instantiated <see cref="GameBox"/> cannot be casted to <see cref="GameBox{T}"/>.</exception>
    public static GameBox<T> ParseHeader<T>(Stream stream,
                                            IProgress<GameBoxReadProgress>? progress = null,
                                            bool readRawBody = false,
                                            ILogger? logger = null) where T : Node
    {
        return (GameBox<T>)ParseHeader(stream, progress, readRawBody, logger);
    }

    /// <summary>
    /// Parses only the header of the GBX.
    /// </summary>
    /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an <see cref="InvalidCastException"/>. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
    /// <param name="fileName">Relative or absolute file path.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readRawBody">If the body should be read in raw bytes. True allows modification (write abilities) of GBX headers.</param>
    /// <param name="logger">Logger.</param>
    /// <returns>A GameBox with specified main node type.</returns>
    /// <exception cref="NotAGbxException">The stream is not a Gbx stream nor starting at the correct position.</exception>
    /// <exception cref="VersionNotSupportedException">Version of Gbx below 3 is not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted Gbx is not supported.</exception>
    /// <exception cref="CompressedRefTableException">Compressed reference is not supported.</exception>
    /// <exception cref="InvalidDataException">Number of reference files is below zero.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="InvalidCastException">Instantiated <see cref="GameBox"/> cannot be casted to <see cref="GameBox{T}"/>.</exception>
    public static GameBox<T> ParseHeader<T>(string fileName,
                                            IProgress<GameBoxReadProgress>? progress = null,
                                            bool readRawBody = false,
                                            ILogger? logger = null) where T : Node
    {
        using var fs = File.OpenRead(fileName);
        return ParseHeader<T>(fs, progress, readRawBody, logger);
    }

    /// <summary>
    /// Easily parses GBX format.
    /// </summary>
    /// <param name="stream">Stream to read GBX format from.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
    /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
    /// <param name="logger">Logger.</param>
    /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast).</returns>
    /// <exception cref="MissingLzoException">LZO algorithm is missing.</exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    /// <exception cref="NotAGbxException">The stream is not a Gbx stream nor starting at the correct position.</exception>
    /// <exception cref="VersionNotSupportedException">Version of Gbx below 3 is not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted Gbx is not supported.</exception>
    /// <exception cref="CompressedRefTableException">Compressed reference is not supported.</exception>
    /// <exception cref="InvalidDataException">Number of reference files is below zero.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public static GameBox Parse(Stream stream,
                                IProgress<GameBoxReadProgress>? progress = null,
                                bool readUncompressedBodyDirectly = false,
                                ILogger? logger = null)
    {
        var gbx = ParseHeader(stream, progress, logger: logger);

        var header = gbx.header;
        var node = gbx.Node;

        // When the node type isn't recognized, there's also no node instance
        // Node instance is required for reading the body
        if (node is null)
        {
            return gbx;
        }

        Guid stateGuid;

        if (header.UserData.Length == 0)
        {
            stateGuid = StateManager.Shared.CreateState(gbx.refTable);
            ((IState)node).StateGuid = stateGuid;
        }
        else
        {
            stateGuid = ((IState)node).StateGuid.GetValueOrDefault();
        }

        using var bodyR = new GameBoxReader(stream, stateGuid, logger: logger);

        // Body resets Id (lookback string) list
        Body.Read(node, header, bodyR, progress, readUncompressedBodyDirectly, logger);

        return gbx;
    }

    /// <summary>
    /// Easily parses a GBX file.
    /// </summary>
    /// <param name="fileName">Relative or absolute file path.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
    /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
    /// <param name="logger">Logger.</param>
    /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast).</returns>
    /// <exception cref="MissingLzoException">LZO algorithm is missing.</exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    /// <exception cref="NotAGbxException">The stream is not a Gbx stream nor starting at the correct position.</exception>
    /// <exception cref="VersionNotSupportedException">Version of Gbx below 3 is not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted Gbx is not supported.</exception>
    /// <exception cref="CompressedRefTableException">Compressed reference is not supported.</exception>
    /// <exception cref="InvalidDataException">Number of reference files is below zero.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    public static GameBox Parse(string fileName,
                                IProgress<GameBoxReadProgress>? progress = null,
                                bool readUncompressedBodyDirectly = false,
                                ILogger? logger = null)
    {
        using var fs = File.OpenRead(fileName);
        return Parse(fs, progress, readUncompressedBodyDirectly, logger);
    }

    /// <summary>
    /// Easily parses GBX format.
    /// </summary>
    /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an <see cref="InvalidCastException"/>. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
    /// <param name="stream">Stream to read GBX format from.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
    /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
    /// <param name="logger">Logger.</param>
    /// <returns>A GameBox with specified main node type.</returns>
    /// <exception cref="MissingLzoException"/>
    /// <exception cref="InvalidCastException"/>
    /// <exception cref="NotSupportedException"/>
    /// <exception cref="GameBoxParseException"/>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public static GameBox<T> Parse<T>(Stream stream,
                                      IProgress<GameBoxReadProgress>? progress = null,
                                      bool readUncompressedBodyDirectly = false,
                                      ILogger? logger = null) where T : Node
    {
        return (GameBox<T>)Parse(stream, progress, readUncompressedBodyDirectly, logger);
    }

    /// <summary>
    /// Easily parses a GBX file.
    /// </summary>
    /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an <see cref="InvalidCastException"/>. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
    /// <param name="fileName">Relative or absolute file path.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
    /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
    /// <param name="logger">Logger.</param>
    /// <returns>A GameBox with specified main node type.</returns>
    /// <exception cref="MissingLzoException"/>
    /// <exception cref="InvalidCastException"/>
    /// <exception cref="NotSupportedException"/>
    /// <exception cref="GameBoxParseException"/>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public static GameBox<T> Parse<T>(string fileName,
                                      IProgress<GameBoxReadProgress>? progress = null,
                                      bool readUncompressedBodyDirectly = false,
                                      ILogger? logger = null) where T : Node
    {
        using var fs = File.OpenRead(fileName);
        return Parse<T>(fs, progress, readUncompressedBodyDirectly, logger);
    }

    /// <summary>
    /// Parses only the header of the Gbx and returns the node of it. <see cref="GameBox"/> class is not accessible this way.
    /// </summary>
    /// <param name="stream">Stream to read GBX format from.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readRawBody">If the body should be read in raw bytes. True allows modification (write abilities) of GBX headers.</param>
    /// <param name="logger">Logger.</param>
    /// <returns>A <see cref="NET.Node"/> with either basic information only (if unknown), or also with specified node information (available by using an explicit cast).</returns>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static Node? ParseNodeHeader(Stream stream,
                                        IProgress<GameBoxReadProgress>? progress = null,
                                        bool readRawBody = false,
                                        ILogger? logger = null)
    {
        return ParseHeader(stream, progress, readRawBody, logger);
    }

    /// <summary>
    /// Parses only the header of the GBX and returns the node of it. <see cref="GameBox"/> class is not accessible this way.
    /// </summary>
    /// <param name="fileName">Relative or absolute file path.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readRawBody">If the body should be read in raw bytes. True allows modification (write abilities) of GBX headers.</param>
    /// <param name="logger">Logger.</param>
    /// <returns>A <see cref="NET.Node"/> with either basic information only (if unknown), or also with specified node information (available by using an explicit cast).</returns>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static Node? ParseNodeHeader(string fileName,
                                        IProgress<GameBoxReadProgress>? progress = null,
                                        bool readRawBody = false,
                                        ILogger? logger = null)
    {
        return ParseHeader(fileName, progress, readRawBody, logger);
    }

    /// <summary>
    /// Parses only the header of the GBX and returns the node of it. <see cref="GameBox"/> class is not accessible this way.
    /// </summary>
    /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an <see cref="InvalidCastException"/>. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
    /// <param name="stream">Stream to read GBX format from.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readRawBody">If the body should be read in raw bytes. True allows modification (write abilities) of GBX headers.</param>
    /// <param name="logger">Logger.</param>
    /// <returns>A <see cref="NET.Node"/> casted to <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidCastException"/>
    /// <exception cref="GameBoxParseException"></exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static T ParseNodeHeader<T>(Stream stream,
                                       IProgress<GameBoxReadProgress>? progress = null,
                                       bool readRawBody = false,
                                       ILogger? logger = null) where T : Node
    {
        return ParseHeader<T>(stream, progress, readRawBody, logger);
    }

    /// <summary>
    /// Parses only the header of the GBX and returns the node of it. <see cref="GameBox"/> class is not accessible this way.
    /// </summary>
    /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an <see cref="InvalidCastException"/>. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
    /// <param name="fileName">Relative or absolute file path.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readRawBody">If the body should be read in raw bytes. True allows modification (write abilities) of GBX headers.</param>
    /// <param name="logger">Logger.</param>
    /// <returns>A <see cref="NET.Node"/> casted to <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidCastException"/>
    /// <exception cref="GameBoxParseException"></exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static T ParseNodeHeader<T>(string fileName,
                                       IProgress<GameBoxReadProgress>? progress = null,
                                       bool readRawBody = false,
                                       ILogger? logger = null) where T : Node
    {
        return ParseHeader<T>(fileName, progress, readRawBody, logger);
    }

    /// <summary>
    /// Easily parses GBX format and returns the node of it. <see cref="GameBox"/> class is not accessible this way.
    /// </summary>
    /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an <see cref="InvalidCastException"/>. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
    /// <param name="stream">Stream to read GBX format from.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
    /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
    /// <param name="logger">Logger.</param>
    /// <returns>A <see cref="NET.Node"/> casted to <typeparamref name="T"/>.</returns>
    /// <exception cref="MissingLzoException"/>
    /// <exception cref="InvalidCastException"/>
    /// <exception cref="NotSupportedException"/>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public static T ParseNode<T>(Stream stream,
                                 IProgress<GameBoxReadProgress>? progress = null,
                                 bool readUncompressedBodyDirectly = false,
                                 ILogger? logger = null) where T : Node
    {
        return Parse<T>(stream, progress, readUncompressedBodyDirectly, logger);
    }

    /// <summary>
    /// Easily parses a GBX file and returns the node of it. <see cref="GameBox"/> class is not accessible this way.
    /// </summary>
    /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an <see cref="InvalidCastException"/>. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
    /// <param name="fileName">Relative or absolute file path.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
    /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
    /// <param name="logger">Logger.</param>
    /// <returns>A <see cref="NET.Node"/> casted to <typeparamref name="T"/>.</returns>
    /// <exception cref="MissingLzoException"/>
    /// <exception cref="InvalidCastException"/>
    /// <exception cref="NotSupportedException"/>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public static T ParseNode<T>(string fileName,
                                 IProgress<GameBoxReadProgress>? progress = null,
                                 bool readUncompressedBodyDirectly = false,
                                 ILogger? logger = null) where T : Node
    {
        return Parse<T>(fileName, progress, readUncompressedBodyDirectly, logger);
    }

    /// <summary>
    /// Easily parses a GBX file and returns the node of it. <see cref="GameBox"/> class is not accessible this way.
    /// </summary>
    /// <param name="fileName">Relative or absolute file path.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
    /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
    /// <param name="logger">Logger.</param>
    /// <returns>A <see cref="NET.Node"/> with either basic information only (if unknown), or also with specified node information (available by using an explicit cast).</returns>
    /// <exception cref="MissingLzoException"/>
    /// <exception cref="NotSupportedException"/>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public static Node? ParseNode(string fileName,
                                  IProgress<GameBoxReadProgress>? progress = null,
                                  bool readUncompressedBodyDirectly = false,
                                  ILogger? logger = null)
    {
        return Parse(fileName, progress, readUncompressedBodyDirectly, logger);
    }

    /// <summary>
    /// Easily parses GBX format and returns the node of it. <see cref="GameBox"/> class is not accessible this way.
    /// </summary>
    /// <param name="stream">Stream to read GBX format from.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
    /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
    /// <param name="logger">Logger.</param>
    /// <returns>A <see cref="NET.Node"/> with either basic information only (if unknown), or also with specified node information (available by using an explicit cast).</returns>
    /// <exception cref="MissingLzoException"/>
    /// <exception cref="NotSupportedException"/>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public static Node? ParseNode(Stream stream,
                                  IProgress<GameBoxReadProgress>? progress = null,
                                  bool readUncompressedBodyDirectly = false,
                                  ILogger? logger = null)
    {
        return Parse(stream, progress, readUncompressedBodyDirectly, logger);
    }

    public static async Task<GameBox> ParseAsync(Stream stream,
                                                 bool readUncompressedBodyDirectly = false,
                                                 ILogger? logger = null,
                                                 GameBoxAsyncReadAction? asyncAction = null,
                                                 CancellationToken cancellationToken = default)
    {
        var gbx = ParseHeader(stream, logger: logger);

        var header = gbx.header;
        var node = gbx.Node;

        // When the node type isn't recognized, there's also no node instance
        // Node instance is required for reading the body
        if (node is null)
        {
            return gbx;
        }

        Guid stateGuid;

        if (header.UserData.Length == 0)
        {
            stateGuid = StateManager.Shared.CreateState(gbx.refTable);
            ((IState)node).StateGuid = stateGuid;
        }
        else
        {
            stateGuid = ((IState)node).StateGuid.GetValueOrDefault();
        }

        using var bodyR = new GameBoxReader(stream, stateGuid, asyncAction, logger);

        // Body resets Id (lookback string) list
        await Body.ReadAsync(node, header, bodyR, readUncompressedBodyDirectly, logger, asyncAction, cancellationToken);

        return gbx;
    }

    public static async Task<GameBox<T>> ParseAsync<T>(Stream stream,
                                                       bool readUncompressedBodyDirectly = false,
                                                       ILogger? logger = null,
                                                       GameBoxAsyncReadAction? asyncAction = null,
                                                       CancellationToken cancellationToken = default)
                                                       where T : Node
    {
        return (GameBox<T>)await ParseAsync(stream, readUncompressedBodyDirectly, logger, asyncAction, cancellationToken);
    }

    public static async Task<Node?> ParseNodeAsync(Stream stream,
                                                   bool readUncompressedBodyDirectly = false,
                                                   ILogger? logger = null,
                                                   GameBoxAsyncReadAction? asyncAction = null,
                                                   CancellationToken cancellationToken = default)
    {
        return await ParseAsync(stream, readUncompressedBodyDirectly, logger, asyncAction, cancellationToken);
    }

    public static async Task<T> ParseNodeAsync<T>(Stream stream,
                                                  bool readUncompressedBodyDirectly = false,
                                                  ILogger? logger = null,
                                                  GameBoxAsyncReadAction? asyncAction = null,
                                                  CancellationToken cancellationToken = default)
                                                  where T : Node
    {
        return await ParseAsync<T>(stream, readUncompressedBodyDirectly, logger, asyncAction, cancellationToken);
    }
}
