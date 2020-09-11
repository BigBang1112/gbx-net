using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GBX.NET
{
    public abstract class GameBoxPart : ILookbackable
    {
        int? ILookbackable.LookbackVersion { get; set; }
        List<string> ILookbackable.LookbackStrings { get; set; } = new List<string>();
        bool ILookbackable.LookbackWritten { get; set; }

        public GameBox GBX { get; }

        public GameBoxPart(GameBox gbx)
        {
            GBX = gbx;
        }

        public abstract T CreateChunk<T>(byte[] data) where T : Chunk;
        public abstract T CreateChunk<T>() where T : Chunk;
        public abstract void InsertChunk(Chunk chunk);
        public abstract void DiscoverChunk<TChunk>() where TChunk : ISkippableChunk;
        public abstract void DiscoverChunks<TChunk1, TChunk2>() where TChunk1 : ISkippableChunk where TChunk2 : ISkippableChunk;
        public abstract void DiscoverChunks<TChunk1, TChunk2, TChunk3>()
            where TChunk1 : ISkippableChunk
            where TChunk2 : ISkippableChunk
            where TChunk3 : ISkippableChunk;
        public abstract void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4>()
            where TChunk1 : ISkippableChunk
            where TChunk2 : ISkippableChunk
            where TChunk3 : ISkippableChunk
            where TChunk4 : ISkippableChunk;
        public abstract void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5>()
            where TChunk1 : ISkippableChunk
            where TChunk2 : ISkippableChunk
            where TChunk3 : ISkippableChunk
            where TChunk4 : ISkippableChunk
            where TChunk5 : ISkippableChunk;
        public abstract void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5, TChunk6>()
            where TChunk1 : ISkippableChunk
            where TChunk2 : ISkippableChunk
            where TChunk3 : ISkippableChunk
            where TChunk4 : ISkippableChunk
            where TChunk5 : ISkippableChunk
            where TChunk6 : ISkippableChunk;

        public abstract void DiscoverAllChunks();

        public abstract T GetChunk<T>() where T : Chunk;

        public abstract bool TryGetChunk<T>(out T chunk) where T : Chunk;

        public abstract bool RemoveChunk<T>() where T : Chunk;
    }
}
