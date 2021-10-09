using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace GBX.NET.Engines.MwFoundations
{
    [Node(0x01001000)]
    public class CMwNod
    {
        [IgnoreDataMember]
        public GameBox GBX { get; internal set; }

        public ChunkSet Chunks { get; internal set; } = new ChunkSet();

        /// <summary>
        /// Chunk where the aux node appeared
        /// </summary>
        public Chunk ParentChunk { get; set; }

        public uint ID { get; }
        [Obsolete]
        public uint? FaultyChunk { get; private set; }
        [Obsolete]
        public byte[] Rest { get; private set; }
        [Obsolete]
        public bool Unknown { get; internal set; }

        public string[] Dependencies { get; set; }

        /// <summary>
        /// Name of the class. The format is <c>Engine::Class</c>.
        /// </summary>
        public string ClassName
        {
            get
            {
                if (NodeCacheManager.Names.TryGetValue(ID, out string name))
                    return name;
                return GetType().FullName.Substring("GBX.NET.Engines".Length + 1).Replace(".", "::");
            }
        }

        internal CMwNod()
        {
            ID = ((NodeAttribute)NodeCacheManager.AvailableClassAttributes[GetType()]
                .First(x => x is NodeAttribute)).ID;
        }

        protected CMwNod(params Chunk[] chunks) : this()
        {
            foreach (var chunk in chunks)
            {
                GetType()
                    .GetMethod("CreateChunk")
                    .MakeGenericMethod(chunk.GetType())
                    .Invoke(this, new object[0]);
            }
        }

        public static T?[] ParseArray<T>(GameBoxReader r) where T : CMwNod
        {
            var count = r.ReadInt32();
            var array = new T?[count];

            for (var i = 0; i < count; i++)
                array[i] = Parse<T>(r);

            return array;
        }

        public static IEnumerable<T?> ParseEnumerable<T>(GameBoxReader r) where T : CMwNod
        {
            var count = r.ReadInt32();

            for (var i = 0; i < count; i++)
                yield return Parse<T>(r);
        }

        public static T? Parse<T>(GameBoxReader r, uint? classID = null, IProgress<GameBoxReadProgress>? progress = null) where T : CMwNod
        {
            if (!classID.HasValue)
                classID = r.ReadUInt32();

            if (classID == uint.MaxValue) return null;

            classID = Remap(classID.Value);

            if (!NodeCacheManager.AvailableClasses.TryGetValue(classID.Value, out Type type))
                throw new NotImplementedException($"Node ID 0x{classID.Value:X8} is not implemented. ({NodeCacheManager.Names.Where(x => x.Key == Chunk.Remap(classID.Value)).Select(x => x.Value).FirstOrDefault() ?? "unknown class"})");

            NodeCacheManager.AvailableClassConstructors.TryGetValue(classID.Value, out Func<CMwNod> constructor);

            var node = (T)constructor();

            Parse(node, r, progress);

            return node;
        }

        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0071:Zjednodušit interpolaci", Justification = "<Čeká>")]
        public static void Parse<T>(T node, GameBoxReader r, IProgress<GameBoxReadProgress>? progress = null) where T : CMwNod
        {
            var stopwatch = Stopwatch.StartNew();

            node.GBX = r.GBX;

            var type = node.GetType();

            var chunks = new ChunkSet { Node = node };
            node.Chunks = chunks;

            uint? previousChunk = null;

            while (!r.BaseStream.CanSeek || r.BaseStream.Position < r.BaseStream.Length)
            {
                if (r.BaseStream.CanSeek && r.BaseStream.Position + 4 > r.BaseStream.Length)
                {
                    Debug.WriteLine($"Unexpected end of the stream: {r.BaseStream.Position.ToString()}/{r.BaseStream.Length.ToString()}");
                    var bytes = r.ReadBytes((int)(r.BaseStream.Length - r.BaseStream.Position));
                    break;
                }

                var chunkID = r.ReadUInt32();

                if (chunkID == 0xFACADE01) // no more chunks
                {
                    break;
                }
                else
                {
                    var logChunk = new StringBuilder("[");
                    logChunk.Append(node.ClassName);
                    logChunk.Append("] 0x");
                    logChunk.Append(chunkID.ToString("X8"));

                    if (r.BaseStream.CanSeek)
                    {
                        logChunk.Append(" (");
                        logChunk.Append(((float)r.BaseStream.Position / r.BaseStream.Length).ToString("0.00%"));
                        logChunk.Append(')');
                    }

                    if (node.GBX?.ID.HasValue == true && Remap(node.GBX.ID.Value) == node.ID)
                    {
                        Log.Write(logChunk.ToString());
                    }
                    else
                    {
                        logChunk.Insert(0, "~ ");
                        Log.Write(logChunk.ToString());
                    }
                }

                Chunk chunk;

                var chunkRemapped = Chunk.Remap(chunkID);

                Type chunkClass = null;

                var reflected = ((chunkRemapped & 0xFFFFF000) == node.ID || NodeCacheManager.AvailableInheritanceClasses[type].Contains(chunkRemapped & 0xFFFFF000))
                    && (NodeCacheManager.AvailableChunkClasses[type].TryGetValue(chunkRemapped, out chunkClass) || NodeCacheManager.AvailableChunkClasses[type].TryGetValue(chunkID & 0xFFF, out chunkClass));

                var skippable = reflected && chunkClass.BaseType.GetGenericTypeDefinition() == typeof(SkippableChunk<>);
                
                // Unknown or skippable chunk
                if (!reflected || skippable)
                {
                    var skip = r.ReadUInt32();

                    if (skip != 0x534B4950)
                    {
                        if (chunkID != 0 && !reflected)
                        {
                            var logChunkError = $"[{node.ClassName}] 0x{chunkID.ToString("X8")} ERROR (wrong chunk format or unknown unskippable chunk)";
                            if (node.GBX?.ID.HasValue == true && Remap(node.GBX.ID.Value) == node.ID)
                                Log.Write(logChunkError, ConsoleColor.Red);
                            else
                                Log.Write("~ " + logChunkError, ConsoleColor.Red);

                            throw new Exception($"Wrong chunk format or unskippable chunk: 0x{chunkID:X8} (" +
                                $"{NodeCacheManager.Names.Where(x => x.Key == Chunk.Remap(chunkID & 0xFFFFF000)).Select(x => x.Value).FirstOrDefault() ?? "unknown class"})" +
                                $"\nPrevious chunk: 0x{previousChunk ?? 0:X8} (" +
                                $"{(previousChunk.HasValue ? (NodeCacheManager.Names.Where(x => x.Key == Chunk.Remap(previousChunk.Value & 0xFFFFF000)).Select(x => x.Value).FirstOrDefault() ?? "unknown class") : "not a class")})");

                            /* Usually breaks in the current state and causes confusion
                             * 
                             * var buffer = BitConverter.GetBytes(chunkID);
                            using (var restMs = new MemoryStream(ushort.MaxValue))
                            {
                                restMs.Write(buffer, 0, buffer.Length);

                                while (r.PeekUInt32() != 0xFACADE01)
                                    restMs.WriteByte(r.ReadByte());

                                node.Rest = restMs.ToArray();
                            }
                            Debug.WriteLine("FACADE found.");*/
                        }
                        break;
                    }

                    var chunkDataSize = r.ReadInt32();
                    var chunkData = new byte[chunkDataSize];
                    if (chunkDataSize > 0)
                        r.Read(chunkData, 0, chunkDataSize);

                    if (reflected)
                    {
                        var attributesAvailable = NodeCacheManager.AvailableChunkAttributes[type].TryGetValue(
                            chunkRemapped, out IEnumerable<Attribute> attributes);

                        if (!attributesAvailable)
                        {
                            throw new Exception();
                        }

                        var ignoreChunkAttribute = default(IgnoreChunkAttribute);
                        var chunkAttribute = default(ChunkAttribute);

                        foreach (var att in attributes)
                        {
                            if (att is IgnoreChunkAttribute ignoreChunkAtt)
                                ignoreChunkAttribute = ignoreChunkAtt;
                            if (att is ChunkAttribute chunkAtt)
                                chunkAttribute = chunkAtt;
                        }

                        if (chunkAttribute == null)
                        {
                            throw new Exception();
                        }

                        NodeCacheManager.AvailableChunkConstructors[type].TryGetValue(chunkRemapped,
                            out Func<Chunk> constructor);

                        var c = constructor();
                        c.Node = node;
                        c.GBX = node.GBX;
                        ((ISkippableChunk)c).Data = chunkData;
                        if (chunkData == null || chunkData.Length == 0)
                            ((ISkippableChunk)c).Discovered = true;
                        chunks.Add(c);

                        if (ignoreChunkAttribute == null)
                        {
                            c.OnLoad();

                            if (chunkAttribute.ProcessSync)
                                ((ISkippableChunk)c).Discover();
                        }

                        chunk = c;
                    }
                    else
                    {
                        Debug.WriteLine("Unknown skippable chunk: " + chunkID.ToString("X"));
                        chunk = (Chunk)Activator.CreateInstance(typeof(SkippableChunk<>).MakeGenericType(type), node, chunkRemapped, chunkData);
                        chunk.GBX = node.GBX;
                        chunks.Add(chunk);
                    }
                }
                else // Known or unskippable chunk
                {
                    // Faster than caching
                    NodeCacheManager.AvailableChunkConstructors[type].TryGetValue(chunkRemapped,
                        out Func<Chunk> constructor);

                    var c = constructor();
                    c.Node = node;

                    c.OnLoad();

                    chunks.Add(c);

                    //r.Chunk = (Chunk)c; // Set chunk temporarily for reading

                    var posBefore = r.BaseStream.Position;

                    GameBoxReaderWriter gbxrw = new GameBoxReaderWriter(r);

                    var attributesAvailable = NodeCacheManager.AvailableChunkAttributes[type].TryGetValue(
                        chunkRemapped, out IEnumerable<Attribute> attributes);

                    if (!attributesAvailable)
                    {
                        throw new Exception();
                    }

                    var ignoreChunkAttribute = default(IgnoreChunkAttribute);
                    var autoReadWriteChunkAttribute = default(AutoReadWriteChunkAttribute);

                    foreach (var att in attributes)
                    {
                        if (att is IgnoreChunkAttribute ignoreChunkAtt)
                            ignoreChunkAttribute = ignoreChunkAtt;
                        if (att is AutoReadWriteChunkAttribute autoReadWriteChunkAtt)
                            autoReadWriteChunkAttribute = autoReadWriteChunkAtt;
                    }

                    try
                    {
                        if (ignoreChunkAttribute == null)
                        {
                            if(autoReadWriteChunkAttribute == null)
                                c.ReadWrite(node, gbxrw);
                            else
                            {
                                var unknown = new GameBoxWriter(c.Unknown, r.Lookbackable);
                                var unknownData = r.ReadUntilFacade().ToArray();
                                unknown.Write(unknownData, 0, unknownData.Length);
                            }
                        }
                        else
                            throw new Exception($"Chunk 0x{(chunkID & 0xFFF).ToString("x3")} from class {node.ClassName} is known but its content is unknown to read.");
                    }
                    catch (EndOfStreamException)
                    {
                        Debug.WriteLine($"Unexpected end of the stream while reading the chunk.");
                    }

                    c.Progress = (int)(r.BaseStream.Position - posBefore);

                    chunk = c;
                }

                progress?.Report(new GameBoxReadProgress(GameBoxReadProgressStage.Body, (float)r.BaseStream.Position / r.BaseStream.Length, node.GBX, chunk));

                previousChunk = chunkID;
            }

            stopwatch.Stop();

            var logNodeCompletion = $"[{node.ClassName}] DONE! ({stopwatch.Elapsed.TotalMilliseconds.ToString()}ms)";
            if (node.GBX.ID.HasValue == true && Remap(node.GBX.ID.Value) == node.ID)
                Log.Write(logNodeCompletion, ConsoleColor.Green);
            else
                Log.Write($"~ {logNodeCompletion}", ConsoleColor.Green);
        }

        public void Read(GameBoxReader r)
        {
            throw new NotImplementedException($"Node doesn't support Read.");
        }

        public void Write(GameBoxWriter w)
        {
            Write(w, IDRemap.Latest);
        }

        public void Write(GameBoxWriter w, IDRemap remap)
        {
            var stopwatch = Stopwatch.StartNew();

            int counter = 0;

            var type = GetType();
            var writingNotSupported = type.GetCustomAttribute<WritingNotSupportedAttribute>() != null;
            if (writingNotSupported)
                throw new NotSupportedException($"Writing of {type.Name} is not supported.");

            foreach (Chunk chunk in Chunks)
            {
                counter++;

                var logChunk = $"[{ClassName}] 0x{chunk.ID.ToString("X8")} ({((float)counter / Chunks.Count).ToString("0.00%")})";
                if (GBX.ID.HasValue == true && GBX.ID.Value == ID)
                    Log.Write(logChunk);
                else
                    Log.Write($"~ {logChunk}");

                chunk.Node = this;
                chunk.Unknown.Position = 0;

                if (chunk is ILookbackable l)
                {
                    l.IdWritten = false;
                    l.IdStrings.Clear();
                }

                using (var ms = new MemoryStream())
                using (var msW = new GameBoxWriter(ms, w.Lookbackable))
                {
                    var rw = new GameBoxReaderWriter(msW);

                    try
                    {
                        if (chunk is ISkippableChunk s && !s.Discovered)
                            s.Write(msW);
                        else if (!Attribute.IsDefined(chunk.GetType(), typeof(AutoReadWriteChunkAttribute)))
                            chunk.ReadWrite(this, rw);
                        else
                            msW.Write(chunk.Unknown.ToArray(), 0, (int)chunk.Unknown.Length);

                        w.Write(Chunk.Remap(chunk.ID, remap));

                        if (chunk is ISkippableChunk)
                        {
                            w.Write(0x534B4950);
                            w.Write((int)ms.Length);
                        }

                        w.Write(ms.ToArray(), 0, (int)ms.Length);
                    }
                    catch (NotImplementedException e)
                    {
                        if (chunk is ISkippableChunk)
                        {
                            Debug.WriteLine(e.Message);
                            Debug.WriteLine("Ignoring the skippable chunk from writing.");
                        }
                        else throw e; // Unskippable chunk must have a Write implementation
                    }
                }
            }

            w.Write(0xFACADE01);

            stopwatch.Stop();

            var logNodeCompletion = $"[{ClassName}] DONE! ({stopwatch.Elapsed.TotalMilliseconds}ms)";
            if (GBX.ID.HasValue == true && GBX.ID.Value == ID)
                Log.Write(logNodeCompletion, ConsoleColor.Green);
            else
                Log.Write($"~ {logNodeCompletion}", ConsoleColor.Green);
        }

        public T GetChunk<T>() where T : Chunk
        {
            return Chunks.Get<T>();
        }

        public T CreateChunk<T>() where T : Chunk
        {
            return Chunks.Create<T>();
        }

        public bool RemoveChunk<T>() where T : Chunk
        {
            return Chunks.Remove<T>();
        }

        public bool RemoveChunk(uint chunkID)
        {
            return Chunks.Remove(chunkID);
        }

        public bool TryGetChunk<T>(out T chunk) where T : Chunk
        {
            return Chunks.TryGet(out chunk);
        }

        public void DiscoverChunk<T>() where T : ISkippableChunk
        {
            Chunks.Discover<T>();
        }

        public void DiscoverChunks<T1, T2>() where T1 : ISkippableChunk where T2 : ISkippableChunk
        {
            Chunks.Discover<T1, T2>();
        }

        public void DiscoverChunks<T1, T2, T3>() where T1 : ISkippableChunk where T2 : ISkippableChunk where T3 : ISkippableChunk
        {
            Chunks.Discover<T1, T2, T3>();
        }

        public void DiscoverChunks<T1, T2, T3, T4>()
            where T1 : ISkippableChunk
            where T2 : ISkippableChunk
            where T3 : ISkippableChunk
            where T4 : ISkippableChunk
        {
            Chunks.Discover<T1, T2, T3, T4>();
        }

        public void DiscoverChunks<T1, T2, T3, T4, T5>()
            where T1 : ISkippableChunk
            where T2 : ISkippableChunk
            where T3 : ISkippableChunk
            where T4 : ISkippableChunk
            where T5 : ISkippableChunk
        {
            Chunks.Discover<T1, T2, T3, T4, T5>();
        }

        public void DiscoverChunks<T1, T2, T3, T4, T5, T6>()
            where T1 : ISkippableChunk
            where T2 : ISkippableChunk
            where T3 : ISkippableChunk
            where T4 : ISkippableChunk
            where T5 : ISkippableChunk
            where T6 : ISkippableChunk
        {
            Chunks.Discover<T1, T2, T3, T4, T5, T6>();
        }

        /// <summary>
        /// Discovers all chunks in the node.
        /// </summary>
        /// <exception cref="AggregateException"/>
        public void DiscoverAllChunks()
        {
            Chunks.DiscoverAll();

            foreach (var nodeProperty in GetType().GetProperties())
            {
                if (nodeProperty.PropertyType.IsSubclassOf(typeof(CMwNod)))
                {
                    var node = nodeProperty.GetValue(this) as CMwNod;
                    node?.DiscoverAllChunks();
                }
            }
        }

        /// <summary>
        /// Makes a <see cref="GameBox"/> from this node. NOTE: Non-generic <see cref="GameBox"/> doesn't have a Save method.
        /// </summary>
        /// <param name="headerInfo"></param>
        /// <returns></returns>
        public GameBox ToGBX(GameBoxHeaderInfo headerInfo)
        {
            return (GameBox)Activator.CreateInstance(typeof(GameBox<>).MakeGenericType(GetType()), this, headerInfo);
        }

        /// <summary>
        /// Makes a <see cref="GameBox"/> from this node. You can explicitly cast it to <see cref="GameBox{T}"/> depending on the <see cref="CMwNod"/>. NOTE: Non-generic <see cref="GameBox"/> doesn't have a Save method.
        /// </summary>
        /// <returns></returns>
        public GameBox ToGBX()
        {
            return (GameBox)Activator.CreateInstance(typeof(GameBox<>).MakeGenericType(GetType()), this);
        }

        private void Save(Type[] types, object[] parameters)
        {
            var type = GetType();
            var gbxType = GBX?.GetType();
            var gbxOfType = typeof(GameBox<>).MakeGenericType(type);

            GameBox gbx;

            if (gbxOfType == gbxType)
                gbx = GBX;
            else
                gbx = (GameBox)Activator.CreateInstance(gbxOfType, this);

            _ = gbxOfType.GetMethod("Save", types).Invoke(gbx, parameters);
        }

        /// <exception cref="NotSupportedException"/>
        public void Save()
        {
            Save(new Type[0], new object[0]);
        }

        /// <exception cref="NotSupportedException"/>
        public void Save(IDRemap remap)
        {
            Save(new Type[] { typeof(IDRemap) }, new object[] { remap });
        }

        /// <exception cref="NotSupportedException"/>
        public void Save(string fileName)
        {
            Save(new Type[] { typeof(string) }, new object[] { fileName });
        }

        /// <exception cref="NotSupportedException"/>
        public void Save(string fileName, IDRemap remap)
        {
            Save(new Type[] { typeof(string), typeof(IDRemap) }, new object[] { fileName, remap });
        }

        /// <exception cref="NotSupportedException"/>
        public void Save(Stream stream)
        {
            Save(new Type[] { typeof(Stream) }, new object[] { stream });
        }

        /// <exception cref="NotSupportedException"/>
        public void Save(Stream stream, IDRemap remap)
        {
            Save(new Type[] { typeof(Stream), typeof(IDRemap) }, new object[] { stream, remap });
        }

        public static uint Remap(uint id)
        {
            if (NodeCacheManager.Mappings.TryGetValue(id, out uint newerClassID))
                return newerClassID;
            return id;
        }
    }
}
