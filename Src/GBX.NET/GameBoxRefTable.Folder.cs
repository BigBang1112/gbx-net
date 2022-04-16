namespace GBX.NET;

public partial class GameBoxRefTable
{
    public class Folder
    {
        public string Name { get; }
        public int Index { get; }
        public Folder? ParentFolder { get; }
        public IList<Folder> Folders { get; }
        public IList<File> Files { get; }

        public Folder(string name, int index, Folder? parentFolder = null)
        {
            Name = name;
            Index = index;
            ParentFolder = parentFolder;
            Folders = new List<Folder>();
            Files = new List<File>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
