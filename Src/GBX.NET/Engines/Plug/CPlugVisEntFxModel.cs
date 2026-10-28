using GBX.NET.Engines.Function;
using GBX.NET.Engines.Graphic;

namespace GBX.NET.Engines.Plug;

public partial class CPlugVisEntFxModel
{
    public CPlugVisEntFxModel()
    {
        EliminatedTeleportIntens = new PlugCurve
        {
            Flags = 2,
            Count = 3,
            Values = [new(0, 1), new(1, 1), new(2.7f, 0)]
        };
        EliminatedHighlight = new PlugCurve
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
}
