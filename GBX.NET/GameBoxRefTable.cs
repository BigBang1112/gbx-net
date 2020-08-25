namespace GBX.NET
{
    public class GameBoxRefTable
    {
        public GameBoxRefTableFolder RootFolder { get; }
        public ExternalNode[] ExternalNodes { get; }

        public GameBoxRefTable(GameBoxRefTableFolder rootFolder, params ExternalNode[] externalNodes)
        {
            RootFolder = rootFolder;
            ExternalNodes = externalNodes;
        }

        public void Write(GameBoxWriter w)
        {
            w.Write(ExternalNodes.Length);
            w.Write(RootFolder.Folders.Count);

            // ...
        }
    }
}