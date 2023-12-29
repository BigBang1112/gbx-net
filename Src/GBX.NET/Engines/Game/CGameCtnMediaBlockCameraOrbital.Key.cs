namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockCameraOrbital
{
    public new class Key : CGameCtnMediaBlock.Key
    {
        private float radius;
        private float longitude;
        private float latitude;
        private Vec3 targetPosition;
        private float fov;
        private float u01;
        private float u02;
        private int u03;
        private int u04;
        private int u05;
        private int u06;
        private int u07;

        public float Radius { get => radius; set => radius = value; }
        public float Longitude { get => longitude; set => longitude = value; }
        public float Latitude { get => latitude; set => latitude = value; }
        public Vec3 TargetPosition { get => targetPosition; set => targetPosition = value; }
        public float Fov { get => fov; set => fov = value; }
        public float U01 { get => u01; set => u01 = value; }
        public float U02 { get => u02; set => u02 = value; }
        public int U03 { get => u03; set => u03 = value; }
        public int U04 { get => u04; set => u04 = value; }
        public int U05 { get => u05; set => u05 = value; }
        public int U06 { get => u06; set => u06 = value; }
        public int U07 { get => u07; set => u07 = value; }

        public override void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            base.ReadWrite(rw, version);

            // GmCamOrbitVal::Archive
            rw.Byte(0);
            rw.Single(ref radius);
            rw.Single(ref longitude);
            rw.Single(ref latitude);
            rw.Vec3(ref targetPosition);
            rw.Single(ref fov);
            rw.Single(ref u01);
            rw.Single(ref u02);
            //

            rw.Int32(ref u03);

            if (version >= 1)
            {
                rw.Int32(ref u04);
                rw.Int32(ref u05);
                rw.Int32(ref u06);
                rw.Int32(ref u07);
            }
        }
    }
}
