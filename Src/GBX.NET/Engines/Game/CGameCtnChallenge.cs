using System.Text;

namespace GBX.NET.Engines.Game;

public sealed partial class CGameCtnChallenge :
    CGameCtnChallenge.ITM2020
{
    public string MapUid
    {
        get => mapInfo.Id;
        set
        {
            mapInfo = new Ident(value, mapInfo.Collection, mapInfo.Author);

            CalculateCRC32();
        }
    }

    string ITM2020.MapUid
    {
        get => MapUid ?? throw new Exception("MapUid not available");
        set => MapUid = value;
    }

    public void CalculateCRC32()
    {
        string toHash;

        if (HashedPassword is null)
        {
            toHash = $"0x00000000000000000000000000000000???{MapUid}";
        }
        else
        {
#if NET6_0_OR_GREATER
            Span<char> hex = stackalloc char[HashedPassword.Length * 2];
#else
            var hex = new char[HashedPassword.Length * 2];
#endif
            TryHex(HashedPassword, hex);
            toHash = $"0x{hex}???{MapUid}";
        }

        Crc32 = Gbx.CRC32?.Hash(Encoding.ASCII.GetBytes(toHash)) ?? throw new NullReferenceException("CRC32 is not imported (ICrc32).");

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
    public struct HeaderChunk03043005 : IHeaderChunk<CGameCtnChallenge>, IEquatable<HeaderChunk03043005>
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

        /// <inheritdoc />
        public override readonly bool Equals(object? obj) => obj is HeaderChunk03043005 other && Equals(other);
        /// <inheritdoc />
        public readonly bool Equals(HeaderChunk03043005 other) => Id == other.Id;
        /// <inheritdoc />
        public override readonly int GetHashCode() => Id.GetHashCode();
        public static bool operator ==(HeaderChunk03043005 left, HeaderChunk03043005 right) => left.Equals(right);
        public static bool operator !=(HeaderChunk03043005 left, HeaderChunk03043005 right) => !(left == right);
    }

    public interface ITM2020 : IClassVersion<CGameCtnChallenge>
    {
        string MapUid { get; set; }
    }

    public class BotPath : IReadableWritable
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
