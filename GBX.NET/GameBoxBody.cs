using ManagedLZO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GBX.NET
{
    public class GameBoxBody<T> : IGameBoxBody where T : Node
    {
        int? ILookbackable.LookbackVersion { get; set; }
        List<string> ILookbackable.LookbackStrings { get; set; } = new List<string>();
        bool ILookbackable.LookbackWritten { get; set; }

        public IGameBox GBX { get; }
        public int? CompressedSize { get; }
        public int UncompressedSize { get; }
        public T MainNode { get; }
        public List<Node> AuxilaryNodes { get; }
        public byte[] Rest { get; }
        public bool Aborting { get; private set; }

        /// <summary>
        /// Body with uncompressed data with compression parameters
        /// </summary>
        /// <param name="data">UNCOMPRESSED</param>
        /// <param name="compressedSize"></param>
        /// <param name="uncompressedSize"></param>
        public GameBoxBody(GameBox<T> gbx, uint mainNodeID, byte[] data, int? compressedSize, int uncompressedSize)
        {
            GBX = gbx;
            CompressedSize = compressedSize;
            UncompressedSize = uncompressedSize;
            AuxilaryNodes = new List<Node>();

            using var s = new MemoryStream(data);
            using var gbxr = new GameBoxReader(s, this);
            MainNode = (T)Node.Parse(this, mainNodeID, gbxr);
            Debug.WriteLine("Amount read: " + (s.Position / (float)s.Length).ToString("P"));

            byte[] restBuffer = new byte[s.Length - s.Position];
            gbxr.Read(restBuffer, 0, restBuffer.Length);
            Rest = restBuffer;
        }

        public static GameBoxBody<T> DecompressAndConstruct(GameBox<T> gbx, uint mainNodeID, byte[] data, int compressedSize, int uncompressedSize)
        {
            byte[] buffer = new byte[uncompressedSize];
            MiniLZO.Decompress(data, buffer);
            return new GameBoxBody<T>(gbx, mainNodeID, buffer, compressedSize, uncompressedSize);
        }

        public void Write(GameBoxWriter w)
        {
            Write(w, ClassIDRemap.Latest);
        }

        public void Write(GameBoxWriter w, ClassIDRemap remap)
        {
            if(((GameBox<T>)GBX).Header.Result.BodyCompression == 'C')
            {
                using var msBody = new MemoryStream();
                using var gbxwBody = new GameBoxWriter(msBody, this);

                MainNode.Write(gbxwBody, remap);
                MiniLZO.Compress(msBody.ToArray(), out byte[] output);

                w.Write((int)msBody.Length); // Uncompressed
                w.Write(output.Length); // Compressed
                w.Write(output, 0, output.Length); // Compressed body data
            }
            else
                MainNode.Write(w);

            // ...
        }

        [Obsolete]
        public void Abort()
        {
            Aborting = true;
        }

        public void RemoveAllChunks()
        {
            MainNode.Chunks.Clear();
        }

        public bool RemoveChunk<TChunk>() where TChunk : Chunk
        {
            return MainNode.Chunks.Remove<TChunk>();
        }
    }
}
