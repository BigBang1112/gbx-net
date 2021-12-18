using System.Diagnostics;

namespace GBX.NET;

public class GameBoxBody<T> : GameBoxBody where T : CMwNod
{
    public byte[]? Rest { get; set; }

    public new GameBox<T> GBX => (GameBox<T>)base.GBX;

    /// <summary>
    /// Body with uncompressed data with compression parameters.
    /// </summary>
    /// <param name="gbx">Owner of the GBX body.</param>
    public GameBoxBody(GameBox<T> gbx) : base(gbx)
    {

    }

    /// <exception cref="MissingLzoException"></exception>
    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public void Read(byte[] data, int uncompressedSize, IProgress<GameBoxReadProgress>? progress = null)
    {
        var buffer = new byte[uncompressedSize];

        Lzo.Decompress(data, buffer);

#if DEBUG
        Debugger.CompressedData = data;
        Debugger.UncompressedData = buffer;
#endif

        Read(buffer, progress);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public void Read(byte[] data, IProgress<GameBoxReadProgress>? progress = null)
    {
        using var ms = new MemoryStream(data);
        Read(ms, progress);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public void Read(Stream stream, IProgress<GameBoxReadProgress>? progress = null)
    {
        using var gbxr = new GameBoxReader(stream, this);
        Read(gbxr, progress);
    }

    /// <exception cref="NodeNotImplementedException">Auxiliary node is not implemented and is not parseable.</exception>
    /// <exception cref="ChunkReadNotImplementedException">Chunk does not support reading.</exception>
    /// <exception cref="IgnoredUnskippableChunkException">Chunk is known but its content is unknown to read.</exception>
    public void Read(GameBoxReader reader, IProgress<GameBoxReadProgress>? progress = null)
    {
        CMwNod.Parse(GBX.Node, reader, progress);

        IsParsed = true;

        using var ms = new MemoryStream();
        var s = reader.BaseStream;
        s.CopyTo(ms);
        Rest = ms.ToArray();
        Debug.WriteLine("Amount read: " + (s.Position / (float)(s.Position + Rest.Length)).ToString("P"));
    }

    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="MissingLzoException"></exception>
    public void Write(GameBoxWriter w, IDRemap remap = default)
    {
        GBX.Remap = remap;

        if (GBX.Header.CompressionOfBody == GameBoxCompression.Uncompressed)
        {
            GBX.Node.Write(w);
            return;
        }

        using var msBody = new MemoryStream();
        using var gbxwBody = new GameBoxWriter(msBody, body: this);

        GBX.Node.Write(gbxwBody, remap);

        var buffer = msBody.ToArray();

        var output = Lzo.Compress(buffer);

        w.Write((int)msBody.Length); // Uncompressed
        w.Write(output.Length); // Compressed
        w.WriteBytes(output); // Compressed body data
    }

    public TChunk CreateChunk<TChunk>(byte[] data) where TChunk : Chunk<T>
    {
        return GBX.Node.Chunks.Create<TChunk>(data);
    }

    public TChunk CreateChunk<TChunk>() where TChunk : Chunk<T>
    {
        return CreateChunk<TChunk>(Array.Empty<byte>());
    }

    public void InsertChunk(Chunk<T> chunk)
    {
        GBX.Node.Chunks.Add(chunk);
    }

    public void DiscoverChunk<TChunk>() where TChunk : SkippableChunk<T>
    {
        foreach (var chunk in GBX.Node.Chunks)
            if (chunk is TChunk c)
                c.Discover();
    }

    public void DiscoverChunks<TChunk1, TChunk2>() where TChunk1 : SkippableChunk<T> where TChunk2 : SkippableChunk<T>
    {
        foreach (var chunk in GBX.Node.Chunks)
        {
            switch (chunk)
            {
                case TChunk1 c: c.Discover(); break;
                case TChunk2 c: c.Discover(); break;
            }
        }
    }

    public void DiscoverChunks<TChunk1, TChunk2, TChunk3>()
        where TChunk1 : SkippableChunk<T>
        where TChunk2 : SkippableChunk<T>
        where TChunk3 : SkippableChunk<T>
    {
        foreach (var chunk in GBX.Node.Chunks)
        {
            switch (chunk)
            {
                case TChunk1 c: c.Discover(); break;
                case TChunk2 c: c.Discover(); break;
                case TChunk3 c: c.Discover(); break;
            }
        }
    }

    public void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4>()
        where TChunk1 : SkippableChunk<T>
        where TChunk2 : SkippableChunk<T>
        where TChunk3 : SkippableChunk<T>
        where TChunk4 : SkippableChunk<T>
    {
        foreach (var chunk in GBX.Node.Chunks)
        {
            switch (chunk)
            {
                case TChunk1 c: c.Discover(); break;
                case TChunk2 c: c.Discover(); break;
                case TChunk3 c: c.Discover(); break;
                case TChunk4 c: c.Discover(); break;
            }
        }
    }

    public void DiscoverChunks<TChunk1, TChunk2, TChunk3, TChunk4, TChunk5>()
        where TChunk1 : SkippableChunk<T>
        where TChunk2 : SkippableChunk<T>
        where TChunk3 : SkippableChunk<T>
        where TChunk4 : SkippableChunk<T>
        where TChunk5 : SkippableChunk<T>
    {
        foreach (var chunk in GBX.Node.Chunks)
        {
            switch (chunk)
            {
                case TChunk1 c: c.Discover(); break;
                case TChunk2 c: c.Discover(); break;
                case TChunk3 c: c.Discover(); break;
                case TChunk4 c: c.Discover(); break;
                case TChunk5 c: c.Discover(); break;
            }
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
        foreach (var chunk in GBX.Node.Chunks)
        {
            switch (chunk)
            {
                case TChunk1 c: c.Discover(); break;
                case TChunk2 c: c.Discover(); break;
                case TChunk3 c: c.Discover(); break;
                case TChunk4 c: c.Discover(); break;
                case TChunk5 c: c.Discover(); break;
                case TChunk6 c: c.Discover(); break;
            }
        }
    }

    public void DiscoverAllChunks()
    {
        foreach (var chunk in GBX.Node.Chunks)
            if (chunk is SkippableChunk<T> s)
                s.Discover();
    }

    public TChunk? GetChunk<TChunk>() where TChunk : Chunk<T>
    {
        foreach (var chunk in GBX.Node.Chunks)
        {
            if (chunk is not TChunk t)
                continue;

            if (chunk is SkippableChunk<T> s)
                s.Discover();

            return t;
        }

        return default;
    }

    public bool TryGetChunk<TChunk>(out TChunk? chunk) where TChunk : Chunk<T>
    {
        chunk = GetChunk<TChunk>();
        return chunk != default;
    }

    public void RemoveAllChunks()
    {
        GBX.Node.Chunks.Clear();
    }

    public bool RemoveChunk<TChunk>() where TChunk : Chunk<T>
    {
        return GBX.Node.Chunks.Remove<TChunk>();
    }
}