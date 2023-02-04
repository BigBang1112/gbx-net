using GBX.NET.Utils;
using System.Text;

namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x090BB000</remarks>
[Node(0x090BB000)]
[NodeExtension("Mesh")]
[NodeExtension("Solid2")]
public partial class CPlugSolid2Model : CMwNod
{
    private ShadedGeom[] shadedGeoms = Array.Empty<ShadedGeom>();
    private CPlugVisual?[] visuals = Array.Empty<CPlugVisual>();
    private string[] materialIds = Array.Empty<string>();
    private ExternalNode<CPlugMaterial>[] materials = Array.Empty<ExternalNode<CPlugMaterial>>();
    private CPlugSkel? skel;
    private GameBoxRefTable.File? skelFile;
    private int visCstType;
    private PreLightGen? preLightGenerator;
    private DateTime? fileWriteTime;
    private string? materialsFolderName;
    private Light[] lights = Array.Empty<Light>();
    private CPlugMaterialUserInst?[] materialInsts = Array.Empty<CPlugMaterialUserInst>();
    private CPlugLightUserModel?[] lightUserModels = Array.Empty<CPlugLightUserModel>();
    private LightInst[] lightInsts = Array.Empty<LightInst>();
    private int damageZone;
    private uint flags;
    private Material[] customMaterials = Array.Empty<Material>();

    [NodeMember]
    [AppliedWithChunk<Chunk090BB000>]
    public ShadedGeom[] ShadedGeoms { get => shadedGeoms; set => shadedGeoms = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090BB000>(sinceVersion: 6)]
    public CPlugVisual?[] Visuals { get => visuals; set => visuals = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090BB000>]
    public string[] MaterialIds { get => materialIds; set => materialIds = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090BB000>]
    public ExternalNode<CPlugMaterial>[] Materials { get => materials; private set => materials = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090BB000>]
    public CPlugSkel? Skel
    {
        get => skel = GetNodeFromRefTable(skel, skelFile) as CPlugSkel;
        set => skel = value;
    }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090BB000>(sinceVersion: 2)]
    public int VisCstType { get => visCstType; set => visCstType = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090BB000>(sinceVersion: 3)]
    public PreLightGen? PreLightGenerator { get => preLightGenerator; set => preLightGenerator = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090BB000>(sinceVersion: 4)]
    public DateTime? FileWriteTime { get => fileWriteTime; set => fileWriteTime = value; }

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090BB000>(sinceVersion: 7)]
    public string? MaterialsFolderName { get => materialsFolderName; set => materialsFolderName = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090BB000>(sinceVersion: 8)]
    public Light[] Lights { get => lights; set => lights = value; }
    
    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk090BB000>]
    public CPlugMaterialUserInst?[] MaterialInsts { get => materialInsts; set => materialInsts = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090BB000>(sinceVersion: 10)]
    public CPlugLightUserModel?[] LightUserModels { get => lightUserModels; set => lightUserModels = value; }
    
    [NodeMember]
    [AppliedWithChunk<Chunk090BB000>(sinceVersion: 10)]
    public LightInst[] LightInsts { get => lightInsts; set => lightInsts = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090BB000>(sinceVersion: 11)]
    public int DamageZone { get => damageZone; set => damageZone = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090BB000>(sinceVersion: 12)]
    public uint Flags { get => flags; set => flags = value; }

    [NodeMember]
    [AppliedWithChunk<Chunk090BB000>]
    public Material[] CustomMaterials { get => customMaterials; set => customMaterials = value; }

    internal CPlugSolid2Model()
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
        public float[]? U04;
        public string? U07;
        public string? U09;
        public int U12;
        public string U13 = "";
        public int? U14;
        public Box[]? U15;
        public string[]? U16;
        public int[]? U17;
        public int? U18;
        public int[]? U19;
        public int? U20;
        public Node? U21;
        public float? U22;
        public float? U23;
        public string? U24;
        public int? U25;
        public (int, int, int, int, int)[]? U26;

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

                ApplyToGeoms(n, n.visuals, x => x.VisualIndex, (x, y) => x.Visual = y);
            }

            rw.ArrayId(ref n.materialIds!); // MaterialIds

            ApplyToGeoms(n, n.materialIds, x => x.MaterialIndex, (x, y) => x.MaterialId = y);

            var materialCount = version >= 29 ? rw.Int32(n.materialInsts.Length) : 0;

            if (materialCount == 0)
            {
                rw.Int32(10); // listVersion
                rw.ArrayNode<CPlugMaterial>(ref n.materials!);

                ApplyToGeoms(n, n.materials, x => x.MaterialIndex, (x, y) => x.Material = y);
            }

            rw.NodeRef<CPlugSkel>(ref n.skel, ref n.skelFile);

            if (version >= 1)
            {
                rw.Array<float>(ref U04); // something related to lod?

                if (version >= 2)
                {
                    rw.Int32(ref n.visCstType); // 1 - static

                    if (version >= 3)
                    {
                        if (rw.Boolean(n.preLightGenerator is not null))
                        {
                            // SPlugSolidPreLightGen::Archive
                            rw.Archive<PreLightGen>(ref n.preLightGenerator);
                        }

                        if (version >= 4)
                        {
                            rw.FileTime(ref n.fileWriteTime);

                            if (version >= 5)
                            {
                                rw.String(ref U07);

                                if (version >= 7)
                                {
                                    rw.String(ref n.materialsFolderName); // referring to GetMaterialsFolderNameFromFolder

                                    if (version >= 19)
                                    {
                                        rw.String(ref U09); // some shader?
                                    }

                                    if (version >= 8)
                                    {
                                        rw.ArrayArchive<Light>(ref n.lights!);

                                        if (version < 16)
                                        {
                                            rw.ArrayNode<CPlugMaterialUserInst>(ref n.materialInsts!);

                                            ApplyToGeoms(n, n.materialInsts, x => x.MaterialIndex, (x, y) => x.MaterialInst = y);
                                        }

                                        if (version >= 10)
                                        {
                                            rw.ArrayNode<CPlugLightUserModel>(ref n.lightUserModels!);
                                            rw.ArrayArchive<LightInst>(ref n.lightInsts!);

                                            if (version >= 11)
                                            {
                                                rw.Int32(ref n.damageZone);

                                                if (version >= 12)
                                                {
                                                    rw.UInt32(ref n.flags); // DoData

                                                    if (version < 28)
                                                    {
                                                        // Flags are adjusted
                                                    }

                                                    if (version >= 13)
                                                    {
                                                        rw.Int32(ref U12); // fake occ something?

                                                        if (version >= 14)
                                                        {
                                                            rw.String(ref U13!);

                                                            if (version >= 15)
                                                            {
                                                                if (version < 29)
                                                                {
                                                                    rw.Int32(ref materialCount);
                                                                }

                                                                if (version >= 30)
                                                                {
                                                                    rw.Int32(ref U14);
                                                                }

                                                                rw.ArrayArchive<Material>(ref n.customMaterials!, version, materialCount);

                                                                ApplyToGeoms(n, n.customMaterials, x => x.MaterialIndex, (x, y) => x.CustomMaterial = y);

                                                                if (version >= 17)
                                                                {
                                                                    if (version < 21)
                                                                    {
                                                                        rw.Array<Box>(ref U15);
                                                                    }

                                                                    if (version >= 20)
                                                                    {
                                                                        rw.ArrayId(ref U16); // slightly related to skels and joints

                                                                        // END OF MP4

                                                                        if (version >= 22)
                                                                        {
                                                                            rw.Array<int>(ref U17);

                                                                            if (version >= 23)
                                                                            {
                                                                                rw.Int32(ref U18); // array

                                                                                if (U18 > 0)
                                                                                {
                                                                                    throw new Exception("U18 > 0");
                                                                                }

                                                                                rw.Array<int>(ref U19);

                                                                                if (version >= 24)
                                                                                {
                                                                                    rw.Int32(ref U20);

                                                                                    if (version >= 25)
                                                                                    {
                                                                                        rw.NodeRef(ref U21);
                                                                                        rw.Single(ref U22);
                                                                                        rw.Single(ref U23);

                                                                                        if (version >= 27)
                                                                                        {
                                                                                            rw.Id(ref U24);

                                                                                            if (version >= 31)
                                                                                            {
                                                                                                rw.Int32(ref U25); // array of 2 int-sized values

                                                                                                if (U25 > 0)
                                                                                                {
                                                                                                    throw new Exception("U25 > 0");
                                                                                                }

                                                                                                if (version >= 33)
                                                                                                {
                                                                                                    if (version < 34)
                                                                                                    {
                                                                                                        rw.Int32(0);
                                                                                                    }

                                                                                                    rw.Array<(int, int, int, int, int)>(ref U26);
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
                }
            }
        }

        private static void ApplyToGeoms<T>(CPlugSolid2Model n, T[] array, Func<ShadedGeom, int> indexFunc, Action<ShadedGeom, T> setFunc)
        {
            if (array.Length == 0)
            {
                return;
            }
            
            foreach (var geom in n.shadedGeoms)
            {
                var i = indexFunc(geom);

                if (i < array.Length)
                {
                    setFunc(geom, array[i]);
                }
            }
        }
    }

    #endregion
    
    #endregion
}
