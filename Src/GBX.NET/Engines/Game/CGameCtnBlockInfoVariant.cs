namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlockInfoVariant
{
    private CGameCtnBlockInfoMobil[][]? Mobils { get; set; }

    public partial class Chunk0315B005 : IVersionable
    {
        public int Version { get; set; }

        public int U01;
        public int U02;
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
                U01 = r.ReadInt32(); // HelperSolidFid?
                U02 = r.ReadInt32(); // FacultativeHelperSolidFid?

                if (Version >= 3)
                {
                    U03 = r.ReadInt32();
                }
            }
        }
    }
}
