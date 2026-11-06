using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

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
                        var builder = Example.Create();
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
        foreach (var generated in result.GeneratedSources)
            await Assert.That(generated.SyntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>()).IsEmpty();
        var builder = compilation.GetTypeByMetadataName("GBX.NET.Builders.Engines.Game.ExampleBuilder")!;
        var mp4 = builder.GetTypeMembers("MP4").Single();
        var modern = builder.GetTypeMembers("TM2020").Single();
        await Assert.That(mp4.BaseType!.OriginalDefinition).IsEqualTo(modern.BaseType!.OriginalDefinition);
        await Assert.That(((IMethodSymbol)mp4.BaseType.GetMembers("WithModern").Single()).ReturnType).IsEqualTo(mp4);
        for (var type = builder.GetTypeMembers("TM10").Single(); type is not null; type = type.BaseType)
            await Assert.That(type.GetMembers("WithModern")).IsEmpty();
        await Assert.That(builder.GetMembers().OfType<IMethodSymbol>().Count(static m => m.Name.StartsWith("CreateRecipe"))).IsEqualTo(2);
        var builderSource = result.GeneratedSources.Single(static s => s.HintName == "Builders/Game/ExampleBuilder.g.cs").SyntaxTree;
        await Assert.That(builderSource.GetRoot().DescendantNodes().OfType<ObjectCreationExpressionSyntax>()
            .Count(static creation => creation.Type.ToString().EndsWith(".Chunk03043001", StringComparison.Ordinal))).IsEqualTo(1);
        var commonBase = compilation.GetTypeByMetadataName("GBX.NET.Builders.Engines.Game.ExampleBuilderBase`1")!;
        await Assert.That(commonBase.BaseType!.Name).IsEqualTo("NodeBuilder");
        await Assert.That(commonBase.GetMembers("state")).IsEmpty();
        await Assert.That(commonBase.GetMembers("SetValue")).IsEmpty();
        await Assert.That(commonBase.GetMembers("SetModern")).IsEmpty();
        foreach (var stage in new[] { builder, mp4, modern, builder.GetTypeMembers("TM10").Single() })
        {
            for (var type = stage; type is not null; type = type.BaseType)
                await Assert.That(type.GetMembers().OfType<IPropertySymbol>()).IsEmpty();
        }
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
    public async Task Generate_GameBuilders_SharedRecipePrefixesPreserveVersionsAndFreshNodes()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static bool Verify()
                    {
                        var legacy = Example.Create().ForTM10().Build();
                        var modern = Example.Create().ForMP4().Build();
                        var stage = Example.Create().ForTM2020();
                        var extended = stage.Build();
                        var repeated = stage.Build();
                        return legacy.Chunks.Count == 1 && modern.Chunks.Count == 1 && extended.Chunks.Count == 2
                            && legacy.GetChunk<Chunk03043001>()!.Version == 1
                            && modern.GetChunk<Chunk03043001>()!.Version == 2
                            && extended.GetChunk<Chunk03043001>()!.Version == 2
                            && extended.GetChunk<Chunk03043002>()!.Version == 3
                            && !object.ReferenceEquals(extended, repeated)
                            && !object.ReferenceEquals(extended.GetChunk<Chunk03043001>(), repeated.GetChunk<Chunk03043001>());
                    }
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10.v1, MP4.v2, TM2020.v2]
              version
              int Value
            0x002 [TM2020.v3]
              version
              int Modern
            """));

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var builder = result.GeneratedSources.Single(static s => s.HintName == "Builders/Game/ExampleBuilder.g.cs").SyntaxTree;
        await Assert.That(builder.GetRoot().DescendantNodes().OfType<ObjectCreationExpressionSyntax>()
            .Count(static creation => creation.Type.ToString().EndsWith(".Chunk03043001", StringComparison.Ordinal))).IsEqualTo(1);
        await Assert.That((bool)InvokeGame(compilation, "Verify")!).IsTrue();
    }

    [Test]
    public async Task Generate_GameBuilders_DescriptorsShareCopiesWithoutCompilerDelegateCaches()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static bool Verify()
                    {
                        var input = new[] { 7 };
                        var stage = Example.Create().WithFirst(input).WithSecond(input).ForTM10();
                        input[0] = 8;
                        var first = stage.Build();
                        first.First![0] = 9;
                        var second = stage.Build();
                        var assignments = typeof(Example).Assembly.GetType("GBX.NET.Builders.Engines.Game.ExampleBuilderAssignments")!;
                        var flags = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic;
                        return second.First![0] == 7 && second.Second![0] == 7
                            && !object.ReferenceEquals(second.First, second.Second)
                            && assignments.GetNestedTypes(System.Reflection.BindingFlags.NonPublic).Length == 0
                            && System.Linq.Enumerable.Count(assignments.GetMethods(flags), method => method.Name.StartsWith("Copy", System.StringComparison.Ordinal)) == 1;
                    }
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10]
              int[1] First
              int[1] Second
            """));

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        await Assert.That((bool)InvokeGame(compilation, "Verify")!).IsTrue();
    }

    [Test]
    public async Task Generate_GameBuilders_ReferenceSettersRejectNullWithoutChangingState()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static bool Verify()
                    {
                        var child = CChild.Create().ForMP4().Build();
                        var stage = Example.Create().ForMP4();
                        if (!object.ReferenceEquals(stage.WithChildReference(new GBX.NET.NodeRef<CChild> { Node = child }), stage)) return false;
                        try { stage.WithChildReference(null!); return false; }
                        catch (System.ArgumentNullException exception)
                        {
                            return exception.ParamName == "value" && object.ReferenceEquals(stage.Build().Child, child);
                        }
                    }
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [MP4]
              CChild Child
            """), new Text("Engines/Game/CChild.chunkl", """
            CChild 0x03044000
            0x001 [MP4]
              int Value
            """));

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
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
                        if (Example.Create().WithLegacyModern(17).ForMP4().Build().Modern != 17) return false;
                        try { Example.Create().WithLegacyModern(18).ForTM10().Build(); return false; }
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
                        var builder = Example.Create().ForMP4().WithLegacyLink(reference);
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
    public async Task Generate_GameBuilders_FluentSetterHonorsAHandwrittenPropertyCustomization()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Builders.Engines.Game
            {
                public partial class ExampleBuilder
                {
                    public partial class MP4
                    {
                        private int Modern { set => SetModern(state, value + 1); }
                    }
                }
            }
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static bool Verify() => Example.Create().ForMP4().WithModern(7).Build().Modern == 8
                        && Example.Create().ForTM2020().WithModern(7).Build().Modern == 7;
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10, MP4, TM2020]
              int Value
            0x002 [MP4, TM2020]
              int Modern
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
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
                    public static bool Verify() => Example.Create().ForMP4().WithModern(7).WithShared(2).Build().Modern == 8
                        && Example.Create().ForMP4().WithShared(2).Build().Shared == 12
                        && Example.Create().ForTM2020().WithShared(2).Build().Shared == 2;
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
                        var node = Example.Create().ForMP4().WithLink(child).Build();
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

                    public static int Verify() => Example.Create().ForTM10().WithFlags(0).WithEnabled(true).WithFlags(8).Build().Flags;
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
                        var outer = Example.Create().WithValues(1);
                        var selected = outer.ForTM10();
                        outer.WithValues(2);
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
                    public static int Verify() => Example.Create().ForTM10().Build().Value;
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
                    public static int Verify() => Example.Create().ForSM().WithValue(7).Build().Value;
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
    public async Task Generate_GameBuilders_BuildsEveryGameWithoutIgnoredChunks()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static bool Verify()
                    {
                        var legacy = Example.Create().ForTM10().WithValue(7).Build();
                        var modern = Example.Create().ForMP4().WithValue(8).Build();
                        return legacy.Value == 7 && legacy.Chunks.Count == 2 && legacy.GetChunk<Chunk03043002>() is not null
                            && modern.Value == 8 && modern.Chunks.Count == 1 && modern.GetChunk<Chunk03043003>() is null;
                    }
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10, MP4]
              int Value
            0x002 [TM10]
            0x003 (ignore) [MP4]
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        // A chunk without a writer stays in the recipe and fails when written.
        // An ignored chunk is never written, so the recipe leaves it out.
        await Assert.That((bool)InvokeGame(compilation, "Verify")!).IsTrue();
    }

    [Test]
    public async Task Generate_RetiredChunks_StayQualifiedButLeaveTheGameRecipe()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static bool Verify()
                    {
                        var legacy = Example.Create().ForTM10().WithValue(7).Build();
                        var modern = Example.Create().ForMP4().WithValue(8).Build();
                        var latest = Example.Create().ForTM2020().WithValue(9).Build();
                        return legacy.Chunks.Count == 3
                            && modern.Chunks.Count == 1 && modern.GetChunk<Chunk03043002>() is null && modern.GetChunk<Chunk03043003>() is null
                            && latest.Chunks.Count == 2 && latest.GetChunk<Chunk03043002>() is not null && latest.GetChunk<Chunk03043003>() is null
                            && (new Chunk03043002().GameVersion & GBX.NET.GameVersion.MP4) != 0;
                    }
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10, MP4, TM2020]
              int Value
            0x002 (retired: MP4) [TM10, MP4, TM2020]
            0x003 (retired: MP4 | TM2020) [TM10, MP4, TM2020]
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        await Assert.That((bool)InvokeGame(compilation, "Verify")!).IsTrue();
    }

    [Test]
    [Arguments("TM2021")]
    [Arguments("TMF")]
    [Arguments("MP4 | TM2021")]
    public async Task Generate_RetiredGameOutsideTheQualifiers_ReportsInvalidMetadata(string games)
    {
        var (result, _) = RunGame("", new Text("Engines/Game/Example.chunkl", $"""
            Example 0x03043000
            0x001 (retired: {games}) [TM10, MP4]
              int Value
            """));

        await Assert.That(result.Diagnostics.Single(static d => d.Id == "GBXNETGEN300").GetMessage()).Contains("Chunk Chunk03043001 retires");
    }

    [Test]
    public async Task Generate_GameBuilders_OmitsNodesThatCannotBeWritten()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Engines.Game
            {
                [GBX.NET.Attributes.WriteNotSupported]
                public partial class Base { }
            }
            """, new Text("Engines/Game/Base.chunkl", """
            Base 0x03044000
            0x001 [TM10]
              int Value
            """), new Text("Engines/Game/Derived.chunkl", """
            Derived 0x03045000
            - inherits: Base
            0x001 [TM10]
              int Other
            """), new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10]
              int Value
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var hints = result.GeneratedSources.Select(static s => s.HintName).ToArray();
        await Assert.That(hints.Where(static h => h.StartsWith("Builders/", StringComparison.Ordinal) &&
            (h.Contains("Base") || h.Contains("Derived")))).IsEmpty();
        await Assert.That(hints).Contains("Builders/Game/ExampleBuilder.g.cs");
        // The game version interfaces remain available for reading.
        await Assert.That(compilation.GetTypeByMetadataName("GBX.NET.Interfaces.Game.IBaseTM10")).IsNotNull();
        await Assert.That(compilation.GetTypeByMetadataName("GBX.NET.Interfaces.Game.IDerivedTM10")).IsNotNull();
    }

    [Test]
    public async Task Generate_GameBuilders_CustomizationCanChangeRecipeVersions()
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
                    public static int Verify() => Example.Create().ForTM10().Build().GetChunk<Chunk03043001>()!.Version;
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
        var builder = result.GeneratedSources.Single(static s => s.HintName == "Builders/Game/ExampleBuilder.g.cs").SourceText.ToString();
        await Assert.That(builder.Contains("ValidateRecipe", StringComparison.Ordinal)).IsFalse();
        await Assert.That(builder.Contains("ValidateNode", StringComparison.Ordinal)).IsFalse();
        await Assert.That((int)InvokeGame(compilation, "Verify")!).IsEqualTo(99);
    }

    [Test]
    public async Task Generate_GameBuilders_LeavesRequiredMemberChecksToTheCaller()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public string Required { get; set; } = null!;

                    public static bool Verify() => Example.Create().ForTM10().Build().Required is null
                        && ((Example)Example.Create().ForTM10().BuildInterface()).Required is null;
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10]
              string Required
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

    [Test]
    public async Task Generate_BuilderPackage_EmitsOnlyBuildersFromLibrarySources()
    {
        var layout = new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10, MP4, TM2020]
              int Value = 1 [MP4 = 4, TM2020 = 20]
            0x002 [MP4, TM2020]
              int Modern
            """);
        var (libraryResult, library) = Run("namespace GBX.NET.Engines.Game { public partial class Example { } }" + GameSupport, true, [layout]);
        await Assert.That(libraryResult.Diagnostics).IsEmpty();
        await AssertNoErrors(library);
        await Assert.That(libraryResult.GeneratedSources.Select(static s => s.HintName))
            .Contains("Interfaces/Game/IExample.g.cs");
        await Assert.That(libraryResult.GeneratedSources.Any(static s => s.HintName.StartsWith("Builders/", StringComparison.Ordinal))).IsFalse();
        await Assert.That(library.GetTypeByMetadataName("GBX.NET.Engines.Game.Example")!.GetMembers("Create")).IsEmpty();

        var (result, builders) = RunBuilders(library, layout, """
            using GBX.NET.Engines.Game;

            namespace Checks
            {
                public static class Check
                {
                    public static bool Verify() => Example.Create().ForMP4().WithModern(7).Build().Modern == 7
                        && Example.Create().ForTM10().Build().Value == 1;
                }
            }
            """);
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(builders);
        await Assert.That(result.GeneratedSources.Select(static s => s.HintName)).IsEquivalentTo(new[]
        {
            "Builders/Game/ExampleBuilderAssignments.g.cs", "Builders/Game/ExampleBuilder.g.cs",
            "Builders/Game/GameNodeBuilderExtensions.g.cs"
        });
        await Assert.That(InvokeBuilders(library, builders)).IsEqualTo(true);
    }

    [Test]
    public async Task Generate_GameBuilders_RejectsCombinedRecipeGames()
    {
        var (result, _) = RunGame("""
            namespace GBX.NET.Engines.Game
            {
                [GBX.NET.Attributes.NodeBuilderRecipe(GBX.NET.GameVersion.TM10 | GBX.NET.GameVersion.MP4, typeof(Example.Chunk03043001))]
                public partial class Example { }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              int Value
            """));
        await Assert.That(result.Diagnostics.Select(static d => d.Id).ToArray()).IsEquivalentTo(new[] { "GBXNETGEN300" });
        await Assert.That(result.GeneratedSources.Any(static s => s.HintName == "Builders/Game/ExampleBuilder.g.cs")).IsFalse();
    }

    [Test]
    public async Task Generate_GameBuilders_ShareRecipesDifferingOnlyInConstructorVersions()
    {
        var (result, compilation) = RunGame("""
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static int Verify() => Example.Create().ForTM10().Build().GetChunk<Chunk03043001>()!.Version * 10 +
                        Example.Create().ForMP4().Build().GetChunk<Chunk03043001>()!.Version;
                }
            }
            """, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10.v1, MP4.v2]
              version
              int Value
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var builder = result.GeneratedSources.Single(static s => s.HintName == "Builders/Game/ExampleBuilder.g.cs").SourceText.ToString();
        await Assert.That(builder).Contains("CreateRecipe0(");
        await Assert.That(builder.Contains("CreateRecipe1(", StringComparison.Ordinal)).IsFalse();
        await Assert.That((int)InvokeGame(compilation, "Verify")!).IsEqualTo(12);
    }

    [Test]
    public async Task Generate_BuilderPackage_ReportsInvalidRecipe()
    {
        var layout = new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10]
              int Value
            """);
        var (_, library) = Run("""
            namespace GBX.NET.Engines.Game
            {
                [GBX.NET.Attributes.NodeBuilderRecipe(GBX.NET.GameVersion.Unspecified)]
                public partial class Example { }
            }
            """ + GameSupport, true, [layout]);
        var (result, builders) = RunBuilders(library, layout, "");
        await Assert.That(result.Diagnostics.Select(static d => d.Id).ToArray()).IsEquivalentTo(new[] { "GBXNETGEN300" });
        await Assert.That(result.Diagnostics.Single().GetMessage()).Contains("NodeBuilderRecipe requires one known GameVersion value.");
    }

    [Test]
    public async Task Generate_GameInterfaces_UnrelatedEditReusesGameSymbols()
    {
        var options = new CSharpParseOptions(LanguageVersion.Preview);
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(static x => MetadataReference.CreateFromFile(x));
        var compilation = CSharpCompilation.Create("Fixture", [CSharpSyntaxTree.ParseText(Support + GameSupport, options)], references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        GeneratorDriver driver = CSharpGeneratorDriver.Create([new GbxGenerator().AsSourceGenerator()],
            additionalTexts: [new Text("Engines/Game/Example.chunkl", "Example 0x03043000\n0x001 [TM10]\n  int Value\n")],
            parseOptions: options, driverOptions: new GeneratorDriverOptions(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: true));
        driver = driver.RunGenerators(compilation);
        driver = driver.RunGenerators(compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText("namespace Other { class Unrelated { } }", options)));

        var result = driver.GetRunResult().Results.Single();
        await Assert.That(result.TrackedSteps["PlannedEngineTypes"].SelectMany(static s => s.Outputs)
            .All(static o => o.Reason == IncrementalStepRunReason.Cached)).IsTrue();
        await Assert.That(result.TrackedSteps["GameSymbols"].SelectMany(static s => s.Outputs)
            .All(static o => o.Reason == IncrementalStepRunReason.Unchanged)).IsTrue();
        await Assert.That(result.TrackedOutputSteps["SourceOutput"].SelectMany(static s => s.Outputs)
            .All(static o => o.Reason is IncrementalStepRunReason.Cached or IncrementalStepRunReason.Unchanged)).IsTrue();
    }

    private static (GeneratorRunResult Result, Compilation Compilation) RunGame(string source, params Text[] files)
        => Run(source + GameSupport, true, files, "using System; using System.Collections.Generic;\n" + Resource("NodeBuilderState.cs") +
            "\n" + Resource("NodeBuilderProperty.cs").Replace("namespace GBX.NET.Builders;", "") +
            "\n" + Resource("NodeBuilder.cs").Replace("namespace GBX.NET.Builders;", ""));

    // Mirrors GBX.NET.Builders: the library is a reference, and its handwritten
    // declarations reach the generator as additional files.
    private static (GeneratorRunResult Result, Compilation Compilation) RunBuilders(Compilation library, Text layout, string source)
    {
        var options = new CSharpParseOptions(LanguageVersion.Preview);
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(static x => (MetadataReference)MetadataReference.CreateFromFile(x)).Append(library.ToMetadataReference());
        var librarySources = library.SyntaxTrees.Where(static tree => tree.FilePath.Length == 0)
            .Select(static (tree, index) => new Text($"Library{index}.cs", tree.ToString()));
        var trees = new[] { source, "global using System; global using System.Collections.Generic; global using System.Linq;",
            Resource("NodeBuilder.cs"), Resource("NodeBuilderState.cs"), Resource("NodeBuilderProperty.cs") }
            .Select(text => CSharpSyntaxTree.ParseText(text, options));
        var compilation = CSharpCompilation.Create(library.AssemblyName + ".Builders", trees, references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        GeneratorDriver driver = CSharpGeneratorDriver.Create([new GbxGenerator().AsSourceGenerator()],
            additionalTexts: librarySources.Prepend(layout), parseOptions: options, optionsProvider: new BuilderTargetOptions());
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var output, out _);
        return (driver.GetRunResult().Results.Single(), output);
    }

    private static string Resource(string name)
    {
        using var stream = typeof(GenerationTests).Assembly.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static object? InvokeGame(Compilation compilation, string method)
    {
        var assembly = System.Reflection.Assembly.Load(Emit(compilation).ToArray());
        return assembly.GetType("GBX.NET.Engines.Game.Example")!.GetMethod(method)!.Invoke(null, null);
    }

    private static object? InvokeBuilders(Compilation library, Compilation builders)
    {
        var context = new AssemblyLoadContext(null, isCollectible: true);
        try
        {
            var libraryAssembly = context.LoadFromStream(Emit(library));
            context.Resolving += (_, name) => name.Name == library.AssemblyName ? libraryAssembly : null;
            return context.LoadFromStream(Emit(builders)).GetType("Checks.Check")!.GetMethod("Verify")!.Invoke(null, null);
        }
        finally
        {
            context.Unload();
        }
    }

    private static MemoryStream Emit(Compilation compilation)
    {
        var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        if (!emitted.Success) throw new InvalidOperationException(string.Join("\n", emitted.Diagnostics));
        stream.Position = 0;
        return stream;
    }

    private sealed class BuilderTargetOptions : AnalyzerConfigOptionsProvider
    {
        private static readonly AnalyzerConfigOptions Options = new Values();

        public override AnalyzerConfigOptions GlobalOptions => Options;
        public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) => Options;
        public override AnalyzerConfigOptions GetOptions(AdditionalText textFile) => Options;

        private sealed class Values : AnalyzerConfigOptions
        {
            public override bool TryGetValue(string key, out string value)
            {
                value = "Builders";
                return key == "build_property.GbxGeneratorTarget";
            }
        }
    }

    private const string GameSupport = """

        namespace GBX.NET { public interface IClassVersion<T> { } }
        namespace GBX.NET.Attributes
        {
            [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
            public class NodeBuilderRecipeAttribute(GBX.NET.GameVersion game, params System.Type[] chunks) : System.Attribute { }
            public class MethodGameVersionAttribute(GBX.NET.GameVersion game) : System.Attribute { }
            public class WriteNotSupportedAttribute : System.Attribute { }
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
                public string? String(string? value) => value;
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
