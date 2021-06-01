namespace GBX.NET.Engines.Game
{
    [Node(0x03081000)]
    public class CGameCtnMediaBlockFxBlurDepth : CGameCtnMediaBlockFx
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x001 chunk

        [Chunk(0x03081001)]
        public class Chunk03081001 : Chunk<CGameCtnMediaBlockFxBlurDepth>
        {
            public override void Read(CGameCtnMediaBlockFxBlurDepth n, GameBoxReader r, GameBoxWriter unknownW)
            {
                n.Keys = r.ReadArray(r1 => new Key()
                {
                    Time = r1.ReadSingle(),
                    LensSize = r1.ReadSingle(),
                    ForceFocus = r1.ReadBoolean(),
                    FocusZ = r1.ReadSingle(),
                });
            }

            public override void Write(CGameCtnMediaBlockFxBlurDepth n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(n.Keys, (x, w1) =>
                {
                    w1.Write(x.Time);
                    w1.Write(x.LensSize);
                    w1.Write(x.ForceFocus);
                    w1.Write(x.FocusZ);
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public new class Key : CGameCtnMediaBlock.Key
        {
            public float LensSize { get; set; }
            public bool ForceFocus { get; set; }
            public float FocusZ { get; set; }
        }

        #endregion
    }
}
