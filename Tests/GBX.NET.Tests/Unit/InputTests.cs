using GBX.NET.Inputs;

namespace GBX.NET.Tests.Unit;

[Category("Unit")]
public class InputTests
{
    [Test]
    [Arguments("Accelerate")]
    [Arguments("Brake")]
    [Arguments("Horn")]
    [Arguments("Respawn")]
    [Arguments("SteerLeft")]
    [Arguments("SteerRight")]
    public async Task StateInputsPreserveNameAndPressedData(string name)
    {
        var pressed = Input.Parse(default, name, 128);
        var released = Input.Parse(default, name, 0);

        await Assert.That(pressed.GetType().Name).IsEqualTo(name);
        await Assert.That(((IInputState)pressed).Pressed).IsTrue();
        await Assert.That(((IInputState)released).Pressed).IsFalse();
        await Assert.That(Input.GetName(pressed)).IsEqualTo(name);
        await Assert.That(Input.GetData(pressed)).IsEqualTo(128u);
        await Assert.That(Input.GetData(released)).IsEqualTo(0u);
    }

    [Test]
    [Arguments(0xFF0000u, 65536)]
    [Arguments(0x00010000u, -65536)]
    [Arguments(0u, 0)]
    public async Task SteerParsingPreservesSignedExtremes(uint data, int expectedValue)
    {
        var input = (Steer)Input.Parse(default, "Steer", data);

        await Assert.That(input.Value).IsEqualTo(expectedValue);
        await Assert.That(Input.GetData(input)).IsEqualTo(data);
    }

    [Test]
    [Arguments(-65536, -1f)]
    [Arguments(32768, 0.5f)]
    [Arguments(0, 0f)]
    public async Task RealInputValueUsesSignedSixteenBitFraction(int raw, float expected)
    {
        var input = new Steer(default, raw);

        await Assert.That(input.GetValue()).IsEqualTo(expected);
        await Assert.That(input.NormalizedValue).IsEqualTo(expected);
    }

    [Test]
    public async Task UnknownInputKeepsOriginalNameAndPayload()
    {
        var input = Input.Parse(default, "CustomAction", 0xDEADBEEFu);
        var unknown = (await Assert.That(input).IsTypeOf<UnknownInput>())!;

        await Assert.That(unknown.Name).IsEqualTo("CustomAction");
        await Assert.That(unknown.Data).IsEqualTo(0xDEADBEEFu);
    }
}
