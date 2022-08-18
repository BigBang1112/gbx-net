namespace GBX.NET.Engines.Game;

public partial class CGameCtnMacroBlockInfo
{
    public class BlockSpawn : IReadableWritable, IVersionable
    {
        private int ver;
        private Ident blockModel = Ident.Empty;
        private Int3? coord;
        private Direction? direction;
        private int flags;
        private CGameWaypointSpecialProperty? waypoint;
        private CMwNod? U01;
        private short? U02;
        private Vec3? absolutePositionInMap;
        private Vec3? pitchYawRoll;

        public int Version { get => ver; set => ver = value; }
        public Ident BlockModel { get => blockModel; set => blockModel = value; }
        public Int3? Coord { get => coord; set => coord = value; }
        public Direction? Direction { get => direction; set => direction = value; }
        public int Flags { get => flags; set => flags = value; }
        public CGameWaypointSpecialProperty? Waypoint { get => waypoint; set => waypoint = value; }
        public Vec3? AbsolutePositionInMap { get => absolutePositionInMap; set => absolutePositionInMap = value; }
        public Vec3? PitchYawRoll { get => pitchYawRoll; set => pitchYawRoll = value; }

        public string Name
        {
            get => BlockModel.Id;
            set => BlockModel = BlockModel with { Id = value };
        }

        public bool IsFree => false;
        public bool IsGround => false;

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref ver);
            rw.Ident(ref blockModel!);

            if (ver < 2)
            {
                rw.Int3(ref coord);
                rw.EnumInt32<Direction>(ref direction);

                throw new VersionNotSupportedException(ver);
            }

            if (ver >= 2)
            {
                if (ver < 5)
                {
                    rw.Byte3(ref coord);
                    rw.EnumByte<Direction>(ref direction);
                }

                rw.Int32(ref flags);

                if (ver >= 3)
                {
                    if (ver >= 5)
                    {
                        if (((flags >> 26) & 1) != 0)
                        {
                            rw.Vec3(ref absolutePositionInMap);
                            rw.Vec3(ref pitchYawRoll);
                        }
                        else
                        {
                            rw.Byte3(ref coord);
                            rw.EnumByte<Direction>(ref direction);
                        }
                    }

                    rw.NodeRef<CGameWaypointSpecialProperty>(ref waypoint);

                    if (ver >= 4)
                    {
                        if (ver >= 6 && ver < 8)
                        {
                            throw new ChunkVersionNotSupportedException(ver);
                        }

                        if (ver < 6)
                        {
                            rw.NodeRef(ref U01);

                            if (U01 is not null)
                            {
                                throw new NotImplementedException();
                            }
                        }

                        if (ver >= 8)
                        {
                            rw.Int16(ref U02);
                        }
                    }
                }
            }
        }
    }
}
