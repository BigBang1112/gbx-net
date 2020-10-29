using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public ChunkSet Chunks { get; set; }

        public GameBoxHeader(GameBox<T> gbx, byte[] userData) : base(gbx, userData)
        {
            if (gbx.Version >= 3)
            {
                if (gbx.Version >= 6)
                {
                    if (userData != null && userData.Length > 0)
                    {
                        using (var ms = new MemoryStream(userData))
                        using (var r = new GameBoxReader(ms, this))
                        {
                            var numHeaderChunks = r.ReadInt32();

                            var chunks = new Chunk[numHeaderChunks];

                            var chunkList = new Dictionary<uint, (int Size, bool IsHeavy)>();

                            for (var i = 0; i < numHeaderChunks; i++)
                            {
                                var chunkID = r.ReadUInt32();
                                var chunkSize = r.ReadUInt32();

                                var chId = chunkID & 0xFFF;
                                var clId = chunkID & 0xFFFFF000;

                                chunkList[clId + chId] = ((int)(chunkSize & ~0x80000000), (chunkSize & (1 << 31)) != 0);
                            }

                            Log.Write("Header data chunk list:");

                            foreach (var c in chunkList)
                            {
                                if (c.Value.IsHeavy)
                                    Log.Write($"| 0x{c.Key:X8} | {c.Value.Size} B (Heavy)");
                                else
                                    Log.Write($"| 0x{c.Key:X8} | {c.Value.Size} B");
                            }

                            int counter = 0;
                            foreach (var chunk in chunkList)
                            {
                                var chunkId = Chunk.Remap(chunk.Key);
                                var nodeId = chunkId & 0xFFFFF000;

                                if (!Node.AvailableClasses.TryGetValue(nodeId, out Type nodeType))
                                    Log.Write($"Node ID 0x{nodeId:X8} is not implemented. This occurs only in the header therefore it's not a fatal problem. ({Node.Names.Where(x => x.Key == nodeId).Select(x => x.Value).FirstOrDefault() ?? "unknown class"})");

                                var chunkTypes = new Dictionary<uint, Type>();

                                if(nodeType != null)
                                    Node.AvailableHeaderChunkClasses.TryGetValue(nodeType, out chunkTypes);

                                var d = r.ReadBytes(chunk.Value.Size);

                                if (chunkTypes.TryGetValue(chunkId, out Type type))
                                {
                                    var constructor = type.GetConstructors().First();
                                    var constructorParams = constructor.GetParameters();
                                    if (constructorParams.Length == 0)
                                    {
                                        ISkippableChunk headerChunk = (ISkippableChunk)constructor.Invoke(new object[0]);
                                        headerChunk.Node = GBX.MainNode;
                                        headerChunk.Part = this;
                                        headerChunk.Stream = new MemoryStream(d, 0, d.Length, false);
                                        if (d == null || d.Length == 0)
                                            headerChunk.Discovered = true;
                                        chunks[counter] = (Chunk)headerChunk;
                                    }
                                    else if (constructorParams.Length == 2)
                                        chunks[counter] = (HeaderChunk<T>)constructor.Invoke(new object[] { GBX.MainNode, d });
                                    else throw new ArgumentException($"{type.FullName} has an invalid amount of parameters.");

                                    using (var msChunk = new MemoryStream(d))
                                    using (var rChunk = new GameBoxReader(msChunk, this))
                                    {
                                        var rw = new GameBoxReaderWriter(rChunk);
                                        ((IHeaderChunk)chunks[counter]).ReadWrite(gbx.MainNode, rw);
                                        ((ISkippableChunk)chunks[counter]).Discovered = true;
                                    }

                                    ((IHeaderChunk)chunks[counter]).IsHeavy = chunk.Value.IsHeavy;
                                }
                                else if (nodeType != null)
                                    chunks[counter] = (Chunk)Activator.CreateInstance(typeof(HeaderChunk<>).MakeGenericType(nodeType), GBX.MainNode, chunkId, d);
                                else
                                    chunks[counter] = new HeaderChunk(chunkId, d) { IsHeavy = chunk.Value.IsHeavy };

                                counter++;
                            }
                            Chunks = new ChunkSet(chunks);
                        }
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
            w.Write(GBX.Version);

            if (GBX.Version >= 3)
            {
                w.Write((byte)GBX.ByteFormat.GetValueOrDefault());
                w.Write((byte)GBX.RefTableCompression.GetValueOrDefault());
                w.Write((byte)GBX.BodyCompression.GetValueOrDefault());
                if (GBX.Version >= 4) w.Write((byte)GBX.UnknownByte.GetValueOrDefault());
                w.Write(GBX.ClassID.GetValueOrDefault());

                if (GBX.Version >= 6)
                {
                    if (Chunks == null)
                    {
                        w.Write(0);
                    }
                    else
                    {
                        using (var userData = new MemoryStream())
                        using (var gbxw = new GameBoxWriter(userData, this))
                        {
                            var gbxrw = new GameBoxReaderWriter(gbxw);

                            Dictionary<uint, int> lengths = new Dictionary<uint, int>();

                            foreach (var chunk in Chunks)
                            {
                                chunk.Unknown.Position = 0;

                                var pos = userData.Position;
                                if (((ISkippableChunk)chunk).Discovered)
                                    ((IChunk)chunk).ReadWrite(GBX.MainNode, gbxrw);
                                else
                                    ((ISkippableChunk)chunk).Write(gbxw);

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
                    }
                }

                w.Write(numNodes);
            }
        }

        public void Write(GameBoxWriter w, int numNodes)
        {
            Write(w, numNodes, ClassIDRemap.Latest);
        }

        public new TChunk CreateChunk<TChunk>(byte[] data) where TChunk : Chunk
        {
            return Chunks.Create<TChunk>(data);
        }

        public new TChunk CreateChunk<TChunk>() where TChunk : Chunk
        {
            return CreateChunk<TChunk>(new byte[0]);
        }

        public void InsertChunk(IHeaderChunk chunk)
        {
            Chunks.Add((Chunk)chunk);
        }

        public new void DiscoverChunk<TChunk>() where TChunk : IHeaderChunk
        {
            foreach (var chunk in Chunks)
                if (chunk is TChunk c)
                    c.Discover();
        }

        public new void DiscoverChunks<TChunk1, TChunk2>() where TChunk1 : IHeaderChunk where TChunk2 : IHeaderChunk
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
            where TChunk1 : IHeaderChunk
            where TChunk2 : IHeaderChunk
            where TChunk3 : IHeaderChunk
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
            where TChunk1 : IHeaderChunk
            where TChunk2 : IHeaderChunk
            where TChunk3 : IHeaderChunk
            where TChunk4 : IHeaderChunk
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
            where TChunk1 : IHeaderChunk
            where TChunk2 : IHeaderChunk
            where TChunk3 : IHeaderChunk
            where TChunk4 : IHeaderChunk
            where TChunk5 : IHeaderChunk
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
            where TChunk1 : IHeaderChunk
            where TChunk2 : IHeaderChunk
            where TChunk3 : IHeaderChunk
            where TChunk4 : IHeaderChunk
            where TChunk5 : IHeaderChunk
            where TChunk6 : IHeaderChunk
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
                if (chunk is IHeaderChunk s)
                    s.Discover();
        }

        public new TChunk GetChunk<TChunk>() where TChunk : IHeaderChunk
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

        public new bool TryGetChunk<TChunk>(out TChunk chunk) where TChunk : IHeaderChunk
        {
            chunk = GetChunk<TChunk>();
            return chunk != null;
        }

        public void RemoveAllChunks()
        {
            Chunks.Clear();
        }

        public new bool RemoveChunk<TChunk>() where TChunk : Chunk
        {
            return Chunks.Remove<TChunk>();
        }
    }

    public class GameBoxHeader : GameBoxPart, ILookbackable
    {
        int? ILookbackable.LookbackVersion { get; set; }
        List<string> ILookbackable.LookbackStrings { get; set; } = new List<string>();
        bool ILookbackable.LookbackWritten { get; set; }

        public GameBoxHeader(GameBox gbx, byte[] userData) : base(gbx)
        {
            
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
