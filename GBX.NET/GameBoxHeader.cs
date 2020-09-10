using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GBX.NET
{
    public class GameBoxHeader<T> : GameBoxHeader, ILookbackable where T : Node
    {
        int? ILookbackable.LookbackVersion { get; set; }
        List<string> ILookbackable.LookbackStrings { get; set; } = new List<string>();
        bool ILookbackable.LookbackWritten { get; set; }

        public new GameBox<T> GBX => (GameBox<T>)base.GBX;

        public ChunkList Chunks { get; set; }

        public GameBoxHeader(GameBox<T> gbx, GameBoxHeaderParameters parameters) : base(gbx, parameters)
        {
            if (parameters.Version >= 3)
            {
                var classID = parameters.ClassID.GetValueOrDefault();

                var modernID = classID;
                if (Node.Mappings.TryGetValue(classID, out uint newerClassID))
                    modernID = newerClassID;

                var availableClass = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass
                    && x.Namespace.StartsWith("GBX.NET.Engines") && GetBaseType(x) == typeof(Node)
                    && x.GetCustomAttribute<NodeAttribute>().ID == modernID).FirstOrDefault();

                if (parameters.Version == 6)
                {
                    if (parameters.UserData != null && parameters.UserData.Length > 0)
                    {
                        var headerChunkBaseType = typeof(HeaderChunk<>).MakeGenericType(availableClass);

                        var availableChunkClasses = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass
                        && x.Namespace.StartsWith("GBX.NET.Engines") && x.BaseType == headerChunkBaseType
                        && x.GetCustomAttribute<ChunkAttribute>().ClassID == modernID).ToDictionary(x => x.GetCustomAttribute<ChunkAttribute>().ID);

                        var inheritanceClasses = new List<uint>();
                        if (GetBaseType(availableClass) == typeof(Node))
                            inheritanceClasses = GetInheritance(availableClass);

                        List<uint> GetInheritance(Type t)
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

                        foreach (var cls in inheritanceClasses)
                        {
                            var availableInheritanceClass = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass
                               && x.Namespace.StartsWith("GBX.NET.Engines") && (GetBaseType(x) == typeof(Node))
                               && (x.GetCustomAttribute<NodeAttribute>().ID == cls)).FirstOrDefault();

                            foreach (var chunkType in availableInheritanceClass.GetNestedTypes().Where(x => x.IsClass
                                && x.Namespace.StartsWith("GBX.NET.Engines") && x.BaseType == typeof(Chunk)
                                && (x.GetCustomAttribute<ChunkAttribute>().ClassID == cls)).ToDictionary(x => x.GetCustomAttribute<ChunkAttribute>().ChunkID))
                                availableChunkClasses[chunkType.Key + cls] = chunkType.Value;
                        }

                        using var ms = new MemoryStream(parameters.UserData);
                        using var r = new GameBoxReader(ms, this);

                        var numHeaderChunks = r.ReadInt32();
                        
                        var chunks = new HeaderChunk<T>[numHeaderChunks];

                        var chunkList = new Dictionary<uint, (int, bool)>();

                        for (var i = 0; i < numHeaderChunks; i++)
                        {
                            var chunkID = r.ReadUInt32();
                            var chunkSize = r.ReadUInt32();

                            var chId = chunkID & 0xFFF;
                            var clId = chunkID & 0xFFFFF000;

                            chunkList[clId + chId] = ((int)(chunkSize & ~0x80000000), (chunkSize & (1 << 31)) != 0);
                        }

                        Log.Write("Header data chunk list:");

                        foreach(var c in chunkList)
                        {
                            if(c.Value.Item2)
                                Log.Write($"| 0x{c.Key:x8} | {c.Value.Item1} B (Heavy)");
                            else
                                Log.Write($"| 0x{c.Key:x8} | {c.Value.Item1} B");
                        }

                        int counter = 0;
                        foreach (var chunk in chunkList)
                        {
                            var chunkId = chunk.Key;
                            if (Node.Mappings.TryGetValue(chunk.Key & 0xFFFFF000, out uint remapped))
                                chunkId = remapped + (chunkId & 0xFFF);

                            var d = r.ReadBytes(chunk.Value.Item1);

                            if (availableChunkClasses.TryGetValue(chunkId, out Type type))
                            {
                                var constructor = type.GetConstructors().First();
                                var constructorParams = constructor.GetParameters();
                                if (constructorParams.Length == 0)
                                {
                                    var headerChunk = (HeaderChunk<T>)constructor.Invoke(new object[0]);
                                    headerChunk.Node = GBX.MainNode;
                                    headerChunk.Stream = new MemoryStream(d, 0, d.Length, false);
                                    if (d == null || d.Length == 0)
                                        headerChunk.Discovered = true;
                                    chunks[counter] = headerChunk;
                                }
                                else if (constructorParams.Length == 2)
                                    chunks[counter] = (HeaderChunk<T>)constructor.Invoke(new object[] { GBX.MainNode, d });
                                else throw new ArgumentException($"{type.FullName} has an invalid amount of parameters.");

                                using (var msChunk = new MemoryStream(d))
                                using (var rChunk = new GameBoxReader(msChunk, this))
                                    chunks[counter].ReadWrite(null, new GameBoxReaderWriter(rChunk));

                                chunks[counter].IsHeavy = chunk.Value.Item2;
                            }
                            else
                                chunks[counter] = new HeaderChunk<T>(GBX.MainNode, chunkId, d);

                            counter++;
                        }
                        Chunks = new ChunkList(chunks);
                    }
                }
            }
        }

        Type GetBaseType(Type t)
        {
            if (t == null)
                return null;
            if (t.BaseType == typeof(Node))
                return t.BaseType;
            return GetBaseType(t.BaseType);
        }

        public void Write(GameBoxWriter w, int numNodes, ClassIDRemap remap)
        {
            w.Write("GBX", StringLengthPrefix.None);
            w.Write(Version);

            if (Version >= 3)
            {
                w.Write((byte)ByteFormat.GetValueOrDefault());
                w.Write((byte)RefTableCompression.GetValueOrDefault());
                w.Write((byte)BodyCompression.GetValueOrDefault());
                if (Version >= 4) w.Write((byte)UnknownByte.GetValueOrDefault());
                w.Write(ClassID.GetValueOrDefault());

                if (Version >= 6)
                {
                    using var userData = new MemoryStream();
                    using var gbxw = new GameBoxWriter(userData, this);
                    var gbxrw = new GameBoxReaderWriter(gbxw);

                    Dictionary<uint, int> lengths = new Dictionary<uint, int>();

                    foreach (var chunk in Chunks)
                    {
                        chunk.Unknown.Position = 0;

                        var pos = userData.Position;
                        if (!((ISkippableChunk)chunk).Discovered)
                            ((ISkippableChunk)chunk).Write(gbxw);
                        else
                            ((IHeaderChunk)chunk).ReadWrite(gbxrw);
                        lengths[chunk.ID] = (int)(userData.Position - pos);
                    }

                    // Actual data size plus the class id (4 bytes) and each length (4 bytes) plus the number of chunks integer
                    w.Write((int)userData.Length + Chunks.Count * 8 + 4);

                    // Write number of header chunks integer
                    w.Write(Chunks.Count);

                    foreach (Chunk chunk in Chunks)
                    {
                        w.Write(Chunk.Remap(chunk.ID, remap));
                        var length = lengths[chunk.ID];
                        if (((IHeaderChunk)chunk).IsHeavy)
                            length |= 1 << 31;
                        w.Write(length);
                    }

                    w.Write(userData.ToArray(), 0, (int)userData.Length);
                }

                w.Write(numNodes);
            }
        }

        public void Write(GameBoxWriter w, int numNodes)
        {
            Write(w, numNodes, ClassIDRemap.Latest);
        }

        public new TChunk CreateChunk<TChunk>(byte[] data) where TChunk : HeaderChunk<T>
        {
            return Chunks.Create<TChunk>(data);
        }

        public new TChunk CreateChunk<TChunk>() where TChunk : HeaderChunk<T>
        {
            return CreateChunk<TChunk>(new byte[0]);
        }

        public void InsertChunk(HeaderChunk<T> chunk)
        {
            Chunks.Add(chunk);
        }

        public new void DiscoverChunk<TChunk>() where TChunk : HeaderChunk<T>
        {
            foreach (var chunk in Chunks)
                if (chunk is TChunk c)
                    c.Discover();
        }

        public new void DiscoverChunks<TChunk1, TChunk2>() where TChunk1 : HeaderChunk<T> where TChunk2 : HeaderChunk<T>
        {
            foreach (var chunk in Chunks)
            {
                if (chunk is TChunk1 c1)
                    c1.Discover();
                if (chunk is TChunk2 c2)
                    c2.Discover();
            }
        }

        public new void DiscoverChunks<TChunk1, TChunk2, TChunk3>()
            where TChunk1 : HeaderChunk<T>
            where TChunk2 : HeaderChunk<T>
            where TChunk3 : HeaderChunk<T>
        {
            foreach (var chunk in Chunks)
            {
                if (chunk is TChunk1 c1)
                    c1.Discover();
                if (chunk is TChunk2 c2)
                    c2.Discover();
                if (chunk is TChunk3 c3)
                    c3.Discover();
            }
        }

        public new void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4>()
            where TChunk1 : HeaderChunk<T>
            where TChunk2 : HeaderChunk<T>
            where TChunk3 : HeaderChunk<T>
            where TChunk4 : HeaderChunk<T>
        {
            foreach (var chunk in Chunks)
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

        public new void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5>()
            where TChunk1 : HeaderChunk<T>
            where TChunk2 : HeaderChunk<T>
            where TChunk3 : HeaderChunk<T>
            where TChunk4 : HeaderChunk<T>
            where TChunk5 : HeaderChunk<T>
        {
            foreach (var chunk in Chunks)
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

        public new void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5, TChunk6>()
            where TChunk1 : HeaderChunk<T>
            where TChunk2 : HeaderChunk<T>
            where TChunk3 : HeaderChunk<T>
            where TChunk4 : HeaderChunk<T>
            where TChunk5 : HeaderChunk<T>
            where TChunk6 : HeaderChunk<T>
        {
            foreach (var chunk in Chunks)
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

        public new void DiscoverAllChunks()
        {
            foreach (var chunk in Chunks)
                if (chunk is HeaderChunk<T> s)
                    s.Discover();
        }

        public new TChunk GetChunk<TChunk>() where TChunk : HeaderChunk<T>
        {
            foreach (var chunk in Chunks)
            {
                if (chunk is TChunk t)
                {
                    t.Discover();
                    return t;
                }
            }
            return default;
        }

        public new bool TryGetChunk<TChunk>(out TChunk chunk) where TChunk : HeaderChunk<T>
        {
            chunk = GetChunk<TChunk>();
            return chunk != default;
        }

        public void RemoveAllChunks()
        {
            Chunks.Clear();
        }

        public new bool RemoveChunk<TChunk>() where TChunk : HeaderChunk<T>
        {
            return Chunks.Remove<TChunk>();
        }
    }

    public class GameBoxHeader : GameBoxPart, ILookbackable
    {
        int? ILookbackable.LookbackVersion { get; set; }
        List<string> ILookbackable.LookbackStrings { get; set; } = new List<string>();
        bool ILookbackable.LookbackWritten { get; set; }

        public GameBoxHeaderParameters Parameters { get; }

        public short Version => Parameters.Version;
        public char? ByteFormat => Parameters.ByteFormat;
        public char? RefTableCompression => Parameters.RefTableCompression;
        public char? BodyCompression => Parameters.BodyCompression;
        public char? UnknownByte => Parameters.UnknownByte;
        public uint? ClassID => Parameters.ClassID;
        public int? NumNodes => Parameters.NumNodes;

        public GameBoxHeader(GameBox gbx, GameBoxHeaderParameters parameters) : base(gbx)
        {
            Parameters = parameters;
        }

        public override T CreateChunk<T>(byte[] data)
        {
            throw new NotImplementedException();
        }

        public override T CreateChunk<T>()
        {
            throw new NotImplementedException();
        }

        public override void InsertChunk(Chunk chunk)
        {
            throw new NotImplementedException();
        }

        public override void DiscoverChunk<TChunk>()
        {
            throw new NotImplementedException();
        }

        public override void DiscoverChunks<TChunk1, TChunk2>()
        {
            throw new NotImplementedException();
        }

        public override void DiscoverChunks<TChunk1, TChunk2, TChunk3>()
        {
            throw new NotImplementedException();
        }

        public override void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4>()
        {
            throw new NotImplementedException();
        }

        public override void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5>()
        {
            throw new NotImplementedException();
        }

        public override void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5, TChunk6>()
        {
            throw new NotImplementedException();
        }

        public override void DiscoverAllChunks()
        {
            throw new NotImplementedException();
        }

        public override T GetChunk<T>()
        {
            throw new NotImplementedException();
        }

        public override bool TryGetChunk<T>(out T chunk)
        {
            throw new NotImplementedException();
        }

        public override bool RemoveChunk<T>()
        {
            throw new NotImplementedException();
        }
    }
}
