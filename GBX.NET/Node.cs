using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace GBX.NET
{
    public class Node
    {
        public static Dictionary<uint, string> Names { get; }
        public static Dictionary<uint, uint> Mappings { get; } // key: older, value: newer

        public GameBoxBody Body => Lookbackable as GameBoxBody;
        public GameBoxHeader Header => Lookbackable as GameBoxHeader;
        public GameBox GBX => Body?.GBX ?? Header.GBX;

        public AuxNodeChunkList Chunks { get; internal set; }

        public uint ID { get; }
        public ILookbackable Lookbackable { get; internal set; }
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

        public static Dictionary<uint, Type> AvailableClasses { get; } = new Dictionary<uint, Type>();
        public static Dictionary<Type, Dictionary<uint, Type>> AvailableChunkClasses { get; } = new Dictionary<Type, Dictionary<uint, Type>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookbackable">Usually <see cref="GameBoxHeader{T}"/> or <see cref="GameBoxBody{T}"/></param>
        /// <param name="classID"></param>
        public Node(ILookbackable lookbackable, uint classID)
        {
            Lookbackable = lookbackable;
            ID = classID;
        }

        public Node(ILookbackable lookbackable) 
        {
            Lookbackable = lookbackable;
            ID = GetType().GetCustomAttribute<NodeAttribute>().ID;
        }

        static Type GetBaseType(Type t)
        {
            if (t == null)
                return null;
            if (t.BaseType == typeof(Node))
                return t.BaseType;
            return GetBaseType(t.BaseType);
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
                    array[i] = Parse(type, body, r, out inheritanceClasses, out availableChunkClasses, true);
                else
                    array[i] = Parse(type, body, r, inheritanceClasses, availableChunkClasses, true);
            }

            return array;
        }

        public static T[] ParseArray<T>(ILookbackable body, GameBoxReader r) where T : Node
        {
            return ParseArray(typeof(T), body, r).Cast<T>().ToArray();
        }

        public static Node Parse(ILookbackable body, uint classID, GameBoxReader r, bool isAux = false)
        {
            var hasNewerID = Mappings.TryGetValue(classID, out uint newerClassID);
            if (!hasNewerID) newerClassID = classID;

            if (!AvailableClasses.TryGetValue(newerClassID, out Type availableClass))
            {
                availableClass = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass
                   && x.Namespace.StartsWith("GBX.NET.Engines") && GetBaseType(x) == typeof(Node)
                   && (x.GetCustomAttribute<NodeAttribute>().ID == newerClassID)).FirstOrDefault();
                AvailableClasses.Add(newerClassID, availableClass);
            }

            var inheritanceClasses = new List<uint>();

            if (availableClass == null)
                throw new Exception("Unknown node: 0x" + classID.ToString("x8"));

            return Parse(availableClass, body, r, isAux);
        }

        private static Node Parse(Type type, ILookbackable body, GameBoxReader r, out List<uint> inheritanceClasses, out Dictionary<uint, Type> availableChunkClasses, bool isAux = false)
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

            var chunkType = typeof(Chunk<>).MakeGenericType(type);
            var skippableChunkType = typeof(SkippableChunk<>).MakeGenericType(type);

            if (!AvailableChunkClasses.TryGetValue(type, out availableChunkClasses))
            {
                availableChunkClasses = type.GetNestedTypes().Where(x =>
                {
                    var isChunk = x.IsClass
                    && x.Namespace.StartsWith("GBX.NET.Engines")
                    && (x.BaseType == chunkType || x.BaseType == skippableChunkType || x.BaseType == typeof(Chunk) || x.BaseType == typeof(SkippableChunk));
                    if (!isChunk) return false;

                    var chunkAttribute = x.GetCustomAttribute<ChunkAttribute>();
                    if (chunkAttribute == null) throw new Exception($"Chunk {x.FullName} doesn't have a ChunkAttribute.");

                    var attributesMet = chunkAttribute.ClassID == type.GetCustomAttribute<NodeAttribute>().ID;
                    return isChunk && attributesMet;
                }).ToDictionary(x => x.GetCustomAttribute<ChunkAttribute>().ChunkID);

                foreach (var cls in inheritanceClasses)
                {
                    var availableInheritanceClass = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass
                       && x.Namespace.StartsWith("GBX.NET.Engines") && (GetBaseType(x) == typeof(Node))
                       && (x.GetCustomAttribute<NodeAttribute>().ID == cls)).FirstOrDefault();

                    var inheritChunkType = typeof(Chunk<>).MakeGenericType(availableInheritanceClass);
                    var inheritSkippableChunkType = typeof(SkippableChunk<>).MakeGenericType(availableInheritanceClass);

                    foreach (var chunkT in availableInheritanceClass.GetNestedTypes().Where(x => x.IsClass
                        && x.Namespace.StartsWith("GBX.NET.Engines") && (x.BaseType == inheritChunkType || x.BaseType == inheritSkippableChunkType || x.BaseType == typeof(Chunk) || x.BaseType == typeof(SkippableChunk))
                        && (x.GetCustomAttribute<ChunkAttribute>().ClassID == cls)).ToDictionary(x => x.GetCustomAttribute<ChunkAttribute>().ChunkID))
                    {
                        availableChunkClasses[chunkT.Key + cls] = chunkT.Value;
                    }
                }
                AvailableChunkClasses.Add(type, availableChunkClasses);
            }

            return Parse(type, body, r, inheritanceClasses, availableChunkClasses, isAux);
        }

        public static Node Parse(Type type, ILookbackable body, GameBoxReader r, bool isAux = false)
        {
            return Parse(type, body, r, out List<uint> _, out Dictionary<uint, Type> _, isAux);
        }

        public static T Parse<T>(ILookbackable body, GameBoxReader r, bool isAux = false) where T : Node
        {
            return (T)Parse(typeof(T), body, r, isAux);
        }

        private static Node Parse(Type type, ILookbackable body, GameBoxReader r, List<uint> inheritanceClasses, Dictionary<uint, Type> availableChunkClasses, bool isAux = false)
        {
            var readNodeStart = DateTime.Now;

            Node node = (Node)Activator.CreateInstance(type, body, type.GetCustomAttribute<NodeAttribute>().ID);

            var chunks = new AuxNodeChunkList();

            uint? previousChunk = null;

            while (true)
            {
                var chunkID = r.ReadUInt32();

                if (chunkID == 0xFACADE01) // no more chunks
                {
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

                var skippable = reflected && (chunkClass.BaseType == typeof(Chunk) || chunkClass.BaseType == typeof(SkippableChunk)
                    || chunkClass.BaseType.GetGenericTypeDefinition() == typeof(SkippableChunk<>));

                if (!reflected || skippable)
                {
                    var skip = r.ReadUInt32();

                    if (skip != 0x534B4950)
                    {
                        if (chunkID != 0 && !reflected)
                        {
                            Debug.WriteLine($"Wrong chunk format or unskippable chunk: {chunkID:x8} ({Names.Where(x => x.Key == (chunkID&0xFFFFF000)).Select(x => x.Value).FirstOrDefault() ?? "unknown class"})"); // Read till facade
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
                        dynamic c;

                        var constructor = chunkClass.GetConstructors().First();
                        var constructorParams = constructor.GetParameters();
                        if (constructorParams.Length == 0)
                        {
                            c = constructor.Invoke(new object[0]);
                            if (chunkClass.BaseType != typeof(SkippableChunk))
                            {
                                c.Node = (dynamic)node;
                                c.Stream = new MemoryStream(chunkData, 0, chunkData.Length, false);
                                if (chunkData == null || chunkData.Length == 0)
                                    c.Discovered = true;
                            }
                        }
                        else if (constructorParams.Length == 2)
                            c = constructor.Invoke(new object[] { node, chunkData });
                        else throw new ArgumentException($"{type.FullName} has an invalid amount of parameters.");

                        chunks.Add(c.ID, c);

                        if (!chunkClass.GetCustomAttribute<ChunkAttribute>().ProcessAsync)
                            c.Discover();
                    }
                    else
                    {
                        Debug.WriteLine("Unknown skippable chunk: " + chunkID.ToString("x"));
                        chunks.Add(chunkID, (Chunk)Activator.CreateInstance(typeof(SkippableChunk<>).MakeGenericType(type), node, chunkID, chunkData));
                    }
                }

                if (reflected && !skippable)
                {
                    Debug.WriteLine("Unskippable chunk: " + chunkID.ToString("x8"));

                    if (skippable) // Does it ever happen?
                    {
                        var skip = r.ReadUInt32();
                        var chunkDataSize = r.ReadInt32();
                    }

                    dynamic chunk;

                    var constructor = chunkClass.GetConstructors().First();
                    var constructorParams = constructor.GetParameters();
                    if (constructorParams.Length == 0)
                    {
                        chunk = constructor.Invoke(new object[0]);
                        chunk.Node = (dynamic)node;
                    }
                    else if (constructorParams.Length == 1)
                        chunk = constructor.Invoke(new object[] { node });
                    else throw new ArgumentException($"{type.FullName} has an invalid amount of parameters.");
                    chunks.Add(chunk.ID, chunk);

                    var posBefore = r.BaseStream.Position;

                    GameBoxReaderWriter gbxrw = new GameBoxReaderWriter(r);
                    chunk.ReadWrite((dynamic)node, gbxrw);

                    chunk.Progress = (int)(r.BaseStream.Position - posBefore);
                }

                previousChunk = chunkID;
            }

            if (node.Body != null && node.Body.GBX.ClassID.HasValue && Remap(node.Body.GBX.ClassID.Value) == node.ID)
                Log.Write($"[{node.ClassName}] DONE! ({(DateTime.Now - readNodeStart).TotalMilliseconds}ms)", ConsoleColor.Green);
            else
                Log.Write($"~ [{node.ClassName}] DONE! ({(DateTime.Now - readNodeStart).TotalMilliseconds}ms)", ConsoleColor.Green);

            if (isAux)
                node.Chunks = chunks;
            else
                ((dynamic)body).Chunks.FromAuxNodeChunkList(chunks);

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

            if (Chunks != null)
            {
                foreach (dynamic chunk in Chunks.Values)
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
                        if (chunk is ISkippableChunk s && !s.Discovered)
                            s.Write(msW);
                        else
                            chunk.ReadWrite(this, rw);

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

        [Obsolete]
        public object GetValue<T>(Func<T, object> val) where T : Chunk
        {
            return default;
        }

        [Obsolete]
        public object GetValue<T1, T2>(Func<T1, object> val1, Func<T2, object> val2, DifferenceSolution diffSolution = DifferenceSolution.Default) where T1 : Chunk where T2 : Chunk
        {
            return default;
        }

        [Obsolete]
        public object GetValue<T1, T2, T3>(Func<T1, object> val1, Func<T2, object> val2, Func<T3, object> val3, DifferenceSolution diffSolution = DifferenceSolution.Default) where T1 : Chunk where T2 : Chunk where T3 : Chunk
        {
            return default;
        }

        [Obsolete]
        public object GetValue<T1, T2, T3, T4>(Func<T1, object> val1, Func<T2, object> val2, Func<T3, object> val3, Func<T4, object> val4, DifferenceSolution diffSolution = DifferenceSolution.Default) where T1 : Chunk where T2 : Chunk where T3 : Chunk where T4 : Chunk
        {
            return default;
        }

        [Obsolete]
        public object GetValue<T1, T2, T3, T4, T5>(Func<T1, object> val1, Func<T2, object> val2, Func<T3, object> val3, Func<T4, object> val4, Func<T5, object> val5, DifferenceSolution diffSolution = DifferenceSolution.Default) where T1 : Chunk where T2 : Chunk where T3 : Chunk where T4 : Chunk where T5 : Chunk
        {
            return default;
        }

        [Obsolete]
        public object GetValue<T1, T2, T3, T4, T5, T6>(Func<T1, object> val1, Func<T2, object> val2, Func<T3, object> val3, Func<T4, object> val4, Func<T5, object> val5, Func<T6, object> val6, DifferenceSolution diffSolution = DifferenceSolution.Default) where T1 : Chunk where T2 : Chunk where T3 : Chunk where T4 : Chunk where T5 : Chunk where T6 : Chunk
        {
            return default;
        }

        [Obsolete]
        public void SetValue<T1>(Action<T1> val) where T1 : Chunk
        {
            
        }

        [Obsolete]
        public void SetValue<T1, T2>(Action<T1> val1, Action<T2> val2) where T1 : Chunk where T2 : Chunk
        {
            
        }

        [Obsolete]
        public void SetValue<T1, T2, T3>(Action<T1> val1, Action<T2> val2, Action<T3> val3) where T1 : Chunk where T2 : Chunk where T3 : Chunk
        {
            
        }

        [Obsolete]
        public void SetValue<T1, T2, T3, T4>(Action<T1> val1, Action<T2> val2, Action<T3> val3, Action<T4> val4) where T1 : Chunk where T2 : Chunk where T3 : Chunk where T4 : Chunk
        {
            
        }

        [Obsolete]
        public void SetValue<T1, T2, T3, T4, T5>(Action<T1> val1, Action<T2> val2, Action<T3> val3, Action<T4> val4, Action<T5> val5) where T1 : Chunk where T2 : Chunk where T3 : Chunk where T4 : Chunk where T5 : Chunk
        {
            
        }

        [Obsolete]
        public void SetValue<T1, T2, T3, T4, T5, T6>(Action<T1> val1, Action<T2> val2, Action<T3> val3, Action<T4> val4, Action<T5> val5, Action<T6> val6) where T1 : Chunk where T2 : Chunk where T3 : Chunk where T4 : Chunk where T5 : Chunk where T6 : Chunk
        {
            
        }

        public T GetChunk<T>() where T : Chunk
        {
            if (Chunks != null)
                return Chunks.Get<T>();
            else
                return Body.GetChunk<T>();
        }

        public T CreateChunk<T>() where T : Chunk
        {
            if (Chunks != null)
                return Chunks.Create<T>();
            else
                return Body.CreateChunk<T>();
        }

        public bool RemoveChunk<T>() where T : Chunk
        {
            if (Chunks != null)
                return Chunks.Remove<T>();
            else
                return Body.RemoveChunk<T>();
        }

        public bool TryGetChunk<T>(out T chunk) where T : Chunk
        {
            if (Chunks != null)
                return Chunks.TryGet(out chunk);
            else
            {
                chunk = Body.GetChunk<T>();
                return chunk != default;
            }
        }

        public void DiscoverChunk<T>() where T : ISkippableChunk
        {
            if (Chunks != null)
                Chunks.Discover<T>();
            else if (typeof(T).GetInterface("IHeaderChunk") != null)
                ((dynamic)GBX).Header.Result.DiscoverChunk<T>();
            else
                Body.DiscoverChunk<T>();
        }

        public void DiscoverChunks<T1, T2>() where T1 : ISkippableChunk where T2 : ISkippableChunk
        {
            if (Chunks != null)
                Chunks.Discover<T1, T2>();
            else if (typeof(T1).GetInterface("IHeaderChunk") != null
                  && typeof(T2).GetInterface("IHeaderChunk") != null)
                ((dynamic)GBX).Header.Result.DiscoverChunks<T1, T2>();
            else
                Body.DiscoverChunks<T1, T2>();
        }

        public void DiscoverChunks<T1, T2, T3>() where T1 : ISkippableChunk where T2 : ISkippableChunk where T3 : ISkippableChunk
        {
            if (Chunks != null)
                Chunks.Discover<T1, T2, T3>();
            else
                Body.DiscoverChunks<T1, T2, T3>();
        }

        public void DiscoverChunks<T1, T2, T3, T4>()
            where T1 : ISkippableChunk
            where T2 : ISkippableChunk
            where T3 : ISkippableChunk
            where T4 : ISkippableChunk
        {
            if (Chunks != null)
                Chunks.Discover<T1, T2, T3, T4>();
            else
                Body.DiscoverChunks<T1, T2, T3, T4>();
        }

        public void DiscoverChunks<T1, T2, T3, T4, T5>()
            where T1 : ISkippableChunk
            where T2 : ISkippableChunk
            where T3 : ISkippableChunk
            where T4 : ISkippableChunk
            where T5 : ISkippableChunk
        {
            if (Chunks != null)
                Chunks.Discover<T1, T2, T3, T4, T5>();
            else
                Body.DiscoverChunks<T1, T2, T3, T4, T5>();
        }

        public void DiscoverChunks<T1, T2, T3, T4, T5, T6>()
            where T1 : ISkippableChunk
            where T2 : ISkippableChunk
            where T3 : ISkippableChunk
            where T4 : ISkippableChunk
            where T5 : ISkippableChunk
            where T6 : ISkippableChunk
        {
            if (Chunks != null)
                Chunks.Discover<T1, T2, T3, T4, T5, T6>();
            else
                Body.DiscoverChunks<T1, T2, T3, T4, T5, T6>();
        }

        [Obsolete]
        public void CallChunkMethod<T>(Action<T> method) where T : Chunk
        {
            
        }

        [Obsolete]
        public T2 CallChunkMethod<T1, T2>(Func<T1, T2> method) where T1 : Chunk
        {
            return default;
        }

        public static uint Remap(uint id)
        {
            if (Mappings.TryGetValue(id, out uint newerClassID))
                return newerClassID;
            return id;
        }

        public static T FromGBX<T>(GameBox<T> loadedGbx) where T : Node
        {
            return loadedGbx.MainNode;
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
            return default;
        }
    }
}
