using GBX.NET.Engines.Function;
using static GBX.NET.Tests.Unit.Engines.Plug.CPlugCustomBeamModelLayoutTests;

namespace GBX.NET.Tests.Unit.Engines.Function;

[Category("Unit")]
public class CFuncShaderTweakKeysTranssLayoutTests
{
    [Test]
    public async Task ReadWrite_Chunk000_KeepsNullBoneEntries()
    {
        var node = new CFuncShaderTweakKeysTranss();
        var chunk = new CFuncShaderTweakKeysTranss.Chunk05033000();
        var payload = Payload(w =>
        {
            w.Write(2);
            w.Write(-1);
            w.Write(-1);
        });

        await RoundTrip(payload, rw => chunk.ReadWrite(node, rw));

        await Assert.That(node.BoneKeys).IsNotNull();
        await Assert.That(node.BoneKeys!.Count).IsEqualTo(2);
        await Assert.That(node.BoneKeys[0]).IsNull();
        await Assert.That(node.BoneKeys[1]).IsNull();
    }
}
