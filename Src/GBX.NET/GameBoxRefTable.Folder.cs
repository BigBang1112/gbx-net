using System.Text;

namespace GBX.NET;

public partial class GameBoxRefTable
{
    public record Folder(string Name, Folder? ParentFolder)
    {
        public override string ToString()
        {
            var builder = new StringBuilder(Name);

            var parent = ParentFolder;

            while (parent is not null)
            {
                builder.Insert(0, Path.DirectorySeparatorChar);
                builder.Insert(0, parent.Name);

                parent = parent.ParentFolder;
            }

            return builder.ToString();
        }
    }
}
