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
#if NET6_0_OR_GREATER
            Span<char> hex = stackalloc char[hashedPassword.Length * 2];
#else
            var hex = new char[hashedPassword.Length * 2];
#endif
            TryHex(hashedPassword, hex);
            toHash = $"0x{hex}???{MapUid}";
        }

        Crc32 = Gbx.CRC32?.Hash(Encoding.ASCII.GetBytes(toHash)) ?? throw new Exception("CRC32 is not imported (ICrc32).");

#if NET6_0_OR_GREATER
        static void TryHex(ReadOnlySpan<byte> value, Span<char> chars)
#else
        static void TryHex(byte[] value, char[] chars)
#endif
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

    /// <summary>
    /// [SHeaderCommunity] CGameCtnChallenge 0x005 header chunk (community)
    /// </summary>
    [Chunk(0x03043005, "community")]
    public struct HeaderChunk03043005 : IHeaderChunk<CGameCtnChallenge>
    {
        /// <inheritdoc />
        public readonly uint Id => 0x03043005;

        public bool IsHeavy { get; set; }

        /// <inheritdoc />
        public readonly void Read(CGameCtnChallenge n, IGbxReader r)
        {
            n.Xml = r.ReadString();
        }

        /// <inheritdoc />
        public readonly void Write(CGameCtnChallenge n, IGbxWriter w)
        {
            w.Write(n.Xml);
        }
    }

    public partial class HeaderChunk03043007 : IVersionable
    {
        public int Version { get; set; } = 1;

        internal override void ReadWrite(CGameCtnChallenge n, GbxReaderWriter rw)
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

        public void ReadWrite(IGbxReaderWriter rw, int version = 0)
        {
            /*rw.Int32(ref clan);
            rw.List<Vec3>(ref path, r => r.ReadVec3(), (x, w) => w.Write(x));
            rw.Boolean(ref isFlying);
            rw.NodeRef<CGameWaypointSpecialProperty>(ref waypointSpecialProperty);
            rw.Boolean(ref isAutonomous);*/
        }
    }
}
