using GBX.NET.Managers;
using System.Text;

namespace GBX.NET.Exceptions;

[Serializable]
public class ChunkReadException : Exception
{
    public ChunkReadException(uint chunkId, uint? prevChunkId, bool known) : base(GetMessage(chunkId, prevChunkId, known)) { }

    public ChunkReadException(string? message) : base(message) { }
    public ChunkReadException(string? message, Exception? innerException) : base(message, innerException) { }

    private static string GetMessage(uint chunkId, uint? prevChunkId, bool known)
    {
        var sb = new StringBuilder(known ? "Known" : "Unknown");
        sb.Append(" unskippable chunk (");
        sb.Append(ClassManager.GetName(chunkId & 0xFFFFF000) ?? "???");
        sb.Append(" 0x");
        sb.Append(chunkId.ToString("X8"));
        sb.Append(") cannot be processed.");

        if (prevChunkId.HasValue)
        {
            sb.Append("\nPrevious chunk: ");
            sb.Append(ClassManager.GetName(prevChunkId.Value & 0xFFFFF000) ?? "???");
            sb.Append(" 0x");
            sb.Append(prevChunkId.Value.ToString("X8"));
        }

        return sb.ToString();
    }
}