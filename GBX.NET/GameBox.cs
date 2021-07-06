using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

using GBX.NET.Engines.MwFoundations;

namespace GBX.NET
{
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
        public new GameBoxBody<T> Body
        {
            get => (GameBoxBody<T>)base.Body;
            protected set => base.Body = value;
        }

        /// <summary>
        /// Deserialized node from GBX.
        /// </summary>
        public new T Node
        {
            get => (T)base.Node;
            set => base.Node = value;
        }

        /// <summary>
        /// Creates an empty GameBox object version 6.
        /// </summary>
        public GameBox() : this(new GameBoxHeaderInfo(typeof(T).GetCustomAttribute<NodeAttribute>().ID))
        {
            
        }

        /// <summary>
        /// Creates an empty GameBox object based on defined <see cref="GameBoxHeaderInfo"/>.
        /// </summary>
        /// <param name="header">Header info to use.</param>
        public GameBox(GameBoxHeaderInfo header) : base(header)
        {
            Header = new GameBoxHeader<T>(this);
            Body = new GameBoxBody<T>(this);

            Node = (T)Activator.CreateInstance(typeof(T)); // TODO: new T
            Node.GBX = this;
        }

        /// <summary>
        /// Creates a GameBox object based on an existing node. Useful for saving nodes to GBX files.
        /// </summary>
        /// <param name="node">Node to wrap.</param>
        /// <param name="headerInfo">Header info to use.</param>
        public GameBox(T node, GameBoxHeaderInfo headerInfo) : this(headerInfo)
        {
            // It needs to be sure that GBX is assigned correctly to every node
            AssignGBXToNode(this, node);

            Node = node;
            ID = node.ID;
        }

        /// <summary>
        /// Create a GameBox object based on an existing node. Useful for saving nodes to GBX files.
        /// </summary>
        /// <param name="node">Node to wrap.</param>
        public GameBox(T node) : this(node, new GameBoxHeaderInfo(node.ID))
        {

        }

        private void AssignGBXToNode()
        {
            AssignGBXToNode(this, Node);
        }

        private void AssignGBXToNode(GameBox gbx, CMwNod n)
        {
            if (n == null) return;

            n.GBX = gbx; // Assign the GBX body to this body
            foreach (var chunk in n.Chunks)
                chunk.GBX = gbx; // Assign each chunk to this body

            var type = n.GetType();

            foreach (var prop in type.GetProperties()) // Go through all properties of a node
            {
                if (Attribute.IsDefined(prop, typeof(NodeMemberAttribute))) // Check only NodeMember attributes
                {
                    if (prop.PropertyType.IsSubclassOf(typeof(CMwNod))) // If the property is Node
                    {
                        AssignGBXToNode(gbx, (CMwNod)prop.GetValue(n)); // Recurse through the node
                    }
                    else if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType)) // If the property is a list of something
                    {
                        // If the list has a generic argument of anu kind of Node
                        if (Array.Find(prop.PropertyType.GetGenericArguments(), x => x.IsSubclassOf(typeof(CMwNod))) != null)
                        {
                            // Go through each Node and recurse
                            var enumerable = (IEnumerable)prop.GetValue(n);

                            if (enumerable != null)
                            {
                                foreach (var e in enumerable)
                                    AssignGBXToNode(gbx, (CMwNod)e);
                            }
                        }
                    }
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
            return Node.Chunks.Create<TChunk>(data);
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

        protected override bool ProcessHeader(IProgress<GameBoxReadProgress> progress)
        {
            if (ID != typeof(T).GetCustomAttribute<NodeAttribute>().ID)
            {
                if (!NodeCacheManager.Names.TryGetValue(ID.Value, out string name))
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

        protected internal override bool ReadBody(GameBoxReader reader, bool readUncompressedBodyDirectly, IProgress<GameBoxReadProgress> progress)
        {
            Log.Write("Reading the body...");

            switch (Header.BodyCompression)
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

        internal void Write(GameBoxWriter w, IDRemap remap)
        {
            // It needs to be sure that the Body and Part are assigned to the correct GameBox body
            AssignGBXToNode();

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
            Write(w, IDRemap.Latest);
        }

        /// <summary>
        /// Saves the serialized <see cref="GameBox{T}"/> to a stream.
        /// </summary>
        /// <param name="stream">Any kind of stream that supports writing.</param>
        /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
        public void Save(Stream stream, IDRemap remap)
        {
            using (var w = new GameBoxWriter(stream))
                Write(w, remap);
        }

        /// <summary>
        /// Saves the serialized <see cref="GameBox{T}"/> to a stream.
        /// </summary>
        /// <param name="stream">Any kind of stream that supports writing.</param>
        public void Save(Stream stream)
        {
            Save(stream, IDRemap.Latest);
        }

        /// <summary>
        /// Saves the serialized <see cref="GameBox{T}"/> on a disk.
        /// </summary>
        /// <param name="fileName">Relative or absolute file path.</param>
        /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
        public void Save(string fileName, IDRemap remap)
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
            Save(fileName, IDRemap.Latest);
        }

        /// <summary>
        /// Saves the serialized <see cref="GameBox{T}"/> on the defined <see cref="GameBox.FileName"/>.
        /// </summary>
        /// <param name="remap">What to remap the newest node IDs to. Used for older games.</param>
        public void Save(IDRemap remap)
        {
            Save(FileName, remap);
        }

        /// <summary>
        /// Saves the serialized <see cref="GameBox{T}"/> on the defined <see cref="GameBox.FileName"/>.
        /// </summary>
        public void Save()
        {
            Save(FileName, IDRemap.Latest);
        }

        /// <summary>
        /// Implicitly casts <see cref="GameBox{T}"/> to its <see cref="GameBox{T}.Node"/>.
        /// </summary>
        /// <param name="gbx"></param>
        public static implicit operator T(GameBox<T> gbx) => gbx.Node;
    }

    /// <summary>
    /// An unknown serialized GameBox node with additional attributes. This class can represent deserialized .Gbx file.
    /// </summary>
    public class GameBox
    {
        public const string Magic = "GBX";

        /// <summary>
        /// Tells the library to save this GBX with correct IDs related to the game version.
        /// </summary>
        public IDRemap Remap { get; set; }

        /// <summary>
        /// Header part containing generic GameBox values.
        /// </summary>
        public GameBoxHeaderInfo Header { get; }

        public GameBoxBody Body { get; protected set; }

        public CMwNod Node { get; internal set; }

        /// <summary>
        /// ID of the node.
        /// </summary>
        public uint? ID
        {
            get => Header.ID;
            internal set => Header.ID = value;
        }

        /// <summary>
        /// Reference table, referencing other GBX.
        /// </summary>
        public GameBoxRefTable RefTable { get; private set; }

        /// <summary>
        /// File path of the GameBox.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Creates an empty GameBox object version 6.
        /// </summary>
        public GameBox(uint id)
        {
            Header = new GameBoxHeaderInfo(id);
        }

        /// <summary>
        /// Creates an empty GameBox object based on defined <see cref="GameBoxHeaderInfo"/>.
        /// </summary>
        /// <param name="headerInfo">Header info to use.</param>
        public GameBox(GameBoxHeaderInfo headerInfo)
        {
            Header = headerInfo ?? throw new ArgumentNullException(nameof(headerInfo));
        }

        /// <summary>
        /// Tries to get the <see cref="CMwNod"/> of this GBX.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="CMwNod"/> to look for.</typeparam>
        /// <param name="node">A node that is being extracted from this <see cref="GameBox"/> object. Null if unsuccessful.</param>
        /// <returns>True if the type of this <see cref="GameBox"/> is <see cref="GameBox{T}"/> and <typeparamref name="T"/> matches. Otherwise false.</returns>
        public bool TryNode<T>(out T node) where T : CMwNod
        {
            var property = GetType().GetProperty("MainNode");

            if (property != null && property.PropertyType == typeof(T))
            {
                node = (T)property.GetValue(this);
                return true;
            }

            node = null;
            return false;
        }

        internal bool ReadHeader(GameBoxReader reader, IProgress<GameBoxReadProgress> progress)
        {
            var success = Header.Read(reader);

            progress?.Report(new GameBoxReadProgress(GameBoxReadProgressStage.Header, 1, this));

            return success;
        }

        protected virtual bool ProcessHeader(IProgress<GameBoxReadProgress> progress)
        {
            return true; // There are no header chunks to proccess in an unknown GBX
        }

        protected bool ReadRefTable(GameBoxReader reader, IProgress<GameBoxReadProgress> progress)
        {
            RefTable = new GameBoxRefTable(Header);
            RefTable.Read(reader);

            progress?.Report(new GameBoxReadProgress(GameBoxReadProgressStage.RefTable, 1, this));

            return true;
        }

        protected internal virtual bool ReadBody(GameBoxReader reader, bool readUncompressedBodyDirectly, IProgress<GameBoxReadProgress> progress)
        {
            return false;
        }

        /// <summary>
        /// Implicitly casts <see cref="GameBox"/> to its <see cref="Node"/>.
        /// </summary>
        /// <param name="gbx"></param>
        public static implicit operator CMwNod(GameBox gbx) => gbx.Node;

        private static GameBox ParseHeader(GameBoxReader reader, IProgress<GameBoxReadProgress> progress = null)
        {
            var header = new GameBoxHeaderInfo(reader);

            progress?.Report(new GameBoxReadProgress(header));

            if (header.ID.HasValue)
            {
                GameBox gbx;

                if (NodeCacheManager.AvailableClasses.TryGetValue(header.ID.Value, out Type availableClass))
                {
                    var gbxType = typeof(GameBox<>).MakeGenericType(availableClass);
                    gbx = (GameBox)Activator.CreateInstance(gbxType, header);

                    var processHeaderMethod = gbxType.GetMethod(nameof(ProcessHeader), BindingFlags.Instance | BindingFlags.NonPublic);
                    processHeaderMethod.Invoke(gbx, new object[] { progress });
                }
                else
                    gbx = new GameBox(header);

                if (gbx.ReadRefTable(reader, progress))
                    return gbx;
            }

            return new GameBox(header);
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
        public static GameBox<T> ParseHeader<T>(Stream stream, IProgress<GameBoxReadProgress> progress = null) where T : CMwNod
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
        public static GameBox<T> ParseHeader<T>(string fileName, IProgress<GameBoxReadProgress> progress = null) where T : CMwNod
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
        /// <exception cref="MissingLzoException"/>
        /// <exception cref="InvalidCastException"/>
        /// <exception cref="NotSupportedException"/>
        public static GameBox<T> Parse<T>(Stream stream, bool readUncompressedBodyDirectly, IProgress<GameBoxReadProgress> progress = null) where T : CMwNod
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
        /// <exception cref="MissingLzoException"/>
        /// <exception cref="InvalidCastException"/>
        /// <exception cref="NotSupportedException"/>
        public static GameBox<T> Parse<T>(Stream stream, IProgress<GameBoxReadProgress> progress = null) where T : CMwNod
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
        /// <exception cref="MissingLzoException"/>
        /// <exception cref="InvalidCastException"/>
        /// <exception cref="NotSupportedException"/>
        public static GameBox<T> Parse<T>(string fileName, bool readUncompressedBodyDirectly, IProgress<GameBoxReadProgress> progress = null) where T : CMwNod
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
        /// <exception cref="MissingLzoException"/>
        /// <exception cref="InvalidCastException"/>
        /// <exception cref="NotSupportedException"/>
        /// <example>
        /// var gbx = GameBox.Parse&lt;CGameCtnChallenge&gt;("MyMap.Map.Gbx");
        /// // Node data is available in gbx.MainNode
        /// </example>
        public static GameBox<T> Parse<T>(string fileName, IProgress<GameBoxReadProgress> progress = null) where T : CMwNod
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
        /// <exception cref="MissingLzoException"/>
        /// <exception cref="NotSupportedException"/>
        /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast).</returns>
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
        /// <exception cref="MissingLzoException"/>
        /// <exception cref="NotSupportedException"/>
        /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast).</returns>
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
        /// <exception cref="MissingLzoException"/>
        /// <exception cref="NotSupportedException"/>
        /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast).</returns>
        public static GameBox Parse(Stream stream, bool readUncompressedBodyDirectly, IProgress<GameBoxReadProgress> progress = null)
        {
            using (var rHeader = new GameBoxReader(stream))
            {
                var gbx = ParseHeader(rHeader, progress);

                // Body resets Id (lookback string) list
                using (var rBody = new GameBoxReader(stream, gbx.Body))
                {
                    gbx.ReadBody(rBody, readUncompressedBodyDirectly, progress);
                }

                return gbx;
            }
        }

        /// <summary>
        /// Easily parses GBX format.
        /// </summary>
        /// <param name="stream">Stream to read GBX format from.</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <exception cref="MissingLzoException"/>
        /// <exception cref="NotSupportedException"/>
        /// <returns>A GameBox with either basic information only (if unknown), or also with specified main node type (available by using an explicit <see cref="GameBox{T}"/> cast).</returns>
        public static GameBox Parse(Stream stream, IProgress<GameBoxReadProgress> progress = null)
        {
            return Parse(stream, false, progress);
        }

        /// <summary>
        /// Parses only the header of the GBX and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
        /// </summary>
        /// <param name="stream">Stream to read GBX format from.</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A <see cref="CMwNod"/> with either basic information only (if unknown), or also with specified node information (available by using an explicit cast).</returns>
        public static CMwNod ParseNodeHeader(Stream stream, IProgress<GameBoxReadProgress> progress = null)
        {
            return ParseHeader(stream, progress).Node;
        }

        /// <summary>
        /// Parses only the header of the GBX and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
        /// </summary>
        /// <param name="fileName">Relative or absolute file path.</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A <see cref="CMwNod"/> with either basic information only (if unknown), or also with specified node information (available by using an explicit cast).</returns>
        public static CMwNod ParseNodeHeader(string fileName, IProgress<GameBoxReadProgress> progress = null)
        {
            return ParseHeader(fileName, progress);
        }

        /// <summary>
        /// Parses only the header of the GBX and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
        /// </summary>
        /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
        /// <param name="stream">Stream to read GBX format from.</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A <see cref="CMwNod"/> casted to <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidCastException"/>
        public static T ParseNodeHeader<T>(Stream stream, IProgress<GameBoxReadProgress> progress = null) where T : CMwNod
        {
            return ParseHeader<T>(stream, progress);
        }

        /// <summary>
        /// Parses only the header of the GBX and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
        /// </summary>
        /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
        /// <param name="fileName">Relative or absolute file path.</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A <see cref="CMwNod"/> casted to <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidCastException"/>
        public static T ParseNodeHeader<T>(string fileName, IProgress<GameBoxReadProgress> progress = null) where T : CMwNod
        {
            return ParseHeader<T>(fileName, progress);
        }

        /// <summary>
        /// Easily parses GBX format and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
        /// </summary>
        /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
        /// <param name="stream">Stream to read GBX format from.</param>
        /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
        /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A <see cref="CMwNod"/> casted to <typeparamref name="T"/>.</returns>
        /// <exception cref="MissingLzoException"/>
        /// <exception cref="InvalidCastException"/>
        /// <exception cref="NotSupportedException"/>
        public static T ParseNode<T>(Stream stream, bool readUncompressedBodyDirectly, IProgress<GameBoxReadProgress> progress = null) where T : CMwNod
        {
            return Parse<T>(stream, readUncompressedBodyDirectly, progress);
        }

        /// <summary>
        /// Easily parses GBX format and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
        /// </summary>
        /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
        /// <param name="stream">Stream to read GBX format from.</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A <see cref="CMwNod"/> casted to <typeparamref name="T"/>.</returns>
        /// <exception cref="MissingLzoException"/>
        /// <exception cref="InvalidCastException"/>
        /// <exception cref="NotSupportedException"/>
        public static T ParseNode<T>(Stream stream, IProgress<GameBoxReadProgress> progress = null) where T : CMwNod
        {
            return ParseNode<T>(stream, false, progress);
        }

        /// <summary>
        /// Easily parses a GBX file and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
        /// </summary>
        /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
        /// <param name="fileName">Relative or absolute file path.</param>
        /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
        /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A <see cref="CMwNod"/> casted to <typeparamref name="T"/>.</returns>
        /// <exception cref="MissingLzoException"/>
        /// <exception cref="InvalidCastException"/>
        /// <exception cref="NotSupportedException"/>
        public static T ParseNode<T>(string fileName, bool readUncompressedBodyDirectly, IProgress<GameBoxReadProgress> progress = null) where T : CMwNod
        {
            return Parse<T>(fileName, readUncompressedBodyDirectly, progress);
        }

        /// <summary>
        /// Easily parses a GBX file and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
        /// </summary>
        /// <typeparam name="T">Known node of the GBX file parsed. Unmatching node will throw an exception. Nodes to use are located in the GBX.NET.Engines namespace.</typeparam>
        /// <param name="fileName">Relative or absolute file path.</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <returns>A <see cref="CMwNod"/> casted to <typeparamref name="T"/>.</returns>
        /// <exception cref="MissingLzoException"/>
        /// <exception cref="InvalidCastException"/>
        /// <exception cref="NotSupportedException"/>
        public static T ParseNode<T>(string fileName, IProgress<GameBoxReadProgress> progress = null) where T : CMwNod
        {
            return ParseNode<T>(fileName, false, progress);
        }

        /// <summary>
        /// Easily parses a GBX file and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
        /// </summary>
        /// <param name="fileName">Relative or absolute file path.</param>
        /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
        /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <exception cref="MissingLzoException"/>
        /// <exception cref="NotSupportedException"/>
        /// <returns>A <see cref="CMwNod"/> with either basic information only (if unknown), or also with specified node information (available by using an explicit cast).</returns>
        public static CMwNod ParseNode(string fileName, bool readUncompressedBodyDirectly, IProgress<GameBoxReadProgress> progress = null)
        {
            return Parse(fileName, readUncompressedBodyDirectly, progress);
        }

        /// <summary>
        /// Easily parses a GBX file and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
        /// </summary>
        /// <param name="fileName">Relative or absolute file path.</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <exception cref="MissingLzoException"/>
        /// <exception cref="NotSupportedException"/>
        /// <returns>A <see cref="CMwNod"/> with either basic information only (if unknown), or also with specified node information (available by using an explicit cast).</returns>
        public static CMwNod ParseNode(string fileName, IProgress<GameBoxReadProgress> progress = null)
        {
            return ParseNode(fileName, false, progress);
        }

        /// <summary>
        /// Easily parses GBX format and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
        /// </summary>
        /// <param name="stream">Stream to read GBX format from.</param>
        /// <param name="readUncompressedBodyDirectly">If the body (if presented uncompressed) should be parsed directly from the stream (true), or loaded to memory first (false).
        /// This set to true makes the parse slower with files and <see cref="FileStream"/> but could potentially speed up direct parses from the internet. Use wisely!</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <exception cref="MissingLzoException"/>
        /// <exception cref="NotSupportedException"/>
        /// <returns>A <see cref="CMwNod"/> with either basic information only (if unknown), or also with specified node information (available by using an explicit cast).</returns>
        public static CMwNod ParseNode(Stream stream, bool readUncompressedBodyDirectly, IProgress<GameBoxReadProgress> progress = null)
        {
            return Parse(stream, readUncompressedBodyDirectly, progress);
        }

        /// <summary>
        /// Easily parses GBX format and returns the node of it. <see cref="GameBox"/> is then accessible with <see cref="CMwNod.GBX"/>.
        /// </summary>
        /// <param name="stream">Stream to read GBX format from.</param>
        /// <param name="progress">Callback that reports any read progress.</param>
        /// <exception cref="MissingLzoException"/>
        /// <exception cref="NotSupportedException"/>
        /// <returns>A <see cref="CMwNod"/> with either basic information only (if unknown), or also with specified node information (available by using an explicit cast).</returns>
        public static CMwNod ParseNode(Stream stream, IProgress<GameBoxReadProgress> progress = null)
        {
            return ParseNode(stream, false, progress);
        }

        private static uint? ReadNodeID(GameBoxReader reader)
        {
            uint? classID = null;

            if (!reader.HasMagic(Magic)) // If the file doesn't have GBX magic
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
                if (NodeCacheManager.Mappings.TryGetValue(classID.GetValueOrDefault(), out uint newerClassID))
                    modernID = newerClassID;

                Debug.WriteLine("GetGameBoxType: " + modernID.ToString("x8"));

                var availableClass = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass
                        && x.Namespace?.StartsWith("GBX.NET.Engines") == true && x.IsSubclassOf(typeof(CMwNod))
                        && x.GetCustomAttribute<NodeAttribute>().ID == modernID).FirstOrDefault();

                if (availableClass == null) return null;

                return typeof(GameBox<>).MakeGenericType(availableClass);
            }

            return null;
        }
    }
}
