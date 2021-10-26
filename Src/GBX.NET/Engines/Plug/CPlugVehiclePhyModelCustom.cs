using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Plug
{
    [Node(0x0911E000)]
    public sealed class CPlugVehiclePhyModelCustom : CMwNod
    {
        #region Fields

        private float accelCoef;
        private float controlCoef;
        private float gravityCoef;

        #endregion

        #region Properties

        [NodeMember]
        public float AccelCoef
        {
            get => accelCoef;
            set => accelCoef = value;
        }

        [NodeMember]
        public float ControlCoef
        {
            get => controlCoef;
            set => controlCoef = value;
        }

        [NodeMember]
        public float GravityCoef
        {
            get => gravityCoef;
            set => gravityCoef = value;
        }

        #endregion

        #region Constructors

        private CPlugVehiclePhyModelCustom()
        {

        }

        #endregion

        #region Chunks

        #region 0x000 chunk

        /// <summary>
        /// CPlugVehiclePhyModelCustom 0x000 chunk
        /// </summary>
        [Chunk(0x0911E000)]
        public class Chunk0911E000 : Chunk<CPlugVehiclePhyModelCustom>, IVersionable
        {
            private int version;

            public int Version
            {
                get => version;
                set => version = value;
            }

            public override void ReadWrite(CPlugVehiclePhyModelCustom n, GameBoxReaderWriter rw)
            {
                rw.Int32(ref version);
                rw.Single(ref n.accelCoef);
                rw.Single(ref n.controlCoef);
                rw.Single(ref n.gravityCoef);
            }
        }

        #endregion

        #endregion
    }
}
