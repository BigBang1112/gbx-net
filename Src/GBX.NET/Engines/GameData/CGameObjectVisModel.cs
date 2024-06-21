using GBX.NET.Components;

namespace GBX.NET.Engines.GameData;

public partial class CGameObjectVisModel
{
    private CPlugSolid2Model? meshShaded;
    public CPlugSolid2Model? MeshShaded
    {
        get => meshShadedFile?.GetNode(ref meshShaded) ?? meshShaded;
        set => meshShaded = value;
    }
    private GbxRefTableFile? meshShadedFile;
    public GbxRefTableFile? MeshShadedFile { get => meshShadedFile; set => meshShadedFile = value; }
    public CPlugSolid2Model? GetMeshShaded(GbxReadSettings settings = default) => meshShadedFile?.GetNode(ref meshShaded, settings);

    private string? mesh;
    public string? Mesh { get => mesh; set => mesh = value; }

    private string? smashParticleRef;
    public string? SmashParticleRef { get => smashParticleRef; set => smashParticleRef = value; }

    private string? visEntFx;
    public string? VisEntFx { get => visEntFx; set => visEntFx = value; }

    private CMwNod? meshShadedFid;
    public CMwNod? MeshShadedFid { get => meshShadedFid; set => meshShadedFid = value; }

    private Vec3? domeShaderColor;
    public Vec3? DomeShaderColor { get => domeShaderColor; set => domeShaderColor = value; }

    private CPlugAnimLocSimple? locAnim;
    public CPlugAnimLocSimple? LocAnim { get => locAnim; set => locAnim = value; }

    public partial class Chunk2E007001 : IVersionable
    {
        public int Version { get; set; }

        public CMwNod? U01;
        public CMwNod? U02;
        public int U03;
        public string? U04;
        public string? U05;
        public string? U06;
        public string? U07;
        public int? U08;
        public int? U09;
        public CMwNod? U10;
        public float? U12;
        public CMwNod? U13;
        public CMwNod? U14;
        public GbxRefTableFile? U14File;
        public CMwNod? U15;
        public CMwNod? U16;
        public GbxRefTableFile? U16File;
        public string? U17;
        public float? U18;
        public float? U19;
        public float? U20;
        public string? U21;
        public CMwNod? U22;

        public override void ReadWrite(CGameObjectVisModel n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version < 9)
            {
                rw.NodeRef(ref U14, ref U14File);
            }
            else
            {
                rw.String(ref n.mesh);

                if (string.IsNullOrEmpty(n.mesh))
                {
                    rw.NodeRef<CPlugSolid2Model>(ref n.meshShaded, ref n.meshShadedFile);
                }
            }

            if (Version < 18) // CPlugParticleEmitterModel?
            {
                rw.NodeRef(ref U16); // CPlugAnimFile
            }

            if (Version >= 2)
            {
                if (Version < 9)
                {
                    rw.String(ref n.mesh);
                }

                rw.NodeRef<CPlugAnimLocSimple>(ref n.locAnim);

                rw.Int32(ref U03); // SPlugLightBallStateSimple array
            }

            if (Version < 17)
            {
                rw.String(ref n.soundRefSpawn);
                rw.String(ref n.soundRefUnspawn);
                rw.String(ref n.soundRefGrab);
            }

            if (Version >= 10)
            {
                rw.String(ref U07);

                if (Version >= 11 && !string.IsNullOrEmpty(U07))
                {
                    rw.Single(ref U18);
                    rw.Single(ref U19);
                    rw.Single(ref U20);
                }

                if (Version >= 12)
                {
                    rw.String(ref n.smashParticleRef);

                    if (!string.IsNullOrEmpty(n.smashParticleRef))
                    {
                        rw.Id(ref U21);
                    }

                    if (Version >= 13)
                    {
                        rw.Int32(ref U08); // CPlugFileImg array
                        rw.Int32(ref U09); // SSpriteParam array

                        if (Version >= 14)
                        {
                            rw.String(ref U17);

                            if (string.IsNullOrEmpty(U17))
                            {
                                rw.NodeRef(ref U22); // CPlugSolid
                            }

                            if (Version >= 16)
                            {
                                rw.NodeRef(ref U10); // CPlugParticleEmitterModel

                                if (Version >= 19)
                                {
                                    rw.Single(ref U12);

                                    if (Version >= 20)
                                    {
                                        rw.String(ref n.visEntFx);
                                        // if empty then maybe nodref? proly not

                                        if (string.IsNullOrEmpty(n.visEntFx))
                                        {
                                            rw.NodeRef(ref U13);
                                        }

                                        if (Version >= 21)
                                        {
                                            rw.NodeRef(ref n.meshShadedFid);

                                            if (Version >= 22)
                                            {
                                                rw.Vec3(ref n.domeShaderColor);
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
