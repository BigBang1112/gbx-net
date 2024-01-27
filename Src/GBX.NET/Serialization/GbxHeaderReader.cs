using GBX.NET.Components;
using GBX.NET.Managers;

namespace GBX.NET.Serialization;

internal sealed class GbxHeaderReader(GbxReader reader, GbxReadSettings settings)
{
    public GbxHeader<T> Parse<T>(out T node) where T : notnull, IClass, new()
    {
        var basic = GbxHeaderBasic.Parse(reader);

        var classId = ClassManager.Wrap(reader.ReadHexUInt32());

        var expectedClassId = ClassManager.GetClassId<T>();

        if (classId != expectedClassId)
        {
            throw new InvalidCastException($"Class ID 0x{classId:X8} ({ClassManager.GetName(classId) ?? "unknown class name"}) does not match expected class ID 0x{expectedClassId:X8} ({typeof(T).Name}).");
        }

        node = new T();

        if (basic.Version >= 6)
        {
            ReadUserData(node);
        }

        var numNodes = reader.ReadInt32();

        return new GbxHeader<T>(basic)
        {
            NumNodes = numNodes
        };
    }

    public GbxHeader Parse(out IClass? node)
    {
        var basic = GbxHeaderBasic.Parse(reader);

        var classId = ClassManager.Wrap(reader.ReadHexUInt32());

        node = ClassManager.New(classId);

        var header = node is null
            ? new GbxHeaderUnknown(basic, classId)
            : ClassManager.NewHeader(basic, classId) ?? new GbxHeaderUnknown(basic, classId);

        if (basic.Version >= 6)
        {
            ReadUserData(node, header as GbxHeaderUnknown);
        }

        header.NumNodes = reader.ReadInt32();

        return header;
    }

    internal bool ReadUserData<T>(T node) where T : notnull, IClass
    {
        var userDataNums = ValidateUserDataNumbers();

        if (userDataNums.Length == 0 || userDataNums.NumChunks == 0)
        {
            return false;
        }

        using var readerWriter = new GbxReaderWriter(reader, leaveOpen: true);

#if NET6_0_OR_GREATER
        Span<HeaderChunkInfo> headerChunkInfos = stackalloc HeaderChunkInfo[userDataNums.NumChunks];
#else
        var headerChunkInfos = new HeaderChunkInfo[userDataNums.NumChunks];
#endif

        FillHeaderChunkInfo(headerChunkInfos, reader, userDataNums);

        foreach (var desc in headerChunkInfos)
        {
            // Used to validate chunk size
            var chunkStartPos = settings.SkipLengthValidation ? 0 : reader.BaseStream.Position;

            var chunk = node.CreateHeaderChunk(desc.Id);

            if (chunk is null)
            {
                ReadAndAddUnknownHeaderChunk(node, unknownHeader: null, desc);
            }
            else
            {
                ReadKnownHeaderChunk(chunk, node, readerWriter, desc, chunkStartPos);
            }

            // Used to validate user data length
            var chunkEndPos = settings.SkipLengthValidation ? 0 : reader.BaseStream.Position;

            // Non-matching chunk data length will throw
            if (chunkEndPos - chunkStartPos != desc.Size)
            {
                if (chunkEndPos - chunkStartPos > desc.Size)
                {
                    throw new InvalidDataException($"Chunk size {desc.Size} does not match actual data length {chunkEndPos - chunkStartPos}.");
                }

                reader.SkipData(desc.Size - (int)(chunkEndPos - chunkStartPos));
            }
        }

        return true;
    }

    internal bool ReadUserData(IClass? node, GbxHeaderUnknown? unknownHeader)
    {
        var userDataNums = ValidateUserDataNumbers();

        if (userDataNums.Length == 0 || userDataNums.NumChunks == 0)
        {
            return false;
        }

        using var readerWriter = new GbxReaderWriter(reader, leaveOpen: true);

#if NET6_0_OR_GREATER
        Span<HeaderChunkInfo> headerChunkInfos = stackalloc HeaderChunkInfo[userDataNums.NumChunks];
#else
        var headerChunkInfos = new HeaderChunkInfo[userDataNums.NumChunks];
#endif

        FillHeaderChunkInfo(headerChunkInfos, reader, userDataNums);

        foreach (var desc in headerChunkInfos)
        {
            // Used to validate chunk size
            var chunkStartPos = settings.SkipLengthValidation ? 0 : reader.BaseStream.Position;

            var chunk = node?.CreateHeaderChunk(desc.Id);

            if (chunk is null)
            {
                ReadAndAddUnknownHeaderChunk(node, unknownHeader, desc);
            }
            else if (node is not null)
            {
                ReadKnownHeaderChunk(chunk, node, readerWriter, desc, chunkStartPos);
            }
            else
            {
                throw new Exception($"Chunk 0x{desc.Id:X8} requires a node to read into.");
            }

            // Used to validate user data length
            var chunkEndPos = settings.SkipLengthValidation ? 0 : reader.BaseStream.Position;

            // Non-matching chunk data length will throw
            if (chunkEndPos - chunkStartPos == desc.Size) continue;
            if (chunkEndPos - chunkStartPos > desc.Size)
            {
                throw new InvalidDataException($"Chunk size {desc.Size} does not match actual data length {chunkEndPos - chunkStartPos}.");
            }

            reader.SkipData(desc.Size - (int)(chunkEndPos - chunkStartPos));
        }

        return true;
    }

#if NET6_0_OR_GREATER
    internal static void FillHeaderChunkInfo(Span<HeaderChunkInfo> headerChunkDescs, GbxReader reader, UserDataNumbers userDataNums)
#else
    internal static void FillHeaderChunkInfo(HeaderChunkInfo[] headerChunkDescs, GbxReader reader, UserDataNumbers userDataNums)
#endif
    {
        var totalSize = 4; // Includes the number of header chunks

        for (var i = 0; i < userDataNums.NumChunks; i++)
        {
            var chunkId = reader.ReadHexUInt32();
            var chunkSize = reader.ReadInt32();
            var actualChunkSize = (int)(chunkSize & ~0x80000000);
            var isHeavy = (chunkSize & 0x80000000) != 0;

            if (actualChunkSize > GbxReader.MaxDataSize)
            {
                throw new LengthLimitException($"Header chunk size {actualChunkSize} exceeds maximum data size {GbxReader.MaxDataSize}.");
            }

            var mappedChunkId = ClassManager.Wrap(chunkId);

            headerChunkDescs[i] = new HeaderChunkInfo(mappedChunkId, actualChunkSize, isHeavy);

            // sizeof(uint) + sizeof(int) + actualChunkSize
            totalSize += 8 + actualChunkSize;

            if (totalSize > userDataNums.Length)
            {
                throw new InvalidDataException($"Header chunk 0x{mappedChunkId:X8} (size {actualChunkSize}) exceeds user data length ({totalSize} > {userDataNums.Length}).");
            }
        }

        // Non-matching user data length will throw
        if (totalSize != userDataNums.Length)
        {
            throw new InvalidDataException($"User data length {userDataNums.Length} does not match actual data length {totalSize}.");
        }
    }

    /// <summary>
    /// Reads the user data length and the number of header chunks, also validating the values and returning them as <see cref="UserDataNumbers"/>.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="LengthLimitException"></exception>
    internal UserDataNumbers ValidateUserDataNumbers()
    {
        var userDataLength = reader.ReadInt32();

        if (userDataLength == 0)
        {
            return new UserDataNumbers(0, 0);
        }

        // Maybe should be much stricter... and configurable
        if (userDataLength > GbxReader.MaxDataSize)
        {
            throw new LengthLimitException($"User data length {userDataLength} exceeds maximum data size {GbxReader.MaxDataSize}.");
        }

        // The idea is to preferably not create sub-buffers to reduce pressure on the GC.
        // If SkipUserData is true, it can be faster to reach the next parse stage (reference table):
        // - if seeking is supported, position moves past the user data
        // - if seeking is NOT supported:
        //   - .NET Standard 2.0: unavoidable byte array allocation with ReadBytes
        //   - .NET 6+: no allocation with Read(stackalloc byte[])

        if (settings.SkipUserData)
        {
            reader.SkipData(userDataLength);
            return new UserDataNumbers(userDataLength, NumChunks: 0);
        }

        // Header chunk count
        var numHeaderChunks = reader.ReadInt32();

        return new UserDataNumbers(userDataLength, numHeaderChunks);
    }

    private void ReadAndAddUnknownHeaderChunk(IClass? node, GbxHeaderUnknown? unknownHeader, HeaderChunkInfo desc)
    {
        var chunk = new HeaderChunk(desc.Id)
        {
            IsHeavy = desc.IsHeavy,
            Data = reader.ReadBytes(desc.Size)
        };

        if (node is not null)
        {
            node.Chunks.Add(chunk);
        }
        else if (unknownHeader is not null)
        {
            unknownHeader.UserData.Add(chunk);
        }
        else
        {
            throw new Exception($"Chunk 0x{desc.Id:X8} cannot be stored anywhere.");
        }
    }

    private void ReadKnownHeaderChunk<T>(IHeaderChunk chunk, T node, GbxReaderWriter rw, HeaderChunkInfo desc, long chunkStartPos)
        where T : notnull, IClass
    {
        chunk.IsHeavy = desc.IsHeavy;

        var nodeToRead = chunk is ISelfContainedChunk scChunk ? scChunk.Node : node;

        switch (chunk)
        {
            case IReadableWritableChunk<T> readableWritableT:
                readableWritableT.ReadWrite(node, rw);
                break;
            case IReadableChunk<T> readableT:
                readableT.Read(node, reader);
                break;
            case IReadableWritableChunk readableWritable:
                readableWritable.ReadWrite(nodeToRead, rw);
                break;
            case IReadableChunk readable:
                readable.Read(nodeToRead, reader);
                break;
            default:
                reader.SkipData(desc.Size);
                break;
        }

        if (settings.SkipDataUntilLengthMatches)
        {
            reader.SkipData(desc.Size - (int)(reader.BaseStream.Position - chunkStartPos));
        }
    }

    private void ReadKnownHeaderChunk(IHeaderChunk chunk, IClass node, GbxReaderWriter rw, HeaderChunkInfo desc, long chunkStartPos)
    {
        chunk.IsHeavy = desc.IsHeavy;

        var nodeToRead = ((chunk as ISelfContainedChunk)?.Node ?? node) ?? throw new Exception($"Chunk 0x{desc.Id:X8} requires a node to read into.");

        switch (chunk)
        {
            case IReadableWritableChunk readableWritable:
                readableWritable.ReadWrite(nodeToRead, rw);

                if (settings.SkipDataUntilLengthMatches)
                {
                    reader.SkipData(desc.Size - (int)(reader.BaseStream.Position - chunkStartPos));
                }

                break;
            case IReadableChunk readable:
                readable.Read(nodeToRead, reader);

                if (settings.SkipDataUntilLengthMatches)
                {
                    reader.SkipData(desc.Size - (int)(reader.BaseStream.Position - chunkStartPos));
                }

                break;
            default:
                reader.SkipData(desc.Size);
                break;
        }
    }
}
