namespace GBX.NET.Engines.Game;

[Node(0x03199000)]
public sealed class CGameCtnMediaBlockFog : CGameCtnMediaBlock, CGameCtnMediaBlock.IHasKeys
{
    #region Fields

    private IList<Key> keys;

    #endregion

    #region Constructors

    private CGameCtnMediaBlockFog()
    {
        keys = null!;
    }

    #endregion

    #region Properties

    IEnumerable<CGameCtnMediaBlock.Key> IHasKeys.Keys
    {
        get => keys.Cast<CGameCtnMediaBlock.Key>();
        set => keys = value.Cast<Key>().ToList();
    }

    [NodeMember]
    public IList<Key> Keys
    {
        get => keys;
        set => keys = value;
    }

    #endregion

    #region Chunks

    #region 0x000 chunk

    [Chunk(0x03199000)]
    public class Chunk03199000 : Chunk<CGameCtnMediaBlockFog>, IVersionable
    {
        public int Version { get; set; }

        public override void Read(CGameCtnMediaBlockFog n, GameBoxReader r)
        {
            Version = r.ReadInt32();

            n.keys = r.ReadList(r1 =>
            {
                var time = r1.ReadSingle_s();
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

        public override void Write(CGameCtnMediaBlockFog n, GameBoxWriter w)
        {
            w.Write(Version);

            w.Write(n.Keys, (x, w1) =>
            {
                w1.WriteSingle_s(x.Time);
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
