using GBX.NET.Components;

namespace GBX.NET.Engines.Plug;

public partial class CPlugTree
{
    [AppliedWithChunk<Chunk0904F00D>]
    public string Name { get; set; } = "";

    [AppliedWithChunk<Chunk0904F015>]
    [AppliedWithChunk<Chunk0904F018>]
    [AppliedWithChunk<Chunk0904F019>]
    [AppliedWithChunk<Chunk0904F01A>]
    public ulong Flags { get; set; }

    [AppliedWithChunk<Chunk0904F015>]
    [AppliedWithChunk<Chunk0904F018>]
    [AppliedWithChunk<Chunk0904F019>]
    [AppliedWithChunk<Chunk0904F01A>]
    public Iso4? Location { get; set; }

    public CPlugMaterial? Material
    {
        get => Shader as CPlugMaterial;
        set => Shader = value;
    }

    public GbxRefTableFile? MaterialFile
    {
        get => ShaderFile;
        set => ShaderFile = value;
    }

    public bool IsVisible
    {
        get => (Flags >> 0x3 & 0x1) != 0;
        set => Flags = value ? (Flags | 0x8) : (Flags & ~0x8u);
    }

    public bool IsCollidable
    {
        get => (Flags >> 0x7 & 0x1) != 0;
        set => Flags = value ? (Flags | 0x80) : (Flags & ~0x80u);
    }

    public bool IsRooted
    {
        get => (Flags >> 0xF & 0x1) != 0;
        set => Flags = value ? (Flags | 0x1000) : (Flags & ~0x1000u);
    }

    public bool IsLightVolume
    {
        get => (Flags >> 0x9 & 0x1) != 0;
        set => Flags = value ? (Flags | 0x200) : (Flags & ~0x200u);
    }

    public bool IsLightVolumeVisible
    {
        get => (Flags >> 0xA & 0x1) != 0;
        set => Flags = value ? (Flags | 0x400) : (Flags & ~0x400u);
    }

    public bool UseLocation
    {
        get => (Flags >> 0x2 & 0x1) != 0;
        set => Flags = value ? (Flags | 0x4) : (Flags & ~0x4u);
    }

    public bool IsShadowCaster
    {
        get => (Flags >> 0xE & 0x1) != 0;
        set => Flags = value ? (Flags | 0x800) : (Flags & ~0x800u);
    }

    public bool IsFixedRatio2D
    {
        get => (Flags >> 0x8 & 0x1) != 0;
        set => Flags = value ? (Flags | 0x100) : (Flags & ~0x100u);
    }

    public bool IsPickable
    {
        get => (Flags >> 0x6 & 0x1) != 0;
        set => Flags = value ? (Flags | 0x40) : (Flags & ~0x40u);
    }

    public bool IsPickableVisual
    {
        get => (Flags >> 0xB & 0x1) != 0;
        set => Flags = value ? (Flags | 0x800) : (Flags & ~0x800u);
    }

    public bool IsPortal
    {
        get => (Flags & 0x1) != 0;
        set => Flags = value ? (Flags | 0x1) : (Flags & ~0x1u);
    }

    public bool TestBBoxVisibility
    {
        get => (Flags >> 0xD & 0x1) != 0;
        set => Flags = value ? (Flags | 0x4000) : (Flags & ~0x4000u);
    }

    public bool UseRenderBefore
    {
        get => (Flags >> 0xC & 0x1) != 0;
        set => Flags = value ? (Flags | 0x2000) : (Flags & ~0x2000u);
    }

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
                yield return child;

                if (includeVisualMipLevels && child is CPlugTreeVisualMip mip)
                {
                    foreach (var level in mip.Levels)
                    {
                        foreach (var descendant in GetAllChildren(level.Tree, includeVisualMipLevels))
                        {
                            yield return descendant;
                        }
                    }
                }

                foreach (var descendant in GetAllChildren(child, includeVisualMipLevels))
                {
                    yield return descendant;
                }
            }
        }
    }

    public IEnumerable<(CPlugTree Tree, Iso4 Location)> GetAllChildrenWithLocation(int lod = 0)
    {
        return GetAllChildren(this, lod);

        static IEnumerable<(CPlugTree, Iso4)> GetAllChildren(CPlugTree tree, int lod = 0, Iso4 location = default)
        {
            if (location == default)
            {
                location = tree.Location ?? Iso4.Identity;
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

                a.XX * b.TX + a.XY * b.TY + a.XZ * b.TZ + a.TX,
                a.YX * b.TX + a.YY * b.TY + a.YZ * b.TZ + a.TY,
                a.ZX * b.TX + a.ZY * b.TY + a.ZZ * b.TZ + a.TZ
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

    public partial class Chunk0904F015
    {
        public override void ReadWrite(CPlugTree n, GbxReaderWriter rw)
        {
            n.Flags = rw.DataUInt32((uint)n.Flags) & 0x1FFFF;

            if ((n.Flags & 4) != 0)
            {
                n.Location = rw.Iso4(n.Location);
            }
        }
    }

    public partial class Chunk0904F018
    {
        public override void ReadWrite(CPlugTree n, GbxReaderWriter rw)
        {
            n.Flags = rw.DataUInt64(n.Flags);

            if ((n.Flags & 4) != 0)
            {
                n.Location = rw.Iso4(n.Location);
            }

            n.Flags |= 0x2800;
            // Flags = Flags | 0x8000; (if Bucr3IsR)
        }
    }

    public partial class Chunk0904F019
    {
        public override void ReadWrite(CPlugTree n, GbxReaderWriter rw)
        {
            n.Flags = rw.DataUInt32((uint)n.Flags);

            if ((n.Flags & 4) != 0)
            {
                n.Location = rw.Iso4(n.Location);
            }

            n.Flags |= 0x2800;
            // Flags = Flags | 0x8000; (if Bucr3IsR)
        }
    }

    public partial class Chunk0904F01A
    {
        public override void ReadWrite(CPlugTree n, GbxReaderWriter rw)
        {
            n.Flags = rw.DataUInt32((uint)n.Flags);

            if ((n.Flags & 4) != 0)
            {
                n.Location = rw.Iso4(n.Location);
            }

            n.Flags |= 0x2000;
            // Flags = Flags | 0x8000; (if Bucr3IsR)
        }
    }
}
