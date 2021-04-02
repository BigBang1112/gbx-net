using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace GBX.NET
{
    public enum GameBoxReadProgressStage
    {
        Header,
        HeaderUserData,
        RefTable,
        Body
    }

    /// <summary>
    /// A known serialized GameBox node with additional attributes. This class can represent deserialized .Gbx file.
    /// </summary>
    /// <typeparam name="T">The main node of the GBX. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
    public class GameBox<T> : GameBox where T : Node
    {
        /// <summary>
        /// Header part, typically storing metadata for quickest access.
        /// </summary>
        public GameBoxHeader<T> Header { get; }

        /// <summary>
        /// Body part, storing information about the node that realistically affects the game.
        /// </summary>
        public GameBoxBody<T> Body { get; }

        /// <summary>
        /// Node containing data taken from the body part.
        /// </summary>
        public T MainNode { get; set; }

        /// <summary>
        /// Constructs an empty GameBox object.
        /// </summary>
        public GameBox()
        {
            Header = new GameBoxHeader<T>(this);
            Body = new GameBoxBody<T>(this);
        }

        public GameBox(GameBoxHeaderInfo headerInfo) : base(headerInfo)
        {
            Header = new GameBoxHeader<T>(this);
            Body = new GameBoxBody<T>(this);
        }

        /// <summary>
        /// Create a GameBox object based on an existing node. Useful for saving nodes to GBX files.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="headerInfo"></param>
        public GameBox(T node, GameBoxHeaderInfo headerInfo) : this(headerInfo)
        {
            MainNode = node;
            ClassID = node.ID;
        }

        /// <summary>
        /// Create a GameBox object based on an existing node. Useful for saving nodes to GBX files.
        /// </summary>
        /// <param name="node"></param>
        public GameBox(T node) : this(node, null)
        {
            
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
            return Header.Chunks.RemoveWhere(x => x.ID == typeof(TChunk).GetCustomAttribute<ChunkAttribute>().ID) > 0;
        }

        /// <summary>
        /// Creates a body chunk based on the attributes from <typeparamref name="TChunk"/>.
        /// </summary>
        /// <typeparam name="TChunk">A chunk type that isn't a header chunk.</typeparam>
        /// <param name="data">If the chunk is <see cref="ISkippableChunk"/>, the bytes to initiate the chunks with, unparsed. If it's not a skippable chunk, data is parsed immediately.</param>
        /// <returns>A newly created chunk.</returns>
        public TChunk CreateBodyChunk<TChunk>(byte[] data) where TChunk : Chunk
        {
            return MainNode.Chunks.Create<TChunk>(data);
        }

        /// <summary>
        /// Creates a body chunk based on the attributes from <typeparamref name="TChunk"/> with no inner data provided.
        /// </summary>
        /// <typeparam name="TChunk">A chunk type that isn't a header chunk.</typeparam>
        /// <returns>A newly created chunk.</returns>
        public TChunk CreateBodyChunk<TChunk>() where TChunk : Chunk
        {
            return CreateBodyChunk<TChunk>(new byte[0]);
        }

        /// <summary>
        /// Removes all body chunks.
        /// </summary>
        public void RemoveAllBodyChunks()
        {
            MainNode.Chunks.Clear();
        }

        /// <summary>
        /// Removes a body chunk based on the attributes from <typeparamref name="TChunk"/>.
        /// </summary>
        /// <typeparam name="TChunk">A chunk type that isn't a header chunk.</typeparam>
        /// <returns>True, if the chunk was removed, otherwise false.</returns>
        public bool RemoveBodyChunk<TChunk>() where TChunk : Chunk
        {
            return MainNode.Chunks.Remove<TChunk>();
        }

        /// <summary>
        /// Discovers all chunks in the GBX.
        /// </summary>
        /// <exception cref="AggregateException"/>
        public void DiscoverAllChunks()
        {
            MainNode.DiscoverAllChunks();
        }

        protected override bool ProcessHeader(IProgress<GameBoxReadProgress> progress)
        {
            if (ClassID != typeof(T).GetCustomAttribute<NodeAttribute>().ID)
            {
                if (!Node.Names.TryGetValue(ClassID.Value, out string name))
                    name = "unknown class";
                throw new InvalidCastException($"GBX with ID 0x{ClassID:X8} ({name}) can't be casted to GameBox<{typeof(T).Name}>.");
            }

            MainNode = Activator.CreateInstance<T>();

            Log.Write("Working out the header chunks...");

            try
            {
                Header.Read(UserData, progress);
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

        internal bool ReadBody(GameBoxReader reader, bool readUncompressedBodyDirectly, IProgress<GameBoxReadProgress> progress)
        {
            Log.Write("Reading the body...");

            switch (BodyCompression)
            {
                case 'C':
                    var uncompressedSize = reader.ReadInt32();
                    var compressedSize = reader.ReadInt32();

                    var data = reader.ReadBytes(compressedSize);
                    Body.Read(data, uncompressedSize, progress);

                    break;
                case 'U':
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

        internal void Write(GameBoxWriter w, ClassIDRemap remap)
        {
            using (MemoryStream ms = new MemoryStream())
            using (GameBoxWriter bodyW = new GameBoxWriter(ms))
            {
                (Body as ILookbackable).IdWritten = false;
                (Body as ILookbackable).IdStrings.Clear();
                Body.AuxilaryNodes.Clear();

                Log.Write("Writing the body...");

                Body.Write(bodyW, remap); // Body is written first so that the aux node count is determined properly

                Log.Write("Writing the header...");

                (Header as ILookbackable).IdWritten = false;
                (Header as ILookbackable).IdStrings.Clear();
                Header.Write(w, Body.AuxilaryNodes.Count + 1, remap);

                Log.Write("Writing the reference table...");

                if (RefTable == null)
                    w.Write(0);
                else
                    RefTable.Write(w);

                w.Write(ms.ToArray(), 0, (int)ms.Length);
            }
        }

        internal void Write(GameBoxWriter w)
        {
            Write(w, ClassIDRemap.Latest);
        }

        /// <summary>
        /// Saves the serialized <see cref="GameBox{T}"/> to a stream.
        /// </summary>
        /// <param name="stream">Any kind of stream that supports writing.</param>
        /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
        public void Save(Stream stream, ClassIDRemap remap)
        {
            if (IntPtr.Size == 8)
                throw new NotSupportedException("Saving GBX is not supported with x64 platform target, due to LZO implementation. Please force your platform target to x86.");

            using (var w = new GameBoxWriter(stream))
                Write(w, remap);
        }

        /// <summary>
        /// Saves the serialized <see cref="GameBox{T}"/> to a stream.
        /// </summary>
        /// <param name="stream">Any kind of stream that supports writing.</param>
        public void Save(Stream stream)
        {
            Save(stream, ClassIDRemap.Latest);
        }

        /// <summary>
        /// Saves the serialized <see cref="GameBox{T}"/> on a disk.
        /// </summary>
        /// <param name="fileName">Relative or absolute file path.</param>
        /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
        public void Save(string fileName, ClassIDRemap remap)
        {
            using (var fs = File.OpenWrite(fileName))
                Save(fs, remap);

            Log.Write($"GBX file {fileName} saved.");
        }

        /// <summary>
        /// Saves the serialized <see cref="GameBox{T}"/> on a disk.
        /// </summary>
        /// <param name="fileName">Relative or absolute file path.</param>
        public void Save(string fileName)
        {
            Save(fileName, ClassIDRemap.Latest);
        }
    }

    /// <summary>
    /// An unknown serialized GameBox node with additional attributes. This class can represent deserialized .Gbx file.
    /// </summary>
    public class GameBox : IGameBox
    {
        public const string Magic = "GBX";

        public ClassIDRemap Game { get; set; } = ClassIDRemap.ManiaPlanet;

        public GameBoxHeaderInfo HeaderInfo { get; }

        public short Version
        {
            get => HeaderInfo.Version;
            set => HeaderInfo.Version = value;
        }

        public char? ByteFormat
        {
            get => HeaderInfo.ByteFormat;
            set => HeaderInfo.ByteFormat = value;
        }
            
        public char? RefTableCompression
        {
            get => HeaderInfo.RefTableCompression;
            set => HeaderInfo.RefTableCompression = value;
        }

        public char? BodyCompression
        {
            get => HeaderInfo.BodyCompression;
            set => HeaderInfo.BodyCompression = value;
        }

        public char? UnknownByte
        {
            get => HeaderInfo.UnknownByte;
            set => HeaderInfo.UnknownByte = value;
        }

        public uint? ClassID
        {
            get => HeaderInfo.ClassID;
            internal set => HeaderInfo.ClassID = value;
        }

        public byte[] UserData
        {
            get => HeaderInfo.UserData;
        }

        public int NumNodes
        {
            get => HeaderInfo.NumNodes;
        }

        /// <summary>
        /// Reference table, referencing other GBX.
        /// </summary>
        public GameBoxRefTable RefTable { get; private set; }

        public string FileName { get; set; }

        public GameBox() : this(null)
        {
            
        }

        public GameBox(GameBoxHeaderInfo headerInfo)
        {
            if (headerInfo == null)
                HeaderInfo = new GameBoxHeaderInfo();
            else
                HeaderInfo = headerInfo;
        }

        internal bool ReadHeader(GameBoxReader reader, IProgress<GameBoxReadProgress> progress)
        {
            var success = HeaderInfo.Read(reader);

            progress?.Report(new GameBoxReadProgress(GameBoxReadProgressStage.Header, 1, this));

            return success;
        }

        protected virtual bool ProcessHeader(IProgress<GameBoxReadProgress> progress)
        {
            return true; // There are no header chunks to proccess in an unknown GBX
        }

        protected bool ReadRefTable(GameBoxReader reader, IProgress<GameBoxReadProgress> progress)
        {
            var numExternalNodes = reader.ReadInt32();

            if (numExternalNodes > 0)
            {
                var ancestorLevel = reader.ReadInt32();

                GameBoxRefTableFolder rootFolder = new GameBoxRefTableFolder("Root");

                var numSubFolders = reader.ReadInt32();
                ReadRefTableFolders(numSubFolders, ref rootFolder);

                void ReadRefTableFolders(int n, ref GameBoxRefTableFolder folder)
                {
                    for (var i = 0; i < n; i++)
                    {
                        var name = reader.ReadString();
                        var numSubSubFolders = reader.ReadInt32();

                        var f = new GameBoxRefTableFolder(name, folder);
                        folder.Folders.Add(f);

                        ReadRefTableFolders(numSubSubFolders, ref f);
                    }
                }

                var externalNodes = new ExternalNode[numExternalNodes];

                for (var i = 0; i < numExternalNodes; i++)
                {
                    string fileName = null;
                    int? resourceIndex = null;
                    bool? useFile = null;
                    int? folderIndex = null;

                    var flags = reader.ReadInt32();

                    if ((flags & 4) == 0)
                        fileName = reader.ReadString();
                    else
                        resourceIndex = reader.ReadInt32();

                    var nodeIndex = reader.ReadInt32();

                    if (Version >= 5)
                        useFile = reader.ReadBoolean();

                    if ((flags & 4) == 0)
                        folderIndex = reader.ReadInt32();

                    var extNode = new ExternalNode(flags, fileName, resourceIndex, nodeIndex, useFile, folderIndex);
                    externalNodes[i] = extNode;
                }

                var refTable = new GameBoxRefTable(rootFolder, externalNodes);
                RefTable = refTable;
            }
            else
                Log.Write("No external nodes found, reference table completed.", ConsoleColor.Green);

            progress?.Report(new GameBoxReadProgress(GameBoxReadProgressStage.RefTable, 1, this));

            return true;
        }

        private static GameBox ParseHeader(GameBoxReader reader, IProgress<GameBoxReadProgress> progress = null)
        {
            var headerInfo = new GameBoxHeaderInfo();
            headerInfo.Read(reader);

            progress?.Report(new GameBoxReadProgress(headerInfo));

            if (headerInfo.ClassID.HasValue)
            {
                GameBox gbx;

                if (Node.AvailableClasses.TryGetValue(headerInfo.ClassID.Value, out Type availableClass))
                {
                    var gbxType = typeof(GameBox<>).MakeGenericType(availableClass);
                    gbx = (GameBox)Activator.CreateInstance(gbxType, headerInfo);

                    var processHeaderMethod = gbxType.GetMethod(nameof(ProcessHeader), BindingFlags.Instance | BindingFlags.NonPublic);
                    processHeaderMethod.Invoke(gbx, new object[] { progress });
                }
                else
                    gbx = new GameBox(headerInfo);

                if (gbx.ReadRefTable(reader, progress))
                    return gbx;
            }

            return null;
        }
        /// <summary>
        /// Parses only the header of the GBX.
        /// </summary>
        /// <param name="stream">Stream to read GBX format from.</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast.</returns>
        public static GameBox ParseHeader(Stream stream, IProgress<GameBoxReadProgress> progress = null)
        {
            using (var r = new GameBoxReader(stream))
                return ParseHeader(r, progress);
        }

        /// <summary>
        /// Parses only the header of the GBX.
        /// </summary>
        /// <param name="fileName">Relative or absolute file path.</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast.</returns>
        public static GameBox ParseHeader(string fileName, IProgress<GameBoxReadProgress> progress = null)
        {
            using (var fs = File.OpenRead(fileName))
            {
                var gbx = ParseHeader(fs, progress);
                gbx.FileName = fileName;
                return gbx;
            }
        }

        /// <summary>
        /// Parses only the header of the GBX.
        /// </summary>
        /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
        /// <param name="stream">Stream to read GBX format from.</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A GameBox with specified main node type.</returns>
        /// <exception cref="InvalidCastException"/>
        public static GameBox<T> ParseHeader<T>(Stream stream, IProgress<GameBoxReadProgress> progress = null) where T : Node
        {
            GameBox<T> gbx = new GameBox<T>();

            using (var r = new GameBoxReader(stream))
                if (gbx.ReadHeader(r, progress) && gbx.ProcessHeader(progress) && gbx.ReadRefTable(r, progress))
                    return gbx;

            return null;
        }

        /// <summary>
        /// Parses only the header of the GBX.
        /// </summary>
        /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
        /// <param name="fileName">Relative or absolute file path.</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A GameBox with specified main node type.</returns>
        /// <exception cref="InvalidCastException"/>
        public static GameBox<T> ParseHeader<T>(string fileName, IProgress<GameBoxReadProgress> progress = null) where T : Node
        {
            using (var fs = File.OpenRead(fileName))
            {
                var gbx = ParseHeader<T>(fs, progress);
                gbx.FileName = fileName;
                return gbx;
            }
        }

        /// <summary>
        /// Easily parses GBX format.
        /// </summary>
        /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
        /// <param name="stream">Stream to read GBX format from.</param>
        /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
        /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A GameBox with specified main node type.</returns>
        /// <exception cref="InvalidCastException"/>
        public static GameBox<T> Parse<T>(Stream stream, bool readUncompressedBodyDirectly, IProgress<GameBoxReadProgress> progress = null) where T : Node
        {
            var gbx = ParseHeader<T>(stream, progress);

            using (var r = new GameBoxReader(stream))
            {
                if (gbx.ReadBody(r, readUncompressedBodyDirectly, progress))
                    return gbx;
            }

            return null;
        }

        /// <summary>
        /// Easily parses GBX format.
        /// </summary>
        /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
        /// <param name="stream">Stream to read GBX format from.</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A GameBox with specified main node type.</returns>
        /// <exception cref="InvalidCastException"/>
        public static GameBox<T> Parse<T>(Stream stream, IProgress<GameBoxReadProgress> progress = null) where T : Node
        {
            return Parse<T>(stream, false, progress);
        }

        /// <summary>
        /// Easily parses a GBX file.
        /// </summary>
        /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
        /// <param name="fileName">Relative or absolute file path.</param>
        /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
        /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A GameBox with specified main node type.</returns>
        /// <exception cref="InvalidCastException"/>
        public static GameBox<T> Parse<T>(string fileName, bool readUncompressedBodyDirectly, IProgress<GameBoxReadProgress> progress = null) where T : Node
        {
            using (var fs = File.OpenRead(fileName))
            {
                var gbx = Parse<T>(fs, readUncompressedBodyDirectly, progress);
                if (gbx == null) return null;
                gbx.FileName = fileName;
                return gbx;
            }
        }

        /// <summary>
        /// Easily parses a GBX file.
        /// </summary>
        /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
        /// <param name="fileName">Relative or absolute file path.</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A GameBox with specified main node type.</returns>
        /// <exception cref="InvalidCastException"/>
        /// <example>
        /// var gbx = GameBox.Parse&lt;CGameCtnChallenge&gt;("MyMap.Map.Gbx");
        /// // Node data is available in gbx.MainNode
        /// </example>
        public static GameBox<T> Parse<T>(string fileName, IProgress<GameBoxReadProgress> progress = null) where T : Node
        {
            return Parse<T>(fileName, false, progress);
        }

        /// <summary>
        /// Easily parses a GBX file.
        /// </summary>
        /// <param name="fileName">Relative or absolute file path.</param>
        /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
        /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast.</returns>
        public static GameBox Parse(string fileName, bool readUncompressedBodyDirectly, IProgress<GameBoxReadProgress> progress = null)
        {
            using (var fs = File.OpenRead(fileName))
            {
                var gbx = Parse(fs, readUncompressedBodyDirectly, progress);
                if (gbx == null) return null;
                gbx.FileName = fileName;
                return gbx;
            }
        }

        /// <summary>
        /// Easily parses a GBX file.
        /// </summary>
        /// <param name="fileName">Relative or absolute file path.</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast.</returns>
        /// <example>
        /// var gbx = GameBox.Parse("MyMap.Map.Gbx");
        /// 
        /// if (gbx is GameBox&lt;CGameCtnChallenge&gt; gbxMap)
        /// {
        ///     // Node data is available in gbxMap.MainNode
        /// }
        /// else if (gbx is GameBox&lt;CGameCtnReplayRecord&gt; gbxReplay)
        /// {
        ///     // Node data is available in gbxReplay.MainNode
        /// }
        /// </example>
        public static GameBox Parse(string fileName, IProgress<GameBoxReadProgress> progress = null)
        {
            return Parse(fileName, false, progress);
        }

        /// <summary>
        /// Easily parses GBX format.
        /// </summary>
        /// <param name="stream">Stream to read GBX format from.</param>
        /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
        /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <exception cref="NotSupportedException"/>
        /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast.</returns>
        public static GameBox Parse(Stream stream, bool readUncompressedBodyDirectly, IProgress<GameBoxReadProgress> progress = null)
        {
            using (var r = new GameBoxReader(stream))
            {
                var gbx = ParseHeader(r, progress);

                if (gbx == null) return null;

                var gbxType = gbx.GetType();

                if (gbxType.IsGenericType && gbxType.GetGenericTypeDefinition() == typeof(GameBox<>))
                {
                    var bodyProperty = gbxType.GetProperty("Body");
                    var readBodyMethod = gbxType.GetMethod("ReadBody", BindingFlags.Instance | BindingFlags.NonPublic);

                    r.Lookbackable = (ILookbackable)bodyProperty.GetValue(gbx);

                    try
                    {
                        readBodyMethod.Invoke(gbx, new object[] { r, readUncompressedBodyDirectly, progress });
                    }
                    catch (TargetInvocationException e)
                    {
                        Log.Write("\nException while parsing the body of GBX!", ConsoleColor.Red);
                        Log.Write(e.InnerException.ToString());
                        ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                    }
                }

                return gbx;
            }
        }

        /// <summary>
        /// Easily parses GBX format.
        /// </summary>
        /// <param name="stream">Stream to read GBX format from.</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <exception cref="NotSupportedException"/>
        /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast.</returns>
        public static GameBox Parse(Stream stream, IProgress<GameBoxReadProgress> progress = null)
        {
            return Parse(stream, false, progress);
        }

        private static uint? ReadNodeID(GameBoxReader reader)
        {
            uint? classID = null;

            if (reader.ReadString(Magic.Length) != Magic) // If the file doesn't have GBX magic
                return null;

            var version = reader.ReadInt16(); // Version

            if (version >= 3)
            {
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();

                if (version >= 4)
                    reader.ReadByte();

                classID = reader.ReadUInt32();
            }

            return classID;
        }

        /// <summary>
        /// Reads the GBX node ID the quickest possible way.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>ID of the main node presented in GBX. Null if not a GBX stream.</returns>
        public static uint? ReadNodeID(Stream stream)
        {
            using (GameBoxReader r = new GameBoxReader(stream))
                return ReadNodeID(r);
        }

        /// <summary>
        /// Reads the GBX node ID the quickest possible way.
        /// </summary>
        /// <param name="fileName">File to read from.</param>
        /// <returns>ID of the main node presented in GBX. Null if not a GBX stream.</returns>
        public static uint? ReadNodeID(string fileName)
        {
            using (var fs = File.OpenRead(fileName))
                return ReadNodeID(fs);
        }

        /// <summary>
        /// Reads the type of the main node from GBX file.
        /// </summary>
        /// <param name="fileName">File to read from.</param>
        /// <returns>Type of the main node.</returns>
        public static Type ReadNodeType(string fileName)
        {
            using (var fs = File.OpenRead(fileName))
                return ReadNodeType(fs);
        }

        /// <summary>
        /// Reads the type of the main node from GBX stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>Type of the main node.</returns>
        public static Type ReadNodeType(Stream stream)
        {
            using (var r = new GameBoxReader(stream))
                return ReadNodeType(r);
        }

        private static Type ReadNodeType(GameBoxReader reader)
        {
            var classID = ReadNodeID(reader);

            if (classID.HasValue)
            {
                var modernID = classID.GetValueOrDefault();
                if (Node.Mappings.TryGetValue(classID.GetValueOrDefault(), out uint newerClassID))
                    modernID = newerClassID;

                Debug.WriteLine("GetGameBoxType: " + modernID.ToString("x8"));

                var availableClass = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass
                        && x.Namespace?.StartsWith("GBX.NET.Engines") == true && x.IsSubclassOf(typeof(Node))
                        && x.GetCustomAttribute<NodeAttribute>().ID == modernID).FirstOrDefault();

                if (availableClass == null) return null;

                return typeof(GameBox<>).MakeGenericType(availableClass);
            }

            return null;
        }
    }
}
