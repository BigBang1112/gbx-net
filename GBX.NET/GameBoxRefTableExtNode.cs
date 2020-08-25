namespace GBX.NET
{
    public class ExternalNode
    {
        public int Flags { get; }
        public string FileName { get; }
        public int? ResourceIndex { get; }
        public int NodeIndex { get; }
        public bool? UseFile { get; }
        public int? FolderIndex { get; }

        public ExternalNode(int flags, string fileName, int? resourceIndex, int nodeIndex, bool? useFile, int? folderIndex)
        {
            Flags = flags;
            FileName = fileName;
            ResourceIndex = resourceIndex;
            NodeIndex = nodeIndex;
            UseFile = useFile;
            FolderIndex = folderIndex;
        }
    }
}