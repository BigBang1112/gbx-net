namespace GBX.NET.Engines.Plug;

public partial class CPlugSolid2Model
{
    private ShadedGeom[]? shadedGeoms;
    private CPlugVisual[]? visuals;
    private string[]? materialIds;
    public External<CPlugMaterial>[]? materials;
    private CPlugSkel? skel;
    private int visCstType;
    private PreLightGen? preLightGenerator;
    private DateTime fileWriteTime;
    private string? materialsFolderName;
    private Light[]? lights;
    public CPlugMaterialUserInst[]? materialInsts;
    private CPlugLightUserModel[]? lightUserModels;
    private LightInst[] lightInsts;
    private int damageZone;
    private uint flags;
    private Material[]? customMaterials;

    public int VisCstType { get => visCstType; set => visCstType = value; }
    public string? MaterialsFolderName { get => materialsFolderName; set => materialsFolderName = value; }

    public partial class Chunk090BB000 : IVersionable
    {
        public int Version { get; set; }

        public string? U01;
        public float[]? U02;
        public string? U03;
        public string? U04;
        public int U05;
        public string U06 = "";
        public int? U07;
        public BoxAligned[]? U08;
        public string[]? U09;
        public int[]? U10;
        public int? U11;
        public int[]? U12;
        public int? U13;
        public CMwNod? U14;
        public float? U15;
        public float? U16;
        public string? U17;
        public int? U18;
        public (int, int, int, int, int)[]? U19;

        public override void ReadWrite(CPlugSolid2Model n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Id(ref U01);
            rw.ArrayReadableWritable<ShadedGeom>(ref n.shadedGeoms, version: Version);

            if (Version >= 6)
            {
                rw.ArrayNodeRef_deprec<CPlugVisual>(ref n.visuals);
            }

            rw.ArrayId(ref n.materialIds!); // MaterialIds

            var materialCount = Version >= 29 ? rw.Int32(n.customMaterials?.Length ?? 0) : 0;

            if (materialCount == 0)
            {
                rw.ArrayNodeRef_deprec<CPlugMaterial>(ref n.materials!);
            }

            rw.NodeRef<CPlugSkel>(ref n.skel);

            if (Version >= 1)
            {
                rw.Array<float>(ref U02); // something related to lod?

                if (Version >= 2)
                {
                    rw.Int32(ref n.visCstType); // 1 - static

                    if (Version >= 3)
                    {
                        if (rw.Boolean(n.preLightGenerator is not null))
                        {
                            // SPlugSolidPreLightGen::Archive
                            rw.ReadableWritable<PreLightGen>(ref n.preLightGenerator);
                        }

                        if (Version >= 4)
                        {
                            rw.FileTime(ref n.fileWriteTime);

                            if (Version >= 5)
                            {
                                rw.String(ref U03);

                                if (Version >= 7)
                                {
                                    rw.String(ref n.materialsFolderName); // referring to GetMaterialsFolderNameFromFolder

                                    if (Version >= 19)
                                    {
                                        rw.String(ref U04); // some shader?
                                    }

                                    if (Version >= 8)
                                    {
                                        rw.ArrayReadableWritable<Light>(ref n.lights!);

                                        if (Version < 16)
                                        {
                                            rw.ArrayNodeRef<CPlugMaterialUserInst>(ref n.materialInsts!);
                                        }

                                        if (Version >= 10)
                                        {
                                            rw.ArrayNodeRef<CPlugLightUserModel>(ref n.lightUserModels!);
                                            rw.ArrayReadableWritable<LightInst>(ref n.lightInsts!);

                                            if (Version >= 11)
                                            {
                                                rw.Int32(ref n.damageZone);

                                                if (Version >= 12)
                                                {
                                                    rw.UInt32(ref n.flags); // DoData

                                                    if (Version < 28)
                                                    {
                                                        // Flags are adjusted
                                                    }

                                                    if (Version >= 13)
                                                    {
                                                        rw.Int32(ref U05); // fake occ something?

                                                        if (Version >= 14)
                                                        {
                                                            rw.String(ref U06!);

                                                            if (Version >= 15)
                                                            {
                                                                if (Version < 29)
                                                                {
                                                                    rw.Int32(ref materialCount);
                                                                }

                                                                if (Version >= 30)
                                                                {
                                                                    rw.Int32(ref U07);
                                                                }

                                                                rw.ArrayReadableWritable<Material>(ref n.customMaterials!, materialCount, Version);

                                                                if (Version >= 17)
                                                                {
                                                                    if (Version < 21)
                                                                    {
                                                                        rw.Array<BoxAligned>(ref U08);
                                                                    }

                                                                    if (Version >= 20)
                                                                    {
                                                                        rw.ArrayId(ref U09); // slightly related to skels and joints

                                                                        // END OF MP4

                                                                        if (Version >= 22)
                                                                        {
                                                                            rw.Array<int>(ref U10);

                                                                            if (Version >= 23)
                                                                            {
                                                                                rw.Int32(ref U11); // array

                                                                                if (U11 > 0)
                                                                                {
                                                                                    throw new Exception("U18 > 0");
                                                                                }

                                                                                rw.Array<int>(ref U12);

                                                                                if (Version >= 24)
                                                                                {
                                                                                    rw.Int32(ref U13);

                                                                                    if (Version >= 25)
                                                                                    {
                                                                                        rw.NodeRef(ref U14);
                                                                                        rw.Single(ref U15);
                                                                                        rw.Single(ref U16);

                                                                                        if (Version >= 27)
                                                                                        {
                                                                                            rw.Id(ref U17);

                                                                                            if (Version >= 31)
                                                                                            {
                                                                                                rw.Int32(ref U18); // array of 2 int-sized values

                                                                                                if (U18 > 0)
                                                                                                {
                                                                                                    throw new Exception("U25 > 0");
                                                                                                }

                                                                                                if (Version >= 33)
                                                                                                {
                                                                                                    if (Version < 34)
                                                                                                    {
                                                                                                        rw.Int32(0);
                                                                                                    }

                                                                                                    rw.Array<(int, int, int, int, int)>(ref U19);
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
    }
}
