namespace GBX.NET;

public partial class GameBoxRefTable
{
    public class File
    {
        public int Flags { get; set; }
        public string? FileName { get; set; }
        public int? ResourceIndex { get; set; }
        public int NodeIndex { get; set; }
        public bool? UseFile { get; set; }
        public int FolderIndex { get; set; }
        
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