using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x03199000)]
    [DebuggerTypeProxy(typeof(DebugView))]
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
                n.Keys = r.ReadArray(i =>
                {
                    var time = r.ReadSingle();
                    var intensity = r.ReadSingle();
                    var skyIntensity = r.ReadSingle();
                    var distance = r.ReadSingle();

                    float? coefficient = null;
                    Vec3? color = null;
                    float? cloudsOpacity = null;
                    float? cloudsSpeed = null;

                    if (Version >= 1)
                    {
                        coefficient = r.ReadSingle();
                        color = r.ReadVec3();

                        if (Version >= 2)
                        {
                            cloudsOpacity = r.ReadSingle();
                            cloudsSpeed = r.ReadSingle();
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

                w.Write(n.Keys, x =>
                {
                    w.Write(x.Time);
                    w.Write(x.Intensity);
                    w.Write(x.SkyIntensity);
                    w.Write(x.Distance);

                    if (Version >= 1)
                    {
                        w.Write(x.Coefficient.GetValueOrDefault(1));
                        w.Write(x.Coefficient.GetValueOrDefault());

                        if (Version >= 2)
                        {
                            w.Write(x.CloudsOpacity.GetValueOrDefault(1));
                            w.Write(x.CloudsSpeed.GetValueOrDefault(1));
                        }
                    }
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Key : MediaBlockKey
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

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaBlockFog node;

            public Key[] Keys => node.Keys;

            public DebugView(CGameCtnMediaBlockFog node) => this.node = node;
        }

        #endregion
    }
}
