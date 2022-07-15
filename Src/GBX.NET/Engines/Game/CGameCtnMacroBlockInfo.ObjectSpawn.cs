namespace GBX.NET.Engines.Game;

public partial class CGameCtnMacroBlockInfo
{
    public class ObjectSpawn : IReadableWritable, IVersionable
    {
        private int ver;
        private Ident itemModel = Ident.Empty;
        private byte? quarterY;
        private byte? additionalDir;
        private Vec3 pitchYawRoll;
        private Int3 blockCoord;
        private string? anchorTreeId;
        private Vec3 absolutePositionInMap;
        private Vec3 pivotPosition;
        private CGameWaypointSpecialProperty? waypointSpecialProperty;
        private float scale;
        private FileRef? packDesc;
        private FileRef? foregroundPackDesc;

        public int U01;
        public int U02;
        public short U03;
        public Int3 U05;
        public int? U06;
        public byte? U07;
        public int? U08;

        public int Version { get => ver; set => ver = value; }
        public Ident ItemModel { get => itemModel; set => itemModel = value; }
        public byte? QuarterY { get => quarterY; set => quarterY = value; }
        public byte? AdditionalDir { get => additionalDir; set => additionalDir = value; }
        public Vec3 PitchYawRoll { get => pitchYawRoll; set => pitchYawRoll = value; }
        public Int3 BlockCoord { get => blockCoord; set => blockCoord = value; }
        public string? AnchorTreeId { get => anchorTreeId; set => anchorTreeId = value; }
        public Vec3 AbsolutePositionInMap { get => absolutePositionInMap; set => absolutePositionInMap = value; }
        public CGameWaypointSpecialProperty? WaypointSpecialProperty { get => waypointSpecialProperty; set => waypointSpecialProperty = value; }
        public Vec3 PivotPosition { get => pivotPosition; set => pivotPosition = value; }
        public float Scale { get => scale; set => scale = value; }
        public FileRef? PackDesc { get => packDesc; set => packDesc = value; }
        public FileRef? ForegroundPackDesc { get => foregroundPackDesc; set => foregroundPackDesc = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref ver);
            rw.Ident(ref itemModel!);

            if (ver < 3)
            {
                rw.Byte(ref quarterY);

                if (ver >= 1)
                {
                    rw.Byte(ref additionalDir);
                }
            }
            else
            {
                rw.Vec3(ref pitchYawRoll);
            }

            rw.Int3(ref blockCoord);
            rw.Id(ref anchorTreeId);
            rw.Vec3(ref absolutePositionInMap);

            if (ver < 5)
            {
                rw.Int32(ref U01);
            }

            if (ver < 6)
            {
                rw.Int32(ref U02);
            }

            if (ver >= 6)
            {
                rw.Int16(ref U03); // 0

                if (ver >= 7)
                {
                    rw.Vec3(ref pivotPosition);

                    if (ver >= 8)
                    {
                        rw.NodeRef(ref waypointSpecialProperty); // probably waypoint

                        if (ver >= 9)
                        {
                            rw.Single(ref scale);

                            if (ver >= 10)
                            {
                                rw.Int3(ref U05); // 0 1 -1

                                if (ver >= 11 && ver < 14)
                                {
                                    throw new VersionNotSupportedException(ver);
                                }

                                if (ver >= 14)
                                {
                                    rw.Int32(ref U06); // 0
                                    rw.Byte(ref U07);

                                    if (U07 == 1)
                                    {
                                        rw.FileRef(ref packDesc);
                                        rw.FileRef(ref foregroundPackDesc);
                                    }

                                    rw.Int32(ref U08); // -1

                                    if (U08 != -1)
                                    {
                                        throw new Exception("U08 != -1");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
