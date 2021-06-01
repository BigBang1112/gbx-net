namespace GBX.NET.Engines.Game
{
    [Node(0x03127000)]
    public class CGameCtnMediaBlockToneMapping : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x004 chunk

        [Chunk(0x03127004)]
        public class Chunk03127004 : Chunk<CGameCtnMediaBlockToneMapping>
        {
            public override void ReadWrite(CGameCtnMediaBlockToneMapping n, GameBoxReaderWriter rw)
            {
                n.Keys = rw.Array(n.Keys, r => new Key()
                {
                    Time = r.ReadSingle(),
                    Exposure = r.ReadSingle(),
                    MaxHDR = r.ReadSingle(),
                    LightTrailScale = r.ReadSingle(),
                    Unknown = r.ReadInt32()
                },
                (x, w) =>
                {
                    w.Write(x.Time);
                    w.Write(x.Exposure);
                    w.Write(x.MaxHDR);
                    w.Write(x.LightTrailScale);
                    w.Write(x.Unknown);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public new class Key : CGameCtnMediaBlock.Key
        {
            public float Exposure { get; set; }
            public float MaxHDR { get; set; }
            public float LightTrailScale { get; set; }
            public int Unknown { get; set; }
        }

        #endregion
    }
}
