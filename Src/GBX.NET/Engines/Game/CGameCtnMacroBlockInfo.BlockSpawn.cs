namespace GBX.NET.Engines.Game;

public partial class CGameCtnMacroBlockInfo
{
    public class BlockSpawn : IReadableWritable, IVersionable
    {
        private int ver;
        private Ident blockModel = Ident.Empty;
        private Int3 coord;
        private Direction direction;
        private int flags;
        private CGameWaypointSpecialProperty? waypoint;
        private CMwNod? U01;

        public int Version { get => ver; set => ver = value; }
        public Ident BlockModel { get => blockModel; set => blockModel = value; }
        public Int3 Coord { get => coord; set => coord = value; }
        public Direction Direction { get => direction; set => direction = value; }
        public int Flags { get => flags; set => flags = value; }
        public CGameWaypointSpecialProperty? Waypoint { get => waypoint; set => waypoint = value; }

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
                coord = (Int3)rw.Byte3((Byte3)coord);
                rw.EnumByte<Direction>(ref direction);

                rw.Int32(ref flags);

                if (ver >= 3)
                {
                    rw.NodeRef<CGameWaypointSpecialProperty>(ref waypoint);

                    if (ver >= 4)
                    {
                        rw.NodeRef(ref U01);

                        if (U01 is not null)
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
            }
        }
    }
}
