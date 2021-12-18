using System.Collections;
using System.Reflection;

namespace GBX.NET;

/// <summary>
/// A known serialized GameBox node with additional attributes. This class can represent deserialized .Gbx file.
/// </summary>
/// <typeparam name="T">The main node of the GBX. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
public class GameBox<T> : GameBox where T : CMwNod
{
    /// <summary>
    /// Header part specialized for <typeparamref name="T"/>, typically storing metadata for quickest access.
    /// </summary>
    public new GameBoxHeader<T> Header { get; }

    /// <summary>
    /// Body part, storing information about the node that realistically affects the game.
    /// </summary>
    public new GameBoxBody<T>? Body
    {
        get => base.Body as GameBoxBody<T>;
        protected set => base.Body = value;
    }

    /// <summary>
    /// Deserialized node from GBX.
    /// </summary>
    public new T Node
    {
        get => (T)base.Node!;
        set => base.Node = value;
    }

    /// <summary>
    /// Creates an empty GameBox object version 6.
    /// </summary>
    internal GameBox() : this(new GameBoxHeaderInfo(typeof(T).GetCustomAttribute<NodeAttribute>()!.ID))
    {

    }

    /// <summary>
    /// Creates an empty GameBox object based on defined <see cref="GameBoxHeaderInfo"/>.
    /// </summary>
    /// <param name="header">Header info to use.</param>
    private GameBox(GameBoxHeaderInfo header) : base(header)
    {
        Header = new GameBoxHeader<T>(this);
        Body = new GameBoxBody<T>(this);

        Node = (T)Activator.CreateInstance(typeof(T), true)!;
        Node.SetIDAndChunks();
        Node.GBX = this;
    }

    /// <summary>
    /// Creates a GameBox object based on an existing node. Useful for saving nodes to GBX files.
    /// </summary>
    /// <param name="node">Node to wrap.</param>
    /// <param name="headerInfo">Header info to use.</param>
    public GameBox(T node, GameBoxHeaderInfo? headerInfo = null) : this(headerInfo ?? new GameBoxHeaderInfo(node.ID))
    {
        Node = node;
        ID = node.ID;

        // It needs to be sure that GBX is assigned correctly to every node
        AssignGbxToNode();
    }

    private void AssignGbxToNode()
    {
        AssignGbxToNode(this, Node);
    }

    private void AssignGbxToNode(GameBox gbx, CMwNod? n)
    {
        if (n is null) return;

        n.GBX = gbx; // Assign the GBX body to this body

        var type = n.GetType();

        foreach (var prop in type.GetProperties()) // Go through all properties of a node
        {
            if (!Attribute.IsDefined(prop, typeof(NodeMemberAttribute))) // Check only NodeMember attributes
                continue;

            if (prop.PropertyType.IsSubclassOf(typeof(CMwNod))) // If the property is Node
            {
                AssignGbxToNode(gbx, prop.GetValue(n) as CMwNod); // Recurse through the node
                continue;
            }
            
            if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType)) // If the property is a list of something
            {
                // If the list has a generic argument of anu kind of Node
                if (Array.Find(prop.PropertyType.GetGenericArguments(), x => x.IsSubclassOf(typeof(CMwNod))) is null)
                    continue;

                // Go through each Node and recurse
                if (prop.GetValue(n) is not IEnumerable enumerable)
                    continue;

                foreach (var e in enumerable)
                    AssignGbxToNode(gbx, (CMwNod)e);
            }
        }
    }

    /// <summary>
    /// Creates a header chunk based on the attributes from <typeparamref name="TChunk"/>.
    /// </summary>
    /// <typeparam name="TChunk">A chunk type compatible with <see cref="IHeaderChunk"/>.</typeparam>
    /// <returns>A newly created chunk.</returns>
    public TChunk CreateHeaderChunk<TChunk>() where TChunk : Chunk, IHeaderChunk
    {
        return Header.Chunks.Create<TChunk>();
    }

    /// <summary>
    /// Removes all header chunks.
    /// </summary>
    public void RemoveAllHeaderChunks()
    {
        Header.Chunks.Clear();
    }

    /// <summary>
    /// Removes a header chunk based on the attributes from <typeparamref name="TChunk"/>.
    /// </summary>
    /// <typeparam name="TChunk">A chunk type compatible with <see cref="IHeaderChunk"/>.</typeparam>
    /// <returns>True, if the chunk was removed, otherwise false.</returns>
    public bool RemoveHeaderChunk<TChunk>() where TChunk : Chunk, IHeaderChunk
    {
        return Header.Chunks.RemoveWhere(x => x.ID == typeof(TChunk).GetCustomAttribute<ChunkAttribute>()?.ID) > 0;
    }

    /// <summary>
    /// Creates a body chunk based on the attributes from <typeparamref name="TChunk"/>.
    /// </summary>
    /// <typeparam name="TChunk">A chunk type that isn't a header chunk.</typeparam>
    /// <param name="data">If the chunk is <see cref="ISkippableChunk"/>, the bytes to initiate the chunks with, unparsed. If it's not a skippable chunk, data is parsed immediately.</param>
    /// <returns>A newly created chunk.</returns>
    public TChunk CreateBodyChunk<TChunk>(byte[] data) where TChunk : Chunk
    {
        return Node.Chunks.Create<TChunk>(data);
    }

    /// <summary>
    /// Creates a body chunk based on the attributes from <typeparamref name="TChunk"/> with no inner data provided.
    /// </summary>
    /// <typeparam name="TChunk">A chunk type that isn't a header chunk.</typeparam>
    /// <returns>A newly created chunk.</returns>
    public TChunk CreateBodyChunk<TChunk>() where TChunk : Chunk
    {
        return CreateBodyChunk<TChunk>(Array.Empty<byte>());
    }

    /// <summary>
    /// Removes all body chunks.
    /// </summary>
    public void RemoveAllBodyChunks()
    {
        Node.Chunks.Clear();
    }

    /// <summary>
    /// Removes a body chunk based on the attributes from <typeparamref name="TChunk"/>.
    /// </summary>
    /// <typeparam name="TChunk">A chunk type that isn't a header chunk.</typeparam>
    /// <returns>True, if the chunk was removed, otherwise false.</returns>
    public bool RemoveBodyChunk<TChunk>() where TChunk : Chunk
    {
        return Node.Chunks.Remove<TChunk>();
    }

    /// <summary>
    /// Discovers all chunks in the GBX.
    /// </summary>
    /// <exception cref="AggregateException"/>
    public void DiscoverAllChunks()
    {
        Node.DiscoverAllChunks();
    }

    protected override bool ProcessHeader(IProgress<GameBoxReadProgress>? progress)
    {
        if (ID.HasValue && ID != typeof(T).GetCustomAttribute<NodeAttribute>()?.ID)
        {
            if (!NodeCacheManager.Names.TryGetValue(ID.Value, out string? name) || name is null)
                name = "unknown class";
            throw new InvalidCastException($"GBX with ID 0x{ID:X8} ({name}) can't be casted to GameBox<{typeof(T).Name}>.");
        }

        Log.Write("Working out the header chunks...");

        try
        {
            Header.Read(Header.UserData, progress);
            Log.Write("Header chunks parsed without any exceptions.", ConsoleColor.Green);
        }
        catch (Exception e)
        {
            Log.Write("Header chunks parsed with exceptions.", ConsoleColor.Red);
            Log.Write(e.ToString(), ConsoleColor.Red);
        }

        progress?.Report(new GameBoxReadProgress(GameBoxReadProgressStage.HeaderUserData, 1, this));

        return true;
    }

    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    protected internal override bool ReadBody(GameBoxReader reader, IProgress<GameBoxReadProgress>? progress, bool readUncompressedBodyDirectly)
    {
        if (Body is null)
            return false;

        Log.Write("Reading the body...");

        switch (Header.CompressionOfBody)
        {
            case GameBoxCompression.Compressed:
                var uncompressedSize = reader.ReadInt32();
                var compressedSize = reader.ReadInt32();

                var data = reader.ReadBytes(compressedSize);
                Body.Read(data, uncompressedSize, progress);

                break;
            case GameBoxCompression.Uncompressed:
                if (readUncompressedBodyDirectly)
                {
                    Body.Read(reader, progress);
                }
                else
                {
                    var uncompressedData = reader.ReadToEnd();
                    Body.Read(uncompressedData, progress);
                }
                break;
            default:
                Log.Write("Body can't be read!", ConsoleColor.Red);
                return false;
        }

        Log.Write("Body completed!");

        return true;
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="HeaderOnlyParseLimitationException">Writing is not supported in <see cref="GameBox"/> where only the header was parsed (without raw body being read).</exception>
    internal void Write(Stream stream, IDRemap remap)
    {
        if (Body is null)
            return;

        // It needs to be sure that the Body and Part are assigned to the correct GameBox body
        AssignGbxToNode();

        (Body as ILookbackable).IdWritten = false;
        (Body as ILookbackable).IdStrings.Clear();
        Body.AuxilaryNodes.Clear();

        Log.Write("Writing the body...");

        using var ms = new MemoryStream();
        using var bodyW = new GameBoxWriter(ms, Body);

        if (Body.RawData is null)
        {
            if (!Body.IsParsed)
                throw new HeaderOnlyParseLimitationException();

            Body.Write(bodyW, remap); // Body is written first so that the aux node count is determined properly
        }
        else
        {
            if (Header.CompressionOfBody == GameBoxCompression.Compressed)
            {
                bodyW.Write(Body.UncompressedSize);
                bodyW.Write(Body.RawData.Length);
            }

            bodyW.WriteBytes(Body.RawData);
        }

        Log.Write("Writing the header...");

        using var headerW = new GameBoxWriter(stream, lookbackable: Header);
        (Header as ILookbackable).IdWritten = false;
        (Header as ILookbackable).IdStrings.Clear();
        Header.Write(headerW, Body.AuxilaryNodes.Count + 1, remap);

        Log.Write("Writing the reference table...");

        if (RefTable is null)
            headerW.Write(0);
        else
            RefTable.Write(headerW);

        headerW.WriteBytes(ms.ToArray());
    }

    /// <summary>
    /// Saves the serialized <see cref="GameBox{T}"/> to a stream.
    /// </summary>
    /// <param name="stream">Any kind of stream that supports writing.</param>
    /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="HeaderOnlyParseLimitationException">Saving is not supported in <see cref="GameBox"/> where only the header was parsed (without raw body being read).</exception>
    public void Save(Stream stream, IDRemap remap = default)
    {
        Write(stream, remap);
    }

    /// <summary>
    /// Saves the serialized <see cref="GameBox{T}"/> on a disk.
    /// </summary>
    /// <param name="fileName">Relative or absolute file path. Null will pick the <see cref="GameBox.FileName"/> value instead.</param>
    /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
    /// <exception cref="PropertyNullException"><see cref="GameBox.FileName"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="fileName"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by System.IO.Path.InvalidPathChars.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fileName"/> is null.</exception>
    /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="UnauthorizedAccessException"><paramref name="fileName"/> specified a file that is read-only. -or- <paramref name="fileName"/> specified a file that is hidden. -or- This operation is not supported on the current platform. -or- <paramref name="fileName"/> specified a directory. -or- The caller does not have the required permission.</exception>
    /// <exception cref="NotSupportedException"><paramref name="fileName"/> is in an invalid format.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="HeaderOnlyParseLimitationException">Saving is not supported in <see cref="GameBox"/> where only the header was parsed (without raw body being read).</exception>
    public void Save(string? fileName = default, IDRemap remap = default)
    {
        fileName ??= (FileName ?? throw new PropertyNullException(nameof(FileName)));

        using var fs = File.Create(fileName);

        Save(fs, remap);

        Log.Write($"GBX file {fileName} saved.");
    }

    /// <summary>
    /// Implicitly casts <see cref="GameBox{T}"/> to its <see cref="GameBox{T}.Node"/>.
    /// </summary>
    /// <param name="gbx"></param>
    public static implicit operator T(GameBox<T> gbx) => gbx.Node;
}