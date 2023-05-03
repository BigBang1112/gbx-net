using GBX.NET.Utils;
using System.Text;

namespace GBX.NET.Engines.Plug;

/// <summary>
/// An official mesh or model.
/// </summary>
/// <remarks>ID: 0x09005000</remarks>
[Node(0x09005000)]
[NodeExtension("Solid")]
public class CPlugSolid : CPlug
{
    private int typeAndIndex;
    private CPlug? tree;
    private GameBoxRefTable.File? treeFile;
    private PreLightGen? solidPreLightGen;
    private DateTime? fileWriteTime;

    [NodeMember(ExactlyNamed = true)]
    public int TypeAndIndex { get => typeAndIndex; set => typeAndIndex = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk0900500D>]
    [AppliedWithChunk<Chunk09005011>]
    public CPlug? Tree
    {
        get => tree = GetNodeFromRefTable(tree, treeFile) as CPlug;
        set => tree = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09005017>]
    public PreLightGen? SolidPreLightGen { get => solidPreLightGen; set => solidPreLightGen = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09005017>]
    public DateTime? FileWriteTime { get => fileWriteTime; set => fileWriteTime = value; }

    internal CPlugSolid()
    {

    }

    /// <summary>
    /// Exports the solid to .obj file.
    /// </summary>
    /// <param name="fileNameWithoutExtension">File name to write OBJ and MTL content into (separately). The files will be automatically suffixed with ".obj" and ".mtl".</param>
    /// <param name="gameDataFolderPath">Folder for the Material.Gbx, Texture.Gbx, and .dds lookup.</param>
    /// <param name="encoding">Encoding to use.</param>
    /// <param name="leaveOpen">If to keep the streams open.</param>
    /// <param name="corruptedMaterials">If to use a different way to handle corrupted material files (via header reference table, to avoid body parse). Exists due to TMTurbo problems. Can give much less accurate results.</param>
    public void ExportToObj(string fileNameWithoutExtension,
                            string? gameDataFolderPath = null,
                            Encoding? encoding = null,
                            bool leaveOpen = false,
                            bool corruptedMaterials = false)
    {
        using var objStream = File.Create(fileNameWithoutExtension + ".obj");
        using var mtlStream = File.Create(fileNameWithoutExtension + ".mtl");
        ExportToObj(objStream, mtlStream, gameDataFolderPath, encoding, leaveOpen, corruptedMaterials);
    }

    /// <summary>
    /// Exports the solid to .obj file.
    /// </summary>
    /// <param name="objStream">Stream to write OBJ content into.</param>
    /// <param name="mtlStream">Stream to write MTL content into.</param>
    /// <param name="gameDataFolderPath">Folder for the Material.Gbx, Texture.Gbx, and .dds lookup.</param>
    /// <param name="encoding">Encoding to use.</param>
    /// <param name="leaveOpen">If to keep the streams open.</param>
    /// <param name="corruptedMaterials">If to use a different way to handle corrupted material files (via header reference table, to avoid body parse). Exists due to TMTurbo problems. Can give much less accurate results.</param>
    public void ExportToObj(Stream objStream, 
                            Stream mtlStream, 
                            string? gameDataFolderPath = null,
                            Encoding? encoding = null,
                            bool leaveOpen = false,
                            bool corruptedMaterials = false)
    {
        if (Tree is not CPlugTree tree)
        {
            return;
        }

        using var exporter = new ObjFileExporter(
            objStream,
            mtlStream,
            mergeVerticesDigitThreshold: null,
            gameDataFolderPath,
            encoding,
            leaveOpen,
            corruptedMaterials);
        
        exporter.Export(tree, this);
    }

    /// <summary>
    /// CPlugSolid 0x000 chunk
    /// </summary>
    [Chunk(0x09005000)]
    public class Chunk09005000 : Chunk<CPlugSolid>
    {
        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref n.typeAndIndex);
        }
    }

    /// <summary>
    /// CPlugSolid 0x006 chunk
    /// </summary>
    [Chunk(0x09005006)]
    public class Chunk09005006 : Chunk<CPlugSolid>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;

        public Mat3 U07;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);

            rw.Mat3(ref U07);
        }
    }

    /// <summary>
    /// CPlugSolid 0x007 chunk
    /// </summary>
    [Chunk(0x09005007)]
    public class Chunk09005007 : Chunk<CPlugSolid>
    {
        public bool U01;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
        }
    }

    /// <summary>
    /// CPlugSolid 0x00B chunk
    /// </summary>
    [Chunk(0x0900500B)]
    public class Chunk0900500B : Chunk<CPlugSolid>
    {
        public bool U01;
        public bool U02;
        public bool U03;
        public bool U04;
        public bool U05;
        public bool U06;
        public int U07;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.Boolean(ref U03);
            rw.Boolean(ref U04);
            rw.Boolean(ref U05);
            rw.Boolean(ref U06);
            rw.Int32(ref U07);
        }
    }

    /// <summary>
    /// CPlugSolid 0x00C chunk
    /// </summary>
    [Chunk(0x0900500C)]
    public class Chunk0900500C : Chunk<CPlugSolid>
    {
        public bool U01;
        public bool U02;
        public bool U03;
        public bool U04;
        public bool U05;
        public bool U06;
        public bool U07;
        public bool U08;
        public bool U09;
        public bool U10;
        public float U11;
        public int U12;
        public float U13;
        public float U14;
        public float U15;
        public float U16;
        public int U17;
        public int U18;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.Boolean(ref U03);
            rw.Boolean(ref U04);
            rw.Boolean(ref U05);
            rw.Boolean(ref U06);
            rw.Boolean(ref U07);
            rw.Boolean(ref U08);
            rw.Boolean(ref U09);
            rw.Boolean(ref U10);
            rw.Single(ref U11);
            rw.Int32(ref U12);
            rw.Single(ref U13);
            rw.Single(ref U14);
            rw.Single(ref U15);
            rw.Single(ref U16);
            rw.Int32(ref U17);
            rw.Int32(ref U18);
        }
    }

    /// <summary>
    /// CPlugSolid 0x00D chunk
    /// </summary>
    [Chunk(0x0900500D)]
    public class Chunk0900500D : Chunk<CPlugSolid>
    {
        public bool U01;
        public bool U02;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            rw.NodeRef<CPlug>(ref n.tree, ref n.treeFile);
        }

        public override async Task ReadWriteAsync(CPlugSolid n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);
            n.tree = await rw.NodeRefAsync<CPlug>(n.tree, cancellationToken);
        }
    }

    /// <summary>
    /// CPlugSolid 0x00E chunk
    /// </summary>
    [Chunk(0x0900500E)]
    public class Chunk0900500E : Chunk<CPlugSolid>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public Iso4 U05;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01); // probably related to CPlugPhysicalObject::SetComPos
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);

            rw.Iso4(ref U05);
        }
    }

    /// <summary>
    /// CPlugSolid 0x00F chunk
    /// </summary>
    [Chunk(0x0900500F)]
    public class Chunk0900500F : Chunk<CPlugSolid>
    {
        public float U01;
        public float U02;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
        }
    }

    /// <summary>
    /// CPlugSolid 0x010 chunk
    /// </summary>
    [Chunk(0x09005010)]
    public class Chunk09005010 : Chunk<CPlugSolid>
    {
        public CMwNod? U01;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01); // CSceneVehicleEnvironment
        }
    }

    /// <summary>
    /// CPlugSolid 0x011 chunk
    /// </summary>
    [Chunk(0x09005011)]
    public class Chunk09005011 : Chunk<CPlugSolid>
    {
        public bool U01;
        public bool U02;
        public bool? U03;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);

            if (U02) // True when referenced through CHmsItem?
            {
                rw.Boolean(ref U03);
            }

            rw.NodeRef<CPlug>(ref n.tree, ref n.treeFile); // only of U02 is false?
        }

        public override async Task ReadWriteAsync(CPlugSolid n, GameBoxReaderWriter rw, CancellationToken cancellationToken = default)
        {
            rw.Boolean(ref U01);
            rw.Boolean(ref U02);

            if (U02) // True when referenced through CHmsItem?
            {
                rw.Boolean(ref U03);
            }
            
            n.tree = await rw.NodeRefAsync<CPlug>(n.tree, cancellationToken); // only of U02 is false?
        }
    }

    /// <summary>
    /// CPlugSolid 0x012 chunk
    /// </summary>
    [Chunk(0x09005012)]
    public class Chunk09005012 : Chunk<CPlugSolid>
    {
        public byte U01;

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Byte(ref U01); // relates ti SolidPreLightGen
        }
    }

    #region 0x017 chunk (SolidPreLightGen)

    /// <summary>
    /// CPlugSolid 0x017 chunk (SolidPreLightGen)
    /// </summary>
    [Chunk(0x09005017, "SolidPreLightGen")]
    public class Chunk09005017 : Chunk<CPlugSolid>, IVersionable
    {
        private int version = 3;
        
        public byte U01;
        public float U02;
        public bool U03;
        public Rect U04;
        public Rect U05;
        public Int2 U06;
        public Box U07;
        public bool U09;
        public PreLightGen? U10;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);

            if (version >= 3)
            {
                rw.Boolean(ref U09);

                if (U09)
                {
                    rw.Archive<PreLightGen>(ref n.solidPreLightGen);
                }
            }
            else
            {
                rw.Byte(ref U01);
                rw.Single(ref U02);
                rw.Boolean(ref U03);
                rw.Rect(ref U04);
                rw.Rect(ref U05);
                rw.Int2(ref U06);  // sprite count def

                if (version >= 1)
                {
                    rw.Box(ref U07);
                }
            }

            if (version >= 2)
            {
                rw.FileTime(ref n.fileWriteTime);
            }
        }
    }

    #endregion

    #region 0x019 chunk

    /// <summary>
    /// CPlugSolid 0x019 chunk
    /// </summary>
    [Chunk(0x09005019)]
    public class Chunk09005019 : Chunk<CPlugSolid>, IVersionable
    {
        private int version;
        private int listVersion1 = 10;
        private int listVersion2 = 10;

        public CPlugSound?[]? U01;
        public CPlugParticleEmitterModel?[]? U02;
        public LocatedInstance[]? U03;
        public LocatedInstance[]? U04;
        public int U05;
        public string[]? U06;
        public Iso4[]? U07;
        public string? U08;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugSolid n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Int32(ref listVersion1);
            rw.ArrayNode<CPlugSound>(ref U01);
            rw.Int32(ref listVersion2);
            rw.ArrayNode<CPlugParticleEmitterModel>(ref U02);
            rw.ArrayArchive<LocatedInstance>(ref U03);
            rw.ArrayArchive<LocatedInstance>(ref U04);
            
            if (version >= 1)
            {
                rw.Int32(ref U05);

                if (version >= 2)
                {
                    rw.ArrayId(ref U06);
                    rw.Array<Iso4>(ref U07);
                    
                    if (version >= 3)
                    {
                        rw.String(ref U08);
                    }
                }
            }
        }
    }

    #endregion

    #region 0x01A chunk

    /// <summary>
    /// CPlugSolid 0x01A chunk (lod normal map)
    /// </summary>
    [Chunk(0x0900501A, "lod normal map"), IgnoreChunk]
    public class Chunk0900501A : SkippableChunk<CPlugSolid>
    {
        
    }

    #endregion

    public class PreLightGen : IReadableWritable
    {
        private int v = 1;
        
        private int u01;
        private float u02;
        private bool u03;
        private float u04;
        private float u05;
        private float u06;
        private float u07;
        private float u08;
        private float u09;
        private float u10;
        private float u11;
        private int u12;
        private int u13;
        private Box[]? u14;
        private UvGroup[]? u15;

        public int Version { get => v; set => v = value; }
        
        public int U01 { get => u01; set => u01 = value; }
        public float U02 { get => u02; set => u02 = value; }
        public bool U03 { get => u03; set => u03 = value; }
        public float U04 { get => u04; set => u04 = value; }
        public float U05 { get => u05; set => u05 = value; }
        public float U06 { get => u06; set => u06 = value; }
        public float U07 { get => u07; set => u07 = value; }
        public float U08 { get => u08; set => u08 = value; }
        public float U09 { get => u09; set => u09 = value; }
        public float U10 { get => u10; set => u10 = value; }
        public float U11 { get => u11; set => u11 = value; }
        public int U12 { get => u12; set => u12 = value; }
        public int U13 { get => u13; set => u13 = value; }
        public Box[]? U14 { get => u14; set => u14 = value; }
        public UvGroup[]? U15 { get => u15; set => u15 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref v);
            rw.Int32(ref u01);
            rw.Single(ref u02);
            rw.Boolean(ref u03);
            rw.Single(ref u04);
            rw.Single(ref u05);
            rw.Single(ref u06);
            rw.Single(ref u07);
            rw.Single(ref u08);
            rw.Single(ref u09);
            rw.Single(ref u10);
            rw.Single(ref u11);
            rw.Int32(ref u12); // sprite count def
            rw.Int32(ref u13);
            rw.Array<Box>(ref u14);
            
            if (v >= 1)
            {
                rw.Array<UvGroup>(ref u15);
            }
        }

        public readonly record struct UvGroup(float U01, float U02, float U03, float U04, float U05);
    }

    public class LocatedInstance : IReadableWritable
    {
        private int u01;
        private Iso4 u02;

        public int U01 { get => u01; set => u01 = value; }
        public Iso4 U02 { get => u02; set => u02 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref u01);
            rw.Iso4(ref u02);
        }
    }
}
