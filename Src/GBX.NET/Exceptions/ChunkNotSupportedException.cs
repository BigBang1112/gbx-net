namespace GBX.NET.Exceptions;

public class ChunkNotSupportedException(uint id) : NotSupportedException($"Chunk ID 0x{id:X8} is not supported") { }