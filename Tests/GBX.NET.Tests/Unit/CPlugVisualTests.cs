using GBX.NET.Engines.Plug;

namespace GBX.NET.Tests.Unit;

public class CPlugVisualTests
{
    [Test]
    public async Task NamedFlagPropertiesPreserveUnrelatedBits()
    {
        var visual = new CPlugVisual { Flags = 1 << 20 };

        visual.IsGeometryStatic = true;
        visual.IsIndexationStatic = true;
        visual.HasVertexNormals = true;

        await Assert.That(visual.Flags).IsEqualTo((1 << 20) | (1 << 3) | (1 << 5) | (1 << 7));

        visual.IsIndexationStatic = false;

        await Assert.That(visual.IsGeometryStatic).IsTrue();
        await Assert.That(visual.IsIndexationStatic).IsFalse();
        await Assert.That(visual.HasVertexNormals).IsTrue();
        await Assert.That(visual.IsFlagBitSet(20)).IsTrue();
    }

    [Test]
    public async Task SetFlagBitCanClearAnExistingBitWithoutChangingOthers()
    {
        var visual = new CPlugVisual { Flags = (1 << 2) | (1 << 9) };

        visual.SetFlagBit(2, false);
        visual.SetFlagBit(11, true);

        await Assert.That(visual.Flags).IsEqualTo((1 << 9) | (1 << 11));
    }
}
