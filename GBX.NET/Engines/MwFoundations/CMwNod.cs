using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace GBX.NET.Engines.MwFoundations
{
    [Node(0x01001000)]
    public class CMwNod
    {
        public static Dictionary<uint, string> Names { get; }
        public static Dictionary<uint, uint> Mappings { get; } // key: older, value: newer

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
                if (Names.TryGetValue(ID, out string name))
                    return name;
                return GetType().FullName.Substring("GBX.NET.Engines".Length + 1).Replace(".", "::");
            }
        }

        public static Dictionary<uint, Type> AvailableClasses { get; }
        public static Dictionary<Type, List<uint>> AvailableInheritanceClasses { get; }
        public static Dictionary<Type, Dictionary<uint, Type>> AvailableChunkClasses { get; }
        public static Dictionary<Type, Dictionary<uint, Type>> AvailableHeaderChunkClasses { get; }

        internal CMwNod()
        {
            ID = GetType().GetCustomAttribute<NodeAttribute>().ID;
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

        public static T[] ParseArray<T>(GameBoxReader r) where T : CMwNod
        {
            var count = r.ReadInt32();
            var array = new T[count];

            for (var i = 0; i < count; i++)
                array[i] = Parse<T>(r);

            return array;
        }

        public static T Parse<T>(GameBoxReader r, uint? classID = null, IProgress<GameBoxReadProgress> progress = null) where T : CMwNod
        {
            if (!classID.HasValue)
                classID = r.ReadUInt32();

            if (classID == uint.MaxValue) return null;

            classID = Remap(classID.Value);

            if (!AvailableClasses.TryGetValue(classID.Value, out Type type))
                throw new NotImplementedException($"Node ID 0x{classID.Value:X8} is not implemented. ({Names.Where(x => x.Key == Chunk.Remap(classID.Value)).Select(x => x.Value).FirstOrDefault() ?? "unknown class"})");

            var node = (T)Activator.CreateInstance(type);

            Parse(node, r, progress);

            return node;
        }

        public static void Parse<T>(T node, GameBoxReader r, IProgress<GameBoxReadProgress> progress = null) where T : CMwNod
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
                    Debug.WriteLine($"Unexpected end of the stream: {r.BaseStream.Position}/{r.BaseStream.Length}");
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
                    var logChunk = $"[{node.ClassName}] 0x{chunkID:X8}";
                    if (r.BaseStream.CanSeek)
                        logChunk += $" ({(float)r.BaseStream.Position / r.BaseStream.Length:0.00%})";

                    if (node.GBX?.ID.HasValue == true && Remap(node.GBX.ID.Value) == node.ID)
                        Log.Write(logChunk);
                    else
                        Log.Write($"~ {logChunk}");
                }

                Chunk chunk;

                var chunkRemapped = Chunk.Remap(chunkID);

                Type chunkClass = null;

                var reflected = ((chunkRemapped & 0xFFFFF000) == node.ID || AvailableInheritanceClasses[type].Contains(chunkRemapped & 0xFFFFF000))
                    && (AvailableChunkClasses[type].TryGetValue(chunkRemapped, out chunkClass) || AvailableChunkClasses[type].TryGetValue(chunkID & 0xFFF, out chunkClass));

                var skippable = reflected && chunkClass.BaseType.GetGenericTypeDefinition() == typeof(SkippableChunk<>);
                
                // Unknown or skippable chunk
                if (!reflected || skippable)
                {
                    var skip = r.ReadUInt32();

                    if (skip != 0x534B4950)
                    {
                        if (chunkID != 0 && !reflected)
                        {
                            var logChunkError = $"[{node.ClassName}] 0x{chunkID:X8} ERROR (wrong chunk format or unknown unskippable chunk)";
                            if (node.GBX?.ID.HasValue == true && Remap(node.GBX.ID.Value) == node.ID)
                                Log.Write(logChunkError, ConsoleColor.Red);
                            else
                                Log.Write($"~ {logChunkError}", ConsoleColor.Red);

                            throw new Exception($"Wrong chunk format or unskippable chunk: 0x{chunkID:X8} (" +
                                $"{Names.Where(x => x.Key == Chunk.Remap(chunkID & 0xFFFFF000)).Select(x => x.Value).FirstOrDefault() ?? "unknown class"})" +
                                $"\nPrevious chunk: 0x{previousChunk ?? 0:X8} (" +
                                $"{(previousChunk.HasValue ? (Names.Where(x => x.Key == Chunk.Remap(previousChunk.Value & 0xFFFFF000)).Select(x => x.Value).FirstOrDefault() ?? "unknown class") : "not a class")})");

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

                    if (reflected && chunkClass.GetCustomAttribute<IgnoreChunkAttribute>() == null)
                    {
                        var constructor = Array.Find(chunkClass.GetConstructors(), x => x.GetParameters().Length == 0);
                        if(constructor == null)
                            throw new ArgumentException($"{type.FullName} doesn't have a parameterless constructor.");

                        var c = (Chunk)constructor.Invoke(new object[0]);
                        c.Node = node;
                        c.GBX = node.GBX;
                        ((ISkippableChunk)c).Data = chunkData;
                        if (chunkData == null || chunkData.Length == 0)
                            ((ISkippableChunk)c).Discovered = true;
                        c.OnLoad();

                        chunks.Add(c);

                        if (chunkClass.GetCustomAttribute<ChunkAttribute>().ProcessSync)
                            ((ISkippableChunk)c).Discover();

                        chunk = c;
                    }
                    else
                    {
                        Debug.WriteLine("Unknown skippable chunk: " + chunkID.ToString("X"));
                        chunk = (Chunk)Activator.CreateInstance(typeof(SkippableChunk<>).MakeGenericType(type), node, chunkRemapped, chunkData);
                        chunks.Add(chunk);
                    }
                }
                else // Known or unskippable chunk
                {
                    var constructor = Array.Find(chunkClass.GetConstructors(), x => x.GetParameters().Length == 0);

                    if (constructor == null)
                        throw new ArgumentException($"{type.FullName} doesn't have a parameterless constructor.");

                    var c = (Chunk)constructor.Invoke(new object[0]);
                    c.Node = node;
                    c.GBX = node.GBX;
                    c.OnLoad();

                    chunks.Add(c);

                    //r.Chunk = (Chunk)c; // Set chunk temporarily for reading

                    var posBefore = r.BaseStream.Position;

                    GameBoxReaderWriter gbxrw = new GameBoxReaderWriter(r);

                    var attributes = chunkClass.GetCustomAttributes();
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
                                var unknown = new GameBoxWriter(((Chunk)c).Unknown, r.Lookbackable);
                                var unknownData = r.ReadUntilFacade();
                                unknown.Write(unknownData, 0, unknownData.Length);
                            }
                        }
                        else
                            throw new Exception($"Chunk 0x{chunkID & 0xFFF:x3} from class {node.ClassName} is known but its content is unknown to read.");
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

            var logNodeCompletion = $"[{node.ClassName}] DONE! ({stopwatch.Elapsed.TotalMilliseconds}ms)";
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

            foreach (Chunk chunk in Chunks)
            {
                counter++;

                var logChunk = $"[{ClassName}] 0x{chunk.ID:X8} ({(float)counter / Chunks.Count:0.00%})";
                if (GBX.ID.HasValue == true && Remap(GBX.ID.Value) == ID)
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
                using (var msW = new GameBoxWriter(ms, chunk as ILookbackable ?? GBX.Body))
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
            if (GBX.ID.HasValue == true && Remap(GBX.ID.Value) == ID)
                Log.Write(logNodeCompletion, ConsoleColor.Green);
            else
                Log.Write($"~ {logNodeCompletion}", ConsoleColor.Green);
        }

        private static void DefineNames()
        {
            var watch = Stopwatch.StartNew();

            using (StringReader reader = new StringReader(Resources.ClassID))
            {
                var en = "";
                var engineName = "";

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var ch = "000";

                    var className = "";

                    if (line.StartsWith("  "))
                    {
                        var cl = line.Substring(2, 3);
                        if (line.Length - 6 > 0) className = line.Substring(6);

                        var classIDString = $"{en}{cl}{ch}";

                        if (uint.TryParse(classIDString, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint classID))
                        {
                            Names[classID] = engineName + "::" + className;
                        }
                        else
                        {
                            Debug.WriteLine($"Invalid class ID {classIDString}, skipping");
                        }
                    }
                    else
                    {
                        en = line.Substring(0, 2);
                        if (line.Length - 3 > 0) engineName = line.Substring(3);
                    }
                }
            }

            Debug.WriteLine("Classes named in " + watch.Elapsed.TotalMilliseconds + "ms");
        }

        private static void DefineMappings()
        {
            var watch = Stopwatch.StartNew();

            using (StringReader reader = new StringReader(Resources.ClassIDMappings))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var valueKey = line.Split(new string[] { " -> " }, StringSplitOptions.None);
                    if (valueKey.Length == 2)
                    {
                        if (uint.TryParse(valueKey[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint key)
                        && uint.TryParse(valueKey[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint value))
                        {
                            if (Mappings.ContainsValue(key)) // Virtual Skipper solution
                                Mappings[Mappings.FirstOrDefault(x => x.Value == key).Key] = value;
                            Mappings[key] = value;
                        }
                    }
                }
            }

            Debug.WriteLine("Mappings defined in " + watch.Elapsed.TotalMilliseconds + "ms");
        }

        private static void DefineTypes()
        {
            var watch = Stopwatch.StartNew();

            var assembly = Assembly.GetExecutingAssembly();

            IEnumerable<Type> types;

            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                types = e.Types;
            }

            var engineRelatedTypes = types.Where(t =>
                 t?.IsClass == true
              && t.Namespace?.StartsWith("GBX.NET.Engines") == true);

            var availableClassesByType = new Dictionary<Type, uint>();

            foreach (var type in engineRelatedTypes)
            {
                if (type.IsSubclassOf(typeof(CMwNod)) || type == typeof(CMwNod)) // Engine types
                {
                    var id = type.GetCustomAttribute<NodeAttribute>()?.ID;

                    if (id.HasValue)
                    {
                        AvailableClasses.Add(id.Value, type);
                        availableClassesByType.Add(type, id.Value);
                    }
                    else
                    {
                        throw new Exception($"{type.Name} misses NodeAttribute.");
                    }
                }
            }

            var availableInheritanceTypes = new Dictionary<Type, List<Type>>();

            foreach (var typePair in AvailableClasses)
            {
                var id = typePair.Key;
                var type = typePair.Value;

                List<uint> classes = new List<uint>();
                List<Type> inheritedTypes = new List<Type>();

                Type currentType = type.BaseType;

                while (currentType != typeof(object))
                {
                    classes.Add(availableClassesByType[currentType]);
                    inheritedTypes.Add(currentType);

                    currentType = currentType.BaseType;
                }

                AvailableInheritanceClasses[type] = classes;
                availableInheritanceTypes[type] = inheritedTypes;

                var chunks = type.GetNestedTypes().Where(x => x.IsSubclassOf(typeof(Chunk)));

                var availableChunkClasses = new Dictionary<uint, Type>();
                var availableHeaderChunkClasses = new Dictionary<uint, Type>();

                foreach (var chunk in chunks)
                {
                    var chunkAttribute = chunk.GetCustomAttribute<ChunkAttribute>();

                    if (chunkAttribute == null)
                        throw new Exception($"Chunk {chunk.FullName} doesn't have ChunkAttribute.");

                    if (chunk.GetInterface(nameof(IHeaderChunk)) == null)
                    {
                        availableChunkClasses.Add(chunkAttribute.ID, chunk);
                    }
                    else
                    {
                        availableHeaderChunkClasses.Add(chunkAttribute.ID, chunk);
                    }
                }

                AvailableChunkClasses.Add(type, availableChunkClasses);
                AvailableHeaderChunkClasses.Add(type, availableHeaderChunkClasses);
            }

            foreach (var typePair in availableInheritanceTypes)
            {
                var mainType = typePair.Key;

                foreach (var type in typePair.Value)
                {
                    foreach (var chunkType in AvailableChunkClasses[type])
                    {
                        AvailableChunkClasses[mainType][chunkType.Key] = chunkType.Value;
                    }
                }
            }

            Debug.WriteLine("Types defined in " + watch.Elapsed.TotalMilliseconds + "ms");
        }

        static CMwNod()
        {
            Names = new Dictionary<uint, string>();
            Mappings = new Dictionary<uint, uint>();

            AvailableClasses = new Dictionary<uint, Type>();
            AvailableInheritanceClasses = new Dictionary<Type, List<uint>>();
            AvailableChunkClasses = new Dictionary<Type, Dictionary<uint, Type>>();
            AvailableHeaderChunkClasses = new Dictionary<Type, Dictionary<uint, Type>>();

            DefineNames();
            DefineMappings();
            DefineTypes();
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
        /// <param name="header"></param>
        /// <returns></returns>
        public GameBox ToGBX(GameBoxHeader header)
        {
            return (GameBox)Activator.CreateInstance(typeof(GameBox<>).MakeGenericType(GetType()), this, header);
        }

        /// <summary>
        /// Makes a <see cref="GameBox"/> from this node. You can explicitly cast it to <see cref="GameBox{T}"/> depending on the <see cref="CMwNod"/>. NOTE: Non-generic <see cref="GameBox"/> doesn't have a Save method.
        /// </summary>
        /// <returns></returns>
        public GameBox ToGBX()
        {
            return (GameBox)Activator.CreateInstance(typeof(GameBox<>).MakeGenericType(GetType()), this);
        }

        public static uint Remap(uint id)
        {
            if (Mappings.TryGetValue(id, out uint newerClassID))
                return newerClassID;
            return id;
        }
    }
}
