using System.Text;

namespace GBX.NET.Engines.Game;

public sealed partial class CGameCtnChallenge :
    CGameCtnChallenge.ITM2020
{
    /// <summary>
    /// The map's UID.
    /// </summary>
    public string MapUid
    {
        get => mapInfo.Id;
        set
        {
            mapInfo = new Ident(value, mapInfo.Collection, mapInfo.Author);

            CalculateCrc32();
        }
    }

    private byte[]? thumbnail;
    public byte[]? Thumbnail
    {
        get => hashedPassword;
        set => thumbnail = value;
    }

    public Id? Collection => mapInfo?.Collection;

    public int? NbBlocks => Blocks?.Count(x => x.Name != "Unassigned1");

    private byte[]? hashedPassword;
    public byte[]? HashedPassword
    {
        get => hashedPassword;
        set
        {
            hashedPassword = value;

            CalculateCrc32();
        }
    }

    string ITM2020.MapUid
    {
        get => MapUid ?? throw new Exception("MapUid not available");
        set => MapUid = value;
    }

    public string GetEnvironment()
    {
        return Collection ?? throw new Exception("Environment not available");
    }

    /// <summary>
    /// Calculates the CRC32 of the map.
    /// </summary>
    public void CalculateCrc32()
    {
        string toHash;

        if (hashedPassword is null)
        {
            toHash = $"0x00000000000000000000000000000000???{MapUid}";
        }
        else
        {
            Span<char> hex = stackalloc char[hashedPassword.Length * 2];
            TryHex(hashedPassword, hex);
            toHash = $"0x{hex.ToString()}???{MapUid}";
        }

        Crc32 = Gbx.CRC32?.Hash(Encoding.ASCII.GetBytes(toHash)) ?? throw new Exception("CRC32 is not imported (ICrc32).");

        static void TryHex(ReadOnlySpan<byte> value, Span<char> chars)
        {
            for (var i = 0; i < value.Length; i++)
            {
                var hex1 = HexIntToChar((byte)(value[value.Length - 1 - i] % 16));
                var hex2 = HexIntToChar((byte)(value[value.Length - 1 - i] / 16));

                chars[i * 2 + 1] = hex1;
                chars[i * 2] = hex2;
            }

            return;

            static char HexIntToChar(byte v)
            {
                if (v < 10)
                {
                    return (char)(v + 48);
                }

                return (char)(v + 55);
            }
        }
    }

    public partial struct HeaderChunk03043005;

    public partial class HeaderChunk03043007 : IVersionable
    {
        public int Version { get; set; } = 1;

        public override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);

            if (Version == 0)
            {
                return;
            }

            var thumbnailSize = rw.Int32(n.thumbnail?.Length ?? 0);
            rw.Marker("<Thumbnail.jpg>");
            rw.Data(ref n.thumbnail, thumbnailSize);
            rw.Marker("</Thumbnail.jpg>");
            rw.Marker("<Comments>");
            rw.String(ref n.comments);
            rw.Marker("</Comments>");
        }
    }

    public partial class Chunk0304301F : IVersionable
    {
        public int Version { get; set; } = 6;

        public bool U01;

        public override void Read(CGameCtnChallenge n, GbxReader r)
        {
            n.mapInfo = r.ReadIdent();
            n.mapName = r.ReadString();
            n.decoration = r.ReadIdent();
            n.size = r.ReadInt3();
            U01 = r.ReadBoolean();
            Version = r.ReadInt32();

            var nbBlocks = r.ReadInt32();
            var blocks = new List<CGameCtnBlock>(nbBlocks);

            for (var i = 0; i < nbBlocks; i++)
            {
                var block = r.ReadReadable<CGameCtnBlock>(Version);
                blocks.Add(block);

                if (block.Flags == -1)
                {
                    i--;
                }
            }

            while ((r.PeekUInt32() & 0xC0000000) > 0)
            {
                blocks.Add(r.ReadReadable<CGameCtnBlock>(Version));
            }
        }

        public override void Write(CGameCtnChallenge n, GbxWriter w)
        {
            w.Write(n.mapInfo);
            w.Write(n.mapName);
            w.Write(n.decoration);
            w.Write(n.size);
            w.Write(U01);
            w.Write(Version);

            w.Write(n.NbBlocks.GetValueOrDefault());

            foreach (var block in n.blocks)
            {
                w.WriteWritable(block, Version);
            }
        }
    }

    public interface ITM2020 : IClassVersion<CGameCtnChallenge>
    {
        string MapUid { get; set; }
    }

    public sealed class BotPath : IReadableWritable
    {
        /*private int clan;
        private IList<Vec3>? path;
        private bool isFlying;
        private CGameWaypointSpecialProperty? waypointSpecialProperty;
        private bool isAutonomous;

        public int Clan { get => clan; set => clan = value; }
        public IList<Vec3>? Path { get => path; set => path = value; }
        public bool IsFlying { get => isFlying; set => isFlying = value; }
        public CGameWaypointSpecialProperty? WaypointSpecialProperty { get => waypointSpecialProperty; set => waypointSpecialProperty = value; }
        public bool IsAutonomous { get => isAutonomous; set => isAutonomous = value; }*/

        public void ReadWrite(GbxReaderWriter rw, int version = 0)
        {
            /*rw.Int32(ref clan);
            rw.List<Vec3>(ref path, r => r.ReadVec3(), (x, w) => w.Write(x));
            rw.Boolean(ref isFlying);
            rw.NodeRef<CGameWaypointSpecialProperty>(ref waypointSpecialProperty);
            rw.Boolean(ref isAutonomous);*/
        }
    }
}
