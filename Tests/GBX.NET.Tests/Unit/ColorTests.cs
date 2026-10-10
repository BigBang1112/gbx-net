namespace GBX.NET.Tests.Unit;

[Category("Unit")]
public class ColorTests
{
    [Test]
    public async Task ArgbConstructorAndPackingPreserveChannelOrder()
    {
        const int argb = unchecked((int)0x80123456);
        var color = new Color(argb);

        await Assert.That(color.R).IsEqualTo(18f);
        await Assert.That(color.G).IsEqualTo(52f);
        await Assert.That(color.B).IsEqualTo(86f);
        await Assert.That(color.A).IsEqualTo(128f);
        await Assert.That(color.ToArgb()).IsEqualTo(argb);
        await Assert.That(color.ToRgba()).IsEqualTo(unchecked((int)0x80563412));
    }

    [Test]
    public async Task TupleAndVectorConversionsPreserveChannels()
    {
        Color tupleColor = (1, 2, 3, 4);
        var vectorColor = (Color)new Vec4(1, 2, 3, 4);

        await Assert.That(tupleColor).IsEqualTo(new Color(1, 2, 3, 4));
        await Assert.That(vectorColor).IsEqualTo(tupleColor);
    }
}
