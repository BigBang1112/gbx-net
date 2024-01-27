namespace GBX.NET.Serialization.Chunking;

internal readonly record struct HeaderChunkInfo(uint Id, int Size, bool IsHeavy);
