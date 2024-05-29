namespace GBX.NET.Engines.Plug;

public partial class CPlugTree
{
    public string Name { get; set; } = "";

    public IEnumerable<CPlugTree> GetAllChildren(bool includeVisualMipLevels = false)
    {
        return GetAllChildren(this, includeVisualMipLevels);
        
        static IEnumerable<CPlugTree> GetAllChildren(CPlugTree tree, bool includeVisualMipLevels)
        {
            if (tree.Children is null)
            {
                yield break;
            }

            foreach (var child in tree.Children)
            {
                if (includeVisualMipLevels && child is CPlugTreeVisualMip mip)
                {
                    foreach (var level in mip.Levels)
                    {
                        foreach (var descendant in GetAllChildren(level.Value, includeVisualMipLevels))
                        {
                            yield return descendant;
                        }
                    }

                    continue;
                }

                yield return child;

                foreach (var descendant in GetAllChildren(child, includeVisualMipLevels))
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
