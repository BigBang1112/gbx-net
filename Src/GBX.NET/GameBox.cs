﻿using System.Diagnostics;
using System.Reflection;

namespace GBX.NET;

/// <summary>
/// An unknown serialized GameBox node with additional attributes. This class can represent deserialized .Gbx file.
/// </summary>
public class GameBox
{
    public const string Magic = "GBX";

    /// <summary>
    /// Header part containing generic GameBox values.
    /// </summary>
    public GameBoxHeaderInfo Header { get; }

    /// <summary>
    /// Reference table, referencing other GBX.
    /// </summary>
    public GameBoxRefTable? RefTable { get; private set; }

    public GameBoxBody? Body { get; protected set; }

    public CMwNod? Node { get; internal set; }

    /// <summary>
    /// ID of the node.
    /// </summary>
    public uint? ID
    {
        get => Header.ID;
        internal set => Header.ID = value;
    }

    /// <summary>
    /// File path of the GameBox.
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// Tells the library to save this GBX with correct IDs related to the game version.
    /// </summary>
    public IDRemap Remap { get; set; }

    /// <summary>
    /// Creates an empty GameBox object version 6.
    /// </summary>
    private GameBox(uint id)
    {
        Header = new GameBoxHeaderInfo(id);
        Node = null!;
    }

    /// <summary>
    /// Creates an empty GameBox object based on defined <see cref="GameBoxHeaderInfo"/>.
    /// </summary>
    /// <param name="headerInfo">Header info to use.</param>
    protected GameBox(GameBoxHeaderInfo headerInfo)
    {
        Header = headerInfo ?? throw new ArgumentNullException(nameof(headerInfo));
        Node = null!;
    }

    /// <summary>
    /// Tries to get the <see cref="CMwNod"/> of this GBX.
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="CMwNod"/> to look for.</typeparam>
    /// <param name="node">A node that is being extracted from this <see cref="GameBox"/> object. Null if unsuccessful.</param>
    /// <returns>True if the type of this <see cref="GameBox"/> is <see cref="GameBox{T}"/> and <typeparamref name="T"/> matches. Otherwise false.</returns>
    public bool TryNode<T>(out T? node) where T : CMwNod
    {
        var property = GetType().GetProperty(nameof(Node));

        if (property?.PropertyType == typeof(T))
        {
            node = property.GetValue(this) as T;
            return true;
        }

        node = null;
        return false;
    }

    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    internal bool ReadHeader(GameBoxReader reader, IProgress<GameBoxReadProgress>? progress)
    {
        var success = Header.Read(reader);

        progress?.Report(new GameBoxReadProgress(GameBoxReadProgressStage.Header, 1, this));

        return success;
    }

    protected virtual bool ProcessHeader(IProgress<GameBoxReadProgress>? progress)
    {
        return true; // There are no header chunks to proccess in an unknown GBX
    }

    protected bool ReadRefTable(GameBoxReader reader, IProgress<GameBoxReadProgress>? progress)
    {
        RefTable = new GameBoxRefTable(Header);
        RefTable.Read(reader);

        progress?.Report(new GameBoxReadProgress(GameBoxReadProgressStage.RefTable, 1, this));

        return true;
    }

    protected internal virtual bool ReadBody(GameBoxReader reader, IProgress<GameBoxReadProgress>? progress, bool readUncompressedBodyDirectly)
    {
        return false;
    }

    private void ReadRawBody(GameBoxReader reader)
    {
        if (Header.CompressionOfBody == GameBoxCompression.Uncompressed)
        {
            Body!.RawData = reader.ReadToEnd();
            return;
        }

        Body!.UncompressedSize = reader.ReadInt32();
        Body!.RawData = reader.ReadBytes();
    }

    /// <summary>
    /// Implicitly casts <see cref="GameBox"/> to its <see cref="Node"/>.
    /// </summary>
    /// <param name="gbx"></param>
    public static implicit operator CMwNod?(GameBox gbx) => gbx.Node;

    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    private static GameBox ParseHeader(GameBoxReader reader, IProgress<GameBoxReadProgress>? progress = null, bool readRawBody = false)
    {
        var header = new GameBoxHeaderInfo(reader);

        progress?.Report(new GameBoxReadProgress(header));

        if (!header.ID.HasValue)
            return new GameBox(header);

        GameBox gbx;

        var isNodeAvailable = NodeCacheManager.AvailableClasses.TryGetValue(header.ID.Value, out Type? availableClass);

        if (isNodeAvailable)
        {
            var gbxType = typeof(GameBox<>).MakeGenericType(availableClass!);
            gbx = (GameBox)Activator.CreateInstance(
                gbxType,
                BindingFlags.NonPublic | BindingFlags.Instance,
                binder: null,
                args: new object[] { header },
                culture: null)!;

            var processHeaderMethod = gbxType.GetMethod(nameof(ProcessHeader), BindingFlags.Instance | BindingFlags.NonPublic)!;
            processHeaderMethod.Invoke(gbx, new object?[] { progress });
        }
        else
        {
            gbx = new GameBox(header);
        }

        if (reader.BaseStream is FileStream fs)
            gbx.FileName = fs.Name;

        if (!gbx.ReadRefTable(reader, progress))
            return gbx;

        if (isNodeAvailable && readRawBody)
            gbx.ReadRawBody(reader);

        return gbx;
    }

    /// <summary>
    /// Parses only the header of the GBX.
    /// </summary>
    /// <param name="stream">Stream to read GBX format from.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readRawBody">If the body should be read in raw bytes. True allows modification (write abilities) of GBX headers.</param>
    /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast.</returns>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static GameBox ParseHeader(Stream stream, IProgress<GameBoxReadProgress>? progress = null, bool readRawBody = false)
    {
        using var r = new GameBoxReader(stream);
        return ParseHeader(r, progress, readRawBody);
    }

    /// <summary>
    /// Parses only the header of the GBX.
    /// </summary>
    /// <param name="fileName">Relative or absolute file path.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readRawBody">If the body should be read in raw bytes. True allows modification (write abilities) of GBX headers.</param>
    /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast.</returns>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static GameBox ParseHeader(string fileName, IProgress<GameBoxReadProgress>? progress = null, bool readRawBody = false)
    {
        using var fs = File.OpenRead(fileName);
        var gbx = ParseHeader(fs, progress, readRawBody);
        gbx.FileName = fileName;
        return gbx;
    }

    /// <summary>
    /// Parses only the header of the GBX.
    /// </summary>
    /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
    /// <param name="stream">Stream to read GBX format from.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readRawBody">If the body should be read in raw bytes. True allows modification (write abilities) of GBX headers.</param>
    /// <returns>A GameBox with specified main node type.</returns>
    /// <exception cref="InvalidCastException"/>
    /// <exception cref="GameBoxParseException"/>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static GameBox<T> ParseHeader<T>(Stream stream, IProgress<GameBoxReadProgress>? progress = null, bool readRawBody = false) where T : CMwNod
    {
        var gbx = new GameBox<T>();

        if (stream is FileStream fs)
            gbx.FileName = fs.Name;

        using var r = new GameBoxReader(stream);

        if (!gbx.ReadHeader(r, progress))
            throw new GameBoxParseException();

        if (!gbx.ProcessHeader(progress))
            throw new GameBoxParseException();

        if (!gbx.ReadRefTable(r, progress))
            throw new GameBoxParseException();

        if (readRawBody)
            gbx.ReadRawBody(r);

        return gbx;
    }

    /// <summary>
    /// Parses only the header of the GBX.
    /// </summary>
    /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
    /// <param name="fileName">Relative or absolute file path.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readRawBody">If the body should be read in raw bytes. True allows modification (write abilities) of GBX headers.</param>
    /// <returns>A GameBox with specified main node type.</returns>
    /// <exception cref="InvalidCastException"/>
    /// <exception cref="GameBoxParseException"/>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static GameBox<T> ParseHeader<T>(string fileName, IProgress<GameBoxReadProgress>? progress = null, bool readRawBody = false) where T : CMwNod
    {
        using var fs = File.OpenRead(fileName);
        return ParseHeader<T>(fs, progress, readRawBody);
    }

    /// <summary>
    /// Easily parses GBX format.
    /// </summary>
    /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
    /// <param name="stream">Stream to read GBX format from.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
    /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
    /// <returns>A GameBox with specified main node type.</returns>
    /// <exception cref="MissingLzoException"/>
    /// <exception cref="InvalidCastException"/>
    /// <exception cref="NotSupportedException"/>
    /// <exception cref="GameBoxParseException"/>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public static GameBox<T> Parse<T>(Stream stream, IProgress<GameBoxReadProgress>? progress = null, bool readUncompressedBodyDirectly = false) where T : CMwNod
    {
        var gbx = ParseHeader<T>(stream, progress);

        using var r = new GameBoxReader(stream);

        if (gbx.ReadBody(r, progress, readUncompressedBodyDirectly))
            return gbx;

        throw new GameBoxParseException();
    }

    /// <summary>
    /// Easily parses a GBX file.
    /// </summary>
    /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
    /// <param name="fileName">Relative or absolute file path.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
    /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
    /// <returns>A GameBox with specified main node type.</returns>
    /// <exception cref="MissingLzoException"/>
    /// <exception cref="InvalidCastException"/>
    /// <exception cref="NotSupportedException"/>
    /// <exception cref="GameBoxParseException"/>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public static GameBox<T> Parse<T>(string fileName, IProgress<GameBoxReadProgress>? progress = null, bool readUncompressedBodyDirectly = false) where T : CMwNod
    {
        using var fs = File.OpenRead(fileName);
        return Parse<T>(fs, progress, readUncompressedBodyDirectly) ?? throw new GameBoxParseException();
    }

    /// <summary>
    /// Easily parses a GBX file.
    /// </summary>
    /// <param name="fileName">Relative or absolute file path.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
    /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
    /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast).</returns>
    /// <exception cref="MissingLzoException"/>
    /// <exception cref="NotSupportedException"/>
    /// <exception cref="GameBoxParseException"/>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public static GameBox Parse(string fileName, IProgress<GameBoxReadProgress>? progress = null, bool readUncompressedBodyDirectly = false)
    {
        using var fs = File.OpenRead(fileName);
        return Parse(fs, progress, readUncompressedBodyDirectly) ?? throw new GameBoxParseException();
    }

    /// <summary>
    /// Easily parses GBX format.
    /// </summary>
    /// <param name="stream">Stream to read GBX format from.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
    /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
    /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast).</returns>
    /// <exception cref="MissingLzoException"/>
    /// <exception cref="NotSupportedException"/>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public static GameBox Parse(Stream stream, IProgress<GameBoxReadProgress>? progress = null, bool readUncompressedBodyDirectly = false)
    {
        using var rHeader = new GameBoxReader(stream);

        var gbx = ParseHeader(rHeader, progress);

        // Body resets Id (lookback string) list
        using var rBody = new GameBoxReader(stream, gbx.Body);

        gbx.ReadBody(rBody, progress, readUncompressedBodyDirectly);

        return gbx;
    }

    /// <summary>
    /// Parses only the header of the GBX and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
    /// </summary>
    /// <param name="stream">Stream to read GBX format from.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readRawBody">If the body should be read in raw bytes. True allows modification (write abilities) of GBX headers.</param>
    /// <returns>A <see cref="CMwNod"/> with either basic information only (if unknown), or also with specified node information (available by using an explicit cast).</returns>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static CMwNod? ParseNodeHeader(Stream stream, IProgress<GameBoxReadProgress>? progress = null, bool readRawBody = false)
    {
        return ParseHeader(stream, progress, readRawBody).Node;
    }

    /// <summary>
    /// Parses only the header of the GBX and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
    /// </summary>
    /// <param name="fileName">Relative or absolute file path.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readRawBody">If the body should be read in raw bytes. True allows modification (write abilities) of GBX headers.</param>
    /// <returns>A <see cref="CMwNod"/> with either basic information only (if unknown), or also with specified node information (available by using an explicit cast).</returns>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static CMwNod? ParseNodeHeader(string fileName, IProgress<GameBoxReadProgress>? progress = null, bool readRawBody = false)
    {
        return ParseHeader(fileName, progress, readRawBody);
    }

    /// <summary>
    /// Parses only the header of the GBX and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
    /// </summary>
    /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
    /// <param name="stream">Stream to read GBX format from.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readRawBody">If the body should be read in raw bytes. True allows modification (write abilities) of GBX headers.</param>
    /// <returns>A <see cref="CMwNod"/> casted to <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidCastException"/>
    /// <exception cref="GameBoxParseException"></exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static T ParseNodeHeader<T>(Stream stream, IProgress<GameBoxReadProgress>? progress = null, bool readRawBody = false) where T : CMwNod
    {
        return ParseHeader<T>(stream, progress, readRawBody);
    }

    /// <summary>
    /// Parses only the header of the GBX and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
    /// </summary>
    /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
    /// <param name="fileName">Relative or absolute file path.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readRawBody">If the body should be read in raw bytes. True allows modification (write abilities) of GBX headers.</param>
    /// <returns>A <see cref="CMwNod"/> casted to <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidCastException"/>
    /// <exception cref="GameBoxParseException"></exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static T ParseNodeHeader<T>(string fileName, IProgress<GameBoxReadProgress>? progress = null, bool readRawBody = false) where T : CMwNod
    {
        return ParseHeader<T>(fileName, progress, readRawBody);
    }

    /// <summary>
    /// Easily parses GBX format and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
    /// </summary>
    /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
    /// <param name="stream">Stream to read GBX format from.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
    /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
    /// <returns>A <see cref="CMwNod"/> casted to <typeparamref name="T"/>.</returns>
    /// <exception cref="MissingLzoException"/>
    /// <exception cref="InvalidCastException"/>
    /// <exception cref="NotSupportedException"/>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public static T ParseNode<T>(Stream stream, IProgress<GameBoxReadProgress>? progress = null, bool readUncompressedBodyDirectly = false) where T : CMwNod
    {
        return Parse<T>(stream, progress, readUncompressedBodyDirectly);
    }

    /// <summary>
    /// Easily parses a GBX file and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
    /// </summary>
    /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
    /// <param name="fileName">Relative or absolute file path.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
    /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
    /// <returns>A <see cref="CMwNod"/> casted to <typeparamref name="T"/>.</returns>
    /// <exception cref="MissingLzoException"/>
    /// <exception cref="InvalidCastException"/>
    /// <exception cref="NotSupportedException"/>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public static T ParseNode<T>(string fileName, IProgress<GameBoxReadProgress>? progress = null, bool readUncompressedBodyDirectly = false) where T : CMwNod
    {
        return Parse<T>(fileName, progress, readUncompressedBodyDirectly);
    }

    /// <summary>
    /// Easily parses a GBX file and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
    /// </summary>
    /// <param name="fileName">Relative or absolute file path.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
    /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
    /// <returns>A <see cref="CMwNod"/> with either basic information only (if unknown), or also with specified node information (available by using an explicit cast).</returns>
    /// <exception cref="MissingLzoException"/>
    /// <exception cref="NotSupportedException"/>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public static CMwNod? ParseNode(string fileName, IProgress<GameBoxReadProgress>? progress = null, bool readUncompressedBodyDirectly = false)
    {
        return Parse(fileName, progress, readUncompressedBodyDirectly);
    }

    /// <summary>
    /// Easily parses GBX format and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
    /// </summary>
    /// <param name="stream">Stream to read GBX format from.</param>
    /// <param name="progress">Callback that reports any read progress.</param>
    /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
    /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
    /// <returns>A <see cref="CMwNod"/> with either basic information only (if unknown), or also with specified node information (available by using an explicit cast).</returns>
    /// <exception cref="MissingLzoException"/>
    /// <exception cref="NotSupportedException"/>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public static CMwNod? ParseNode(Stream stream, IProgress<GameBoxReadProgress>? progress = null, bool readUncompressedBodyDirectly = false)
    {
        return Parse(stream, progress, readUncompressedBodyDirectly);
    }

    private static uint? ReadNodeID(GameBoxReader reader)
    {
        if (!reader.HasMagic(Magic)) // If the file doesn't have GBX magic
            return null;

        var version = reader.ReadInt16(); // Version

        if (version < 3)
            return null;

        reader.ReadBytes(3);

        if (version >= 4)
            reader.ReadByte();

        return reader.ReadUInt32();
    }

    /// <summary>
    /// Reads the GBX node ID the quickest possible way.
    /// </summary>
    /// <param name="stream">Stream to read from.</param>
    /// <returns>ID of the main node presented in GBX. Null if not a GBX stream.</returns>
    public static uint? ReadNodeID(Stream stream)
    {
        using var r = new GameBoxReader(stream);
        return ReadNodeID(r);
    }

    /// <summary>
    /// Reads the GBX node ID the quickest possible way.
    /// </summary>
    /// <param name="fileName">File to read from.</param>
    /// <returns>ID of the main node presented in GBX. Null if not a GBX stream.</returns>
    public static uint? ReadNodeID(string fileName)
    {
        using var fs = File.OpenRead(fileName);
        return ReadNodeID(fs);
    }

    /// <summary>
    /// Reads the type of the main node from GBX file.
    /// </summary>
    /// <param name="fileName">File to read from.</param>
    /// <returns>Type of the main node.</returns>
    public static Type? ReadNodeType(string fileName)
    {
        using var fs = File.OpenRead(fileName);
        return ReadNodeType(fs);
    }

    /// <summary>
    /// Reads the type of the main node from GBX stream.
    /// </summary>
    /// <param name="stream">Stream to read from.</param>
    /// <returns>Type of the main node.</returns>
    public static Type? ReadNodeType(Stream stream)
    {
        using var r = new GameBoxReader(stream);
        return ReadNodeType(r);
    }

    private static Type? ReadNodeType(GameBoxReader reader)
    {
        var classID = ReadNodeID(reader);

        if (!classID.HasValue)
            return null;

        var modernID = classID.GetValueOrDefault();
        if (NodeCacheManager.Mappings.TryGetValue(classID.GetValueOrDefault(), out uint newerClassID))
            modernID = newerClassID;

        Debug.WriteLine("GetGameBoxType: " + modernID.ToString("x8"));

        var availableClass = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass
                && x.Namespace?.StartsWith("GBX.NET.Engines") == true && x.IsSubclassOf(typeof(CMwNod))
                && x.GetCustomAttribute<NodeAttribute>()?.ID == modernID).FirstOrDefault();

        if (availableClass is null)
            return null;

        return typeof(GameBox<>).MakeGenericType(availableClass);
    }

    /// <summary>
    /// Decompresses the body part of the GBX file, also setting the header parameter so that the outputted GBX file is compatible with the game. If the file is already detected decompressed, the input is just copied over to the output.
    /// </summary>
    /// <param name="input">GBX stream to decompress.</param>
    /// <param name="output">Output GBX stream in the decompressed form.</param>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">One of the streams is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="VersionNotSupportedException">GBX files below version 3 are not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static void Decompress(Stream input, Stream output)
    {
        using var r = new GameBoxReader(input);
        using var w = new GameBoxWriter(output);

        // Magic
        if (!r.HasMagic(Magic))
            throw new Exception();

        w.Write(Magic, StringLengthPrefix.None);

        // Version
        var version = r.ReadInt16();

        if (version < 3)
            throw new VersionNotSupportedException(version);

        w.Write(version);

        // Format
        var format = r.ReadByte();

        if (format != 'B')
            throw new TextFormatNotSupportedException();

        w.Write(format);

        // Ref table compression
        w.Write(r.ReadByte());

        // Body compression type
        var compressedBody = r.ReadByte();

        if (compressedBody != 'C')
        {
            input.CopyTo(output);
            return;
        }

        w.Write('U');

        // Unknown byte
        if (version >= 4)
            w.Write(r.ReadByte());

        // Id
        w.Write(r.ReadInt32());

        // User data
        if (version >= 6)
        {
            var bytes = r.ReadBytes();
            w.Write(bytes.Length);
            w.WriteBytes(bytes);
        }

        // Num nodes
        w.Write(r.ReadInt32());

        // Ref table, TODO: full read
        w.Write(r.ReadInt32());

        var uncompressedSize = r.ReadInt32();
        var compressedData = r.ReadBytes();

        var buffer = new byte[uncompressedSize];
        Lzo.Decompress(compressedData, buffer);
        w.WriteBytes(buffer);
    }

    /// <summary>
    /// Decompresses the body part of the GBX file, also setting the header parameter so that the outputted GBX file is compatible with the game. If the file is already detected decompressed, the input is just copied over to the output.
    /// </summary>
    /// <param name="inputFileName">GBX file to decompress.</param>
    /// <param name="output">Output GBX stream in the decompressed form.</param>
    /// <exception cref="ArgumentException"><paramref name="inputFileName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.InvalidPathChars"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="inputFileName"/> is null.</exception>
    /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
    /// <exception cref="UnauthorizedAccessException"><paramref name="inputFileName"/> specified a directory. -or- The caller does not have the required permission.</exception>
    /// <exception cref="FileNotFoundException">The file specified in path was not found.</exception>
    /// <exception cref="NotSupportedException"><paramref name="inputFileName"/> is in an invalid format.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">One of the streams is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="VersionNotSupportedException">GBX files below version 3 are not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static void Decompress(string inputFileName, Stream output)
    {
        using var fs = File.OpenRead(inputFileName);
        Decompress(fs, output);
    }

    /// <summary>
    /// Decompresses the body part of the GBX file, also setting the header parameter so that the outputted GBX file is compatible with the game. If the file is already detected decompressed, the input is just copied over to the output.
    /// </summary>
    /// <param name="input">GBX stream to decompress.</param>
    /// <param name="outputFileName">Output GBX file in the decompressed form.</param>
    /// <exception cref="ArgumentException"><paramref name="outputFileName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.InvalidPathChars"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="outputFileName"/> is null.</exception>
    /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
    /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission. -or- path specified a file that is read-only.</exception>
    /// <exception cref="NotSupportedException"><paramref name="outputFileName"/> is in an invalid format.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">One of the streams is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="VersionNotSupportedException">GBX files below version 3 are not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static void Decompress(Stream input, string outputFileName)
    {
        using var fs = File.Create(outputFileName);
        Decompress(input, fs);
    }

    /// <summary>
    /// Decompresses the body part of the GBX file, also setting the header parameter so that the outputted GBX file is compatible with the game. If the file is already detected decompressed, the input is just copied over to the output.
    /// </summary>
    /// <param name="inputFileName">GBX file to decompress.</param>
    /// <param name="outputFileName">Output GBX file in the decompressed form.</param>
    /// <exception cref="ArgumentException"><paramref name="inputFileName"/> or <paramref name="outputFileName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.InvalidPathChars"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="inputFileName"/> or <paramref name="outputFileName"/> is null.</exception>
    /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
    /// <exception cref="UnauthorizedAccessException"><paramref name="inputFileName"/> or <paramref name="outputFileName"/> specified a directory. -or- The caller does not have the required permission.  -or- path specified a file that is read-only.</exception>
    /// <exception cref="FileNotFoundException">The file specified in path was not found.</exception>
    /// <exception cref="NotSupportedException"><paramref name="inputFileName"/> is in an invalid format.</exception>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="ObjectDisposedException">One of the streams is closed.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="VersionNotSupportedException">GBX files below version 3 are not supported.</exception>
    /// <exception cref="TextFormatNotSupportedException">Text-formatted GBX files are not supported.</exception>
    public static void Decompress(string inputFileName, string outputFileName)
    {
        using var fsInput = File.OpenRead(inputFileName);
        using var fsOutput = File.Create(outputFileName);
        Decompress(fsInput, fsOutput);
    }
}
