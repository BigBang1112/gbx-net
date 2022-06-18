using System.Security.Cryptography;
using System.Text;

namespace GBX.NET;

public partial class GameBoxRefTable
{
    public record File
    {
        public int Flags { get; }
        public string? FileName { get; }
        public int? ResourceIndex { get; }
        public int NodeIndex { get; }
        public bool? UseFile { get; }
        public int FolderIndex { get; }

        public File(int flags, string? fileName, int? resourceIndex, int nodeIndex, bool? useFile, int folderIndex)
        {
            Flags = flags;
            FileName = fileName;
            ResourceIndex = resourceIndex;
            NodeIndex = nodeIndex;
            UseFile = useFile;
            FolderIndex = folderIndex;
        }

        public override string ToString()
        {
            return FileName ?? string.Empty;
        }
    }
}