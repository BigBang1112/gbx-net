using GBX.NET.Engines.Function;
using GBX.NET.Engines.Plug;
using GBX.NET.Serialization;
using static GBX.NET.Tests.Unit.Engines.Plug.CPlugCustomBeamModelLayoutTests;

namespace GBX.NET.Tests.Unit.Engines.Plug;

[Category("Unit")]
public class PlugCurveDirectTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ReadWriteDirect_FunctionKeys_ConvertsAndKeepsNode(bool simpleNode)
    {
        var curve = new PlugCurve();
        var payload = Payload(w =>
        {
            w.Write(0x05002001);
            w.Write(2);
            w.Write(0f);
            w.Write(1f);
            w.Write(0x0501A002);
            w.Write(2);
            w.Write(5f);
            w.Write(6f);
            w.Write((int)CFuncKeysReal.ERealInterp.Linear);
            w.Write(0); // ForceTangentMinX
            w.Write(1); // ForceTangentMaxX
            w.Write(0f);
            w.Write(0f);
            w.Write(0xFACADE01);
        });

        await RoundTrip(payload, rw =>
        {
            if (simpleNode) curve.ReadWriteDirect<CPlugCurveSimpleNod>(rw);
            else curve.ReadWriteDirect<CFuncKeysReal>(rw);
        });

        await Assert.That(curve.Values).IsEquivalentTo([new Vec2(0, 5), new Vec2(1, 6)], CollectionOrdering.Matching);
        await Assert.That(curve.Flags).IsEqualTo((ushort)0x20);
        await Assert.That(curve.Curve!.GetType()).IsEqualTo(simpleNode ? typeof(CPlugCurveSimpleNod) : typeof(CFuncKeysReal));
    }

    [Test]
    public async Task ReadWriteDirect_EmptyCurve_WritesEmptyNode()
    {
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            new PlugCurve().ReadWriteDirect<CFuncKeysReal>(new GbxReaderWriter(writer));
        }

        await Assert.That(stream.ToArray()).IsEquivalentTo(BitConverter.GetBytes(0xFACADE01), CollectionOrdering.Matching);
    }

    [Test]
    public async Task ReadWriteDirect_InlineValues_WritesFunctionNode()
    {
        var curve = new PlugCurve { Values = [new(0, 2), new(1, 3)] };
        using var stream = new MemoryStream();
        using (var writer = new GbxWriter(stream))
        {
            curve.ReadWriteDirect<CFuncKeysReal>(new GbxReaderWriter(writer));
        }

        stream.Position = 0;
        var restored = new PlugCurve();
        using (var reader = new GbxReader(stream))
        {
            restored.ReadWriteDirect<CFuncKeysReal>(new GbxReaderWriter(reader));
        }

        await Assert.That(restored.Values).IsEquivalentTo(curve.Values, CollectionOrdering.Matching);
    }
}
