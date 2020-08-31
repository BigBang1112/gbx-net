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

        public T MainNode { get; }

        public GameBoxHeader(GameBoxHeaderParameters parameters) : base(parameters)
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

                if (availableClass == null)
                {
                    MainNode = (T)new Node(this, classID);
                    MainNode.Unknown = true;
                }
                else
                    MainNode = (T)Activator.CreateInstance(availableClass, this, classID);

                if (parameters.Version == 6)
                {
                    if (parameters.UserData != null && parameters.UserData.Length > 0)
                    {
                        var availableChunkClasses = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass
                        && x.Namespace.StartsWith("GBX.NET.Engines") && (x.BaseType == typeof(Chunk) || x.BaseType == typeof(SkippableChunk))
                        && x.GetCustomAttribute<ChunkAttribute>().ClassID == modernID).ToDictionary(x => x.GetCustomAttribute<ChunkAttribute>().ID);

                        var inheritanceClasses = new List<uint>();
                        if (GetBaseType(MainNode.GetType()) == typeof(Node))
                            inheritanceClasses = GetInheritance(MainNode.GetType());

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
                        var chunks = new HeaderChunk[numHeaderChunks];

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
                                chunks[counter] = (HeaderChunk)Activator.CreateInstance(type, MainNode, d);
                                chunks[counter].IsHeavy = chunk.Value.Item2;
                            }
                            else
                                chunks[counter] = new HeaderChunk(MainNode, chunkId, d);

                            counter++;
                        }
                        MainNode.Chunks = new ChunkList(chunks.Cast<Chunk>());
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
                w.Write(MainNode.ID);

                if (Version >= 6)
                {
                    using var userData = new MemoryStream();
                    using var gbxw = new GameBoxWriter(userData, this);
                    var gbxrw = new GameBoxReaderWriter(gbxw);

                    Dictionary<uint, int> lengths = new Dictionary<uint, int>();

                    foreach (var chunk in MainNode.Chunks.Values)
                    {
                        chunk.Unknown.Position = 0;

                        var pos = userData.Position;
                        if (chunk is SkippableChunk s && !s.Discovered)
                            s.Write(gbxw);
                        else
                            chunk.ReadWrite(gbxrw);
                        lengths[chunk.ID] = (int)(userData.Position - pos);
                    }

                    // Actual data size plus the class id (4 bytes) and each length (4 bytes) plus the number of chunks integer
                    w.Write((int)userData.Length + MainNode.Chunks.Count * 8 + 4);

                    // Write number of header chunks integer
                    w.Write(MainNode.Chunks.Count);

                    foreach (HeaderChunk chunk in MainNode.Chunks.Values)
                    {
                        w.Write(Chunk.Remap(chunk.ID, remap));
                        var length = lengths[chunk.ID];
                        if (chunk.IsHeavy)
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
    }

    public class GameBoxHeader : ILookbackable
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

        public GameBoxHeader(GameBoxHeaderParameters parameters)
        {
            Parameters = parameters;
        }
    }
}
