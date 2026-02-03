namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfile
{
    private string? description;
    public string? Description { get => description; set => description = value; }

    private UInt128 cryptedPassword;
    public UInt128 CryptedPassword { get => cryptedPassword; set => cryptedPassword = value; }

    private bool loginValidated;
    public bool LoginValidated { get => loginValidated; set => loginValidated = value; }

    private bool rememberOnlinePassword;
    public bool RememberOnlinePassword { get => rememberOnlinePassword; set => rememberOnlinePassword = value; }

    private ProfileChunk[]? profileChunks;
    public ProfileChunk[]? ProfileChunks { get => profileChunks; set => profileChunks = value; }

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
            rw.String(ref n.description);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.String(ref n.onlineLogin);
            rw.Int32(ref U05);

            if (U05 != 0)
            {
                rw.UInt128(ref n.cryptedPassword);
                rw.Data(ref U06, U05);
            }

            rw.String(ref U07);
            rw.String(ref U08);
            rw.Boolean(ref n.loginValidated);
            rw.Boolean(ref n.rememberOnlinePassword);
            rw.String(ref U11);
            rw.String(ref U12);
        }
    }

    public partial class Chunk0308C07C
    {
        public string? U01;

        public override void Read(CGamePlayerProfile n, GbxReader r)
        {
            U01 = r.ReadId();
            n.profileName = r.ReadString();

            n.profileChunks = new ProfileChunk[r.ReadInt32()];

            for (var i = 0; i < n.profileChunks.Length; i++)
            {
                var chunkId = r.ReadUInt32();
                var u01 = r.ReadString();
                var u02 = r.ReadString();
                var u03 = r.ReadString();
                var u04 = r.ReadInt32();
                var archiveVersion = r.ReadInt32();

                ProfileChunk? chunk = chunkId switch
                {
                    0x0312C000 => new AccountSettings(), // CGamePlayerProfileChunk_AccountSettings::ArchiveOldVersion
                    _ => throw new NotImplementedException($"ProfileChunk 0x{chunkId:X8} is not implemented."),
                };

                if (chunk is not null)
                {
                    chunk.Read(n, r, archiveVersion);
                    n.profileChunks[i] = chunk;
                }
            }
        }
    }

    public abstract class ProfileChunk
    {
        public abstract void Read(CGamePlayerProfile n, GbxReader r, int version);
    }

    public partial class AccountSettings : ProfileChunk
    {
        public int U01;
        public bool U02;
        public string LastUsedMSAddress { get; set; } = "";
        public string LastUsedMSPath { get; set; } = "";
        public string League { get; set; } = "";
        public string? RSAPublicKey { get; set; }
        public string? RSAPrivateKey { get; set; }

        public override void Read(CGamePlayerProfile n, GbxReader r, int version)
        {
            n.description = r.ReadString();
            n.nickName = r.ReadString();
            var u01 = r.ReadByte();
            n.onlineLogin = r.ReadString();
            var u02 = r.ReadString();
            var u03 = r.ReadString();
            var u04 = r.ReadString();
            LastUsedMSAddress = r.ReadString();
            LastUsedMSPath = r.ReadString();
            var u07 = r.ReadString();
            League = r.ReadString();
            if (version < 4)
            {
                var u08 = r.ReadInt32();
            }
            var u09 = r.ReadInt32();
            var u10 = r.ReadInt32();
            if (version >= 1)
            {
                RSAPublicKey = r.ReadString();
            }
            RSAPrivateKey = r.ReadString();
            var wtf = r.ReadArray<int>(20);
        }
    }
}
