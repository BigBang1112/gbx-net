using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using LZO;

namespace GBX.NET
{
    public class GameBoxBody<T> : GameBoxPart, IGameBoxBody where T : Node
    {
        private bool checkedForLzo;
        private MethodInfo methodLzoCompress;
        private MethodInfo methodLzoDecompress;

        public byte[] Rest { get; set; }
        public bool Aborting { get; private set; }

        [IgnoreDataMember]
        public SortedDictionary<int, Node> AuxilaryNodes { get; } = new SortedDictionary<int, Node>();

        public new GameBox<T> GBX => (GameBox<T>)base.GBX;

        /// <summary>
        /// Body with uncompressed data with compression parameters.
        /// </summary>
        /// <param name="gbx">Owner of the GBX body.</param>
        public GameBoxBody(GameBox<T> gbx) : base(gbx)
        {
            
        }

        public void Read(byte[] data, IProgress<GameBoxReadProgress> progress = null)
        {
            using (var ms = new MemoryStream(data))
                Read(ms, progress);
        }

        public void Read(Stream stream, IProgress<GameBoxReadProgress> progress = null)
        {
            using (var gbxr = new GameBoxReader(stream, this))
                Read(gbxr, progress);
        }

        public void Read(GameBoxReader reader, IProgress<GameBoxReadProgress> progress = null)
        {
            GBX.MainNode = Node.Parse(reader, GBX.ID.Value, GBX, progress);

            using (MemoryStream ms = new MemoryStream())
            {
                var s = reader.BaseStream;
                s.CopyTo(ms);
                Rest = ms.ToArray();
                Debug.WriteLine("Amount read: " + (s.Position / (float)(s.Position + Rest.Length)).ToString("P"));
            }
        }

        public void Read(byte[] data, int uncompressedSize, IProgress<GameBoxReadProgress> progress = null)
        {
            byte[] buffer = new byte[uncompressedSize];

            CheckForLZO();

            methodLzoDecompress.Invoke(null, new object[] { data, buffer });

            Read(buffer, progress);
        }

        public void Write(GameBoxWriter w)
        {
            Write(w, IDRemap.Latest);
        }

        public void Write(GameBoxWriter w, IDRemap remap)
        {
            if(GBX.Header.BodyCompression == 'C')
            {
                using (var msBody = new MemoryStream())
                using (var gbxwBody = new GameBoxWriter(msBody, this))
                {
                    GBX.MainNode.Write(gbxwBody, remap);
                    CheckForLZO();
                    var output = MiniLZO.Compress(msBody.ToArray());

                    w.Write((int)msBody.Length); // Uncompressed
                    w.Write(output.Length); // Compressed
                    w.Write(output, 0, output.Length); // Compressed body data
                }
            }
            else
                GBX.MainNode.Write(w);

            // ...
        }

        [Obsolete]
        public void Abort()
        {
            Aborting = true;
        }

        public TChunk CreateChunk<TChunk>(byte[] data) where TChunk : Chunk<T>
        {
            return GBX.MainNode.Chunks.Create<TChunk>(data);
        }

        public TChunk CreateChunk<TChunk>() where TChunk : Chunk<T>
        {
            return CreateChunk<TChunk>(new byte[0]);
        }

        public void InsertChunk(Chunk<T> chunk)
        {
            GBX.MainNode.Chunks.Add(chunk);
        }

        public void DiscoverChunk<TChunk>() where TChunk : SkippableChunk<T>
        {
            foreach (var chunk in GBX.MainNode.Chunks)
                if (chunk is TChunk c)
                    c.Discover();
        }

        public void DiscoverChunks<TChunk1, TChunk2>() where TChunk1 : SkippableChunk<T> where TChunk2 : SkippableChunk<T>
        {
            foreach (var chunk in GBX.MainNode.Chunks)
            {
                if (chunk is TChunk1 c1)
                    c1.Discover();
                if (chunk is TChunk2 c2)
                    c2.Discover();
            }
        }

        public void DiscoverChunks<TChunk1, TChunk2, TChunk3>()
            where TChunk1 : SkippableChunk<T>
            where TChunk2 : SkippableChunk<T>
            where TChunk3 : SkippableChunk<T>
        {
            foreach (var chunk in GBX.MainNode.Chunks)
            {
                if (chunk is TChunk1 c1)
                    c1.Discover();
                if (chunk is TChunk2 c2)
                    c2.Discover();
                if (chunk is TChunk3 c3)
                    c3.Discover();
            }
        }

        public void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4>()
            where TChunk1 : SkippableChunk<T>
            where TChunk2 : SkippableChunk<T>
            where TChunk3 : SkippableChunk<T>
            where TChunk4 : SkippableChunk<T>
        {
            foreach (var chunk in GBX.MainNode.Chunks)
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

        public void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5>()
            where TChunk1 : SkippableChunk<T>
            where TChunk2 : SkippableChunk<T>
            where TChunk3 : SkippableChunk<T>
            where TChunk4 : SkippableChunk<T>
            where TChunk5 : SkippableChunk<T>
        {
            foreach (var chunk in GBX.MainNode.Chunks)
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

        public void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5, TChunk6>()
            where TChunk1 : SkippableChunk<T>
            where TChunk2 : SkippableChunk<T>
            where TChunk3 : SkippableChunk<T>
            where TChunk4 : SkippableChunk<T>
            where TChunk5 : SkippableChunk<T>
            where TChunk6 : SkippableChunk<T>
        {
            foreach (var chunk in GBX.MainNode.Chunks)
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
            foreach (var chunk in GBX.MainNode.Chunks)
                if (chunk is SkippableChunk<T> s)
                    s.Discover();
        }

        public TChunk GetChunk<TChunk>() where TChunk : Chunk<T>
        {
            foreach (var chunk in GBX.MainNode.Chunks)
            {
                if (chunk is TChunk t)
                {
                    if (chunk is SkippableChunk<T> s) s.Discover();
                    return t;
                }
            }
            return default;
        }

        public bool TryGetChunk<TChunk>(out TChunk chunk) where TChunk : Chunk<T>
        {
            chunk = GetChunk<TChunk>();
            return chunk != default;
        }

        public void RemoveAllChunks()
        {
            GBX.MainNode.Chunks.Clear();
        }

        public bool RemoveChunk<TChunk>() where TChunk : Chunk<T>
        {
            return GBX.MainNode.Chunks.Remove<TChunk>();
        }

        private void CheckForLZO()
        {
            if (checkedForLzo) return;

            var references = Assembly.GetEntryAssembly().GetReferencedAssemblies();

            var lzoFound = false;

            foreach (var reference in references)
            {
                lzoFound = CheckForLZO(Assembly.Load(reference));
                if (lzoFound) break;
            }

            if (!lzoFound)
                throw new MissingLzoException();

            checkedForLzo = true;
        }

        private bool CheckForLZO(Assembly assembly)
        {
            var type = assembly.GetType("MiniLZO", false);

            if (type == null)
            {
                return false;
            }
            else
            {
                methodLzoCompress = type.GetMethod("Compress");
                methodLzoDecompress = type.GetMethod("Decompress");

                if (methodLzoCompress == null || methodLzoDecompress == null)
                    return false;

                return true;
            }
        }
    }
}
