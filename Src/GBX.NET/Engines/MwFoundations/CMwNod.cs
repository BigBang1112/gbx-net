using GBX.NET.Components;
using GBX.NET.Managers;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;

namespace GBX.NET.Engines.MwFoundations;

/// <remarks>ID: 0x01001000</remarks>
[Class(0x01001000)]
public partial class CMwNod : IClass
{
    private const uint SKIP = 0x534B4950;
    private const uint FACADE = 0xFACADE01;

    private IChunkSet? chunks;
    public IChunkSet Chunks => chunks ??= new ChunkSet();

#if NET8_0_OR_GREATER
    static void IClass.Read<T>(T node, GbxReaderWriter rw)
    {
        var r = rw.Reader ?? throw new Exception("Reader is required but not available.");

        r.TryInitializeDecryption(node);

        var prevChunkId = default(uint?);

        while (true)
        {
            var rawChunkId = r.ReadHexUInt32();

            if (rawChunkId == FACADE)
            {
                r.Logger?.LogDebug("- FACADE -");
                return;
            }
            
            _ = TryRemapChunkId(r, rawChunkId, out var chunkId);

            var chunk = node.CreateChunk(chunkId);

            var stopwatch = default(Stopwatch);

            // Unknown or skippable chunk
            if (chunk is null or ISkippableChunk)
            {
                // Text format skippable chunk
                if (chunk is null && r.Format == GbxFormat.Text)
                {
                    var skippableBuffer = Encoding.ASCII.GetString(r.ReadBytes(12));

                    if (skippableBuffer != "1397442896\r\n")
                    {
                        if (chunk is not null)
                        {
                            return;
                        }

                        throw new ChunkReadException(chunkId, prevChunkId, known: false);
                    }
                }
                // Binary format skippable chunk
                else if (r.ReadUInt32() != SKIP)
                {
                    if (chunk is not null)
                    {
                        return;
                    }

                    throw new ChunkReadException(chunkId, prevChunkId, known: false);
                }

                var chunkSize = r.ReadInt32();

                if (r.Logger is not null)
                {
                    if (chunkId == rawChunkId)
                    {
                        r.Logger.LogDebug("0x{ChunkId:X8} ({SkippableType}, size: {Size})", chunkId, chunk is null ? "unknown skippable" : "skippable", chunkSize);
                    }
                    else
                    {
                        r.Logger.LogDebug("0x{ChunkId:X8} ({SkippableType}, size: {Size}, raw: 0x{RawChunkId:X8})", chunkId, chunk is null ? "unknown skippable" : "skippable", chunkSize, rawChunkId);
                    }

                    if (r.Logger.IsEnabled(LogLevel.Trace))
                    {
                        stopwatch = Stopwatch.StartNew();
                    }
                }

                if (r.Settings.SkipChunkIds?.Contains(chunkId) == true)
                {
                    r.SkipData(chunkSize);
                }
                else
                {
                    switch (chunk)
                    {
                        case IReadableWritableChunk<T> readableWritableT:

                            if (readableWritableT.Ignore)
                            {
                                // TODO: possibility to skip (not read into memory)
                                ((ISkippableChunk)readableWritableT).Data = r.ReadBytes(chunkSize);
                                break;
                            }

                            if (r.Settings.SafeSkippableChunks)
                            {
                                var data = r.ReadBytes(chunkSize);
                                using var ms = new MemoryStream(data);
                                using var rSafe = new GbxReader(ms, r.Settings);
                                rSafe.LoadFrom(r);
                                using var rwSafe = new GbxReaderWriter(rSafe);

                                try
                                {
                                    readableWritableT.ReadWrite(node, rwSafe);
                                }
                                catch (Exception ex)
                                {
                                    r.Logger?.LogError(ex, "Skippable chunk data failed to read. Some properties may have been read. Buffer will be stored and used instead.");
                                    ((ISkippableChunk)readableWritableT).Data = data;
                                }

                                r.LoadFrom(rSafe);

                                break;
                            }

                            readableWritableT.ReadWrite(node, rw);
                            // TODO: validate chunk size

                            break;
                        case IReadableChunk<T> readableT:

                            if (readableT.Ignore)
                            {
                                // TODO: possibility to skip (not read into memory)
                                ((ISkippableChunk)readableT).Data = r.ReadBytes(chunkSize);
                                break;
                            }

                            if (r.Settings.SafeSkippableChunks)
                            {
                                var data = r.ReadBytes(chunkSize);
                                using var ms = new MemoryStream(data);
                                using var rSafe = new GbxReader(ms, r.Settings);
                                rSafe.LoadFrom(r);

                                try
                                {
                                    readableT.Read(node, rSafe);
                                }
                                catch (Exception ex)
                                {
                                    r.Logger?.LogError(ex, "Skippable chunk data failed to read. Some properties may have been read. Buffer will be stored and used instead.");
                                    ((ISkippableChunk)readableT).Data = data;
                                }

                                r.LoadFrom(rSafe);

                                break;
                            }

                            readableT.Read(node, r);
                            // TODO: validate chunk size

                            break;
                        case IReadableWritableChunk readableWritable:

                            if (readableWritable.Ignore)
                            {
                                // TODO: possibility to skip (not read into memory)
                                ((ISkippableChunk)readableWritable).Data = r.ReadBytes(chunkSize);
                                break;
                            }

                            if (r.Settings.SafeSkippableChunks)
                            {
                                var data = r.ReadBytes(chunkSize);
                                using var ms = new MemoryStream(data);
                                using var rSafe = new GbxReader(ms, r.Settings);
                                rSafe.LoadFrom(r);
                                using var rwSafe = new GbxReaderWriter(rSafe);

                                try
                                {
                                    readableWritable.ReadWrite(node, rwSafe);
                                }
                                catch (Exception ex)
                                {
                                    r.Logger?.LogError(ex, "Skippable chunk data failed to read. Some properties may have been read. Buffer will be stored and used instead.");
                                    ((ISkippableChunk)readableWritable).Data = data;
                                }

                                r.LoadFrom(rSafe);

                                break;
                            }

                            readableWritable.ReadWrite(node, rw);
                            // TODO: validate chunk size

                            break;
                        case IReadableChunk readable:

                            if (readable.Ignore)
                            {
                                // TODO: possibility to skip (not read into memory)
                                ((ISkippableChunk)readable).Data = r.ReadBytes(chunkSize);
                                break;
                            }

                            if (r.Settings.SafeSkippableChunks)
                            {
                                var data = r.ReadBytes(chunkSize);
                                using var ms = new MemoryStream(data);
                                using var rSafe = new GbxReader(ms, r.Settings);
                                rSafe.LoadFrom(r);

                                try
                                {
                                    readable.Read(node, rSafe);
                                }
                                catch (Exception ex)
                                {
                                    r.Logger?.LogError(ex, "Skippable chunk data failed to read. Some properties may have been read. Buffer will be stored and used instead.");
                                    ((ISkippableChunk)readable).Data = data;
                                }

                                r.LoadFrom(rSafe);

                                break;
                            }

                            readable.Read(node, r);
                            // TODO: validate chunk size

                            break;
                        case ISkippableChunk skippable: // Known skippable but does not include reading/writing logic
                                                        // TODO: possibility to skip (not read into memory)
                            skippable.Data = r.ReadBytes(chunkSize);
                            break;
                        default: // Unknown skippable

                            // TODO: possibility to skip (not read into memory), maybe create typed skippable chunk in the future?
                            var skippableChunk = new SkippableChunk(chunkId)
                            {
                                Data = r.ReadBytes(chunkSize)
                            };

                            node.Chunks.Add(skippableChunk); // as its an unknown chunk, its not implicitly added by CreateChunk

                            break;
                    }
                }

                if (r.Logger?.IsEnabled(LogLevel.Trace) == true && stopwatch is not null)
                {
                    stopwatch.Stop();
                    r.Logger.LogTrace("0x{ChunkId:X8} DONE ({Elapsed}ms)", chunkId, stopwatch.Elapsed.TotalMilliseconds);
                }

                prevChunkId = chunkId;

                continue;
            }

            // Unskippable chunk
            if (r.Logger is not null)
            {
                if (chunkId == rawChunkId)
                {
                    r.Logger.LogDebug("0x{ChunkId:X8}", chunkId);
                }
                else
                {
                    r.Logger.LogDebug("0x{ChunkId:X8} (raw: 0x{RawChunkId:X8})", chunkId, rawChunkId);
                }

                if (r.Logger.IsEnabled(LogLevel.Trace))
                {
                    stopwatch = Stopwatch.StartNew();
                }
            }

            if (chunk.Ignore)
            {
                throw new ChunkReadException(chunkId, prevChunkId, known: true);
            }

            switch (chunk)
            {
                case IReadableWritableChunk<T> readableWritableT:
                    readableWritableT.ReadWrite(node, rw);
                    break;
                case IReadableChunk<T> readableT:
                    readableT.Read(node, r);
                    break;
                case IReadableWritableChunk readableWritable:
                    readableWritable.ReadWrite(node, rw);
                    break;
                case IReadableChunk readable:
                    readable.Read(node, r);
                    break;
                default:
                    throw new ChunkReadException(chunkId, prevChunkId, known: true);
            }

            if (r.Logger?.IsEnabled(LogLevel.Trace) == true && stopwatch is not null)
            {
                stopwatch.Stop();
                r.Logger.LogTrace("0x{ChunkId:X8} DONE ({Elapsed}ms)", chunkId, stopwatch.Elapsed.TotalMilliseconds);
            }

            prevChunkId = chunkId;
        }
    }
#endif

    internal virtual void Read(GbxReaderWriter rw)
    {
        var r = rw.Reader ?? throw new Exception("Reader is required but not available.");

        r.TryInitializeDecryption(this);

        var prevChunkId = default(uint?);

        while (true)
        {
            var rawChunkId = r.ReadHexUInt32();

            if (rawChunkId == FACADE)
            {
                r.Logger?.LogDebug("- FACADE -");
                return;
            }

            _ = TryRemapChunkId(r, rawChunkId, out var chunkId);

            var chunk = CreateChunk(chunkId);

            var stopwatch = default(Stopwatch);

            // Unknown or skippable chunk
            if (chunk is null or ISkippableChunk)
            {
                // Text format skippable chunk
                if (chunk is null && r.Format == GbxFormat.Text)
                {
                    var skippableBuffer = Encoding.ASCII.GetString(r.ReadBytes(12));

                    if (skippableBuffer != "1397442896\r\n")
                    {
                        if (chunk is not null)
                        {
                            return;
                        }

                        throw new ChunkReadException(chunkId, prevChunkId, known: false);
                    }
                }
                // Binary format skippable chunk
                else if (r.ReadUInt32() != SKIP)
                {
                    if (chunk is not null)
                    {
                        return;
                    }

                    throw new ChunkReadException(chunkId, prevChunkId, known: false);
                }

                var chunkSize = r.ReadInt32();

                if (r.Logger is not null)
                {
                    if (chunkId == rawChunkId)
                    {
                        r.Logger.LogDebug("0x{ChunkId:X8} ({SkippableType}, size: {Size})", chunkId, chunk is null ? "unknown skippable" : "skippable", chunkSize);
                    }
                    else
                    {
                        r.Logger.LogDebug("0x{ChunkId:X8} ({SkippableType}, size: {Size}, raw: 0x{RawChunkId:X8})", chunkId, chunk is null ? "unknown skippable" : "skippable", chunkSize, rawChunkId);
                    }

                    if (r.Logger.IsEnabled(LogLevel.Trace))
                    {
                        stopwatch = Stopwatch.StartNew();
                    }
                }

                if (r.Settings.SkipChunkIds?.Contains(chunkId) == true)
                {
                    r.SkipData(chunkSize);
                }
                else
                {
                    switch (chunk)
                    {
                        case IReadableWritableChunk readableWritable:

                            if (readableWritable.Ignore)
                            {
                                // TODO: possibility to skip (not read into memory)
                                ((ISkippableChunk)readableWritable).Data = r.ReadBytes(chunkSize);
                                break;
                            }

                            if (r.Settings.SafeSkippableChunks)
                            {
                                var data = r.ReadBytes(chunkSize);
                                using var ms = new MemoryStream(data);
                                using var rSafe = new GbxReader(ms, r.Settings);
                                rSafe.LoadFrom(r);
                                using var rwSafe = new GbxReaderWriter(rSafe);

                                try
                                {
                                    readableWritable.ReadWrite(this, rwSafe);
                                }
                                catch (Exception ex)
                                {
                                    r.Logger?.LogError(ex, "Skippable chunk data failed to read. Some properties may have been read. Buffer will be stored and used instead.");
                                    ((ISkippableChunk)readableWritable).Data = data;
                                }

                                r.LoadFrom(rSafe);

                                break;
                            }

                            readableWritable.ReadWrite(this, rw);
                            // TODO: validate chunk size

                            break;

                        case IReadableChunk readable:

                            if (readable.Ignore)
                            {
                                // TODO: possibility to skip (not read into memory)
                                ((ISkippableChunk)readable).Data = r.ReadBytes(chunkSize);
                                break;
                            }

                            if (r.Settings.SafeSkippableChunks)
                            {
                                var data = r.ReadBytes(chunkSize);
                                using var ms = new MemoryStream(data);
                                using var rSafe = new GbxReader(ms, r.Settings);
                                rSafe.LoadFrom(r);

                                try
                                {
                                    readable.Read(this, rSafe);
                                }
                                catch (Exception ex)
                                {
                                    r.Logger?.LogError(ex, "Skippable chunk data failed to read. Some properties may have been read. Buffer will be stored and used instead.");
                                    ((ISkippableChunk)readable).Data = data;
                                }

                                r.LoadFrom(rSafe);

                                break;
                            }

                            readable.Read(this, r);
                            // TODO: validate chunk size

                            break;

                        case ISkippableChunk skippable: // Known skippable but does not include reading/writing logic
                                                        // TODO: possibility to skip (not read into memory)
                            skippable.Data = r.ReadBytes(chunkSize);
                            break;
                        default: // Unknown skippable

                            // TODO: possibility to skip (not read into memory), maybe create typed skippable chunk in the future?
                            var skippableChunk = new SkippableChunk(chunkId)
                            {
                                Data = r.ReadBytes(chunkSize)
                            };

                            Chunks.Add(skippableChunk); // as its an unknown chunk, its not implicitly added by CreateChunk

                            break;
                    }
                }

                if (r.Logger?.IsEnabled(LogLevel.Trace) == true && stopwatch is not null)
                {
                    stopwatch.Stop();
                    r.Logger.LogTrace("0x{ChunkId:X8} DONE ({Elapsed}ms)", chunkId, stopwatch.Elapsed.TotalMilliseconds);
                }

                prevChunkId = chunkId;

                continue;
            }

            // Unskippable chunk
            if (r.Logger is not null)
            {
                if (chunkId == rawChunkId)
                {
                    r.Logger.LogDebug("0x{ChunkId:X8}", chunkId);
                }
                else
                {
                    r.Logger.LogDebug("0x{ChunkId:X8} (raw: 0x{RawChunkId:X8})", chunkId, rawChunkId);
                }

                if (r.Logger.IsEnabled(LogLevel.Trace))
                {
                    stopwatch = Stopwatch.StartNew();
                }
            }

            if (chunk.Ignore)
            {
                throw new ChunkReadException(chunkId, prevChunkId, known: true);
            }

            switch (chunk)
            {
                case IReadableWritableChunk readableWritable:
                    readableWritable.ReadWrite(this, rw);
                    break;
                case IReadableChunk readable:
                    readable.Read(this, r);
                    break;
                default:
                    throw new ChunkReadException(chunkId, prevChunkId, known: true);
            }

            if (r.Logger?.IsEnabled(LogLevel.Trace) == true && stopwatch is not null)
            {
                stopwatch.Stop();
                r.Logger.LogTrace("0x{ChunkId:X8} DONE ({Elapsed}ms)", chunkId, stopwatch.Elapsed.TotalMilliseconds);
            }

            prevChunkId = chunkId;
        }
    }

    internal virtual void Write(GbxReaderWriter rw)
    {
        var w = rw.Writer ?? throw new Exception("Writer is required but not available.");

        foreach (var chunk in Chunks)
        {
            if (chunk is IHeaderChunk)
            {
                continue;
            }

            WriteChunkId(w, chunk.Id);

            var chunkW = w;
            var chunkRw = rw;

            var ms = default(MemoryStream);

            if (chunk is ISkippableChunk skippable)
            {
                w.WriteHexUInt32(SKIP);

                if (skippable.Data is not null)
                {
                    w.Write(skippable.Data.Length);
                    w.Write(skippable.Data);
                    continue;
                }

                ms = new MemoryStream();
                chunkW = new GbxWriter(ms);
                chunkW.LoadFrom(w);
                chunkRw = new GbxReaderWriter(chunkW);
            }

            switch (chunk)
            {
                case IReadableWritableChunk readableWritable:
                    readableWritable.ReadWrite(this, chunkRw);
                    break;
                case IWritableChunk writable:
                    writable.Write(this, chunkW);
                    break;
                default:
                    throw new Exception($"Chunk (ID 0x{chunk.Id:X8}, {ClassManager.GetName(chunk.Id & 0xFFFFF000)}) cannot be processed");
            }

            // Memory stream is not null only if chunk is skippable and not ignored
            if (ms is not null)
            {
                w.Write((uint)ms.Length);
                ms.WriteTo(w.BaseStream);
                w.LoadFrom(chunkW);
                ms.Dispose();
            }
        }

        w.WriteHexUInt32(FACADE);
    }

    /// <inheritdoc />
    public virtual void ReadWrite(GbxReaderWriter rw)
    {
        if (rw.Reader is not null)
        {
            Read(rw);
        }

        if (rw.Writer is not null)
        {
            Write(rw);
        }
    }

    public virtual IHeaderChunk? CreateHeaderChunk(uint chunkId)
    {
        return null;
    }

    public virtual IChunk? CreateChunk(uint chunkId)
    {
        var chunk = chunkId switch
        {
            0x01001000 => new Chunk01001000(),
            _ => null
        };

        if (chunk is not null)
        {
            Chunks.Add(chunk);
        }

        return chunk;
    }

    public virtual CMwNod DeepClone()
    {
        var clone = (CMwNod)MemberwiseClone();
        DeepCloneChunks(clone);
        return clone;
    }

    IClass IClass.DeepClone()
    {
        var clone = (IClass)MemberwiseClone();
        DeepCloneChunks(clone);
        return clone;
    }

    protected void DeepCloneChunks(IClass dest)
    {
        if (chunks is null)
        {
            return;
        }
        
        foreach (var chunk in chunks)
        {
            var chunkClone = chunk.DeepClone();
            dest.Chunks.Add(chunkClone);
        }
    }

    /// <summary>
    /// The version of the game where this node should be accepted without a game crash.
    /// </summary>
    public GameVersion GameVersion
    {
        get
        {
            var version = (GameVersion)int.MaxValue;

            foreach (var chunk in Chunks)
            {
                version &= chunk.GameVersion;
            }

            return (int)version == int.MaxValue ? GameVersion.Unspecified : version;
        }
    }

    /// <summary>
    /// Checks if the node is accepted in the given game version without a game crash.
    /// </summary>
    /// <param name="version">Game version flags.</param>
    /// <param name="strict">If enabled, checks for the exact flag match (if accepted in MP4 and TM2020, only <c>MP4 | TM2020</c> will return true).</param>
    /// <returns>True if matches, false if not.</returns>
    public virtual bool IsGameVersion(GameVersion version, bool strict = false)
    {
        return strict
            ? GameVersion == version
            : (GameVersion & version) == version;
    }

    /// <summary>
    /// Checks if the node could be accepted in the given game version.
    /// </summary>
    /// <param name="version">Game version flags.</param>
    /// <returns>True if matches, false if not.</returns>
    public bool CanBeGameVersion(GameVersion version)
    {
        return (GameVersion & version) != 0;
    }

    public Gbx ToGbx(GbxHeaderBasic headerBasic)
    {
        var classId = ClassManager.GetId(GetType()) ?? throw new Exception("Class ID not found.");
        var header = ClassManager.NewHeader(headerBasic, classId) ?? throw new Exception("Header cannot be created.");
        return ClassManager.NewGbx(header, new GbxBody(), this) ?? throw new Exception("Gbx cannot be created.");
    }

    public Gbx ToGbx()
    {
        return ToGbx(GbxHeaderBasic.Default);
    }

    public void Save(Stream stream, GbxWriteSettings settings = default)
    {
        ToGbx().Save(stream, settings);
    }

    public void Save(string fileName, GbxWriteSettings settings = default)
    {
        ToGbx().Save(fileName, settings);
    }

    public T CreateChunk<T>() where T : IChunk, new()
    {
        return Chunks.Create<T>();
    }

    private static bool TryRemapChunkId(GbxReader reader, uint chunkId, out uint remappedChunkId)
    {
        if (!ClassManager.IsChunkIdRemapped(chunkId))
        {
            remappedChunkId = chunkId;
            return false;
        }

        remappedChunkId = ClassManager.Wrap(chunkId);

        if (chunkId == remappedChunkId)
        {
            return false;
        }

        // Make sure 2008 doesn't get remapped to 2006 again after
        if (reader.ClassIdRemapMode != ClassIdRemapMode.Id2008)
        {
            reader.ClassIdRemapMode = (chunkId & 0xFFFFF000) is 0x0301A000 // CGameCtnCollector in TMF
                ? ClassIdRemapMode.Id2008
                : ClassIdRemapMode.Id2006;
        }

        return true;
    }

    private static void WriteChunkId(GbxWriter writer, uint chunkId)
    {
        if (writer.ClassIdRemapMode == ClassIdRemapMode.Latest)
        {
            writer.WriteHexUInt32(chunkId);
            return;
        }

        if (writer.ClassIdRemapMode == ClassIdRemapMode.Id2008 && (chunkId & 0xFFFFF000) == 0x2E001000)
        {
            writer.WriteHexUInt32(0x0301A000 | (chunkId & 0xFFF));
            return;
        }

        var unwrappedChunkId = ClassManager.Unwrap(chunkId);
        writer.WriteHexUInt32(unwrappedChunkId);
    }
}
