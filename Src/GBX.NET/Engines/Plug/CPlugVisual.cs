namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09006000</remarks>
[Node(0x09006000)]
[NodeExtension("Visual")]
[WritingNotSupported]
public abstract class CPlugVisual : CPlug
{
    [Flags]
    public enum VisualFlags
    {
        SkinFlag1 = (1 << 0),
        SkinFlag2 = (1 << 1),
        SkinFlag3 = (1 << 2),
        IsGeometryStatic = (1 << 3),
        IsIndexationStatic = (1 << 5),
        HasVertexNormals = (1 << 7),
        UnknownFlag8 = (1 << 8),
        UnknownFlag20 = (1 << 20),
        UnknownFlag21 = (1 << 21),
        UnknownFlag22 = (1 << 22)
    }

    private VisualFlags flags;
    private int count;
    private Vec2[][]? texCoords;
    private Int3[]? indices;

    public VisualFlags Flags
    {
        get => flags;
        set => flags = value;
    }

    public int Count
    {
        get => count;
        set => count = value;
    }

    public Vec2[][]? TexCoords
    {
        get => texCoords;
        set => texCoords = value;
    }

    internal CPlugVisual()
    {
    }

    void SetFlag(VisualFlags f, bool enable)
    {
        if (enable)
        {
            flags |= f;
        }
        else
        {
            flags &= ~f;
        }
    }

    /// <summary>
    /// CPlugVisual 0x001 chunk
    /// </summary>
    [Chunk(0x09006001)]
    public class Chunk09006001 : Chunk<CPlugVisual>
    {
        public string U01;

        public Chunk09006001()
        {
            U01 = "";
        }

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01!);
        }
    }

    /// <summary>
    /// CPlugVisual 0x004 chunk
    /// </summary>
    [Chunk(0x09006004)]
    public class Chunk09006004 : Chunk<CPlugVisual>
    {
        public CMwNod? U01;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01); // sometimes not present
        }
    }

    /// <summary>
    /// CPlugVisual 0x005 chunk
    /// </summary>
    [Chunk(0x09006005)]
    public class Chunk09006005 : Chunk<CPlugVisual>
    {
        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.Array<Int3>(ref n.indices);
        }
    }

    /// <summary>
    /// CPlugVisual 0x006 chunk
    /// </summary>
    [Chunk(0x09006006)]
    public class Chunk09006006 : Chunk<CPlugVisual>
    {
        public bool U01;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            n.SetFlag(VisualFlags.HasVertexNormals, U01);
        }
    }

    /// <summary>
    /// CPlugVisual 0x007 chunk
    /// </summary>
    [Chunk(0x09006007)]
    public class Chunk09006007 : Chunk<CPlugVisual>
    {
        public bool U01;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CPlugVisual 0x008 chunk
    /// </summary>
    [Chunk(0x09006008)]
    public class Chunk09006008 : Chunk<CPlugVisual>
    {
        public int NumTexCoordSets;
        public int SkinFlags;

        public override void Read(CPlugVisual n, GameBoxReader r)
        {
            n.SetFlag(VisualFlags.IsGeometryStatic, r.ReadBoolean());
            n.SetFlag(VisualFlags.IsIndexationStatic, r.ReadBoolean());
            NumTexCoordSets = r.ReadInt32(); 
            SkinFlags = r.ReadInt32();
            n.SetFlag(VisualFlags.SkinFlag1, (SkinFlags & 1) != 0);
            n.SetFlag(VisualFlags.SkinFlag2, (SkinFlags & 2) != 0);
            n.SetFlag(VisualFlags.SkinFlag3, (SkinFlags & 4) != 0);
            n.count = r.ReadInt32();

            n.texCoords = r.ReadArray(NumTexCoordSets, r =>
            {
                var version = r.ReadInt32();

                return r.ReadArray<Vec2>(n.count, r =>
                {
                    var uv = r.ReadVec2();

                    if (version >= 1)
                    {
                        var u01 = r.ReadInt32();

                        if (version >= 2)
                        {
                            var u02 = r.ReadInt32();
                        }
                    }

                    return uv;
                });
            });

            if (SkinFlags != 0)
            {
                throw new NotImplementedException();
            }
            n.SetFlag(VisualFlags.UnknownFlag8, r.ReadBoolean());
        }
    }

    /// <summary>
    /// CPlugVisual 0x009 chunk
    /// </summary>
    [Chunk(0x09006009)]
    public class Chunk09006009 : Chunk<CPlugVisual>
    {
        public float U01;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
        }
    }

    /// <summary>
    /// CPlugVisual 0x00A chunk
    /// </summary>
    [Chunk(0x0900600A)]
    public class Chunk0900600A : Chunk<CPlugVisual>
    {
        public int NumTexCoordSets;
        public int SkinFlags;

        public override void Read(CPlugVisual n, GameBoxReader r)
        {
            n.SetFlag(VisualFlags.IsGeometryStatic, r.ReadBoolean());
            n.SetFlag(VisualFlags.IsIndexationStatic, r.ReadBoolean());
            NumTexCoordSets = r.ReadInt32(); 
            SkinFlags = r.ReadInt32();
            n.SetFlag(VisualFlags.SkinFlag1, (SkinFlags & 1) != 0);
            n.SetFlag(VisualFlags.SkinFlag2, (SkinFlags & 2) != 0);
            n.SetFlag(VisualFlags.SkinFlag3, (SkinFlags & 4) != 0);
            n.count = r.ReadInt32();
            
            // vertex streams
            var vertStreams = r.ReadArray(r => r.ReadNodeRef());

            n.texCoords = r.ReadArray(NumTexCoordSets, r =>
            {
                var version = r.ReadInt32();

                return r.ReadArray<Vec2>(n.count, r =>
                {
                    var uv = r.ReadVec2();

                    if (version >= 1)
                    {
                        var u01 = r.ReadInt32();

                        if (version >= 2)
                        {
                            var u02 = r.ReadInt32();
                        }
                    }

                    return uv;
                });
            });

            if (SkinFlags != 0)
            {
                throw new NotImplementedException();
            }

            n.SetFlag(VisualFlags.UnknownFlag8, r.ReadBoolean());
            // apparently one more boolean in TMS
            r.ReadBoolean();
        }
    }

    /// <summary>
    /// CPlugVisual 0x00B chunk
    /// </summary>
    [Chunk(0x0900600B)]
    public class Chunk0900600B : Chunk<CPlugVisual>
    {
        public object[]? U01;

        public override void ReadWrite(CPlugVisual n, GameBoxReaderWriter rw)
        {
            // array of SSplits
            U01 = rw.Array<object>(null, (i, r) => new
            {
                x = r.ReadInt32(),
                y = r.ReadInt32(),
                box = r.ReadBox() // GmBoxAligned::ArchiveABox
            }, (x, w) => { });
        }
    }

    [Chunk(0x0900600C)]
    public class Chunk0900600C : Chunk<CPlugVisual>
    {
        public Box U01;
        
        public override void Read(CPlugVisual n, GameBoxReader r)
        {
            n.SetFlag(VisualFlags.IsGeometryStatic, r.ReadBoolean());
            n.SetFlag(VisualFlags.IsIndexationStatic, r.ReadBoolean());

            var numTexCoordSets = r.ReadInt32();

            var skinFlags = r.ReadInt32();
            n.SetFlag(VisualFlags.SkinFlag1, (skinFlags & 1) != 0);
            n.SetFlag(VisualFlags.SkinFlag2, (skinFlags & 2) != 0);
            n.SetFlag(VisualFlags.SkinFlag3, (skinFlags & 4) != 0);

            n.count = r.ReadInt32();

            var vertStreams = r.ReadArray(r => r.ReadNodeRef()); // vertex streams

            n.texCoords = r.ReadArray(numTexCoordSets, r =>
            {
                var version = r.ReadInt32();

                return r.ReadArray<Vec2>(n.count, r =>
                {
                    var uv = r.ReadVec2();

                    if (version >= 1)
                    {
                        var u01 = r.ReadInt32();

                        if (version >= 2)
                        {
                            var u02 = r.ReadInt32();
                        }
                    }

                    return uv;
                });
            });

            n.SetFlag(VisualFlags.UnknownFlag8, r.ReadBoolean());

            if (n.flags.HasFlag(VisualFlags.SkinFlag1) || n.flags.HasFlag(VisualFlags.SkinFlag2) || n.flags.HasFlag(VisualFlags.SkinFlag3))
            {
                throw new NotImplementedException();
            }

            U01 = r.ReadBox(); // ArchiveABox
        }
    }

    void ConvertChunkFlagsToFlags(int chunkFlags)
    {
        flags = 0;
        flags |= (VisualFlags)(chunkFlags & 15);
        flags |= (VisualFlags)((chunkFlags << 1) & 0x20);
        flags |= (VisualFlags)((chunkFlags << 2) & 0x80);
        flags |= (VisualFlags)((chunkFlags << 2) & 0x100);
        flags |= (VisualFlags)((chunkFlags << 13) & 0x100000);
        flags |= (VisualFlags)((chunkFlags << 13) & 0x200000);
        flags |= (VisualFlags)((chunkFlags << 13) & 0x400000);
    }

    /// <summary>
    /// CPlugVisual 0x00D chunk
    /// </summary>
    [Chunk(0x0900600D)]
    public class Chunk0900600D : Chunk<CPlugVisual>
    {
        public int chunkFlags;
        public Box U04;

        public override void Read(CPlugVisual n, GameBoxReader r)
        {
            n.ConvertChunkFlagsToFlags(r.ReadInt32());
            var numTexCoordSets = r.ReadInt32();
            n.count = r.ReadInt32();
            r.ReadArray(r => r.ReadNodeRef());
            
            n.texCoords = r.ReadArray(numTexCoordSets, r =>
            {
                var version = r.ReadInt32();

                return r.ReadArray<Vec2>(n.count, r =>
                {
                    var uv = r.ReadVec2();

                    if (version >= 1)
                    {
                        var u01 = r.ReadInt32();

                        if (version >= 2)
                        {
                            var u02 = r.ReadInt32();
                        }
                    }

                    return uv;
                });
            });
            
            if (n.flags.HasFlag(VisualFlags.SkinFlag1) || n.flags.HasFlag(VisualFlags.SkinFlag2) || n.flags.HasFlag(VisualFlags.SkinFlag3))
            {
                throw new NotImplementedException();
            }
            
            U04 = r.ReadBox(); // ArchiveABox
        }
    }

    /// <summary>
    /// CPlugVisual 0x00E chunk
    /// </summary>
    [Chunk(0x0900600E)]
    public class Chunk0900600E : Chunk<CPlugVisual>
    {
        public int NumTexCoordSets;
        public Box U04;

        public override void Read(CPlugVisual n, GameBoxReader r)
        {
            n.ConvertChunkFlagsToFlags(r.ReadInt32());
            NumTexCoordSets = r.ReadInt32();
            n.count = r.ReadInt32();
            r.ReadArray(r => r.ReadNodeRef());
            
            n.texCoords = r.ReadArray(NumTexCoordSets, r =>
            {
                var version = r.ReadInt32();

                return r.ReadArray<Vec2>(n.count, r =>
                {
                    var uv = r.ReadVec2();

                    if (version >= 1)
                    {
                        var u01 = r.ReadInt32();

                        if (version >= 2)
                        {
                            var u02 = r.ReadInt32();
                        }
                    }

                    return uv;
                });
            });
            
            if (n.flags.HasFlag(VisualFlags.SkinFlag1) || n.flags.HasFlag(VisualFlags.SkinFlag2) || n.flags.HasFlag(VisualFlags.SkinFlag3))
            {
                throw new NotImplementedException();
            }
            
            U04 = r.ReadBox(); // ArchiveABox
            
            r.ReadArray(r => r.ReadBytes(20));
        }
    }
}