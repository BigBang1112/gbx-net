using GBX.NET.Engines.Function;
using GBX.NET.Engines.Graphic;

namespace GBX.NET.Engines.Plug;

public partial class CPlugVisEntFxModel
{
    public CPlugVisEntFxModel()
    {
        EliminatedTeleportIntens = new Keys
        {
            Flags = 2,
            Count = 3,
            Values = [new(0, 1), new(1, 1), new(2.7f, 0)]
        };
        EliminatedHighlight = new Keys
        {
            Count = 6,
            Values = [new(0, 0), new(0.005f, 6), new(0.05f, 6), new(0.12f, 1), new(0.3f, 0.25f), new(2.5f, 0)]
        };
        TacticalLight = new GxLightBall
        {
            Color = Vec3.One,
            Intensity = 1,
            DiffuseIntensity = 1,
            SpecularIntens = 1,
            SpecularPower = 1,
            ShadowIntensity = 1,
            FlareIntensity = 1,
            ShadowRGB = Vec3.One,
            FlareSize = 5
        };
        ((GxLight)TacticalLight).Flags = (GxLight.EFlags)0x4D;
        TacticalLight.CreateChunk<GxLight.Chunk04001009>();
        TacticalLight.CreateChunk<GxLightPoint.Chunk04003004>();
        TacticalLight.CreateChunk<GxLightBall.Chunk04002008>();
        TacticalLight.CreateChunk<GxLightBall.Chunk04002009>();
        TacticalLight.CreateChunk<GxLightBall.Chunk0400200A>();
    }

    public partial class Keys : IVersionable
    {
        public void ReadWrite(GbxReaderWriter rw, int version = 0)
        {
            if (version == 0)
            {
                if (rw.Writer is not null && curve is null && curveFile is null && values is { Length: > 0 })
                {
                    curve = new CFuncKeysReal
                    {
                        Xs = Array.ConvertAll(values, value => value.X),
                        Ys = Array.ConvertAll(values, value => value.Y),
                        RealInterp = (flags & 1) != 0 ? CFuncKeysReal.ERealInterp.None
                            : (flags & 0x10) != 0 ? CFuncKeysReal.ERealInterp.Hermite
                            : (flags & 2) != 0 ? CFuncKeysReal.ERealInterp.SmoothStep
                            : CFuncKeysReal.ERealInterp.Linear,
                        ForceTangentMaxX = (flags & 0x20) != 0
                    };
                    curve.CreateChunk<CFuncKeys.Chunk05002001>();
                    curve.CreateChunk<CFuncKeysReal.Chunk0501A002>();
                }

                rw.NodeRef<CFuncKeysReal>(ref curve, ref curveFile);
                if (rw.Reader is not null)
                {
                    ReadLegacyCurve();
                }
                return;
            }

            rw.VersionInt32(this);
            count = rw.Int32(values?.Length ?? 0);
            if (Version == 1)
            {
                rw.Byte(ref flagsV1);
                if (rw.Reader is not null)
                {
                    Flags = (ushort)(flagsV1 > 0 ? 1 : 0);
                }
            }
            if (Version == 2)
            {
                rw.Int32(ref capacity);
            }
            if (Version >= 3)
            {
                rw.UInt16(ref flags);
            }
            rw.Array<Vec2>(ref values, count);
        }

        private void ReadLegacyCurve()
        {
            // Trackmania 2020 also accepts a CPlugCurveSimpleNod in the legacy slot.
            if (curve is CPlugCurveSimpleNod simple && simple.Chunks.Get<CPlugCurveSimpleNod.Chunk09185000>() is { } chunk)
            {
                Flags = simple.Version2 switch
                {
                    1 => (ushort)(chunk.U01 > 0 ? 1 : 0),
                    >= 3 => unchecked((ushort)chunk.U03),
                    _ => 0
                };
                Values = chunk.U04 is null ? [] : Array.ConvertAll(chunk.U04, value => new Vec2(
                    BitConverter.ToSingle(BitConverter.GetBytes((uint)value), 0),
                    BitConverter.ToSingle(BitConverter.GetBytes((uint)(value >> 32)), 0)));
            }
            else
            {
                Flags = curve?.RealInterp switch
                {
                    CFuncKeysReal.ERealInterp.None => 1,
                    CFuncKeysReal.ERealInterp.Hermite => 0x10,
                    CFuncKeysReal.ERealInterp.SmoothStep => 2,
                    _ => 0
                };
                if (curve?.ForceTangentMaxX == true)
                {
                    Flags |= 0x20;
                }
                Values = new Vec2[Math.Min(curve?.Xs?.Length ?? 0, curve?.Ys?.Length ?? 0)];
                for (var i = 0; i < Values.Length; i++)
                {
                    Values[i] = new Vec2(curve!.Xs![i], curve.Ys![i]);
                }
            }
            count = Values.Length;
        }
    }
}
