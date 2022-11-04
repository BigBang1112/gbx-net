using GBX.NET.Utils;
using System.Text;

namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090BB000</remarks>
[Node(0x090BB000)]
[NodeExtension("Mesh")]
[NodeExtension("Solid2")]
public class CPlugSolid2Model : CMwNod
{
    private ShadedGeom[] shadedGeoms = Array.Empty<ShadedGeom>();
    private CPlugVisual?[] visuals = Array.Empty<CPlugVisual>();
    private ExternalNode<CPlugMaterial>[] materials = Array.Empty<ExternalNode<CPlugMaterial>>();
    private Light[] lights = Array.Empty<Light>();
    private CPlugMaterialUserInst?[] materialUserInsts = Array.Empty<CPlugMaterialUserInst>();
    private CPlugLightUserModel?[] lightUserModels = Array.Empty<CPlugLightUserModel>();
    private LightInst[] lightInsts = Array.Empty<LightInst>();
    private PreLightGen? preLightGenerator;

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk090BB000))]
    public ShadedGeom[] ShadedGeoms { get => shadedGeoms; set => shadedGeoms = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk090BB000), sinceVersion: 6)]
    public CPlugVisual?[] Visuals { get => visuals; set => visuals = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk090BB000))]
    public ExternalNode<CPlugMaterial>[] Materials { get => materials; private set => materials = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk090BB000), sinceVersion: 8)]
    public Light[] Lights { get => lights; set => lights = value; }
    
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk090BB000))]
    public CPlugMaterialUserInst?[] MaterialUserInsts { get => materialUserInsts; set => materialUserInsts = value; }
    
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk090BB000), sinceVersion: 10)]
    public CPlugLightUserModel?[] LightUserModels { get => lightUserModels; set => lightUserModels = value; }
    
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk090BB000), sinceVersion: 10)]
    public LightInst[] LightInsts { get => lightInsts; set => lightInsts = value; }

    [NodeMember]
    [AppliedWithChunk(typeof(Chunk090BB000), sinceVersion: 3)]
    public PreLightGen? PreLightGenerator { get => preLightGenerator; set => preLightGenerator = value; }

    internal CPlugSolid2Model()
    {

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
        using var exporter = new ObjFileExporter(
            objStream,
            mtlStream,
            mergeVerticesDigitThreshold: null,
            gameDataFolderPath,
            encoding,
            leaveOpen,
            corruptedMaterials);

        exporter.Export(this);
    }

    #region Chunks

    #region 0x000 chunk

    /// <summary>
    /// CPlugSolid2Model 0x000 chunk
    /// </summary>
    [Chunk(0x090BB000)]
    public class Chunk090BB000 : Chunk<CPlugSolid2Model>, IVersionable
    {
        private int version;

        public string? U01;
        public object[]? ShadedGeoms;
        public string[]? U02;
        public Node? U03;
        public float[]? U04;
        public int U05;
        public ulong U06;
        public string? U07;
        public string? U08;
        public string? U09;
        public int U10;
        public uint U11;
        public int U12;
        public string? U13;
        public Box[]? U14;
        public string[]? U15;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugSolid2Model n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.Id(ref U01);
            rw.ArrayArchive<ShadedGeom>(ref n.shadedGeoms!, version);

            if (version >= 6)
            {
                rw.Int32(10); // listVersion
                rw.ArrayNode<CPlugVisual>(ref n.visuals!);
            }

            rw.ArrayId(ref U02);

            rw.Int32(10); // listVersion
            rw.ArrayNode<CPlugMaterial>(ref n.materials!);

            rw.NodeRef(ref U03);

            if (version >= 1)
            {
                rw.Array<float>(ref U04);

                if (version >= 2)
                {
                    rw.Int32(ref U05);

                    if (version >= 3)
                    {
                        if (rw.Boolean(n.preLightGenerator is not null))
                        {
                            // SPlugSolidPreLightGen::Archive
                            rw.Archive<PreLightGen>(ref n.preLightGenerator);
                        }

                        if (version >= 4)
                        {
                            rw.UInt64(ref U06);

                            if (version >= 5)
                            {
                                rw.String(ref U07);

                                if (version >= 7)
                                {
                                    rw.String(ref U08);

                                    if (version >= 19)
                                    {
                                        rw.String(ref U09);
                                    }

                                    if (version >= 8)
                                    {
                                        rw.ArrayArchive<Light>(ref n.lights!);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (version >= 8 && version < 16)
            {
                rw.ArrayNode<CPlugMaterialUserInst>(ref n.materialUserInsts!);
            }

            if (version >= 10)
            {
                rw.ArrayNode<CPlugLightUserModel>(ref n.lightUserModels!);
                rw.ArrayArchive<LightInst>(ref n.lightInsts!);

                if (version >= 11)
                {
                    rw.Int32(ref U10);

                    if (version >= 12)
                    {
                        rw.UInt32(ref U11); // DoData

                        if (version >= 13)
                        {
                            rw.Int32(ref U12);

                            if (version >= 14)
                            {
                                rw.String(ref U13);

                                if (version >= 15)
                                {
                                    rw.Array(ref n.materialUserInsts!, r =>
                                    {
                                        var name = r.ReadString();

                                        if (name.Length > 0)  // If the material file exists (name != ""), it references the file instead
                                        {
                                            return null;
                                        }

                                        var material = r.ReadNodeRef<CPlugMaterialUserInst>();

                                        return material;
                                    }, (x, w) =>
                                    {
                                        w.Write("");

                                        if (x is null)
                                        {
                                            throw new NotSupportedException();
                                        }

                                        w.Write(x);
                                    });

                                    if (version >= 17)
                                    {
                                        rw.Array<Box>(ref U14);

                                        if (version >= 20)
                                        {
                                            rw.ArrayId(ref U15);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion

    #endregion

    public class ShadedGeom : IReadableWritable
    {
        private int u01;
        private int u02;
        private int u03;
        private int? u04;

        public int U01 { get => u01; set => u01 = value; }
        public int U02 { get => u02; set => u02 = value; }
        public int U03 { get => u03; set => u03 = value; }
        public int? U04 { get => u04; set => u04 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
            rw.Int32(ref u03);

            if (version >= 1)
            {
                rw.Int32(ref u04);
            }
        }
    }

    public class Light : IReadableWritable
    {
        private string u01 = "";
        private bool u02 = true;
        private Node? u03;
        private string? u04;
        private float u05;
        private float u06;
        private float u07;
        private float u08;
        private float u09;
        private float u10;
        private float u11;
        private float u12;
        private float u13;
        private float u14;
        private float u15;
        private float u16;
        private bool u17;
        private bool u18;
        private bool u19;
        private bool u20;
        private float? u21;
        private float? u22;
        private float? u23;

        public string U01 { get => u01; set => u01 = value; }
        public bool U02 { get => u02; set => u02 = value; }
        public Node? U03 { get => u03; set => u03 = value; }
        public string? U04 { get => u04; set => u04 = value; }
        public float U05 { get => u05; set => u05 = value; }
        public float U06 { get => u06; set => u06 = value; }
        public float U07 { get => u07; set => u07 = value; }
        public float U08 { get => u08; set => u08 = value; }
        public float U09 { get => u09; set => u09 = value; }
        public float U10 { get => u10; set => u10 = value; }
        public float U11 { get => u11; set => u11 = value; }
        public float U12 { get => u12; set => u12 = value; }
        public float U13 { get => u13; set => u13 = value; }
        public float U14 { get => u14; set => u14 = value; }
        public float U15 { get => u15; set => u15 = value; }
        public float U16 { get => u16; set => u16 = value; }
        public bool U17 { get => u17; set => u17 = value; }
        public bool U18 { get => u18; set => u18 = value; }
        public bool U19 { get => u19; set => u19 = value; }
        public bool U20 { get => u20; set => u20 = value; }
        public float? U21 { get => u21; set => u21 = value; }
        public float? U22 { get => u22; set => u22 = value; }
        public float? U23 { get => u23; set => u23 = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Id(ref u01!);
            rw.Boolean(ref u02); // defaults to true

            if (u02)
            {
                rw.NodeRef(ref u03);
            }
            else
            {
                rw.String(ref u04);
            }

            rw.Single(ref u05);
            rw.Single(ref u06);
            rw.Single(ref u07);
            rw.Single(ref u08);
            rw.Single(ref u09);
            rw.Single(ref u10);
            rw.Single(ref u11);
            rw.Single(ref u12);
            rw.Single(ref u13);
            rw.Single(ref u14);
            rw.Single(ref u15);
            rw.Single(ref u16);
            rw.Boolean(ref u17);
            rw.Boolean(ref u18);
            rw.Boolean(ref u19);
            rw.Boolean(ref u20);

            if (u20)
            {
                rw.Single(ref u21);
                rw.Single(ref u22);
                rw.Single(ref u23);
            }
        }
    }

    public class LightInst : IReadableWritable
    {
        private int u01;
        private int u02;
        
        public int U01 { get => u01; set => u01 = value; }
        public int U02 { get => u02; set => u02 = value; }
        
        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref u01);
            rw.Int32(ref u02);
        }
    }

    public class PreLightGen : IReadableWritable
    {
        private int v;
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
        private Box[] u14 = Array.Empty<Box>();
        private (int, int, int, int)[]? uvGroups;

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
        public Box[] U14 { get => u14; set => u14 = value; }
        public (int, int, int, int)[]? UvGroups { get => uvGroups; set => uvGroups = value; }

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
            rw.Int32(ref u12);
            rw.Int32(ref u13);
            rw.Array<Box>(ref u14!);

            if (v >= 1)
            {
                rw.Array(ref uvGroups);
            }
        }
    }
}
