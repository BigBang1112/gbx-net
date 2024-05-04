namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfile
{
    private string? profileDescription;
    public string? ProfileDescription { get => profileDescription; set => profileDescription = value; }

    private UInt128 cryptedPassword;
    public UInt128 CryptedPassword { get => cryptedPassword; set => cryptedPassword = value; }

    public partial class Chunk0308C068
    {
        public Dictionary<string, int>? U01;

        public override void Read(CGamePlayerProfile n, GbxReader r)
        {
            var count = r.ReadInt32();
            U01 = new(count);

            for (var i = 0; i < count; i++)
            {
                U01.Add(r.ReadIdAsString(), r.ReadInt32());
            }
        }

        public override void Write(CGamePlayerProfile n, GbxWriter w)
        {
            if (U01 is null)
            {
                w.Write(0);
                return;
            }

            w.Write(U01.Count);

            foreach (var pair in U01)
            {
                w.WriteIdAsString(pair.Key);
                w.Write(pair.Value);
            }
        }
    }

    public partial class Chunk0308C069
    {
        public string? U01;
        public int U02;
        public int U03;
        public string? U04;
        public int U05;
        public byte[]? U06;
        public string? U07;
        public string? U08;
        public bool U09;
        public bool U10;
        public string? U11;
        public string? U12;

        public override void ReadWrite(CGamePlayerProfile n, GbxReaderWriter rw)
        {
            rw.String(ref U01);
            rw.String(ref n.profileDescription);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.String(ref U04);
            rw.Int32(ref U05);

            if (U05 != 0)
            {
                rw.UInt128(ref n.cryptedPassword);
                rw.Data(ref U06, U05);
            }

            rw.String(ref U07);
            rw.String(ref U08);
            rw.Boolean(ref U09);
            rw.Boolean(ref U10);
            rw.String(ref U11);
            rw.String(ref U12);
        }
    }
}
