using GBX.NET.Components;

namespace GBX.NET.Serialization;

internal sealed class GbxHeaderReader(GbxReader reader, GbxReadSettings settings)
{
    public GbxHeader<T> Parse<T>(out T node) where T : notnull, IClass, new()
    {
        var basic = GbxHeaderBasic.Parse(reader);

        var classId = reader.ReadHexUInt32();

        // REMAP CLASS ID HERE, read out game version from it

        var expectedClassId = ClassManager.GetClassId<T>();

        if (classId != expectedClassId)
        {
            throw new InvalidCastException($"ClassId 0x{classId:X8} does not match {typeof(T).Name} classId 0x{expectedClassId:X8}.");
        }

        node = new T();

        var userData = basic.Version >= 6 ? ReadUserData(node) : null;

        var numNodes = reader.ReadInt32();

        return new GbxHeader<T>(basic)
        {
            UserData = userData,
            NumNodes = numNodes
        };
    }

    public GbxHeader Parse(out IClass? node)
    {
        var basic = GbxHeaderBasic.Parse(reader);

        var classId = reader.ReadHexUInt32();

        // REMAP CLASS ID HERE, read out game version from it

        node = ClassManager.New(classId);

        var header = ClassManager.NewHeader(basic, classId) ?? new GbxHeaderUnknown(basic, classId);

        header.UserData = basic.Version >= 6 ? ReadUserData(node) : null;

        header.NumNodes = reader.ReadInt32();

        return header;
    }

    internal HeaderChunkSet? ReadUserData<T>(T node) where T : notnull, IClass
    {
        var userDataNums = ValidateUserDataNumbers();

        if (userDataNums.Length == 0 || userDataNums.NumChunks == 0)
        {
            return null;
        }

        using var readerWriter = new GbxReaderWriter(reader, leaveOpen: true);

#if NET6_0_OR_GREATER
        Span<HeaderChunkInfo> headerChunkInfos = stackalloc HeaderChunkInfo[userDataNums.NumChunks];
#else
        var headerChunkInfos = new HeaderChunkInfo[userDataNums.NumChunks];
#endif

        FillHeaderChunkInfo(headerChunkInfos, reader, userDataNums);

        var userData = new HeaderChunkSet(userDataNums.NumChunks);

        foreach (var desc in headerChunkInfos)
        {
            // Used to validate chunk size
            var chunkStartPos = settings.SkipLengthValidation ? 0 : reader.BaseStream.Position;

#if NET8_0_OR_GREATER
            var chunk = T.NewHeaderChunk(desc.Id);
#else
            var chunk = ClassManager.NewHeaderChunk(desc.Id);
#endif

            if (chunk is null)
            {
                chunk = new HeaderChunk(desc.Id)
                {
                    IsHeavy = desc.IsHeavy,
                    Data = reader.ReadBytes(desc.Size)
                };

                userData.Add(chunk);
            }
            else
            {
                chunk.IsHeavy = desc.IsHeavy;

                userData.Add(chunk);

                var nodeToRead = chunk is ISelfContainedChunk scChunk ? scChunk.Node : node;

                switch (chunk)
                {
                    case IReadableWritableChunk<T> readableWritableT:
                        readableWritableT.ReadWrite(node, readerWriter);

                        if (settings.SkipDataUntilLengthMatches)
                        {
                            reader.SkipData(desc.Size - (int)(reader.BaseStream.Position - chunkStartPos));
                        }

                        break;
                    case IReadableChunk<T> readableT:
                        readableT.Read(node, reader);

                        if (settings.SkipDataUntilLengthMatches)
                        {
                            reader.SkipData(desc.Size - (int)(reader.BaseStream.Position - chunkStartPos));
                        }

                        break;
                    case IReadableWritableChunk readableWritable:
                        readableWritable.ReadWrite(nodeToRead, readerWriter);

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

        return userData;
    }

    internal HeaderChunkSet? ReadUserData(IClass? node)
    {
        var userDataNums = ValidateUserDataNumbers();

        if (userDataNums.Length == 0 || userDataNums.NumChunks == 0)
        {
            return null;
        }

        using var readerWriter = new GbxReaderWriter(reader, leaveOpen: true);

#if NET6_0_OR_GREATER
        Span<HeaderChunkInfo> headerChunkInfos = stackalloc HeaderChunkInfo[userDataNums.NumChunks];
#else
        var headerChunkInfos = new HeaderChunkInfo[userDataNums.NumChunks];
#endif

        FillHeaderChunkInfo(headerChunkInfos, reader, userDataNums);

        var userData = new HeaderChunkSet(userDataNums.NumChunks);

        foreach (var desc in headerChunkInfos)
        {
            // Used to validate chunk size
            var chunkStartPos = settings.SkipLengthValidation ? 0 : reader.BaseStream.Position;

            var chunk = ClassManager.NewHeaderChunk(desc.Id);
            
            if (chunk is null)
            {
                chunk = new HeaderChunk(desc.Id)
                {
                    IsHeavy = desc.IsHeavy,
                    Data = reader.ReadBytes(desc.Size)
                };

                userData.Add(chunk);
            }
            else
            {
                chunk.IsHeavy = desc.IsHeavy;
                userData.Add(chunk);

                var nodeToRead = ((chunk as ISelfContainedChunk)?.Node ?? node) ?? throw new Exception($"Chunk 0x{desc.Id:X8} requires a node to read into.");
                
                switch (chunk)
                {
                    case IReadableWritableChunk readableWritable:
                        readableWritable.ReadWrite(nodeToRead, readerWriter);

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

            // Used to validate user data length
            var chunkEndPos = settings.SkipLengthValidation ? 0 : reader.BaseStream.Position;

            // Non-matching chunk data length will throw
            if (chunkEndPos - chunkStartPos != desc.Size)
            {
                throw new InvalidDataException($"Chunk size {desc.Size} does not match actual data length {chunkEndPos - chunkStartPos}.");
            }
        }

        return userData;
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

            headerChunkDescs[i] = new HeaderChunkInfo(chunkId, actualChunkSize, isHeavy);

            // sizeof(uint) + sizeof(int) + actualChunkSize
            totalSize += 8 + actualChunkSize;

            if (totalSize > userDataNums.Length)
            {
                throw new InvalidDataException($"Header chunk 0x{chunkId:X8} (size {actualChunkSize}) exceeds user data length ({totalSize} > {userDataNums.Length}).");
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
            return new(0, 0);
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
            return new(userDataLength, NumChunks: 0);
        }

        // Header chunk count
        var numHeaderChunks = reader.ReadInt32();

        return new(userDataLength, numHeaderChunks);
    }
}
