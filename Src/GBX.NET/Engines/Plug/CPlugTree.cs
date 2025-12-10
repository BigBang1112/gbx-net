using GBX.NET.Components;

using static GBX.NET.BitHelper;

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
        get => GetBit(Flags, 0x3);
        set => Flags = SetBit(Flags, 0x3, value);
    }

    public bool IsCollidable
    {
        get => GetBit(Flags, 0x7);
        set => Flags = SetBit(Flags, 0x7, value);
    }

    public bool IsRooted
    {
        get => GetBit(Flags, 0xF);
        set => Flags = SetBit(Flags, 0xF, value);
    }

    public bool IsLightVolume
    {
        get => GetBit(Flags, 0x9);
        set => Flags = SetBit(Flags, 0x9, value);
    }

    public bool IsLightVolumeVisible
    {
        get => GetBit(Flags, 0xA);
        set => Flags = SetBit(Flags, 0xA, value);
    }

    public bool UseLocation
    {
        get => GetBit(Flags, 0x2);
        set => Flags = SetBit(Flags, 0x2, value);
    }

    public bool IsShadowCaster
    {
        get => GetBit(Flags, 0xE);
        set => Flags = SetBit(Flags, 0xE, value);
    }

    public bool IsFixedRatio2D
    {
        get => GetBit(Flags, 0x8);
        set => Flags = SetBit(Flags, 0x8, value);
    }

    public bool IsPickable
    {
        get => GetBit(Flags, 0x6);
        set => Flags = SetBit(Flags, 0x6, value);
    }

    public bool IsPickableVisual
    {
        get => GetBit(Flags, 0xB);
        set => Flags = SetBit(Flags, 0xB, value);
    }

    public bool IsPortal
    {
        get => GetBit(Flags, 0x0);
        set => Flags = SetBit(Flags, 0x0, value);
    }

    public bool TestBBoxVisibility
    {
        get => GetBit(Flags, 0xD);
        set => Flags = SetBit(Flags, 0xD, value);
    }

    public bool UseRenderBefore
    {
        get => GetBit(Flags, 0xC);
        set => Flags = SetBit(Flags, 0xC, value);
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

            if (n.UseLocation)
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

            if (n.UseLocation)
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

            if (n.UseLocation)
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

            if (n.UseLocation)
            {
                n.Location = rw.Iso4(n.Location);
            }

            n.Flags |= 0x2000;
            // Flags = Flags | 0x8000; (if Bucr3IsR)
        }
    }
}
