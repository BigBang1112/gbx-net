using GBX.NET.Engines.Script;
using EScriptType = GBX.NET.Engines.Script.CScriptTraitsMetadata.EScriptType;

namespace GBX.NET.Tests.Unit;

public class CScriptTraitsMetadataBuilderTests
{
    [Test]
    public async Task TypeBuilderRecordsPrimitiveMemberTypesAndIgnoresSameTypeDuplicates()
    {
        var builder = CScriptTraitsMetadata.ScriptStructType.Create("Vehicle")
            .WithBoolean("Active")
            .WithInteger("Lap")
            .WithReal("Speed")
            .WithText("Driver")
            .WithVec2("Screen")
            .WithVec3("Position")
            .WithInt2("Cell")
            .WithInt3("Grid")
            .WithBoolean("Active");

        var type = builder.Build();

        await Assert.That(type.Name).IsEqualTo("Vehicle");
        await Assert.That(type.Members.Count).IsEqualTo(8);
        await Assert.That(type.Members["Active"].Type.Type).IsEqualTo(EScriptType.Boolean);
        await Assert.That(type.Members["Lap"].Type.Type).IsEqualTo(EScriptType.Integer);
        await Assert.That(type.Members["Speed"].Type.Type).IsEqualTo(EScriptType.Real);
        await Assert.That(type.Members["Driver"].Type.Type).IsEqualTo(EScriptType.Text);
        await Assert.That(type.Members["Screen"].Type.Type).IsEqualTo(EScriptType.Vec2);
        await Assert.That(type.Members["Position"].Type.Type).IsEqualTo(EScriptType.Vec3);
        await Assert.That(type.Members["Cell"].Type.Type).IsEqualTo(EScriptType.Int2);
        await Assert.That(type.Members["Grid"].Type.Type).IsEqualTo(EScriptType.Int3);
    }

    [Test]
    public async Task TraitBuilderKeepsValuesAndTheirDeclaredTypes()
    {
        var builder = CScriptTraitsMetadata.ScriptStructTrait.Create("Vehicle")
            .WithBoolean("Active", true)
            .WithInteger("Lap", 3)
            .WithReal("Speed", 12.5f)
            .WithText("Driver", "Alice")
            .WithVec3("Position", new Vec3(1, 2, 3));

        var trait = builder.Build();

        await Assert.That(trait.Type.ToString()).IsEqualTo("Vehicle");
        await Assert.That(trait.Value.Count).IsEqualTo(5);
        await Assert.That((bool)trait.Value["Active"].GetValue()!).IsTrue();
        await Assert.That(trait.Value["Lap"].GetValue()).IsEqualTo(3);
        await Assert.That(trait.Value["Speed"].GetValue()).IsEqualTo(12.5f);
        await Assert.That(trait.Value["Driver"].GetValue()).IsEqualTo("Alice");
        await Assert.That(trait.Value["Position"].GetValue()).IsEqualTo(new Vec3(1, 2, 3));
    }

    [Test]
    public async Task NestedStructAndArrayTypeRetainTheirNames()
    {
        var nested = CScriptTraitsMetadata.ScriptStructType.Create("Stats").WithInteger("Wins");
        var parent = CScriptTraitsMetadata.ScriptStructType.Create("Player").WithStruct("Stats", nested).Build();
        var array = new CScriptTraitsMetadata.ScriptArrayType(
            new CScriptTraitsMetadata.ScriptType(EScriptType.Integer), parent);

        await Assert.That(parent.Members["Stats"].Type.Type).IsEqualTo(EScriptType.Struct);
        await Assert.That(parent.Members["Stats"].Type.ToString()).IsEqualTo("Stats");
        await Assert.That(array.Type).IsEqualTo(EScriptType.Array);
        await Assert.That(array.ToString()).IsEqualTo("Player[Integer]");
    }
}
