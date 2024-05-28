namespace GBX.NET.Engines.Plug;

public partial class CPlugTree
{
    public string Name { get; set; } = "";

    public IEnumerable<CPlugTree> GetAllChildren()
    {
        return GetAllChildren(this);
        
        static IEnumerable<CPlugTree> GetAllChildren(CPlugTree tree)
        {
            if (tree.Children is null)
            {
                yield break;
            }

            foreach (var child in tree.Children)
            {
                yield return child;

                foreach (var descendant in GetAllChildren(child))
                {
                    yield return descendant;
                }
            }
        }
    }

    public partial class Chunk0904F00D
    {
        public override void Read(CPlugTree n, GbxReader r)
        {
            n.Name = r.ReadIdAsString();
            U01 = r.ReadIdAsString();
        }

        public override void Write(CPlugTree n, GbxWriter w)
        {
            if (int.TryParse(n.Name, out var numericName) && (numericName & 0xC0000000) == 0)
            {
                w.Write(new Id(numericName));
            }
            else
            {
                w.WriteIdAsString(n.Name);
            }

            w.WriteIdAsString(U01);
        }
    }
}
