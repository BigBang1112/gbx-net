namespace GBX.NET.Engines.Plug;

public partial class CPlugTree
{
    [AppliedWithChunk<Chunk0904F00D>]
    public string Name { get; set; } = "";

    [AppliedWithChunk<Chunk0904F015>]
    [AppliedWithChunk<Chunk0904F018>]
    [AppliedWithChunk<Chunk0904F019>]
    [AppliedWithChunk<Chunk0904F01A>]
    public int Flags { get; set; }

    [AppliedWithChunk<Chunk0904F015>]
    [AppliedWithChunk<Chunk0904F018>]
    [AppliedWithChunk<Chunk0904F019>]
    [AppliedWithChunk<Chunk0904F01A>]
    public Iso4? Location { get; set; }

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
                        foreach (var descendant in GetAllChildren(level.Tree, includeVisualMipLevels))
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

    internal IEnumerable<(CPlugTree Tree, Iso4 Location)> GetAllChildrenWithLocation(int lod = 0)
    {
        return GetAllChildren(this, lod);

        static IEnumerable<(CPlugTree, Iso4)> GetAllChildren(CPlugTree tree, int lod = 0, Iso4 location = default)
        {
            if (location == default)
            {
                location = Iso4.Identity;
            }

            if (tree is CPlugTreeVisualMip mip)
            {
                var lodChild = GetLodTree(mip, lod);

                var newLocation = MultiplyAddIso4(location, lodChild.Location.GetValueOrDefault(Iso4.Identity));

                foreach (var descendant in GetAllChildren(lodChild, lod, newLocation))
                {
                    yield return descendant;
                }
            }

            if (tree.Children is null)
            {
                yield break;
            }

            foreach (var child in tree.Children)
            {
                var childLocation = child.Location.GetValueOrDefault(Iso4.Identity);

                var newLocation = MultiplyAddIso4(location, childLocation);

                yield return (child, newLocation);

                foreach (var descendant in GetAllChildren(child, lod, newLocation))
                {
                    yield return descendant;
                }
            }
        }

        static Iso4 MultiplyAddIso4(Iso4 a, Iso4 b)
        {
            return new Iso4(
                a.XX * b.XX + a.XY * b.YX + a.XZ * b.ZX,
                a.XX * b.XY + a.XY * b.YY + a.XZ * b.ZY,
                a.XX * b.XZ + a.XY * b.YZ + a.XZ * b.ZZ,

                a.YX * b.XX + a.YY * b.YX + a.YZ * b.ZX,
                a.YX * b.XY + a.YY * b.YY + a.YZ * b.ZY,
                a.YX * b.XZ + a.YY * b.YZ + a.YZ * b.ZZ,

                a.ZX * b.XX + a.ZY * b.YX + a.ZZ * b.ZX,
                a.ZX * b.XY + a.ZY * b.YY + a.ZZ * b.ZY,
                a.ZX * b.XZ + a.ZY * b.YZ + a.ZZ * b.ZZ,

                a.TX + b.TX,
                a.TY + b.TY,
                a.TZ + b.TZ
            );
        }

        static CPlugTree GetLodTree(CPlugTreeVisualMip mip, int lod)
        {
            return mip.Levels
                .OrderBy(x => x.FarZ)
                .Select(x => x.Tree)
                .ElementAtOrDefault(lod) ?? mip.Levels
                    .OrderBy(x => x.FarZ)
                    .First()
                    .Tree;
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
