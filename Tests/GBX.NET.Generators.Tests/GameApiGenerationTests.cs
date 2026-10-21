using Microsoft.CodeAnalysis;

namespace GBX.NET.Generators.Tests;

public partial class GenerationTests
{
    [Test]
    public async Task Generate_GameBuilders_SharesSettersAndRecipesWithoutLeakingGameMembers()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static bool Verify()
                    {
                        var builder = Create();
                        return builder.ForMP4().WithModern(7).Build().Modern == 7
                            && builder.ForTM2020().WithModern(8).Build().Modern == 8
                            && builder.ForMP4().Build().Value == 4
                            && builder.ForTM2020().Build().Value == 20;
                    }
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10, MP4, TM2020]
              int Value = 1 [MP4 = 4, TM2020 = 20]
            0x002 [MP4, TM2020]
              int Modern
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var builder = compilation.GetTypeByMetadataName("GBX.NET.Builders.Engines.Game.ExampleBuilder")!;
        var mp4 = builder.GetTypeMembers("MP4").Single();
        var modern = builder.GetTypeMembers("TM2020").Single();
        await Assert.That(mp4.BaseType!.OriginalDefinition).IsEqualTo(modern.BaseType!.OriginalDefinition);
        await Assert.That(((IMethodSymbol)mp4.BaseType.GetMembers("WithModern").Single()).ReturnType).IsEqualTo(mp4);
        for (var type = builder.GetTypeMembers("TM10").Single(); type is not null; type = type.BaseType)
            await Assert.That(type.GetMembers("WithModern")).IsEmpty();
        await Assert.That(builder.GetMembers().OfType<IMethodSymbol>().Count(static m => m.Name.StartsWith("CreateRecipe"))).IsEqualTo(2);
        var commonBase = compilation.GetTypeByMetadataName("GBX.NET.Builders.Engines.Game.ExampleBuilderBase`1")!;
        await Assert.That(commonBase.GetMembers("SetValue")).IsEmpty();
        await Assert.That(commonBase.GetMembers("SetModern")).IsEmpty();
        // Availability remains correct through shared contracts as well as
        // through the concrete builder stages.
        var oldView = compilation.GetTypeByMetadataName("GBX.NET.Interfaces.Game.IExampleTM10")!;
        await Assert.That(oldView.GetMembers("Modern")).IsEmpty();
        await Assert.That(oldView.AllInterfaces.SelectMany(static i => i.GetMembers("Modern"))).IsEmpty();
        var sharedView = compilation.GetTypeByMetadataName("GBX.NET.Interfaces.Game.IExampleMP4")!.AllInterfaces
            .Single(static i => i.GetMembers("Modern").Length > 0);
        await Assert.That(compilation.GetTypeByMetadataName("GBX.NET.Interfaces.Game.IExampleTM2020")!.AllInterfaces.Contains(sharedView)).IsTrue();
        await Assert.That((bool)InvokeGame(compilation, "Verify")!).IsTrue();
    }

    [Test]
    public async Task Generate_GameBuilders_RetainsOnlyHelpersReferencedByTheirHandwrittenPartials()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Builders.Engines.Game
            {
                public partial class ExampleBuilder
                {
                    private const string HelperName = nameof(SetSpare);
                    private const string IgnoredText = "SetValue";

                    [System.Obsolete("Select ForMP4 or ForTM2020 before assigning Modern.")]
                    public ExampleBuilder WithLegacyModern(int value)
                    {
                        System.Action<GBX.NET.Builders.NodeBuilderState<GBX.NET.Engines.Game.Example>, int> assign = SetModern;
                        assign(state, value);
                        return this;
                    }
                }
            }
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static bool Verify()
                    {
                        if (Create().WithLegacyModern(17).ForMP4().Build().Modern != 17) return false;
                        try { Create().WithLegacyModern(18).ForTM10().Build(); return false; }
                        catch (System.InvalidOperationException) { return true; }
                    }
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10, MP4, TM2020]
              int Value
              int Spare
            0x002 [MP4, TM2020]
              int Modern
            """), new Text("Engines/Game/Other.chunkl", """
            Other 0x03044000
            0x001 [MP4]
              int Modern
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var commonBase = compilation.GetTypeByMetadataName("GBX.NET.Builders.Engines.Game.ExampleBuilderBase`1")!;
        await Assert.That(commonBase.GetMembers("SetValue")).IsEmpty();
        await Assert.That(commonBase.GetMembers("SetModern").Length).IsEqualTo(1);
        await Assert.That(commonBase.GetMembers("SetSpare").Length).IsEqualTo(1);
        await Assert.That(compilation.GetTypeByMetadataName("GBX.NET.Builders.Engines.Game.OtherBuilderBase`1")!.GetMembers("SetModern")).IsEmpty();
        await Assert.That((bool)InvokeGame(compilation, "Verify")!).IsTrue();
    }

    [Test]
    public async Task Generate_GameBuilders_PartialReferenceHelperSnapshotsItsInput()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Builders.Engines.Game
            {
                public partial class ExampleBuilder
                {
                    public partial class MP4
                    {
                        public MP4 WithLegacyLink(GBX.NET.NodeRef<GBX.NET.Engines.Game.CChild> value)
                        {
                            SetLinkReference(state, value);
                            return this;
                        }
                    }
                }
            }
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static bool Verify()
                    {
                        var child = CChild.Create().ForMP4().Build();
                        var reference = new GBX.NET.NodeRef<CChild> { Node = child };
                        var builder = Create().ForMP4().WithLegacyLink(reference);
                        reference.Clear();
                        return object.ReferenceEquals(builder.Build().Link, child);
                    }
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [MP4]
              CChild Link
            """), new Text("Engines/Game/CChild.chunkl", """
            CChild 0x03044000
            0x001 [MP4]
              int Value
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var commonBase = compilation.GetTypeByMetadataName("GBX.NET.Builders.Engines.Game.ExampleBuilderBase`1")!;
        await Assert.That(commonBase.GetMembers("SetLink")).IsEmpty();
        await Assert.That(commonBase.GetMembers("SetLinkReference").Length).IsEqualTo(1);
        await Assert.That((bool)InvokeGame(compilation, "Verify")!).IsTrue();
    }

    [Test]
    public async Task Generate_GameBuilders_PreservesPartialSettersWhenSharingOtherMembers()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Builders.Engines.Game
            {
                public partial class ExampleBuilder
                {
                    public partial class MP4
                    {
                        public MP4 WithModern(int value) { SetModern(state, value + 1); return this; }
                        partial void CustomizeNode(GBX.NET.Engines.Game.Example node) => node.Shared += 10;
                    }
                }
            }
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static bool Verify() => Create().ForMP4().WithModern(7).WithShared(2).Build().Modern == 8
                        && Create().ForMP4().WithShared(2).Build().Shared == 12
                        && Create().ForTM2020().WithShared(2).Build().Shared == 2;
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10, MP4, TM2020]
              int Value
            0x002 [MP4, TM2020]
              int Modern
              int Shared
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        await Assert.That((bool)InvokeGame(compilation, "Verify")!).IsTrue();
    }

    [Test]
    public async Task Generate_GameInterfaces_PreservesProjectionsWhenTheBaseHasALegacyRawContract()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Interfaces.Game
            {
                public partial interface IBase : GBX.NET.IClassVersion<GBX.NET.Engines.Game.Base>
                {
                    GBX.NET.Engines.Game.CChild? Link { get; set; }
                }
            }
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static bool Verify()
                    {
                        var child = CChild.Create().ForMP4().Build();
                        var node = Create().ForMP4().WithLink(child).Build();
                        GBX.NET.Interfaces.Game.IExample projected = node;
                        GBX.NET.Interfaces.Game.IBase legacy = node;
                        return object.ReferenceEquals(projected.Link, child) && object.ReferenceEquals(legacy.Link, child);
                    }
                }
            }
            """, new Text("Engines/Game/Base.chunkl", """
            Base 0x03044000
            - abstract
            0x001 [MP4, TM2020]
              CChild Link
            """), new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            - inherits: Base
            0x001 [MP4, TM2020]
              int Value
            """), new Text("Engines/Game/CChild.chunkl", """
            CChild 0x03045000
            0x001 [MP4, TM2020]
              int Value
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var projected = compilation.GetTypeByMetadataName("GBX.NET.Interfaces.Game.IExample")!;
        await Assert.That(((IPropertySymbol)projected.GetMembers("Link").Single()).Type.Name).IsEqualTo("IChild");
        await Assert.That((bool)InvokeGame(compilation, "Verify")!).IsTrue();
    }

    [Test]
    public async Task Generate_GameBuilders_FiltersVersionedMembersAndPreservesStageTypes()
    {
        var (result, compilation) = RunGame("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [MP4.v0, TM2020.v1]
              version
              int Shared
              v1+
                int Modern
            """));

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var outer = compilation.GetTypeByMetadataName("GBX.NET.Builders.Engines.Game.ExampleBuilder")!;
        var mp4 = outer.GetTypeMembers("MP4").Single();
        var modern = outer.GetTypeMembers("TM2020").Single();
        await Assert.That(mp4.GetMembers("WithModern")).IsEmpty();
        await Assert.That(modern.GetMembers("WithModern").Length).IsEqualTo(1);
        foreach (var name in new[] { "ForMP4", "ForTM2", "ForSM" })
            await Assert.That(((IMethodSymbol)outer.GetMembers(name).Single()).ReturnType).IsEqualTo(mp4);
        var shared = (IMethodSymbol)mp4.BaseType!.GetMembers("WithShared").Single();
        await Assert.That(shared.ReturnType).IsEqualTo(mp4);
        await Assert.That(((IMethodSymbol)mp4.GetMembers("Build").Single()).ReturnType.Name).IsEqualTo("Example");
        await Assert.That(((IMethodSymbol)mp4.GetMembers("BuildInterface").Single()).ReturnType.Name).IsEqualTo("IExampleMP4");
    }

    [Test]
    public async Task Generate_GameBuilders_ReplaysOverlappingSettersInCallerOrder()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    [GBX.NET.Attributes.AppliedWithChunk<Chunk03043001>]
                    public bool Enabled { get => (Flags & 1) != 0; set => Flags = value ? Flags | 1 : Flags & ~1; }

                    public static int Verify() => Create().ForTM10().WithFlags(0).WithEnabled(true).WithFlags(8).Build().Flags;
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10]
              int Flags
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        await Assert.That((int)InvokeGame(compilation, "Verify")!).IsEqualTo(8);
    }

    [Test]
    public async Task Generate_GameBuilders_SnapshotsStagesAndBuildContainers()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static GBX.NET.Builders.Engines.Game.ExampleBuilder Create(int value)
                        => new GBX.NET.Builders.Engines.Game.ExampleBuilder().WithValues(value);

                    public static bool Verify()
                    {
                        var outer = Create().WithValues(1);
                        var selected = outer.ForTM10();
                        outer.Values![0] = 2;
                        var first = selected.Build();
                        first.Values![0] = 3;
                        return selected.Build().Values![0] == 1 && outer.ForTM10().Build().Values![0] == 2;
                    }
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10]
              int[1] Values
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        await Assert.That((bool)InvokeGame(compilation, "Verify")!).IsTrue();
    }

    [Test]
    public async Task Generate_GameBuilders_RunsPartialHooksBeforeFinalValidation()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Builders.Engines.Game
            {
                public partial class ExampleBuilder
                {
                    public partial class TM10
                    {
                        partial void CustomizeNode(GBX.NET.Engines.Game.Example node) => node.Value = 42;
                    }
                }
            }
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static int Verify() => Create().ForTM10().Build().Value;
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10]
              int Value
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        await Assert.That((int)InvokeGame(compilation, "Verify")!).IsEqualTo(42);
    }

    [Test]
    public async Task Generate_GameBuilders_OmitsPrivateSerializationMembers()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    private int Secret { get; set; }
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10]
              int Secret
              int PublicValue
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var contract = compilation.GetTypeByMetadataName("GBX.NET.Interfaces.Game.IExample")!;
        await Assert.That(contract.GetMembers("Secret")).IsEmpty();
        await Assert.That(contract.GetMembers("PublicValue").Length).IsEqualTo(1);
    }

    [Test]
    public async Task Generate_GameBuilders_ExplicitRecipeSupportsOtherwiseUnqualifiedChunks()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Engines.Game
            {
                [GBX.NET.Attributes.NodeBuilderRecipe(GBX.NET.GameVersion.MP4, typeof(Example.Chunk03043001))]
                public partial class Example
                {
                    public static int Verify() => Create().ForSM().WithValue(7).Build().Value;
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              int Value
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        await Assert.That((int)InvokeGame(compilation, "Verify")!).IsEqualTo(7);
    }

    [Test]
    public async Task Generate_GameBuilders_InvalidRecipeDoesNotPreventSiblingGeneration()
    {
        var (result, _) = RunGame("""
            namespace GBX.NET.Engines.Game
            {
                [GBX.NET.Attributes.NodeBuilderRecipe(GBX.NET.GameVersion.Unspecified)]
                public partial class Bad { }
            }
            """, new Text("Engines/Game/Bad.chunkl", """
            Bad 0x03044000
            0x001 [TM10]
              int Value
            """), new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10]
              int Value
            """));
        await Assert.That(result.Diagnostics.Select(static d => d.Id).ToArray()).IsEquivalentTo(new[] { "GBXNETGEN300" });
        await Assert.That(result.GeneratedSources.Any(static s => s.HintName == "Builders/Game/ExampleBuilder.g.cs")).IsTrue();
    }

    [Test]
    public async Task Generate_GameBuilders_CustomizationCannotRemoveVersionGuarantees()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Builders.Engines.Game
            {
                public partial class ExampleBuilder
                {
                    public partial class TM10
                    {
                        partial void CustomizeNode(GBX.NET.Engines.Game.Example node)
                            => node.GetChunk<GBX.NET.Engines.Game.Example.Chunk03043001>()!.Version = 99;
                    }
                }
            }
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static bool Verify()
                    {
                        try { Create().ForTM10().Build(); return false; }
                        catch (System.InvalidOperationException) { return true; }
                    }
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10.v1]
              version
              int Value
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        await Assert.That((bool)InvokeGame(compilation, "Verify")!).IsTrue();
    }

    [Test]
    public async Task Generate_GameInterfaces_UsesMethodGameVersionAvailability()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    [GBX.NET.Attributes.MethodGameVersion(GBX.NET.GameVersion.TM2020)]
                    public int ReadValue(int offset = 0) => Value + offset;
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            - inherits: Base
            0x001 [MP4, TM2020]
              int Value
            """), new Text("Engines/Game/Base.chunkl", """
            Base 0x03044000
            - abstract
            0x001 [MP4, TM2020]
              int BaseValue
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        await Assert.That(compilation.GetTypeByMetadataName("GBX.NET.Interfaces.Game.IExampleMP4")!.GetMembers("ReadValue")).IsEmpty();
        await Assert.That(compilation.GetTypeByMetadataName("GBX.NET.Interfaces.Game.IExampleTM2020")!.GetMembers("ReadValue").Length).IsEqualTo(1);
        await Assert.That(compilation.GetTypeByMetadataName("GBX.NET.Interfaces.Game.IExampleTM2020")!.AllInterfaces
            .Any(static i => i.Name == "IBaseTM2020")).IsTrue();
    }

    private static (GeneratorRunResult Result, Compilation Compilation) RunGame(string source, params Text[] files)
    {
        using var runtime = typeof(GenerationTests).Assembly.GetManifestResourceStream("NodeBuilderState.cs")!;
        using var reader = new StreamReader(runtime);
        using var property = typeof(GenerationTests).Assembly.GetManifestResourceStream("NodeBuilderProperty.cs")!;
        using var propertyReader = new StreamReader(property);
        return Run(source + GameSupport, true, files, "using System; using System.Collections.Generic;\n" + reader.ReadToEnd() +
            "\n" + propertyReader.ReadToEnd().Replace("namespace GBX.NET.Builders;", ""));
    }

    private static object? InvokeGame(Compilation compilation, string method)
    {
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        if (!emitted.Success) throw new InvalidOperationException(string.Join("\n", emitted.Diagnostics));
        var assembly = System.Reflection.Assembly.Load(stream.ToArray());
        return assembly.GetType("GBX.NET.Engines.Game.Example")!.GetMethod(method)!.Invoke(null, null);
    }

    private const string GameSupport = """

        namespace GBX.NET { public interface IClassVersion<T> { } }
        namespace GBX.NET.Attributes
        {
            [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
            public class NodeBuilderRecipeAttribute(GBX.NET.GameVersion game, params System.Type[] chunks) : System.Attribute { }
            public class MethodGameVersionAttribute(GBX.NET.GameVersion game) : System.Attribute { }
        }
        namespace GBX.NET.Exceptions
        {
            public class MemberNullException(string member) : System.Exception(member) { }
        }
        namespace GBX.NET.Serialization
        {
            public partial class GbxReaderWriter
            {
                public void Array<T>(ref T[]? values, int length) { }
            }
        }
        namespace GBX.NET.Builders
        {
            internal static class NodeBuilderValidation
            {
                public static void Validate(GBX.NET.Engines.Game.CMwNod node, GBX.NET.GameVersion game) { }
            }
        }
        namespace GBX.NET.Engines.Game
        {
            public partial class CMwNod
            {
                public System.Collections.Generic.List<GBX.NET.Serialization.Chunking.IChunk> Chunks { get; } = new();
                public T? GetChunk<T>() where T : class, GBX.NET.Serialization.Chunking.IChunk
                    => System.Linq.Enumerable.FirstOrDefault(System.Linq.Enumerable.OfType<T>(Chunks));
            }
        }
        """;
}
