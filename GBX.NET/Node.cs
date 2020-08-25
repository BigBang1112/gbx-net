using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GBX.NET
{
    public class Node
    {
        public static Dictionary<uint, string> Names { get; }
        public static Dictionary<uint, uint> Mappings { get; } // key: older, value: newer

        public IGameBoxBody Body => Lookbackable as IGameBoxBody;

        public uint ID { get; }
        public ILookbackable Lookbackable { get; }
        public ChunkList Chunks { get; set; }
        public uint? FaultyChunk { get; private set; }
        public byte[] Rest { get; private set; }
        public bool Unknown { get; internal set; }

        public uint ModernID
        {
            get
            {
                if (Mappings.TryGetValue(ID, out uint newerClassID))
                    return newerClassID;
                return ID;
            }
        }

        public string ClassName
        {
            get
            {
                if (Names.TryGetValue(ModernID, out string name))
                    return name;
                return GetType().FullName.Substring("GBX.NET.Engines".Length+1).Replace(".", "::");
            }
        }

        [IgnoreDataMember]
        public uint[] ChunkIDs
        {
            get
            {
                var chunkTypes = GetType().GetNestedTypes().Where(x => x.BaseType == typeof(Chunk)).ToArray();
                var array = new uint[chunkTypes.Length];
                
                for(var i = 0; i < array.Length; i++)
                {
                    var att = chunkTypes[i].GetCustomAttribute<ChunkAttribute>();
                    array[i] = att.ClassID + att.ChunkID;
                }

                return array;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookbackable">Usually <see cref="GameBoxHeader{T}"/> or <see cref="GameBoxBody{T}"/></param>
        /// <param name="classID"></param>
        public Node(ILookbackable lookbackable, uint classID, params Chunk[] chunks)
        {
            Lookbackable = lookbackable;
            ID = classID;
            Chunks = new ChunkList(chunks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookbackable">Usually <see cref="GameBoxHeader{T}"/> or <see cref="GameBoxBody{T}"/></param>
        /// <param name="classID"></param>
        public Node(ILookbackable lookbackable, uint classID) : this(lookbackable, classID, new Chunk[0])
        {

        }

        public Node(ILookbackable lookbackable) 
        {
            Lookbackable = lookbackable;
            ID = GetType().GetCustomAttribute<NodeAttribute>().ID;
            Chunks = new ChunkList();
        }

        static Type GetBaseType(Type t)
        {
            if (t == null)
                return null;
            if (t.BaseType == typeof(Node))
                return t.BaseType;
            return GetBaseType(t.BaseType);
        }

        public static Node Parse(ILookbackable body, uint classID, GameBoxReader r)
        {
            var hasNewerID = Mappings.TryGetValue(classID, out uint newerClassID);
            if (!hasNewerID) newerClassID = classID;

            var availableClass = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass
                   && x.Namespace.StartsWith("GBX.NET.Engines") && GetBaseType(x) == typeof(Node)
                   && (x.GetCustomAttribute<NodeAttribute>().ID == newerClassID)).FirstOrDefault();

            var inheritanceClasses = new List<uint>();

            if (availableClass == null)
            {
                Debug.WriteLine("Unknown node: " + classID.ToString("x8"));
                return new Node(body, classID) { Unknown = true };
            }

            return Parse(availableClass, body, r);
        }

        public static Node[] ParseArray(Type type, ILookbackable body, GameBoxReader r)
        {
            var count = r.ReadInt32();
            var array = new Node[count];

            List<uint> inheritanceClasses = null;
            Dictionary<uint, Type> availableChunkClasses = null;

            for (var i = 0; i < count; i++)
            {
                _ = r.ReadUInt32();

                if (i == 0)
                    array[i] = Parse(type, body, r, out inheritanceClasses, out availableChunkClasses);
                else
                    array[i] = Parse(type, body, r, inheritanceClasses, availableChunkClasses);
            }

            return array;
        }

        public static T[] ParseArray<T>(ILookbackable body, GameBoxReader r) where T : Node
        {
            return ParseArray(typeof(T), body, r).Cast<T>().ToArray();
        }

        private static Node Parse(Type type, ILookbackable body, GameBoxReader r, out List<uint> inheritanceClasses, out Dictionary<uint, Type> availableChunkClasses)
        {
            inheritanceClasses = GetInheritance(type);

            static List<uint> GetInheritance(Type t)
            {
                List<uint> classes = new List<uint>();

                Type cur = t.BaseType;

                while (cur != typeof(Node))
                {
                    classes.Add(cur.GetCustomAttribute<NodeAttribute>().ID);
                    cur = cur.BaseType;
                }

                return classes;
            }

            availableChunkClasses = type.GetNestedTypes().Where(x => x.IsClass
                && x.Namespace.StartsWith("GBX.NET.Engines") && (x.BaseType == typeof(Chunk) || x.BaseType == typeof(SkippableChunk))
                && (x.GetCustomAttribute<ChunkAttribute>().ClassID == type.GetCustomAttribute<NodeAttribute>().ID)).ToDictionary(x => x.GetCustomAttribute<ChunkAttribute>().ChunkID);

            foreach (var cls in inheritanceClasses)
            {
                var availableInheritanceClass = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass
                   && x.Namespace.StartsWith("GBX.NET.Engines") && (GetBaseType(x) == typeof(Node))
                   && (x.GetCustomAttribute<NodeAttribute>().ID == cls)).FirstOrDefault();

                foreach (var chunkType in availableInheritanceClass.GetNestedTypes().Where(x => x.IsClass
                    && x.Namespace.StartsWith("GBX.NET.Engines") && (x.BaseType == typeof(Chunk) || x.BaseType == typeof(SkippableChunk))
                    && (x.GetCustomAttribute<ChunkAttribute>().ClassID == cls)).ToDictionary(x => x.GetCustomAttribute<ChunkAttribute>().ChunkID))
                {
                    availableChunkClasses[chunkType.Key + cls] = chunkType.Value;
                }
            }

            return Parse(type, body, r, inheritanceClasses, availableChunkClasses);
        }

        public static Node Parse(Type type, ILookbackable body, GameBoxReader r)
        {
            return Parse(type, body, r, out List<uint> _, out Dictionary<uint, Type> _);
        }

        public static T Parse<T>(ILookbackable body, GameBoxReader r) where T : Node
        {
            return (T)Parse(typeof(T), body, r);
        }

        private static Node Parse(Type type, ILookbackable body, GameBoxReader r, List<uint> inheritanceClasses, Dictionary<uint, Type> availableChunkClasses)
        {
            var readNodeStart = DateTime.Now;

            Node node = (Node)Activator.CreateInstance(type, body, type.GetCustomAttribute<NodeAttribute>().ID);

            var chunks = new ChunkList();

            uint? previousChunk = null;

            while (true)
            {
                var chunkID = r.ReadUInt32();

                if (chunkID == 0xFACADE01) // no more chunks
                {
                    Debug.WriteLine("FACADE");
                    break;
                }
                else if(chunkID == 0)
                {
                    // weird case after ending node reference
                }
                else
                {
                    if (node.Body != null && node.Body.GBX.ClassID.HasValue && Remap(node.Body.GBX.ClassID.Value) == node.ID)
                        Log.Write($"[{node.ClassName}] 0x{chunkID:x8} ({(float)r.BaseStream.Position / r.BaseStream.Length:0.00%})");
                    else
                        Log.Write($"~ [{node.ClassName}] 0x{chunkID:x8} ({(float)r.BaseStream.Position / r.BaseStream.Length:0.00%})");
                }

                Type chunkClass = null;

                var reflected = ((Chunk.Remap(chunkID) & 0xFFFFF000) == node.ID || inheritanceClasses.Contains(Chunk.Remap(chunkID) & 0xFFFFF000))
                    && (availableChunkClasses.TryGetValue(chunkID, out chunkClass) || availableChunkClasses.TryGetValue(chunkID & 0xFFF, out chunkClass));

                if (!reflected || chunkClass.BaseType == typeof(SkippableChunk))
                {
                    var skip = r.ReadUInt32();

                    if (skip != 0x534B4950)
                    {
                        if (chunkID != 0 && !reflected)
                        {
                            Debug.WriteLine("Wrong chunk format or unskippable chunk: " + chunkID.ToString("x8")); // Read till facade
                            node.FaultyChunk = chunkID;

                            if (node.Body != null && node.Body.GBX.ClassID.HasValue && Remap(node.Body.GBX.ClassID.Value) == node.ID)
                                Log.Write($"[{node.ClassName}] 0x{chunkID:x8} ERROR (wrong chunk format or unknown unskippable chunk)", ConsoleColor.Red);
                            else
                                Log.Write($"~ [{node.ClassName}] 0x{chunkID:x8} ERROR (wrong chunk format or unknown unskippable chunk)", ConsoleColor.Red);

                            using var restMs = new MemoryStream(ushort.MaxValue);
                            restMs.Write(BitConverter.GetBytes(chunkID));

                            while(r.PeekUInt32() != 0xFACADE01)
                                restMs.WriteByte(r.ReadByte());

                            node.Rest = restMs.ToArray();
                            Debug.WriteLine("FACADE found.");
                        }
                        break;
                    }

                    Debug.WriteLine("Skippable chunk: " + chunkID.ToString("x"));
                    
                    var chunkDataSize = r.ReadInt32();
                    Debug.WriteLine("Chunk size: " + chunkDataSize);
                    var chunkData = new byte[chunkDataSize];
                    if (chunkDataSize > 0)
                        r.Read(chunkData, 0, chunkDataSize);

                    if (reflected && chunkClass.GetCustomAttribute<IgnoreChunkAttribute>() == null)
                    {
                        var c = (SkippableChunk)Activator.CreateInstance(chunkClass, node, chunkData);
                        chunks.Add(c);

                        if (!chunkClass.GetCustomAttribute<ChunkAttribute>().ProcessAsync)
                            c.Discover();
                    }
                    else
                    {
                        Debug.WriteLine("Unknown skippable chunk: " + chunkID.ToString("x"));
                        chunks.Add(new SkippableChunk(node, chunkID, chunkData));
                    }
                }

                if (reflected && chunkClass.BaseType != typeof(SkippableChunk))
                {
                    Debug.WriteLine("Unskippable chunk: " + chunkID.ToString("x8"));

                    if (chunkClass.BaseType == typeof(SkippableChunk)) // Does it ever happen?
                    {
                        var skip = r.ReadUInt32();
                        var chunkDataSize = r.ReadInt32();
                    }

                    var chunk = (Chunk)Activator.CreateInstance(chunkClass, node);
                    chunks.Add(chunk);

                    var posBefore = r.BaseStream.Position;

                    GameBoxReaderWriter gbxrw = new GameBoxReaderWriter(r);
                    chunk.ReadWrite(gbxrw);

                    chunk.Progress = (int)(r.BaseStream.Position - posBefore);
                }

                previousChunk = chunkID;
            }

            if (node.Body != null && node.Body.GBX.ClassID.HasValue && Remap(node.Body.GBX.ClassID.Value) == node.ID)
                Log.Write($"[{node.ClassName}] DONE! ({(DateTime.Now - readNodeStart).TotalMilliseconds}ms)", ConsoleColor.Green);
            else
                Log.Write($"~ [{node.ClassName}] DONE! ({(DateTime.Now - readNodeStart).TotalMilliseconds}ms)", ConsoleColor.Green);

            node.Chunks = chunks;
            return node;
        }

        public void Read(GameBoxReader r)
        {
            throw new NotImplementedException($"Node doesn't support Read.");
        }

        public void Write(GameBoxWriter w)
        {
            Write(w, ClassIDRemap.Latest);
        }

        public void Write(GameBoxWriter w, ClassIDRemap remap)
        {
            int counter = 0;

            foreach (var chunk in Chunks.Values)
            {
                counter += 1;

                chunk.Unknown.Position = 0;

                ILookbackable lb = Lookbackable;

                if (chunk is ILookbackable l)
                {
                    l.LookbackWritten = false;
                    l.LookbackStrings.Clear();

                    lb = l;
                }

                using var ms = new MemoryStream();
                using var msW = new GameBoxWriter(ms, lb);
                var rw = new GameBoxReaderWriter(msW);

                try
                {
                    if (chunk is SkippableChunk s && !s.Discovered)
                        s.Write(msW);
                    else
                        chunk.ReadWrite(rw);

                    w.Write(Chunk.Remap(chunk.ID, remap));

                    if (chunk is SkippableChunk)
                    {
                        w.Write(0x534B4950);
                        w.Write((int)ms.Length);
                    }

                    w.Write(ms.ToArray(), 0, (int)ms.Length);
                }
                catch (NotImplementedException e)
                {
                    if (chunk is SkippableChunk s)
                    {
                        Debug.WriteLine(e.Message);
                        Debug.WriteLine("Ignoring the skippable chunk from writing.");
                    }
                    else throw e; // Unskippable chunk must have a Write implementation
                }
            }

            w.Write(0xFACADE01);
        }

        public T CreateChunk<T>(byte[] data) where T : Chunk
        {
            var chunkId = typeof(T).GetCustomAttribute<ChunkAttribute>().ID;

            if (Chunks.TryGetValue(chunkId, out Chunk c))
                return (T)c;

            T chunk;
            if (typeof(T).BaseType == typeof(SkippableChunk))
                chunk = (T)Activator.CreateInstance(typeof(T), this, data);
            else
            {
                chunk = (T)Activator.CreateInstance(typeof(T), this);
                if (data.Length > 0) chunk.FromData(data);
            }
            if (chunk is ILookbackable l) l.LookbackVersion = 3;
            Chunks.Add(chunk);
            return chunk;
        }

        public T CreateChunk<T>() where T : Chunk
        {
            return CreateChunk<T>(new byte[0]);
        }

        public void InsertChunk(Chunk chunk)
        {
            Chunks.Add(chunk);
        }

        public static T FromGBX<T>(GameBox<T> loadedGbx) where T : Node
        {
            return loadedGbx.MainNode;
        }

        public void DiscoverChunk<TChunk>() where TChunk : SkippableChunk
        {
            foreach (var chunk in Chunks.Values)
                if (chunk is TChunk c)
                    c.Discover();
        }

        public void DiscoverChunks<TChunk1, TChunk2>() where TChunk1 : SkippableChunk where TChunk2 : SkippableChunk
        {
            foreach (var chunk in Chunks.Values)
            {
                if (chunk is TChunk1 c1)
                    c1.Discover();
                if (chunk is TChunk2 c2)
                    c2.Discover();
            }
        }

        public void DiscoverChunks<TChunk1, TChunk2, TChunk3>() where TChunk1 : SkippableChunk where TChunk2 : SkippableChunk where TChunk3 : SkippableChunk
        {
            foreach (var chunk in Chunks.Values)
            {
                if (chunk is TChunk1 c1)
                    c1.Discover();
                if (chunk is TChunk2 c2)
                    c2.Discover();
                if (chunk is TChunk3 c3)
                    c3.Discover();
            }
        }

        public void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4>() where TChunk1 : SkippableChunk where TChunk2 : SkippableChunk where TChunk3 : SkippableChunk where TChunk4 : SkippableChunk
        {
            foreach (var chunk in Chunks.Values)
            {
                if (chunk is TChunk1 c1)
                    c1.Discover();
                if (chunk is TChunk2 c2)
                    c2.Discover();
                if (chunk is TChunk3 c3)
                    c3.Discover();
                if (chunk is TChunk4 c4)
                    c4.Discover();
            }
        }

        public void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5>() where TChunk1 : SkippableChunk where TChunk2 : SkippableChunk where TChunk3 : SkippableChunk where TChunk4 : SkippableChunk where TChunk5 : SkippableChunk
        {
            foreach (var chunk in Chunks.Values)
            {
                if (chunk is TChunk1 c1)
                    c1.Discover();
                if (chunk is TChunk2 c2)
                    c2.Discover();
                if (chunk is TChunk3 c3)
                    c3.Discover();
                if (chunk is TChunk4 c4)
                    c4.Discover();
                if (chunk is TChunk5 c5)
                    c5.Discover();
            }
        }

        public void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5, TChunk6>() where TChunk1 : SkippableChunk where TChunk2 : SkippableChunk where TChunk3 : SkippableChunk where TChunk4 : SkippableChunk where TChunk5 : SkippableChunk where TChunk6 : SkippableChunk
        {
            foreach (var chunk in Chunks.Values)
            {
                if (chunk is TChunk1 c1)
                    c1.Discover();
                if (chunk is TChunk2 c2)
                    c2.Discover();
                if (chunk is TChunk3 c3)
                    c3.Discover();
                if (chunk is TChunk4 c4)
                    c4.Discover();
                if (chunk is TChunk5 c5)
                    c5.Discover();
                if (chunk is TChunk6 c6)
                    c6.Discover();
            }
        }

        public void DiscoverAllChunks()
        {
            foreach (var chunk in Chunks.Values)
                if (chunk is SkippableChunk s)
                    s.Discover();
        }

        public static T FromGBX<T>(string gbxFile) where T : Node
        {
            using var fs = File.OpenRead(gbxFile);

            var type = GameBox.GetGameBoxType(fs);
            fs.Seek(0, SeekOrigin.Begin);

            GameBox gbx;
            if (type == null)
                gbx = new GameBox();
            else
                gbx = (GameBox)Activator.CreateInstance(type);

            if (gbx.Read(fs))
                return FromGBX((GameBox<T>)gbx);
            return null;
        }

        static Node()
        {
            Names = new Dictionary<uint, string>();
            Mappings = new Dictionary<uint, uint>();

            var startTimestamp = DateTime.Now;

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
                        if (line.Length - 6 > 0) className = line[6..];

                        var classIDString = $"{en}{cl}{ch}";

                        if (uint.TryParse(classIDString, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out uint classID))
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
                        if (line.Length - 3 > 0) engineName = line[3..];
                    }
                }
            }

            Debug.WriteLine("Classes named in " + (DateTime.Now - startTimestamp).TotalMilliseconds + "ms");

            startTimestamp = DateTime.Now;

            using (StringReader reader = new StringReader(Resources.ClassIDMappings))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var valueKey = line.Split(new string[] { " -> " }, StringSplitOptions.None);
                    if (valueKey.Length == 2)
                    {
                        if (uint.TryParse(valueKey[0], System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out uint key)
                        && uint.TryParse(valueKey[1], System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out uint value))
                        {
                            if (Mappings.ContainsValue(key)) // Virtual Skipper solution
                                Mappings[Mappings.FirstOrDefault(x => x.Value == key).Key] = value;
                            Mappings[key] = value;
                        }
                    }
                }
            }

            Debug.WriteLine("Mappings defined in " + (DateTime.Now - startTimestamp).TotalMilliseconds + "ms");
        }

        public object GetValue<T>(Func<T, object> val) where T : Chunk
        {
            foreach (var chunk in Chunks.Values)
            {
                if (chunk is T t)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    return val.Invoke(t);
                }
            }
            return default;
        }

        public object GetValue<T1, T2>(Func<T1, object> val1, Func<T2, object> val2, DifferenceSolution diffSolution = DifferenceSolution.Default) where T1 : Chunk where T2 : Chunk
        {
            HashSet<object> values = new HashSet<object>();

            void AddIfNotNull<T>(Chunk chunk, Func<T, object> val)
            {
                if (chunk is T t)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    var value = val.Invoke(t);
                    if (value != null) values.Add(value);
                }
            }

            switch (diffSolution)
            {
                case DifferenceSolution.ExceptionIfDifferent:
                    foreach (var chunk in Chunks.Values)
                    {
                        AddIfNotNull(chunk, val1);
                        AddIfNotNull(chunk, val2);
                    }

                    if (values.Count > 1)
                    {
                        if (values.Count == 1)
                            return values;
                        else
                            throw new Exception("Some values are different.");
                    }
                    else if (values.Count > 0)
                        return values.First();

                    return default;
                case DifferenceSolution.FirstChunk:
                    foreach (var chunk in Chunks.Values)
                    {
                        if (chunk is T1 t1)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val1.Invoke(t1);
                        }

                        if (chunk is T2 t2)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val2.Invoke(t2);
                        }
                    }
                    return default;
                case DifferenceSolution.Average:
                    foreach (var chunk in Chunks.Values)
                    {
                        if (chunk is T1 t1)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val1.Invoke(t1));
                        }

                        if (chunk is T2 t2)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val2.Invoke(t2));
                        }
                    }

                    if (values.Count > 0)
                    {
                        if (values.First() is int i) return values.Average(x => i);
                        else if (values.First() is uint ui) return values.Average(x => ui);
                        else if (values.First() is short s) return values.Average(x => s);
                        else if (values.First() is ushort us) return values.Average(x => us);
                        else if (values.First() is long l) return values.Average(x => l);
                        else if (values.First() is float f) return values.Average(x => f);
                        else throw new Exception("Cannot average this type.");
                    }

                    return default;
                default:
                    return default;
            }
        }

        public object GetValue<T1, T2, T3>(Func<T1, object> val1, Func<T2, object> val2, Func<T3, object> val3, DifferenceSolution diffSolution = DifferenceSolution.Default) where T1 : Chunk where T2 : Chunk where T3 : Chunk
        {
            HashSet<object> values = new HashSet<object>();

            void AddIfNotNull<T>(Chunk chunk, Func<T, object> val)
            {
                if (chunk is T t)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    var value = val.Invoke(t);
                    if (value != null) values.Add(value);
                }
            }

            switch (diffSolution)
            {
                case DifferenceSolution.ExceptionIfDifferent:
                    foreach (var chunk in Chunks.Values)
                    {
                        AddIfNotNull(chunk, val1);
                        AddIfNotNull(chunk, val2);
                        AddIfNotNull(chunk, val3);
                    }

                    if (values.Count > 1)
                    {
                        if (values.Distinct().Skip(1).Any())
                            return values;
                        else
                            throw new Exception("Some values are different.");
                    }
                    else if (values.Count > 0)
                        return values.First();

                    return default;
                case DifferenceSolution.FirstChunk:
                    foreach (var chunk in Chunks.Values)
                    {
                        if (chunk is T1 t1)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val1.Invoke(t1);
                        }
                        if (chunk is T2 t2)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val2.Invoke(t2);
                        }
                        if (chunk is T3 t3)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val3.Invoke(t3);
                        }
                    }
                    return default;
                case DifferenceSolution.Average:
                    foreach (var chunk in Chunks.Values)
                    {
                        if (chunk is T1 t1)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val1.Invoke(t1));
                        }
                        if (chunk is T2 t2)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val2.Invoke(t2));
                        }
                        if (chunk is T3 t3)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val3.Invoke(t3));
                        }
                    }

                    if (values.Count > 0)
                    {
                        if (values.First() is int i) return values.Average(x => i);
                        else if (values.First() is uint ui) return values.Average(x => ui);
                        else if (values.First() is short s) return values.Average(x => s);
                        else if (values.First() is ushort us) return values.Average(x => us);
                        else if (values.First() is long l) return values.Average(x => l);
                        else if (values.First() is float f) return values.Average(x => f);
                        else throw new Exception("Cannot average this type.");
                    }

                    return default;
                default:
                    return default;
            }
        }

        public object GetValue<T1, T2, T3, T4>(Func<T1, object> val1, Func<T2, object> val2, Func<T3, object> val3, Func<T4, object> val4, DifferenceSolution diffSolution = DifferenceSolution.Default) where T1 : Chunk where T2 : Chunk where T3 : Chunk where T4 : Chunk
        {
            HashSet<object> values = new HashSet<object>();

            void AddIfNotNull<T>(Chunk chunk, Func<T, object> val)
            {
                if (chunk is T t)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    var value = val.Invoke(t);
                    if (value != null) values.Add(value);
                }
            }

            switch (diffSolution)
            {
                case DifferenceSolution.ExceptionIfDifferent:
                    foreach (var chunk in Chunks.Values)
                    {
                        AddIfNotNull(chunk, val1);
                        AddIfNotNull(chunk, val2);
                        AddIfNotNull(chunk, val3);
                        AddIfNotNull(chunk, val4);
                    }

                    if (values.Count > 1)
                    {
                        if (values.Where(x => x != null).All(x => x.Equals(values.FirstOrDefault())))
                            return values.Where(x => x != null).FirstOrDefault();
                        else
                            throw new Exception("Some values are different.");
                    }
                    else if (values.Count > 0)
                        return values.First();

                    return default;
                case DifferenceSolution.FirstChunk:
                    foreach (var chunk in Chunks.Values)
                    {
                        if (chunk is T1 t1)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val1.Invoke(t1);
                        }
                        if (chunk is T2 t2)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val2.Invoke(t2);
                        }
                        if (chunk is T3 t3)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val3.Invoke(t3);
                        }
                        if (chunk is T4 t4)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val4.Invoke(t4);
                        }
                    }
                    return default;
                case DifferenceSolution.Average:
                    foreach (var chunk in Chunks.Values)
                    {
                        if (chunk is T1 t1)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val1.Invoke(t1));
                        }
                        if (chunk is T2 t2)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val2.Invoke(t2));
                        }
                        if (chunk is T3 t3)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val3.Invoke(t3));
                        }
                        if (chunk is T4 t4)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val4.Invoke(t4));
                        }
                    }

                    if (values.Count > 0)
                    {
                        if (values.First() is int i) return values.Average(x => i);
                        else if (values.First() is uint ui) return values.Average(x => ui);
                        else if (values.First() is short s) return values.Average(x => s);
                        else if (values.First() is ushort us) return values.Average(x => us);
                        else if (values.First() is long l) return values.Average(x => l);
                        else if (values.First() is float f) return values.Average(x => f);
                        else throw new Exception("Cannot average this type.");
                    }

                    return default;
                default:
                    return default;
            }
        }

        public object GetValue<T1, T2, T3, T4, T5>(Func<T1, object> val1, Func<T2, object> val2, Func<T3, object> val3, Func<T4, object> val4, Func<T5, object> val5, DifferenceSolution diffSolution = DifferenceSolution.Default) where T1 : Chunk where T2 : Chunk where T3 : Chunk where T4 : Chunk where T5 : Chunk
        {
            HashSet<object> values = new HashSet<object>();

            void AddIfNotNull<T>(Chunk chunk, Func<T, object> val)
            {
                if (chunk is T t)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    var value = val.Invoke(t);
                    if (value != null) values.Add(value);
                }
            }

            switch (diffSolution)
            {
                case DifferenceSolution.ExceptionIfDifferent:
                    foreach (var chunk in Chunks.Values)
                    {
                        AddIfNotNull(chunk, val1);
                        AddIfNotNull(chunk, val2);
                        AddIfNotNull(chunk, val3);
                        AddIfNotNull(chunk, val4);
                        AddIfNotNull(chunk, val5);
                    }

                    if (values.Count > 1)
                    {
                        if (values.Where(x => x != null).All(x => x.Equals(values.FirstOrDefault())))
                            return values.Where(x => x != null).FirstOrDefault();
                        else
                            throw new Exception("Some values are different.");
                    }
                    else if (values.Count > 0)
                        return values.First();

                    return default;
                case DifferenceSolution.FirstChunk:
                    foreach (var chunk in Chunks.Values)
                    {
                        if (chunk is T1 t1)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val1.Invoke(t1);
                        }
                        if (chunk is T2 t2)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val2.Invoke(t2);
                        }
                        if (chunk is T3 t3)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val3.Invoke(t3);
                        }
                        if (chunk is T4 t4)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val4.Invoke(t4);
                        }
                        if (chunk is T5 t5)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val5.Invoke(t5);
                        }
                    }
                    return default;
                case DifferenceSolution.Average:
                    foreach (var chunk in Chunks.Values)
                    {
                        if (chunk is T1 t1)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val1.Invoke(t1));
                        }
                        if (chunk is T2 t2)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val2.Invoke(t2));
                        }
                        if (chunk is T3 t3)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val3.Invoke(t3));
                        }
                        if (chunk is T4 t4)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val4.Invoke(t4));
                        }
                        if (chunk is T5 t5)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val5.Invoke(t5));
                        }
                    }

                    if (values.Count > 0)
                    {
                        if (values.First() is int i) return values.Average(x => i);
                        else if (values.First() is uint ui) return values.Average(x => ui);
                        else if (values.First() is short s) return values.Average(x => s);
                        else if (values.First() is ushort us) return values.Average(x => us);
                        else if (values.First() is long l) return values.Average(x => l);
                        else if (values.First() is float f) return values.Average(x => f);
                        else throw new Exception("Cannot average this type.");
                    }

                    return default;
                default:
                    return default;
            }
        }

        public object GetValue<T1, T2, T3, T4, T5, T6>(Func<T1, object> val1, Func<T2, object> val2, Func<T3, object> val3, Func<T4, object> val4, Func<T5, object> val5, Func<T6, object> val6, DifferenceSolution diffSolution = DifferenceSolution.Default) where T1 : Chunk where T2 : Chunk where T3 : Chunk where T4 : Chunk where T5 : Chunk where T6 : Chunk
        {
            HashSet<object> values = new HashSet<object>();

            void AddIfNotNull<T>(Chunk chunk, Func<T, object> val)
            {
                if (chunk is T t)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    var value = val.Invoke(t);
                    if (value != null) values.Add(value);
                }
            }

            switch (diffSolution)
            {
                case DifferenceSolution.ExceptionIfDifferent:
                    foreach (var chunk in Chunks.Values)
                    {
                        AddIfNotNull(chunk, val1);
                        AddIfNotNull(chunk, val2);
                        AddIfNotNull(chunk, val3);
                        AddIfNotNull(chunk, val4);
                        AddIfNotNull(chunk, val5);
                        AddIfNotNull(chunk, val6);
                    }

                    if (values.Count > 1)
                    {
                        if (values.Where(x => x != null).All(x => x.Equals(values.FirstOrDefault())))
                            return values.Where(x => x != null).FirstOrDefault();
                        else
                            throw new Exception("Some values are different.");
                    }
                    else if (values.Count > 0)
                        return values.First();

                    return default;
                case DifferenceSolution.FirstChunk:
                    foreach (var chunk in Chunks.Values)
                    {
                        if (chunk is T1 t1)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val1.Invoke(t1);
                        }
                        if (chunk is T2 t2)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val2.Invoke(t2);
                        }
                        if (chunk is T3 t3)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val3.Invoke(t3);
                        }
                        if (chunk is T4 t4)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val4.Invoke(t4);
                        }
                        if (chunk is T5 t5)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val5.Invoke(t5);
                        }
                        if (chunk is T6 t6)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            return val6.Invoke(t6);
                        }
                    }
                    return default;
                case DifferenceSolution.Average:
                    foreach (var chunk in Chunks.Values)
                    {
                        if (chunk is T1 t1)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val1.Invoke(t1));
                        }
                        if (chunk is T2 t2)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val2.Invoke(t2));
                        }
                        if (chunk is T3 t3)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val3.Invoke(t3));
                        }
                        if (chunk is T4 t4)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val4.Invoke(t4));
                        }
                        if (chunk is T5 t5)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val5.Invoke(t5));
                        }
                        if (chunk is T6 t6)
                        {
                            if (chunk is SkippableChunk s) s.Discover();
                            values.Add(val6.Invoke(t6));
                        }
                    }

                    if (values.Count > 0)
                    {
                        if (values.First() is int i) return values.Average(x => i);
                        else if (values.First() is uint ui) return values.Average(x => ui);
                        else if (values.First() is short s) return values.Average(x => s);
                        else if (values.First() is ushort us) return values.Average(x => us);
                        else if (values.First() is long l) return values.Average(x => l);
                        else if (values.First() is float f) return values.Average(x => f);
                        else throw new Exception("Cannot average this type.");
                    }

                    return default;
                default:
                    return default;
            }
        }

        public void SetValue<T1>(Action<T1> val) where T1 : Chunk
        {
            foreach (var chunk in Chunks.Values)
            {
                if (chunk is T1 t)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val.Invoke(t);
                }
            }
        }

        public void SetValue<T1, T2>(Action<T1> val1, Action<T2> val2) where T1 : Chunk where T2 : Chunk
        {
            foreach (var chunk in Chunks.Values)
            {
                if (chunk is T1 t1)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val1.Invoke(t1);
                }
                if (chunk is T2 t2)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val2.Invoke(t2);
                }
            }
        }

        public void SetValue<T1, T2, T3>(Action<T1> val1, Action<T2> val2, Action<T3> val3) where T1 : Chunk where T2 : Chunk where T3 : Chunk
        {
            foreach (var chunk in Chunks.Values)
            {
                if (chunk is T1 t1)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val1.Invoke(t1);
                }
                if (chunk is T2 t2)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val2.Invoke(t2);
                }
                if (chunk is T3 t3)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val3.Invoke(t3);
                }
            }
        }

        public void SetValue<T1, T2, T3, T4>(Action<T1> val1, Action<T2> val2, Action<T3> val3, Action<T4> val4) where T1 : Chunk where T2 : Chunk where T3 : Chunk where T4 : Chunk
        {
            foreach (var chunk in Chunks.Values)
            {
                if (chunk is T1 t1)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val1.Invoke(t1);
                }
                if (chunk is T2 t2)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val2.Invoke(t2);
                }
                if (chunk is T3 t3)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val3.Invoke(t3);
                }
                if (chunk is T4 t4)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val4.Invoke(t4);
                }
            }
        }

        public void SetValue<T1, T2, T3, T4, T5>(Action<T1> val1, Action<T2> val2, Action<T3> val3, Action<T4> val4, Action<T5> val5) where T1 : Chunk where T2 : Chunk where T3 : Chunk where T4 : Chunk where T5 : Chunk
        {
            foreach (var chunk in Chunks.Values)
            {
                if (chunk is T1 t1)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val1.Invoke(t1);
                }
                if (chunk is T2 t2)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val2.Invoke(t2);
                }
                if (chunk is T3 t3)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val3.Invoke(t3);
                }
                if (chunk is T4 t4)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val4.Invoke(t4);
                }
                if (chunk is T5 t5)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val5.Invoke(t5);
                }
            }
        }

        public void SetValue<T1, T2, T3, T4, T5, T6>(Action<T1> val1, Action<T2> val2, Action<T3> val3, Action<T4> val4, Action<T5> val5, Action<T6> val6) where T1 : Chunk where T2 : Chunk where T3 : Chunk where T4 : Chunk where T5 : Chunk where T6 : Chunk
        {
            foreach (var chunk in Chunks.Values)
            {
                if (chunk is T1 t1)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val1.Invoke(t1);
                }
                if (chunk is T2 t2)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val2.Invoke(t2);
                }
                if (chunk is T3 t3)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val3.Invoke(t3);
                }
                if (chunk is T4 t4)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val4.Invoke(t4);
                }
                if (chunk is T5 t5)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val5.Invoke(t5);
                }
                if (chunk is T6 t6)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    val6.Invoke(t6);
                }
            }
        }

        public T GetChunk<T>() where T : Chunk
        {
            foreach (var chunk in Chunks.Values)
            {
                if (chunk is T t)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    return t;
                }
            }
            return default;
        }

        public bool TryGetChunk<T>(out T chunk) where T : Chunk
        {
            chunk = GetChunk<T>();
            return chunk != default;
        }

        public void CallChunkMethod<T>(Action<T> method) where T : Chunk
        {
            foreach (var chunk in Chunks.Values)
            {
                if (chunk is T t)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    method.Invoke(t);
                }
            }
        }

        public T2 CallChunkMethod<T1, T2>(Func<T1, T2> method) where T1 : Chunk
        {
            foreach (var chunk in Chunks.Values)
            {
                if (chunk is T1 t)
                {
                    if (chunk is SkippableChunk s) s.Discover();
                    return method.Invoke(t);
                }
            }
            return default;
        }

        public static uint Remap(uint id)
        {
            if (Mappings.TryGetValue(id, out uint newerClassID))
                return newerClassID;
            return id;
        }
    }
}
