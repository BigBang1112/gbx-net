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
    public CMwNod? MeshShadedFid
    {
        get => meshShadedFidFile?.GetNode(ref meshShadedFid) ?? meshShadedFid;
        set => meshShadedFid = value;
    }

    private Vec3? domeShaderColor;
    public Vec3? DomeShaderColor { get => domeShaderColor; set => domeShaderColor = value; }

    private CPlugAnimLocSimple? locAnim;
    public CPlugAnimLocSimple? LocAnim
    {
        get => locAnimFile?.GetNode(ref locAnim) ?? locAnim;
        set => locAnim = value;
    }

    private CPlugSolid? solid;
    public CPlugSolid? Solid
    {
        get => solidFile?.GetNode(ref solid) ?? solid;
        set => solid = value;
    }

    private string? solidRef;
    public string? SolidRef { get => solidRef; set => solidRef = value; }

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
        public float? U18;
        public float? U19;
        public float? U20;
        public string? U21;
        public CMwNod? U22;
    }
}
