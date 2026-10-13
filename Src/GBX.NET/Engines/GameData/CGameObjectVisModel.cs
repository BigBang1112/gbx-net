using GBX.NET.Components;

namespace GBX.NET.Engines.GameData;

public partial class CGameObjectVisModel
{

    public CPlugSolid2Model? MeshShaded
    {
        get => meshShadedRef.Resolve();
        set => meshShadedRef.SetEmbedded(value);
    }

    private string? mesh;
    public string? Mesh { get => mesh; set => mesh = value; }

    private string? smashParticleRef;
    public string? SmashParticleRef { get => smashParticleRef; set => smashParticleRef = value; }

    private string? visEntFx;
    public string? VisEntFx { get => visEntFx; set => visEntFx = value; }

    public CMwNod? MeshShadedFid
    {
        get => meshShadedFidRef.Resolve();
        set => meshShadedFidRef.SetEmbedded(value);
    }

    private Vec3? domeShaderColor;
    public Vec3? DomeShaderColor { get => domeShaderColor; set => domeShaderColor = value; }

    public CPlugAnimLocSimple? LocAnim
    {
        get => locAnimRef.Resolve();
        set => locAnimRef.SetEmbedded(value);
    }

    public CPlugSolid? Solid
    {
        get => solidNodeRef.Resolve();
        set => solidNodeRef.SetEmbedded(value);
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

        public float? U12;

        public CMwNod? U15;

        public float? U18;
        public float? U19;
        public float? U20;
        public string? U21;
        public CMwNod? U22;
    }
}
