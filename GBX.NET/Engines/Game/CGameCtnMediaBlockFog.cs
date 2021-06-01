namespace GBX.NET.Engines.Game
{
    [Node(0x03199000)]
    public class CGameCtnMediaBlockFog : CGameCtnMediaBlock
    {
        #region Properties

        [NodeMember]
        public Key[] Keys { get; set; }

        #endregion

        #region Chunks

        #region 0x000 chunk

        [Chunk(0x03199000)]
        public class Chunk03199000 : Chunk<CGameCtnMediaBlockFog>
        {
            public int Version { get; set; }

            public override void Read(CGameCtnMediaBlockFog n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                n.Keys = r.ReadArray(r1 =>
                {
                    var time = r1.ReadSingle();
                    var intensity = r1.ReadSingle();
                    var skyIntensity = r1.ReadSingle();
                    var distance = r1.ReadSingle();

                    float? coefficient = null;
                    Vec3? color = null;
                    float? cloudsOpacity = null;
                    float? cloudsSpeed = null;

                    if (Version >= 1)
                    {
                        coefficient = r1.ReadSingle();
                        color = r1.ReadVec3();

                        if (Version >= 2)
                        {
                            cloudsOpacity = r1.ReadSingle();
                            cloudsSpeed = r1.ReadSingle();
                        }
                    }

                    return new Key()
                    {
                        Time = time,
                        Intensity = intensity,
                        SkyIntensity = skyIntensity,
                        Distance = distance,
                        Coefficient = coefficient,
                        Color = color,
                        CloudsOpacity = cloudsOpacity,
                        CloudsSpeed = cloudsSpeed
                    };
                });
            }

            public override void Write(CGameCtnMediaBlockFog n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);

                w.Write(n.Keys, (x, w1) =>
                {
                    w1.Write(x.Time);
                    w1.Write(x.Intensity);
                    w1.Write(x.SkyIntensity);
                    w1.Write(x.Distance);

                    if (Version >= 1)
                    {
                        w1.Write(x.Coefficient.GetValueOrDefault(1));
                        w1.Write(x.Coefficient.GetValueOrDefault());

                        if (Version >= 2)
                        {
                            w1.Write(x.CloudsOpacity.GetValueOrDefault(1));
                            w1.Write(x.CloudsSpeed.GetValueOrDefault(1));
                        }
                    }
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public new class Key : CGameCtnMediaBlock.Key
        {
            public float Intensity { get; set; }
            public float SkyIntensity { get; set; }
            public float Distance { get; set; }
            public float? Coefficient { get; set; }
            public Vec3? Color { get; set; }
            public float? CloudsOpacity { get; set; }
            public float? CloudsSpeed { get; set; }
        }

        #endregion
    }
}
