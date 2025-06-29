using GBX.NET.Components;

namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlockInfoVariant
{
    public CGameCtnBlockInfoMobil[][]? Mobils { get; set; }

    private CPlugSolid? helperSolidFid;
    [AppliedWithChunk<Chunk0315B005>]
    public CPlugSolid? HelperSolidFid { get => helperSolidFidFile?.GetNode(ref helperSolidFid) ?? helperSolidFid; set => helperSolidFid = value; }
    private GbxRefTableFile? helperSolidFidFile;
    public GbxRefTableFile? HelperSolidFidFile { get => helperSolidFidFile; set => helperSolidFidFile = value; }
    public CPlugSolid? GetHelperSolidFid(GbxReadSettings settings = default, bool exceptions = false) => helperSolidFidFile?.GetNode(ref helperSolidFid, settings, exceptions) ?? helperSolidFid;

    private CPlugSolid? facultativeHelperSolidFid;
    [AppliedWithChunk<Chunk0315B005>]
    public CPlugSolid? FacultativeHelperSolidFid { get => facultativeHelperSolidFidFile?.GetNode(ref facultativeHelperSolidFid) ?? facultativeHelperSolidFid; set => facultativeHelperSolidFid = value; }
    private GbxRefTableFile? facultativeHelperSolidFidFile;
    public GbxRefTableFile? FacultativeHelperSolidFidFile { get => facultativeHelperSolidFidFile; set => facultativeHelperSolidFidFile = value; }
    public CPlugSolid? GetFacultativeHelperSolidFid(GbxReadSettings settings = default, bool exceptions = false) => facultativeHelperSolidFidFile?.GetNode(ref facultativeHelperSolidFid, settings, exceptions) ?? facultativeHelperSolidFid;

    public partial class Chunk0315B005 : IVersionable
    {
        public int Version { get; set; }

        public int U03;

        public override void Read(CGameCtnBlockInfoVariant n, GbxReader r)
        {
            Version = r.ReadInt32();

            n.Mobils = new CGameCtnBlockInfoMobil[r.ReadInt32()][];
            for (var i = 0; i < n.Mobils.Length; i++)
            {
                n.Mobils[i] = r.ReadArrayNodeRef<CGameCtnBlockInfoMobil>()!;
            }

            if (Version >= 2)
            {
                n.helperSolidFid = r.ReadNodeRef<CPlugSolid>(out n.helperSolidFidFile); // HelperSolidFid?
                n.facultativeHelperSolidFid = r.ReadNodeRef<CPlugSolid>(out n.facultativeHelperSolidFidFile); // FacultativeHelperSolidFid?

                if (Version >= 3)
                {
                    U03 = r.ReadInt32();
                }
            }
        }

        public override void Write(CGameCtnBlockInfoVariant n, GbxWriter w)
        {
            w.Write(Version);

            w.Write(n.Mobils?.Length ?? 0);
            for (var i = 0; i < n.Mobils?.Length; i++)
            {
                w.WriteArrayNodeRef(n.Mobils[i]);
            }

            if (Version >= 2)
            {
                w.WriteNodeRef(n.helperSolidFid, n.helperSolidFidFile);
                w.WriteNodeRef(n.facultativeHelperSolidFid, n.facultativeHelperSolidFidFile);

                if (Version >= 3)
                {
                    w.Write(U03);
                }
            }
        }
    }
}
