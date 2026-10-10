using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GBX.NET.Generators.Tests;

public class GenerationTests
{
    [Test]
    [Arguments(false, 0)]
    [Arguments(false, 1)]
    [Arguments(true, 0)]
    [Arguments(true, 1)]
    public async Task SerializationAssignmentsApplyOnlyWhenReading(bool archive, int structureKind)
    {
        var targetType = archive ? "Child" : "Example";
        var write = archive ? "node.Write(writer)" : "chunk.Write(node, writer)";
        var read = archive ? "node.Read(reader)" : "chunk.Read(node, reader)";
        var readWrite = archive ? "node.ReadWrite(rw)" : "chunk.ReadWrite(node, rw)";
        var (result, compilation) = Run($$"""
            namespace GBX.NET.Engines.Game;
            public partial class Example
            {
                [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = {{structureKind}})]
                public partial class Chunk03043001 { }
                [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = {{structureKind}})]
                public partial class Child { }

                public static string Verify()
                {
                    var node = new {{targetType}} { Value = 7 };
                    var chunk = new Chunk03043001();
                    var writer = new GBX.NET.Serialization.GbxWriter();
                    var reader = new GBX.NET.Serialization.GbxReader(3);
                    var rw = new GBX.NET.Serialization.GbxReaderWriter(writer);
                    {{(structureKind == 0 ? readWrite : write)}};
                    var afterWrite = node.Value + "," + node.Calls;
                    {{(structureKind == 0 ? "rw = new GBX.NET.Serialization.GbxReaderWriter(); " + readWrite + ";" : "")}}
                    var withoutReader = node.Value + "," + node.Calls;
                    rw = new GBX.NET.Serialization.GbxReaderWriter(reader);
                    {{(structureKind == 0 ? readWrite : read)}};
                    var afterRead = node.Value + "," + node.Calls;
                    {{(structureKind == 0 ? "rw = new GBX.NET.Serialization.GbxReaderWriter(new GBX.NET.Serialization.GbxReader(4), writer); " + readWrite + ";" : write + ";")}}
                    return afterWrite + ";" + withoutReader + ";" + afterRead + ";"
                        + node.Value + "," + node.Calls + ";" + string.Join(",", writer.Values);
                }
            }
            {{(archive ? "public partial class Example { public partial class Child" : "public partial class Example")}}
            {
                public int Calls { get; private set; }
                public int AssignmentValue { get { Calls++; return Value * 2; } }
            }
            {{(archive ? "}" : "")}}
            """, new Text("Engines/Game/Example.chunkl", $$"""
            Example 0x03043000
            {{(archive ? "archive Child" : "0x001")}}
              int Value
              Value = AssignmentValue
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var methods = Engine(result).GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().ToArray();
        if (structureKind == 0)
        {
            await Assert.That(methods.Single(x => x.Identifier.ValueText == "ReadWrite").ToString())
                .Contains("if (rw.Reader is not null)");
        }
        else
        {
            await Assert.That(methods.Single(x => x.Identifier.ValueText == "Read").ToString()).Contains("AssignmentValue");
            await Assert.That(methods.Single(x => x.Identifier.ValueText == "Write").ToString()).DoesNotContain("AssignmentValue");
        }
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Engines.Game.Example")!;
        await Assert.That(type.GetMethod("Verify")!.Invoke(null, null)).IsEqualTo(structureKind == 0
            ? "7,0;7,0;6,1;8,2;7,4" : "7,0;7,0;6,1;6,1;7,6");
    }

    [Test]
    [Arguments("version")]
    [Arguments("versionb")]
    public async Task VersionDefaultsAreRejectedForChunksWithExplicitVersions(string keyword)
    {
        foreach (var qualifiers in new[] { "TM2020.v7", "TM10, TM2020.v7" })
        {
            var (result, _) = Run("", new Text("Engines/Game/Example.chunkl", $"Example 0x03043000\n0x001 [{qualifiers}]\n  {keyword} = 7\n"));
            var diagnostic = await Assert.That(result.Diagnostics).HasSingleItem();
            await Assert.That(diagnostic.Id).IsEqualTo("GBXNETGEN200");
            await Assert.That(diagnostic.GetMessage()).Contains("A version default is not supported when the chunk declares explicit game versions");
            await Assert.That(result.GeneratedSources).IsEmpty();
        }
    }

    [Test]
    [Arguments("version")]
    [Arguments("versionb")]
    public async Task VersionDefaultsRemainSupportedWithoutExplicitQualifierVersions(string keyword)
    {
        var (result, compilation) = Run("", new Text("Engines/Game/Example.chunkl", $"""
            Example 0x03043000
            0x001
              {keyword} = 7
            0x002 [TM2020]
              {keyword} = 7
            """), compile: true);
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var versions = Engine(result).GetRoot().DescendantNodes().OfType<PropertyDeclarationSyntax>()
            .Where(x => x.Identifier.ValueText == "Version").ToArray();
        await Assert.That(versions.Length).IsEqualTo(2);
        await Assert.That(versions).All(x => x.Initializer?.Value.ToString() == "7");
    }

    [Test]
    public async Task ChunkVersionBranchesHaveNoFallbackAssignment()
    {
        const string source = """
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static string Verify()
                    {
                        static string Values(GBX.NET.GameVersion game) => new Chunk03043001(game).Version + "," + new Chunk03043002(game).Version;
                        return Values(GBX.NET.GameVersion.Unspecified) + ";" + Values(GBX.NET.GameVersion.TM10)
                            + ";" + Values(GBX.NET.GameVersion.TM2020)
                            + ";" + Values(GBX.NET.GameVersion.TM10 | GBX.NET.GameVersion.TM2020);
                    }
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM2020.v7]
              version
            0x002 [TM10.v0, TM2020.v7]
              version
            """), compile: true);
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var constructors = Engine(result).GetRoot().DescendantNodes().OfType<ConstructorDeclarationSyntax>()
            .Where(x => x.ParameterList.Parameters.Count == 1).ToArray();
        var constant = constructors.Single(x => x.Identifier.ValueText == "Chunk03043001");
        await Assert.That(constant.Body!.Statements.Count).IsEqualTo(1);
        var constantBranch = (IfStatementSyntax)constant.Body.Statements[0];
        await Assert.That(constantBranch.Condition.ToString()).IsEqualTo("gameVersion == GameVersion.TM2020");
        await Assert.That(constantBranch.Else).IsNull();
        var mixed = constructors.Single(x => x.Identifier.ValueText == "Chunk03043002");
        var branch = mixed.Body!.Statements.OfType<IfStatementSyntax>().Single();
        await Assert.That(branch.Condition.ToString()).IsEqualTo("gameVersion == GameVersion.TM10");
        var otherBranch = (IfStatementSyntax)branch.Else!.Statement;
        await Assert.That(otherBranch.Condition.ToString()).IsEqualTo("gameVersion == GameVersion.TM2020");
        await Assert.That(otherBranch.Else).IsNull();
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Engines.Game.Example")!;
        await Assert.That(type.GetMethod("Verify")!.Invoke(null, null)).IsEqualTo("0,0;0,0;7,7;0,0");
    }

    [Test]
    [Arguments("version")]
    [Arguments("versionb")]
    public async Task GameSpecificVersionDefaultsAreRejected(string keyword)
    {
        foreach (var scope in new[] { "0x001 [TM10.v0]", "archive", "archive Child" })
        {
            var (result, _) = Run("", new Text("Engines/Game/Example.chunkl", $"Example 0x03043000\n{scope}\n  {keyword} = 1 [TM10 = 0]\n"));
            var diagnostic = await Assert.That(result.Diagnostics).HasSingleItem();
            await Assert.That(diagnostic.Id).IsEqualTo("GBXNETGEN200");
            await Assert.That(diagnostic.GetMessage()).Contains("Specify versions on the chunk with game.vN qualifiers instead");
            await Assert.That(result.GeneratedSources).IsEmpty();
        }
    }

    [Test]
    [Arguments("version")]
    [Arguments("versionb")]
    public async Task ChunkConstructorsOnlyAssignExplicitQualifierVersions(string keyword)
    {
        const string source = """
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static string Verify()
                    {
                        static string Values(GBX.NET.GameVersion game) => new Chunk03043001(game).Version + "," + new Chunk03043002(game).Version;
                        return new Chunk03043001().Version + ";" + Values(GBX.NET.GameVersion.TM10)
                            + ";" + Values(GBX.NET.GameVersion.TM2020)
                            + ";" + Values(GBX.NET.GameVersion.TM10 | GBX.NET.GameVersion.TM2020)
                            + ";" + Values((GBX.NET.GameVersion)32);
                    }
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", $"""
            Example 0x03043000
            0x001 [TM10.v0, TM2020.v2]
              {keyword}
            0x002 [TM10, TM2020.v3]
              {keyword}
            """), compile: true);
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var root = Engine(result).GetRoot();
        var constructors = root.DescendantNodes().OfType<ConstructorDeclarationSyntax>().ToArray();
        await Assert.That(constructors.Count(x => x.ParameterList.Parameters.Count == 1)).IsEqualTo(2);
        await Assert.That(constructors.Single(x => x.Identifier.ValueText == "Example").ParameterList.Parameters).IsEmpty();
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Engines.Game.Example")!;
        await Assert.That(type.GetMethod("Verify")!.Invoke(null, null)).IsEqualTo("0;0,0;2,3;0,0;0,0");
    }

    [Test]
    public async Task ChunkVersionQualifiersKeepHandwrittenConstructors()
    {
        const string source = """
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public partial class Chunk03043001 { public Chunk03043001() { Version = 9; } }
                    public partial class Chunk03043002
                    {
                        public Chunk03043002() : this(GBX.NET.GameVersion.Unspecified) { }
                        public Chunk03043002(GBX.NET.GameVersion gameVersion) { Version = 8; }
                    }
                    public static string Verify() => new Chunk03043001().Version + "," + new Chunk03043002(GBX.NET.GameVersion.TM10).Version;
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10.v0]
              version
            0x002 [TM10.v1]
              version
            """), compile: true);
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        await Assert.That(Engine(result).GetRoot().DescendantNodes().OfType<ConstructorDeclarationSyntax>())
            .All(x => x.Identifier.ValueText == "Example");
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Engines.Game.Example")!;
        await Assert.That(type.GetMethod("Verify")!.Invoke(null, null)).IsEqualTo("9,8");
    }

    [Test]
    public async Task GameDefaultConstructorsCombineEnumConditionsAndKeepExclusiveBranches()
    {
        const string source = """
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static string Verify()
                    {
                        static string Values(Example node) => $"{node.First},{node.Second},{node.Third}";
                        return Values(new Example()) + ";" + Values(new Example(GBX.NET.GameVersion.TM10))
                            + ";" + Values(new Example(GBX.NET.GameVersion.TM2020))
                            + ";" + Values(new Example(GBX.NET.GameVersion.TM10 | GBX.NET.GameVersion.TM2020))
                            + ";" + Values(new Example((GBX.NET.GameVersion)32));
                    }
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              int First = 2 [TM10 = 5, TM2020 = 5]
              int Second = First + 2 [TM10 = First + 10, TM2020 = First + 10]
              int Third = 4 [TM10 = 6, TM2020 = 7]
            """), compile: true);
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var constructor = Engine(result).GetRoot().DescendantNodes().OfType<ConstructorDeclarationSyntax>()
            .Single(x => x.ParameterList.Parameters.Count == 1);
        var branches = constructor.Body!.Statements.OfType<IfStatementSyntax>().ToArray();
        await Assert.That(branches.Length).IsEqualTo(2);
        await Assert.That(branches[0].Condition.ToString()).IsEqualTo("gameVersion == GameVersion.TM10 || gameVersion == GameVersion.TM2020");
        await Assert.That(((BlockSyntax)branches[0].Statement).Statements.Count).IsEqualTo(2);
        await Assert.That(((BlockSyntax)branches[0].Else!.Statement).Statements.Count).IsEqualTo(2);
        await Assert.That(branches[1].Else!.Statement is IfStatementSyntax { Else.Statement: BlockSyntax }).IsTrue();
        await Assert.That(constructor.ToString()).DoesNotContain("ToString()");
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Engines.Game.Example")!;
        await Assert.That(type.GetMethod("Verify")!.Invoke(null, null)).IsEqualTo("2,4,4;5,15,6;5,15,7;2,4,4;2,4,4");
    }

    [Test]
    public async Task GameDefaultsSelectOneExpressionInSourceOrderAcrossStoredScopes()
    {
        const string source = """
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    private static readonly System.Collections.Generic.List<string> trace = new();
                    private static int Choose(string label, int value) { trace.Add(label); return value; }
                    private static class Defaults
                    {
                        public static int Fallback => Choose("fallback", 1);
                        public static int Legacy => Choose("legacy", 5);
                        public static int Modern => Choose("modern", 8);
                        public static int Skipped => Choose("skipped", 0);
                        public static int SkippedGame => Choose("skipped-game", 4);
                    }
                    public static string Verify()
                    {
                        trace.Clear();
                        var fallback = new Example();
                        var fallbackTrace = string.Join(",", trace);
                        trace.Clear();
                        var legacy = new Example(GBX.NET.GameVersion.TM10);
                        var legacyTrace = string.Join(",", trace);
                        trace.Clear();
                        var modern = new Example(GBX.NET.GameVersion.TM2020);
                        var modernTrace = string.Join(",", trace);
                        var combined = new Example(GBX.NET.GameVersion.TM10 | GBX.NET.GameVersion.TM2020);
                        var child = new Child(GBX.NET.GameVersion.TM10);
                        var chunk = new Chunk03043001(GBX.NET.GameVersion.TM10);
                        return $"{fallback.Shared},{fallback.First},{fallback.Later},{fallback.Skipped}:{fallbackTrace};"
                            + $"{legacy.Shared},{legacy.First},{legacy.Later},{legacy.Skipped}:{legacyTrace};"
                            + $"{modern.Shared},{modern.First},{modern.Later},{modern.Skipped}:{modernTrace};"
                            + $"{combined.Shared};{child.Value};{chunk.Version},{chunk.U01};"
                            + $"{new Chunk03043001().Version},{new Chunk03043001().U01}";
                    }
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            constructor
              Skipped = 99
            0x001 [TM10.v1]
              version
              int Shared = Defaults.Fallback [TM10 = Defaults.Legacy]
              int First = Shared + 1
              int Skipped = Defaults.Skipped [TM10 = Defaults.SkippedGame]
              int Later = Shared + 10
              int UnspecifiedOnly [Unspecified = Defaults.SkippedGame]
              int = 3 [TM10 = 6]
            0x002
              short Shared [TM2020 = Defaults.Modern]
            archive Child
              int Value = 2 [TM10 = 12]
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Engines.Game.Example")!;
        var attribute = type.GetProperty("Shared")!.GetCustomAttributes(false).Single(x => x.GetType().Name == "GameVersionDefaultAttribute"
            && x.GetType().GetProperty("Game")!.GetValue(x)!.ToString() == "TM10");
        await Assert.That(attribute.GetType().GetProperty("DefaultExpression")!.GetValue(attribute)).IsEqualTo("Defaults.Legacy");
        var trace = type.GetField("trace", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!.GetValue(null)!;
        await Assert.That(((System.Collections.ICollection)trace).Count).IsEqualTo(0);
        await Assert.That(type.GetMethod("Verify")!.Invoke(null, null))
            .IsEqualTo("1,2,11,99:fallback;5,6,15,99:legacy;8,1,10,99:modern;1;12;1,6;0,3");
    }

    [Test]
    public async Task GameDefaultsKeepArchiveEmptyInstancesAndTimeValuesIndependent()
    {
        const string source = """
            namespace TmEssentials { public readonly record struct TimeInt32(int TotalMilliseconds); }
            namespace GBX.NET.Serialization
            {
                public partial class GbxReaderWriter
                {
                    public void TimeInt32(ref TmEssentials.TimeInt32 value) { }
                    public void ReadableWritable<T>(ref T? value, int version = 0) where T : class, IReadableWritable, new() { }
                }
            }
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static string Verify()
                    {
                        var first = new Example(GBX.NET.GameVersion.TM10);
                        var second = new Example(GBX.NET.GameVersion.TM10);
                        first.Data.Value = 42;
                        return $"{first.Delay.TotalMilliseconds},{new Example().Delay.TotalMilliseconds},{second.Data.Value}";
                    }
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              int Delay = 10 (time) [TM10 = 20]
              Child Data = empty [TM10 = empty]
            archive Child
              int Value = 7
            """), compile: true);
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Engines.Game.Example")!;
        await Assert.That(type.GetMethod("Verify")!.Invoke(null, null)).IsEqualTo("20,10,7");
    }

    [Test]
    public async Task InlineGameDefaultsInitializeStorageBeforeComputedPropertySetters()
    {
        const string source = """
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static string Verify() => new Example(GBX.NET.GameVersion.TM10).Score + "," + new Example().Score;
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              int Score = 2 [TM10 = 5]
            property int Score
              get = score * 2
              set
                score = value / 2
            """), compile: true);
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Engines.Game.Example")!;
        await Assert.That(type.GetMethod("Verify")!.Invoke(null, null)).IsEqualTo("10,4");
    }

    [Test]
    public async Task GameDefaultsGenerateMemberAttributesAlongsideContextConstructors()
    {
        const string source = """
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static string Verify() => new Example().Shared + "," + new Child().Value + "," + new Chunk03043001().Version
                        + ";" + new Example(GBX.NET.GameVersion.TM10).Shared + "," + new Child(GBX.NET.GameVersion.TM10).Value + "," + new Chunk03043001(GBX.NET.GameVersion.TM10).Version
                        + ";" + new Example(GBX.NET.GameVersion.TM2020).Shared + "," + new Chunk03043001(GBX.NET.GameVersion.TM2020).Version;
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM10.v1, TM2020.v2]
              version
              int Shared = 3 [TM10 = 5]
              int = 6 [TM10 = 7]
            0x002
              short Shared [TM10 = 5, TM2020 = 8]
            archive Child
              int Value = 2 [TM10 = 12]
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var root = Engine(result).GetRoot();
        var shared = root.DescendantNodes().OfType<PropertyDeclarationSyntax>().Single(x => x.Identifier.ValueText == "Shared");
        var defaults = shared.AttributeLists.SelectMany(x => x.Attributes).Where(x => x.Name.ToString() == "GameVersionDefault").ToArray();
        await Assert.That(defaults.Length).IsEqualTo(2);
        await Assert.That(defaults[0].ToString()).IsEqualTo("GameVersionDefault(GameVersion.TM10, 5)");
        await Assert.That(defaults[1].ToString()).IsEqualTo("GameVersionDefault(GameVersion.TM2020, 8)");
        var version = root.DescendantNodes().OfType<PropertyDeclarationSyntax>().Single(x => x.Identifier.ValueText == "Version");
        await Assert.That(version.AttributeLists.SelectMany(x => x.Attributes)).IsEmpty();
        var unknown = root.DescendantNodes().OfType<FieldDeclarationSyntax>().Single(x => x.Declaration.Variables.Any(v => v.Identifier.ValueText == "U01"));
        await Assert.That(unknown.AttributeLists.ToString()).Contains("[GameVersionDefault(GameVersion.TM10, 7)]");
        var child = root.DescendantNodes().OfType<PropertyDeclarationSyntax>().Single(x => x.Identifier.ValueText == "Value");
        await Assert.That(child.AttributeLists.ToString()).Contains("[GameVersionDefault(GameVersion.TM10, 12)]");
        await Assert.That(root.DescendantNodes().OfType<ConstructorDeclarationSyntax>().Count(x => x.ParameterList.Parameters.Count == 1)).IsEqualTo(3);
        await Assert.That(root.DescendantNodes().OfType<ClassDeclarationSyntax>().SelectMany(x => x.AttributeLists).SelectMany(x => x.Attributes))
            .All(x => x.Name.ToString() != "GameVersionDefault");

        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Engines.Game.Example")!;
        await Assert.That(type.GetMethod("Verify")!.Invoke(null, null)).IsEqualTo("3,2,0;5,12,1;8,2");
    }

    [Test]
    public async Task GameDefaultAttributesSupportTimeComputedAndPartialPropertiesWithHandwrittenConstructors()
    {
        const string source = """
            namespace TmEssentials { public readonly record struct TimeInt32(int TotalMilliseconds); }
            namespace GBX.NET.Serialization
            {
                public partial class GbxReaderWriter
                {
                    public void TimeInt32(ref TmEssentials.TimeInt32 value) { }
                }
            }
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public Example() { }
                    public Example(GBX.NET.GameVersion gameVersion) { }
                    private int amount;
                    public partial int Amount { get => amount; set => amount = value; }
                    public static string Verify() => new Example().Score + "," + new Example().Delay.TotalMilliseconds;
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              int Score = 2 [TM10 = 5]
              int Delay = 10 (time) [TM10 = 20]
              int Amount [TM10 = 15]
            property int Score
              get = score * 2
              set
                score = value / 2
            """), compile: true);
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var root = Engine(result).GetRoot();
        foreach (var name in new[] { "Score", "Delay", "Amount" })
        {
            var property = root.DescendantNodes().OfType<PropertyDeclarationSyntax>().Single(x => x.Identifier.ValueText == name);
            await Assert.That(property.AttributeLists.ToString()).Contains("[GameVersionDefault(GameVersion.TM10,");
        }
        await Assert.That(root.ToString()).Contains("[GameVersionDefault(GameVersion.TM10, 20)]");
        await Assert.That(root.DescendantNodes().OfType<ConstructorDeclarationSyntax>()).IsEmpty();
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Engines.Game.Example")!;
        await Assert.That(type.GetMethod("Verify")!.Invoke(null, null)).IsEqualTo("4,10");
    }

    [Test]
    [Arguments("int Shared [TM10 = 1]", "short Shared [TM10 = 2]")]
    [Arguments("int Shared = 1 [TM10 = 3]", "short Shared = 2")]
    public async Task ConflictingDefaultsAcrossRepeatedFieldsProduceAGenerationDiagnostic(string first, string second)
    {
        var (result, _) = Run("", new Text("Engines/Game/Example.chunkl", $"Example 0x03043000\n0x001\n  {first}\n0x002\n  {second}\n"));
        await Assert.That(result.Diagnostics).Contains(x => x.Id == "GBXNETGEN200" && x.GetMessage().Contains("Conflicting defaults"));
    }

    [Test]
    public async Task EmptyGameDefaultsAreRecordedAsExpressions()
    {
        var (result, _) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              int[] Values [TM10 = empty]
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        await Assert.That(Engine(result).GetText().ToString()).Contains("[GameVersionDefault(GameVersion.TM10, DefaultExpression = \"empty\")]");
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task NullableSignedIntegerAttributesSelectSentinelMethods(int structureKind)
    {
        var source = $$"""
            namespace TmEssentials { public readonly record struct TimeInt32(int TotalMilliseconds); }
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public int? Amount { get; set; }
                    public int? Promoted { get; set; }
                    [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = {{structureKind}})]
                    public partial class Chunk03043001 { }
                    [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = {{structureKind}})]
                    public partial class Metadata { }
                }
            }
            namespace GBX.NET.Serialization
            {
                public partial class GbxReader
                {
                    public sbyte? ReadSByteNullable() => null;
                    public short? ReadInt16Nullable() => null;
                    public int? ReadInt32Nullable() => null;
                    public long? ReadInt64Nullable() => null;
                    public System.Int128? ReadInt128Nullable() => null;
                    public TmEssentials.TimeInt32? ReadTimeInt32Nullable() => null;
                }
                public partial class GbxWriter
                {
                    public void WriteSByteNullable(sbyte? value) { }
                    public void WriteInt16Nullable(short? value) { }
                    public void WriteInt32Nullable(int? value) { }
                    public void WriteInt64Nullable(long? value) { }
                    public void WriteInt128Nullable(System.Int128? value) { }
                    public void WriteTimeInt32Nullable(TmEssentials.TimeInt32? value) { }
                }
                public partial class GbxReaderWriter
                {
                    public void SByteNullable(ref sbyte? value) { }
                    public void Int16Nullable(ref short? value) { }
                    public short? Int16Nullable(short? value) => value;
                    public void Int32Nullable(ref int? value) { }
                    public int? Int32Nullable(int? value) => value;
                    public void Int64Nullable(ref long? value) { }
                    public void Int128Nullable(ref System.Int128? value) { }
                    public void TimeInt32Nullable(ref TmEssentials.TimeInt32? value) { }
                }
            }
            """;
        const string fields = """
              sbyte Tiny (nullable)
              short Small (nullable)
              int Score (nullable)
              long Large (nullable)
              int128 Huge (nullable)
              int8 TinyAlias (nullable)
              int16 SmallAlias (nullable)
              int32 ScoreAlias (nullable)
              int64 LargeAlias (nullable)
              int? Explicit (nullable)
              int Duration (nullable, time)
              int Amount (nullable)
              short Promoted (nullable)
              int DefaultScore = 0 (nullable)
              int Temporary (nullable, local, write: Score)
              int (nullable)

            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl",
            "Example 0x03043000\n0x001\n" + fields + "archive Metadata\n" + fields), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var generated = Engine(result).ToString();
        foreach (var (type, field, method) in new[]
        {
            ("sbyte", "tiny", "SByte"), ("short", "small", "Int16"), ("int", "score", "Int32"),
            ("long", "large", "Int64"), ("Int128", "huge", "Int128"),
            ("sbyte", "tinyAlias", "SByte"), ("short", "smallAlias", "Int16"),
            ("int", "scoreAlias", "Int32"), ("long", "largeAlias", "Int64"),
            ("int", "explicit", "Int32"), ("TimeInt32", "duration", "TimeInt32")
        })
        {
            var escaped = field == "explicit" ? "@explicit" : field;
            await Assert.That(generated).Contains($"private {type}? {escaped};");
            if (structureKind == 0)
            {
                await Assert.That(generated).Contains($"rw.{method}Nullable(ref n.{escaped});");
                await Assert.That(generated).Contains($"rw.{method}Nullable(ref this.{escaped});");
            }
            else
            {
                await Assert.That(generated).Contains($"n.{escaped} = r.Read{method}Nullable();");
                await Assert.That(generated).Contains($"w.Write{method}Nullable(n.{escaped});");
                await Assert.That(generated).Contains($"this.{escaped} = r.Read{method}Nullable();");
            }
        }
        await Assert.That(generated).Contains("private int? defaultScore = 0;");
        await Assert.That(generated).Contains("public int? U01;");
        if (structureKind == 0)
        {
            await Assert.That(generated).Contains("n.Amount = rw.Int32Nullable(n.Amount);");
            await Assert.That(generated).Contains("n.Promoted = (int?)rw.Int16Nullable((short?)n.Promoted);");
            await Assert.That(generated).Contains("var temporary = rw.Int32Nullable((int?)(rw.Writer is null ? default : (n.Score)));");
        }
        else
        {
            await Assert.That(generated).Contains("n.Amount = r.ReadInt32Nullable();");
            await Assert.That(generated).Contains("w.WriteInt32Nullable(n.Amount);");
            await Assert.That(generated).Contains("n.Promoted = (int?)r.ReadInt16Nullable();");
            await Assert.That(generated).Contains("w.WriteInt16Nullable((short?)n.Promoted);");
        }
    }

    [Test]
    [Arguments("uint Value (nullable)")]
    [Arguments("float Value (nullable)")]
    [Arguments("int[] Values (nullable)")]
    [Arguments("short[] Values (nullable, list)")]
    [Arguments("dataint Value (nullable)")]
    [Arguments("int<Kind> Value (nullable)")]
    public async Task ReportsUnsupportedNullableAttributes(string field)
    {
        var (result, _) = Run("", new Text("Engines/Game/Example.chunkl", "Example 0x03043000\n0x001\n  " + field));
        await Assert.That(result.Diagnostics).Contains(x => x.Id == "GBXNETGEN200" &&
            x.GetMessage().Contains("nullable attribute requires a scalar signed integer"));
    }

    [Test]
    public async Task SentinelReaderWriterMethodsAllowNullResultsFromNonNullInputs()
    {
        const string source = """
            namespace GBX.NET.Serialization
            {
                public interface IGbxReader
                {
                    int ReadInt32();
                    int? ReadInt32Nullable();
                }
                public interface IGbxWriter
                {
                    void Write(int value);
                    void WriteInt32Nullable(int? value);
                }
                public partial class GbxReaderWriter
                {
                    public IGbxReader? Reader { get; }
                    public IGbxWriter? Writer { get; }
                }
            }
            """;
        var options = new CSharpParseOptions(LanguageVersion.Preview);
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
            .Select(x => MetadataReference.CreateFromFile(x));
        var compilation = CSharpCompilation.Create("Fixture", [CSharpSyntaxTree.ParseText(source, options)], references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        GeneratorDriver driver = CSharpGeneratorDriver.Create([new GbxReaderWriterGenerator().AsSourceGenerator()], parseOptions: options);
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var output, out _);

        await Assert.That(driver.GetRunResult().Diagnostics).IsEmpty();
        await AssertNoErrors(output);
        var generated = driver.GetRunResult().Results.Single().GeneratedSources.Single().SyntaxTree;
        var methods = generated.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>()
            .Where(x => x.Identifier.ValueText == "Int32Nullable").ToArray();
        await Assert.That(methods.Length).IsEqualTo(4);
        foreach (var method in methods)
        {
            await Assert.That(method.AttributeLists.ToString()).DoesNotContain("NotNullIfNotNull");
            await Assert.That(method.ParameterList.ToString()).DoesNotContain("NotNullIfNotNull");
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    public async Task TimeAttributesPreserveTypesAndSerializationMethods(int structureKind)
    {
        var source = $$"""
            namespace TmEssentials
            {
                public readonly record struct TimeInt32(int TotalMilliseconds);
                public readonly record struct TimeSingle(float TotalSeconds);
            }
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = {{structureKind}})]
                    public partial class Chunk03043001 { }
                }
            }
            namespace GBX.NET.Serialization
            {
                public partial class GbxReader
                {
                    public TmEssentials.TimeInt32 ReadTimeInt32() => default;
                    public TmEssentials.TimeInt32? ReadTimeInt32Nullable() => null;
                    public TmEssentials.TimeSingle ReadTimeSingle() => default;
                    public TmEssentials.TimeSingle? ReadTimeSingleNullable() => null;
                    public T[][] ReadJaggedArray<T>() where T : struct => [];
                    public System.Collections.Generic.List<T> ReadList<T>() where T : struct => [];
                }
                public partial class GbxWriter
                {
                    public void Write(TmEssentials.TimeInt32 value) { }
                    public void Write(TmEssentials.TimeInt32? value) { }
                    public void Write(TmEssentials.TimeSingle value) { }
                    public void Write(TmEssentials.TimeSingle? value) { }
                    public void WriteJaggedArray<T>(T[][]? value) where T : struct { }
                    public void WriteList<T>(System.Collections.Generic.List<T>? value) where T : struct { }
                }
                public partial class GbxReaderWriter
                {
                    public TmEssentials.TimeInt32 TimeInt32(TmEssentials.TimeInt32 value) => value;
                    public void TimeInt32(ref TmEssentials.TimeInt32 value) { }
                    public void TimeInt32Nullable(ref TmEssentials.TimeInt32? value) { }
                    public void TimeSingle(ref TmEssentials.TimeSingle value) { }
                    public void TimeSingleNullable(ref TmEssentials.TimeSingle? value) { }
                    public void Array<T>(ref T[]? value, int length) where T : struct { }
                    public void List<T>(ref System.Collections.Generic.List<T>? value) where T : struct { }
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              int Duration = 10000 (time)
              int? OptionalDuration (time)
              float Time = 1.5f (time)
              float? OptionalTime (time)
              int32 Temporary (time, local, write: Duration)
              float[2] Keys (time)
              int[][] Durations (time)
              float[] Times = empty (time, list)
              float (time)
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("private TimeInt32 duration = new TimeInt32(10000);");
        await Assert.That(generated).Contains("private TimeInt32? optionalDuration;");
        await Assert.That(generated).Contains("private TimeSingle time = new TimeSingle(1.5f);");
        await Assert.That(generated).Contains("private TimeSingle? optionalTime;");
        await Assert.That(generated).Contains("private TimeSingle[]? keys;");
        await Assert.That(generated).Contains("private TimeInt32[][]? durations;");
        await Assert.That(generated).Contains("private List<TimeSingle> times = new();");
        await Assert.That(generated).Contains("public TimeSingle U01;");
        if (structureKind == 0)
        {
            await Assert.That(generated).Contains("rw.TimeInt32(ref n.duration);");
            await Assert.That(generated).Contains("rw.TimeInt32Nullable(ref n.optionalDuration);");
            await Assert.That(generated).Contains("rw.TimeSingle(ref n.time);");
            await Assert.That(generated).Contains("rw.TimeSingleNullable(ref n.optionalTime);");
            await Assert.That(generated).Contains("rw.Array<TimeSingle>(ref n.keys!, 2);");
            await Assert.That(generated).Contains("rw.JaggedArray<TimeInt32>(ref n.durations!);");
            await Assert.That(generated).Contains("rw.List<TimeSingle>(ref n.times!);");
        }
        else
        {
            await Assert.That(generated).Contains("n.duration = r.ReadTimeInt32();");
            await Assert.That(generated).Contains("n.optionalDuration = r.ReadTimeInt32Nullable();");
            await Assert.That(generated).Contains("n.time = r.ReadTimeSingle();");
            await Assert.That(generated).Contains("n.optionalTime = r.ReadTimeSingleNullable();");
            await Assert.That(generated).Contains("n.keys = r.ReadArray<TimeSingle>(2);");
            await Assert.That(generated).Contains("n.times = r.ReadList<TimeSingle>();");
            await Assert.That(generated).Contains("w.Write(n.time);");
        }
    }

    [Test]
    public async Task PackedIntegerPropertiesConvertPromotedValuesAndRunTheirSettersFromChunks()
    {
        const string source = """
            namespace GBX.NET.Serialization
            {
                public partial class GbxReaderWriter
                {
                    public void UInt64(ref ulong value) { }
                    public void UInt16(ref ushort value) { }
                }
            }
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public static string Verify()
                    {
                        var node = new Example { Packed = 0xFEDC012345678900 };
                        new Chunk03043001().ReadWrite(node, new GBX.NET.Serialization.GbxReaderWriter(new GBX.NET.Serialization.GbxReader()));
                        node.IsHidden = true;
                        return $"{node.Decoded:X4},{node.ByteField:X2},{node.Visibility:X4},{node.Packed:X16}";
                    }
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            property byte ByteField
              get = Packed & 0xFF
              set
                Packed = (Packed & 0xFFFFFFFFFFFFFF00) | value
            property ushort Visibility
              get = Packed >> 48
              set
                Packed = (Packed & 0xFFFFFFFFFFFF) | ((value & 0xFFFFFFFFFFFFFFFF) << 48)
            property bool IsHidden
              get = (Visibility & 1) != 0
              set
                if value
                  Visibility = Visibility | 1
                else
                  Visibility = Visibility & 0xFFFE
            0x001
              ulong Packed
              ushort Decoded
              if rw.Reader != null
                Decoded = Packed >> 48
                ByteField = 7
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var generated = Engine(result).GetText().ToString();
        await Assert.That(generated).Contains("get => (byte)(Packed& 0xFF);");
        await Assert.That(generated).Contains("n.Decoded = (ushort)(n.Packed>> 48);");
        await Assert.That(generated).Contains("n.ByteField = 7;");

        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Engines.Game.Example")!;
        await Assert.That(type.GetMethod("Verify")!.Invoke(null, null)).IsEqualTo("FEDC,07,FEDD,FEDD012345678907");
    }

    [Test]
    public async Task EmptyNamedArchiveCreatesIndependentInstancesWithArchiveDefaults()
    {
        const string source = """
            namespace GBX.NET.Serialization
            {
                public partial class GbxReaderWriter
                {
                    public void ReadableWritable<T>(ref T? value, int version = 0) where T : class, IReadableWritable, new() { }
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              Child Data = empty
              Child? OptionalData = empty
            archive Child
              int Value = 7
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        await Assert.That(Engine(result).GetText().ToString()).Contains("private Child data = new();");
        await Assert.That(Engine(result).GetText().ToString()).Contains("private Child? optionalData = new();");

        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Engines.Game.Example")!;
        var first = type.GetProperty("Data")!.GetValue(Activator.CreateInstance(type))!;
        var second = type.GetProperty("Data")!.GetValue(Activator.CreateInstance(type))!;
        await Assert.That(first).IsNotSameReferenceAs(second);
        var value = first.GetType().GetProperty("Value")!;
        value.SetValue(first, 42);
        await Assert.That(value.GetValue(second)).IsEqualTo(7);
    }

    [Test]
    public async Task MapsZeroToTimeInt32ZeroUsingTheComparedFieldType()
    {
        const string source = """
            namespace TmEssentials
            {
                public readonly record struct TimeInt32(int TotalMilliseconds)
                {
                    public static TimeInt32 Zero => new(0);
                    public static bool operator <(TimeInt32 left, TimeInt32 right) => left.TotalMilliseconds < right.TotalMilliseconds;
                    public static bool operator >(TimeInt32 left, TimeInt32 right) => left.TotalMilliseconds > right.TotalMilliseconds;
                    public static bool operator <=(TimeInt32 left, TimeInt32 right) => left.TotalMilliseconds <= right.TotalMilliseconds;
                    public static bool operator >=(TimeInt32 left, TimeInt32 right) => left.TotalMilliseconds >= right.TotalMilliseconds;
                }
            }
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    public TmEssentials.TimeInt32 CustomDuration => TmEssentials.TimeInt32.Zero;
                    [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = 1)]
                    public partial class Chunk03043003 { }

                    public static string Verify(int duration)
                    {
                        var node = new Example { Duration = new(duration), Value = 42 };
                        var writer = new GBX.NET.Serialization.GbxWriter();
                        new Chunk03043002().ReadWrite(node, new GBX.NET.Serialization.GbxReaderWriter(writer));
                        return string.Join(",", writer.Values);
                    }
                }
            }
            namespace GBX.NET.Serialization
            {
                public partial class GbxReaderWriter
                {
                    public TmEssentials.TimeInt32 TimeInt32(TmEssentials.TimeInt32 value) => new(Int32(value.TotalMilliseconds));
                    public void TimeInt32(ref TmEssentials.TimeInt32 value) => value = TimeInt32(value);
                    public void TimeInt32Nullable(ref TmEssentials.TimeInt32? value)
                    {
                        var milliseconds = Int32(value?.TotalMilliseconds ?? -1);
                        value = milliseconds == -1 ? null : new TmEssentials.TimeInt32(milliseconds);
                    }
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              int Duration (time)
              int? OptionalDuration (time)
              int Counter
            0x002
              if Duration != 0
                int Value
            0x003
              if 0 == Duration
                int AtZero
              if (Duration) > 0
                int Positive
              if OptionalDuration != 0
                int OptionalValue
              if CustomDuration == 0
                int CustomValue
              if Counter != 0
                int CounterValue
              if Duration.TotalMilliseconds != 0
                int MillisecondsValue
            0x004
              int Temporary (time, local, write: Duration)
              if Temporary == 0
                int LocalValue
              if Counter == 0
                int Duration (local, write: 0)
                if Duration != 0
                  int ShadowedValue
            archive Metadata
              int Duration (time)
              if Duration != 0
                int Value
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var comparisons = Engine(result).GetRoot().DescendantNodes().OfType<IfStatementSyntax>()
            .Select(x => x.Condition.NormalizeWhitespace().ToString()).ToArray();
        await Assert.That(comparisons).Contains("n.Duration != TimeInt32.Zero");
        await Assert.That(comparisons).Contains("TimeInt32.Zero == n.Duration");
        await Assert.That(comparisons).Contains("(n.Duration) > TimeInt32.Zero");
        await Assert.That(comparisons).Contains("n.OptionalDuration != TimeInt32.Zero");
        await Assert.That(comparisons).Contains("n.CustomDuration == TimeInt32.Zero");
        await Assert.That(comparisons).Contains("temporary == TimeInt32.Zero");
        await Assert.That(comparisons).Contains("Duration != TimeInt32.Zero");
        await Assert.That(comparisons).Contains("n.Counter != 0");
        await Assert.That(comparisons).Contains("n.Duration.TotalMilliseconds != 0");
        await Assert.That(comparisons).Contains("duration != 0");

        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Engines.Game.Example")!;
        await Assert.That(type.GetMethod("Verify")!.Invoke(null, [0])).IsEqualTo("");
        await Assert.That(type.GetMethod("Verify")!.Invoke(null, [5])).IsEqualTo("42");
        await Assert.That(type.GetMethod("Verify")!.Invoke(null, [-5])).IsEqualTo("42");
    }

    [Test]
    public async Task SupportsNullableUnixTimeFieldsInChunksAndArchives()
    {
        const string source = """
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = 1)]
                    public partial class Chunk03043002 { }
                    [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = 1)]
                    public partial class SeparateMetadata { }
                }
            }
            namespace GBX.NET.Serialization
            {
                public partial class GbxReader
                {
                    public System.DateTimeOffset? ReadUnixTime() => null;
                }
                public partial class GbxWriter
                {
                    public void WriteUnixTime(System.DateTimeOffset? value) { }
                }
                public partial class GbxReaderWriter
                {
                    public System.DateTimeOffset? UnixTime(System.DateTimeOffset? value) => value;
                }
            }
            """;
        const string fields = """
              unixtime Timestamp
              unixtime? OptionalTimestamp

            """;
        var layout = "Example 0x03043000\n0x001\n" + fields + "0x002\n" + fields +
            "archive Metadata\n" + fields + "archive SeparateMetadata\n" + fields;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", layout), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("public DateTimeOffset? Timestamp");
        await Assert.That(generated).Contains("public DateTimeOffset? OptionalTimestamp");
        await Assert.That(generated).Contains("n.timestamp = rw.UnixTime(n.timestamp);");
        await Assert.That(generated).Contains("n.timestamp = r.ReadUnixTime();");
        await Assert.That(generated).Contains("w.WriteUnixTime(n.timestamp);");
    }

    [Test]
    public async Task SupportsIPv4FieldsInChunksAndArchives()
    {
        const string source = """
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = 1)]
                    public partial class Chunk03043002 { }
                    [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = 1)]
                    public partial class SeparateMetadata { }
                }
            }
            namespace GBX.NET.Serialization
            {
                public partial class GbxReader
                {
                    public System.Net.IPAddress ReadIPv4() => System.Net.IPAddress.Any;
                }
                public partial class GbxWriter
                {
                    public void WriteIPv4(System.Net.IPAddress? value) { }
                }
                public partial class GbxReaderWriter
                {
                    public System.Net.IPAddress? IPv4(System.Net.IPAddress? value) => value;
                    public void IPv4(ref System.Net.IPAddress? value) { }
                }
            }
            """;
        const string fields = """
              ipv4 Address
              ipv4? OptionalAddress
              ipv4
              ipv4 Temporary (local, write: Address)

            """;
        var layout = "Example 0x03043000\n0x001\n" + fields + "0x002\n" + fields +
            "archive Metadata\n" + fields + "archive SeparateMetadata\n" + fields;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", layout), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("public global::System.Net.IPAddress? Address");
        await Assert.That(generated).Contains("public global::System.Net.IPAddress? OptionalAddress");
        await Assert.That(generated).Contains("rw.IPv4(ref n.address);");
        await Assert.That(generated).Contains("n.address = r.ReadIPv4();");
        await Assert.That(generated).Contains("w.WriteIPv4(n.address);");
    }

    [Test]
    public async Task ReportsIPv4CollectionsAsUnsupported()
    {
        foreach (var field in new[] { "ipv4[] Addresses", "ipv4[2] Addresses", "ipv4[][] Addresses", "ipv4[] Addresses (list)" })
        {
            var (result, _) = Run("", new Text("Engines/Game/Example.chunkl", $"Example 0x03043000\n0x001\n  {field}\n"));
            await Assert.That(result.Diagnostics).Contains(x => x.Id == "GBXNETGEN200" && x.GetMessage().Contains("IPv4 fields do not support arrays or lists"));
            await Assert.That(result.GeneratedSources.Where(x => x.HintName.StartsWith("Engines/"))).IsEmpty();
        }
    }

    [Test]
    public async Task KeepsLocalFieldsInSerializationScopeAndUsesWriteExpressionsOnlyWhenWriting()
    {
        const string source = """
            namespace GBX.NET.Engines.Game;
            public partial class Example
            {
                public int WriteCount() => 2;
                public int WriteValue() => throw new System.InvalidOperationException("Read evaluated a write expression");

                public static string Verify()
                {
                    var node = new Example();
                    var chunk = new Chunk03043001();
                    chunk.ReadWrite(node, new GBX.NET.Serialization.GbxReaderWriter(new GBX.NET.Serialization.GbxReader(2, 7, 8, 9)));
                    var readValue = node.Value;
                    var writer = new GBX.NET.Serialization.GbxWriter();
                    new Chunk03043002().ReadWrite(node, new GBX.NET.Serialization.GbxReaderWriter(writer));
                    var writtenValue = node.Value;
                    var copyWriter = new GBX.NET.Serialization.GbxWriter();
                    new Chunk03043002().ReadWrite(node, new GBX.NET.Serialization.GbxReaderWriter(new GBX.NET.Serialization.GbxReader(2, 7, 8, 9), copyWriter));
                    return readValue + ":" + string.Join(",", writer.Values) + ":" + writtenValue + ":" + string.Join(",", copyWriter.Values) + ":" + node.Value;
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              int Count (local, write: "WriteCount()")
              loop Count
                int Item (local, write: 5)
                Value = Item
              int Value (write: "WriteValue()")
            0x002
              int Count (local, write: "WriteCount()")
              loop Count
                int Item (local, write: 5)
              int Value (write: "Value + 1")
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("var count = rw.Int32((rw.Writer is null ? default : (n.WriteCount())));");
        await Assert.That(generated).Contains("var item = rw.Int32((rw.Writer is null ? default : (5)));");
        await Assert.That(generated).Contains("i1 < count");
        await Assert.That(generated).Contains("n.Value = item;");
        await Assert.That(generated).Contains("n.value = rw.Int32((rw.Writer is null ? default : (n.Value+ 1)));");
        await Assert.That(generated).Contains("if (rw.Reader is not null)");
        await Assert.That(generated).DoesNotContain("if (rw.Writer is not null)");
        var root = Engine(result).GetRoot();
        await Assert.That(root.DescendantNodes().OfType<PropertyDeclarationSyntax>())
            .DoesNotContain(x => x.Identifier.ValueText is "Count" or "Item");
        await Assert.That(root.DescendantNodes().OfType<FieldDeclarationSyntax>())
            .DoesNotContain(x => x.Declaration.Variables.Any(v => v.Identifier.ValueText is "Count" or "count" or "Item" or "item"));

        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Engines.Game.Example")!;
        await Assert.That(type.GetMethod("Verify")!.Invoke(null, null)).IsEqualTo("9:2,5,5,10:10:2,7,8,9:9");
    }

    [Test]
    public async Task CombinesWriteArgumentsForByteVersionsNarrowNumbersAndExternalLocals()
    {
        var (result, compilation) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              versionb (write: 3)
              short Count (local, write: 2)
              int Event (local, write: 1)
              CMwNod Node (local, external, write: null)
              int[Count] Items (local, write: "new int[Count]")
              int Value (write: Event)
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("Version = rw.Byte((rw.Writer is null ? default : (3)));");
        await Assert.That(generated).Contains("var count = rw.Int16((short)(rw.Writer is null ? default : (2)));");
        await Assert.That(generated).Contains("var @event = rw.Int32((rw.Writer is null ? default : (1)));");
        await Assert.That(generated).Contains("Components.GbxRefTableFile? nodeFile = null;");
        await Assert.That(generated).Contains("var node = rw.NodeRef<CMwNod>((rw.Writer is null ? default : (null)), ref nodeFile);");
        await Assert.That(generated).Contains("var items = rw.Array<int>((rw.Writer is null ? default : (new int[count])), count);");
        await Assert.That(generated).Contains("n.value = rw.Int32((rw.Writer is null ? default : (@event)));");
        await Assert.That(generated).DoesNotContain("if (rw.Reader is not null)");
        await Assert.That(generated).DoesNotContain("if (rw.Writer is not null)");
    }

    [Test]
    public async Task SupportsLocalAndWriteFieldsInSeparateChunksAndArchives()
    {
        const string source = """
            namespace GBX.NET.Engines.Game;
            [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = 1)]
            public partial class Example
            {
                [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = 1)]
                public partial class Chunk03043001 { }
                [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = 1)]
                public partial class Metadata { }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              int Count (local, write: 2)
              loop Count
                int Item (local, write: 5)
                Value = Item
              int Value (write: "Value + 1")
            archive Metadata
              int Count (local, write: 2)
              int Value (write: "Count + 1")
            archive
              int Count (local, write: 2)
              int Value (write: "Count + 1")
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("var count = r.ReadInt32();");
        await Assert.That(generated).Contains("var item = r.ReadInt32();");
        await Assert.That(generated).Contains("int count = (2);");
        await Assert.That(generated).Contains("w.Write((count+ 1));");
        await Assert.That(generated).DoesNotContain("public int Count");
    }

    [Test]
    public async Task ReportsLocalFieldsWithoutWriteAndInvalidWriteExpressions()
    {
        foreach (var flags in new[] { "local", "local, write", "write", "write: \"\"", "write: \"Value +\"" })
        {
            var (result, _) = Run("", new Text("Engines/Game/Example.chunkl", $"Example 0x03043000\n0x001\n  int Value ({flags})\n"));
            await Assert.That(result.Diagnostics).Contains(x => x.Id == "GBXNETGEN200" && x.GetMessage().Contains("write"));
            await Assert.That(result.GeneratedSources.Where(x => x.HintName.StartsWith("Engines/"))).IsEmpty();
        }
    }

    [Test]
    public async Task TracksLocalScopeForConditionsArrayLengthsAndPropertyAttributes()
    {
        var (result, compilation) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              version (write: 3)
              block
                int Count (local, write: Count)
                if Count > 0
                  int[Count] Items (local, write: "new int[Count]")
              block
                int Count (local, write: 3)
                int (local, write: Count)
              int Value (write: Count)
              switch Value
                case 1
                  int Count (local, write: 1)
                case 2
                  int Count (local, write: 2)
                default
                  int Count (local, write: 3)
            0x002
              int Count
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("if (count> 0)");
        await Assert.That(generated).Contains("Version = rw.Int32((rw.Writer is null ? default : (3)));");
        await Assert.That(generated).Contains("var count = rw.Int32((rw.Writer is null ? default : (n.Count)));");
        await Assert.That(generated).Contains("var items = rw.Array<int>((rw.Writer is null ? default : (new int[count])), count);");
        await Assert.That(generated).Contains("var u01 = rw.Int32((rw.Writer is null ? default : (count)));");
        await Assert.That(generated).Contains("n.value = rw.Int32((rw.Writer is null ? default : (n.Count)));");
        await Assert.That(generated).DoesNotContain("public int U01");
        await Assert.That(generated).DoesNotContain("public int[]? Items");
        var count = Engine(result).GetRoot().DescendantNodes().OfType<PropertyDeclarationSyntax>().Single(x => x.Identifier.ValueText == "Count");
        await Assert.That(count.AttributeLists.ToString()).Contains("AppliedWithChunk<Chunk03043002>");
        await Assert.That(count.AttributeLists.ToString()).DoesNotContain("Chunk03043001");
    }

    [Test]
    public async Task CompilesWithCustomPropertiesBackingFieldsOverloadsAndAliasedSerializationMethods()
    {
        const string source = """
            using IO = GBX.NET.Serialization.GbxReaderWriter;
            namespace GBX.NET.Engines.Game;
            public partial class Example
            {
                private int count;
                public int Custom { get; set; }
                public Example() { Custom = 123; }
                public partial class Chunk03043001
                {
                    public override void ReadWrite(Example n, IO rw) { n.Custom++; }
                }
                public partial class Chunk03043002
                {
                    public void ReadWrite(Example n, int differentOverload) { }
                }
            }
            """;
        const string layout = """
            Example 0x03043000
            0x001
              int Custom
            0x002
              int Count
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", layout), compile: true);
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var root = Engine(result).GetRoot();
        await Assert.That(root.DescendantNodes().OfType<ConstructorDeclarationSyntax>()).IsEmpty();
        await Assert.That(root.DescendantNodes().OfType<PropertyDeclarationSyntax>()).DoesNotContain(x => x.Identifier.ValueText == "Custom");
        await Assert.That(root.DescendantNodes().OfType<FieldDeclarationSyntax>())
            .DoesNotContain(x => x.Declaration.Variables.Any(v => v.Identifier.ValueText == "count"));
        var chunks = root.DescendantNodes().OfType<ClassDeclarationSyntax>().Where(x => x.Identifier.ValueText.StartsWith("Chunk")).ToArray();
        await Assert.That(chunks[0].Members.OfType<MethodDeclarationSyntax>().Where(x => x.Identifier.ValueText == "ReadWrite")).IsEmpty();
        await Assert.That(chunks[1].Members.OfType<MethodDeclarationSyntax>().Where(x => x.Identifier.ValueText == "ReadWrite")).HasSingleItem();
        await Assert.That(Engine(result).ToString()).Contains("rw.Int32(ref n.count)");
        await Assert.That(Engine(result).ToString()).Contains("internal override void DeepCloneFields(CMwNod clone, DeepCloneContext context)");
        await Assert.That(Engine(result).ToString()).Contains("((Example)clone).Custom = context.Clone(this.Custom)!");
        await Assert.That(Engine(result).ToString()).Contains("((Example)clone).count = context.Clone(this.count)!");
    }

    [Test]
    public async Task UsesPropertiesWhenThereIsNoBackingFieldAndKeepsUnknownsInTheirChunks()
    {
        const string source = """
            namespace GBX.NET.Engines.Game;
            public partial class Example { public int Value { get; set; } }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              version = 2
              int Value
              int
              v2+
                int Other
            """), compile: true);
        await AssertNoErrors(compilation);
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("n.Value = rw.Int32(n.Value)");
        await Assert.That(generated).Contains("public int U01;");
        await Assert.That(generated).DoesNotContain("private int u01");
        await Assert.That(generated).Contains("if (Version >= 2)");
        await Assert.That(generated).Contains("rw.Int32(ref n.other)");
    }

    [Test]
    public async Task CompilesFixedInnerJaggedArchiveArray()
    {
        var (result, compilation) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000

            property int ItemCount
              get = Items.Length

            0x001
              Item[] Items

            0x002
              Item[Items.Length][] Rows

            archive Item
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);

        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("public Item[][]? Rows");
        await Assert.That(generated).Contains("get => Items?.Length?? 0;");
        await Assert.That(generated).Contains("rw.JaggedArrayReadableWritable<Item>(ref n.rows!, n.Items?.Length?? 0)");
    }

    [Test]
    public async Task CompilesDottedEnumAndNestedMemberExpressionsWithoutChangingStringLiterals()
    {
        const string source = """
            namespace GBX.NET.Engines.Game;
            public partial class Example
            {
                public HeaderInfo Header { get; } = new();
                public class HeaderInfo { public SizeInfo Size { get; } = new(); }
                public class SizeInfo { public int Count => 2; }

                public static string Verify()
                {
                    var node = new Example();
                    var writer = new GBX.NET.Serialization.GbxWriter();
                    new Chunk03043001().ReadWrite(node, new GBX.NET.Serialization.GbxReaderWriter(writer));
                    return node.Label + ":" + node.HeaderCount + ":" + string.Join(",", writer.Values);
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000

            property int HeaderCount
              get = Header.Size.Count

            0x001
              int<Kind> ItemType = Kind.First
              assert Header is not null
              if ItemType is Kind.First
                int Count (local, write: "Header.Size.Count")
                int[Header.Size.Count] Items (local, write: "new int[Count]")
              switch ItemType
                case Kind.First
                  int Value (write: "Header.Size.Count")
              assert !!(ItemType == Kind.First)

            0x002 (demonstration: partial)
              string Label = "literal::value"

            enum Kind
              First
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("private Kind itemType = Kind.First;");
        await Assert.That(generated).Contains("get => Header.Size.Count;");
        await Assert.That(generated).Contains("if (n.ItemType is Kind.First)");
        await Assert.That(generated).Contains("case Kind.First:");
        await Assert.That(generated).Contains("var count = rw.Int32((rw.Writer is null ? default : (n.Header.Size.Count)));");
        await Assert.That(generated).Contains("rw.Array<int>((rw.Writer is null ? default : (new int[count])), n.Header.Size.Count)");
        await Assert.That(generated).Contains("private string label = \"literal::value\";");

        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Engines.Game.Example")!;
        await Assert.That(type.GetMethod("Verify")!.Invoke(null, null)).IsEqualTo("literal::value:2:0,2,2");
    }

    [Test]
    public async Task CompilesJaggedArrayVariants()
    {
        var (result, compilation) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000

            0x001
              int[][] Values
              id[][] Ids
              string[][] Names
              Example[][] Nodes
              Example[][] ExternalNodes (external)
              Item[][] Rows

            archive Item
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);

        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("rw.JaggedArray<int>(ref n.values!)");
        await Assert.That(generated).Contains("rw.JaggedArrayId(ref n.ids!)");
        await Assert.That(generated).Contains("rw.JaggedArrayString(ref n.names!)");
        await Assert.That(generated).Contains("rw.JaggedArrayNodeRef<Example>(ref n.nodes!)");
        await Assert.That(generated).Contains("rw.JaggedArrayExternalNodeRef<Example>(ref n.externalNodes!)");
        await Assert.That(generated).Contains("rw.JaggedArrayReadableWritable<Item>(ref n.rows!)");
    }

    [Test]
    public async Task DemonstrationChunksDoNotGenerateFieldsOrProperties()
    {
        var (result, compilation) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 (demonstration)
              int Demonstrated
              int
            0x002
              int Serialized
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);

        var generated = Engine(result).ToString();
        await Assert.That(generated).DoesNotContain("Demonstrated");
        await Assert.That(generated).DoesNotContain("U01");
        await Assert.That(generated).Contains("public int Serialized");
        await Assert.That(generated).Contains("n.serialized");
    }

    [Test]
    public async Task PartialDemonstrationGeneratesMembersWithoutSerialization()
    {
        var (result, compilation) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 (demonstration: partial)
              int Shown
              int
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);

        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("public int Shown");
        await Assert.That(generated).Contains("public int U01;");
        await Assert.That(generated).DoesNotContain("rw.Int32");
    }

    [Test]
    public async Task GeneratesAttributesOnHandwrittenPartialProperties()
    {
        const string source = """
            namespace GBX.NET.Engines.Game;
            public partial class Example
            {
                private int storedValue;
                public virtual partial int Value
                {
                    get => storedValue;
                    set => storedValue = value;
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              int Value
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);

        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("[AppliedWithChunk<Chunk03043001>]");
        await Assert.That(generated).Contains("public virtual partial int Value { get; set; }");
        await Assert.That(generated).Contains("n.Value = rw.Int32(n.Value)");
        await Assert.That(generated).DoesNotContain("private int value;");
    }

    [Test]
    public async Task UsesInheritedPropertyWithoutGeneratingDuplicateStorage()
    {
        var (result, compilation) = Run("", true,
        [
            new Text("Engines/Game/Base.chunkl", """
                Base 0x03043000
                0x001
                  int Name
                """),
            new Text("Engines/Game/Derived.chunkl", """
                Derived 0x03044000
                - inherits: Base
                0x001
                  int Name (inherited)
                """)
        ]);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);

        var generated = result.GeneratedSources.Single(x => x.HintName == "Engines/Game/Derived.g.cs").SourceText.ToString();
        await Assert.That(generated).Contains("n.Name = rw.Int32(n.Name)");
        await Assert.That(generated).DoesNotContain("int Name");
        await Assert.That(generated).DoesNotContain("name = context.Clone");
    }

    [Test]
    public async Task AddsFormattingAttributeFromAnyFieldOccurrence()
    {
        var (result, _) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              string DisplayName
              string PlainName
            0x002
              string DisplayName (formatted)
            archive Metadata
              string Caption (formatted)
            """));

        await Assert.That(result.Diagnostics).IsEmpty();
        var properties = Engine(result).GetRoot().DescendantNodes().OfType<PropertyDeclarationSyntax>()
            .Where(x => x.Identifier.ValueText is "DisplayName" or "PlainName" or "Caption")
            .ToDictionary(x => x.Identifier.ValueText);
        await Assert.That(properties["DisplayName"].AttributeLists.SelectMany(x => x.Attributes)
            .Count(x => x.Name.ToString() == "SupportsFormatting")).IsEqualTo(1);
        await Assert.That(properties["Caption"].AttributeLists.SelectMany(x => x.Attributes)
            .Count(x => x.Name.ToString() == "SupportsFormatting")).IsEqualTo(1);
        await Assert.That(properties["PlainName"].AttributeLists.SelectMany(x => x.Attributes)
            .Any(x => x.Name.ToString() == "SupportsFormatting")).IsFalse();
    }

    [Test]
    public async Task CompilesPartialComputedPropertiesWithMergedChunkMetadata()
    {
        const string source = """
            using System.Collections.Generic;
            namespace GBX.NET.Engines.Game;
            public partial class Example
            {
                private int value;
                private List<string>? names;
                public partial int Value { get => value; set => this.value = value; }
                public partial int Combined { get => value; set => this.value = value; }
                public partial int? Derived { get => value; }
                private partial List<string>? Names { get => names; set => names = value; }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            property int Combined
              get = Value
            property int? Derived
              get = Value
            0x001 (demonstration: partial)
              version
              v1-
                int Value
              v2-
                int Combined
              v3+
                int Value
              v1+
                string[] Names (list)
            0x002 (base: 0x001)
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var root = Engine(result).GetRoot();
        var properties = root.DescendantNodes().OfType<PropertyDeclarationSyntax>().ToArray();
        var combined = properties.Single(x => x.Identifier.ValueText == "Combined");
        await Assert.That(combined.AttributeLists.ToString()).Contains("AppliedWithChunk<Chunk03043001>");
        await Assert.That(combined.AttributeLists.ToString()).Contains("AppliedWithChunk<Chunk03043002>");
        await Assert.That(combined.AttributeLists.SelectMany(x => x.Attributes)).Count().IsEqualTo(2);
        var derived = properties.Single(x => x.Identifier.ValueText == "Derived");
        await Assert.That(derived.ToString()).Contains("public partial int? Derived { get; }");
        await Assert.That(derived.AttributeLists.ToString()).Contains("AppliedWithChunk<Chunk03043001>(0, 1)");
        await Assert.That(derived.AttributeLists.ToString()).Contains("AppliedWithChunk<Chunk03043001>(3)");
        await Assert.That(derived.AttributeLists.ToString()).Contains("AppliedWithChunk<Chunk03043002>(0, 1)");
        await Assert.That(derived.AttributeLists.ToString()).Contains("AppliedWithChunk<Chunk03043002>(3)");
        await Assert.That(Engine(result).ToString()).Contains("private partial List<string>? Names { get; set; }");
        await Assert.That(Engine(result).ToString()).DoesNotContain("rw.Int32");
    }

    [Test]
    public async Task AnnotatesChunkPropertiesWithTheirSerializationVersionRanges()
    {
        var (result, compilation) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              version = 8
              int Always
              v2-
                int Legacy
              v3+
                int Modern
                v5-
                  int Middle
              v4=
                int Exact
              v0=
                int Repeated
              v3+
                int Repeated
              if Version >= 7
                int Late
              else
                int Early
              if Version < 2
                int FirstBranch
              else if Version < 5
                int MiddleBranch
              else
                int LastBranch
            0x002 (base: 0x001)
              base
              v4+
                int Extra
            0x003 (base: 0x001)
            0x004 (base: 0x001)
              v4+
                base
            0x04000000 (base: 0x001)
            0x005 (base: 0x001)
              int ChildOnly
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var generatedClass = Engine(result).GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()
            .Single(x => x.Identifier.ValueText == "Example");
        var properties = generatedClass.Members.OfType<PropertyDeclarationSyntax>()
            .ToDictionary(x => x.Identifier.ValueText, x => x.AttributeLists.SelectMany(y => y.Attributes)
                .Where(y => y.Name.ToString().StartsWith("AppliedWithChunk<", StringComparison.Ordinal))
                .Select(y => y.ToString()).ToArray());

        await Assert.That(properties["Always"]).IsEquivalentTo(new[]
        {
            "AppliedWithChunk<Chunk03043001>", "AppliedWithChunk<Chunk03043002>", "AppliedWithChunk<Chunk03043003>",
            "AppliedWithChunk<Chunk03043004>(4)", "AppliedWithChunk<Chunk04000000>"
        });
        await Assert.That(properties["Legacy"]).IsEquivalentTo(new[]
        {
            "AppliedWithChunk<Chunk03043001>(0, 2)", "AppliedWithChunk<Chunk03043002>(0, 2)",
            "AppliedWithChunk<Chunk03043003>(0, 2)", "AppliedWithChunk<Chunk04000000>(0, 2)"
        });
        await Assert.That(properties["Modern"]).Contains("AppliedWithChunk<Chunk03043001>(3)");
        await Assert.That(properties["Modern"]).Contains("AppliedWithChunk<Chunk03043004>(4)");
        await Assert.That(properties["Middle"]).Contains("AppliedWithChunk<Chunk03043001>(3, 5)");
        await Assert.That(properties["Middle"]).Contains("AppliedWithChunk<Chunk03043004>(4, 5)");
        await Assert.That(properties["Exact"]).Contains("AppliedWithChunk<Chunk03043001>(4, 4)");
        await Assert.That(properties["Repeated"]).Contains("AppliedWithChunk<Chunk03043001>(0, 0)");
        await Assert.That(properties["Repeated"]).Contains("AppliedWithChunk<Chunk03043001>(3)");
        await Assert.That(properties["Late"]).Contains("AppliedWithChunk<Chunk03043001>(7)");
        await Assert.That(properties["Early"]).Contains("AppliedWithChunk<Chunk03043001>(0, 6)");
        await Assert.That(properties["FirstBranch"]).Contains("AppliedWithChunk<Chunk03043001>(0, 1)");
        await Assert.That(properties["MiddleBranch"]).Contains("AppliedWithChunk<Chunk03043001>(2, 4)");
        await Assert.That(properties["LastBranch"]).Contains("AppliedWithChunk<Chunk03043001>(5)");
        await Assert.That(properties["Extra"]).IsEquivalentTo(new[] { "AppliedWithChunk<Chunk03043002>(4)" });
        await Assert.That(properties["ChildOnly"]).IsEquivalentTo(new[] { "AppliedWithChunk<Chunk03043005>" });
    }

    [Test]
    public async Task PreservesWireWidthsDataIdentifiersContextualArchivesAndGameVersions()
    {
        const string source = """
            namespace GBX.NET.Engines.Game;
            public partial class Example
            {
                public int Value { get; set; }
                [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
                public partial class Chunk03043001 { }
                public partial class VisualId { }
            }
            """;
        var (result, _) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001 [TM2020.v3, MP4]
              int16 Value
              ident Identity
              VisualId Visual
              data[] Bytes
              Context Context
            0x002
              int16 Value
              filetime Date
            archive VisualId
              id Name
            archive Context (contextual)
              int Value
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("w.Write((short)n.Value)");
        await Assert.That(generated).Contains("n.Value = (int)rw.Int16((short)n.Value)");
        await Assert.That(generated).Contains("r.ReadIdent()");
        await Assert.That(generated).DoesNotContain("IdAsStringent");
        await Assert.That(generated).DoesNotContain("VisualIdAsString");
        await Assert.That(generated).Contains("w.WriteWritable<Context, Example>(n.context, n)");
        await Assert.That(generated).Contains("w.WriteData(n.bytes)");
        await Assert.That(generated).Contains("private byte[]? bytes");
        await Assert.That(generated).Contains("private DateTime? date");
        await Assert.That(generated).Contains("[ChunkGameVersion(GameVersion.TM2020 | GameVersion.MP4, 3, -1)]");
    }

    [Test]
    public async Task EmitsArchiveInheritanceEnumsAndDeclaredThrowMessages()
    {
        var (result, _) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            archive
              int Value
              v2+
                throw (type: InvalidOperationException, message: Unsupported format)
            archive Base
              int Value
            archive Derived (inherits: Base)
              base
              int Other
            enum Kind
              First = 1
              Second
            """));
        await Assert.That(result.Diagnostics).IsEmpty();
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("partial class Derived : Base, IReadableWritable");
        await Assert.That(generated).Contains("public override void ReadWrite(GbxReaderWriter rw, int v = 0)");
        await Assert.That(generated).Contains("base.ReadWrite(rw, v)");
        await Assert.That(generated).Contains("base.DeepCloneArchiveFields(clone, context)");
        await Assert.That(generated).Contains("if (v >= 2)");
        await Assert.That(generated).Contains("throw new InvalidOperationException(\"Unsupported format\")");
        await Assert.That(generated).Contains("First = 1,");
    }

    [Test]
    [Arguments("return")]
    [Arguments("throw (type: System.InvalidOperationException)")]
    public async Task OmitsUnreachableSwitchBreaksAfterReturnOrThrow(string defaultExit)
    {
        var (result, compilation) = Run("", new Text("Engines/Game/Example.chunkl", $$"""
            Example 0x03043000
            0x001
              int Value
              switch Value
                case 0
                  int Other
                case 1
                  int Count (local, write: 0)
                  return
                case 2
                  int Count (local, write: 0)
                  throw (type: System.InvalidOperationException)
                default
                  int Count (local, write: 0)
                  {{defaultExit}}
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        await Assert.That(compilation.GetDiagnostics().Where(x => x.Id == "CS0162")).IsEmpty();
        var selection = Engine(result).GetRoot().DescendantNodes().OfType<SwitchStatementSyntax>().Single();
        await Assert.That(selection.Sections.Count).IsEqualTo(4);
        await Assert.That(selection.Sections[0].Statements.Last()).IsTypeOf<BreakStatementSyntax>();
        await Assert.That(selection.DescendantNodes().OfType<BreakStatementSyntax>()).HasSingleItem();
    }

    [Test]
    public async Task PassesCurrentVersionToArchivesAndHonorsExplicitOverrides()
    {
        var (result, _) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              version
              Outer Data
              Outer Legacy (version: 0)
            archive Outer
              version
              v1+ (archive)
                int OwnField
              v2+ (chunk)
                int CallerField
              Inner Child
            archive Inner
              v1+
                int ChildField
            """));

        await Assert.That(result.Diagnostics).IsEmpty();
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("rw.ReadableWritable<Outer>(ref n.data, version: Version)");
        await Assert.That(generated).Contains("rw.ReadableWritable<Outer>(ref n.legacy, version: 0)");
        await Assert.That(generated).Contains("partial class Outer : IReadableWritable, IReadable, IWritable, IVersionable");
        await Assert.That(generated).Contains("if (Version >= 1)");
        await Assert.That(generated).Contains("if (v >= 2)");
        await Assert.That(generated).Contains("rw.ReadableWritable<Inner>(ref this.child, version: Version)");
    }

    [Test]
    public async Task PassesInheritedArchiveVersionToNestedArchives()
    {
        var (result, _) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            archive
              Outer Data
            archive Outer
              Inner Child
            archive Inner
              v1+
                int Value
            """));

        await Assert.That(result.Diagnostics).IsEmpty();
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("rw.ReadableWritable<Outer>(ref this.data, version: v)");
        await Assert.That(generated).Contains("rw.ReadableWritable<Inner>(ref this.child, version: v)");
        await Assert.That(generated).Contains("object IDeepCloneable.DeepClone(DeepCloneContext context)");
        await Assert.That(generated).Contains("if (v >= 1)");
    }

    [Test]
    public async Task ReusesNamedArchivesFromAnotherLayout()
    {
        const string source = """
            namespace GBX.NET.Engines.Game
            {
                public partial class Example
                {
                    [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = 1)]
                    public partial class Chunk03043002 { }
                }
            }
            namespace GBX.NET.Serialization
            {
                public partial class GbxReader
                {
                    public T ReadReadable<T>(int version = 0) where T : IReadable, new() => new();
                    public T[] ReadArrayReadable<T>(int version = 0) where T : IReadable, new() => [];
                    public System.Collections.Generic.List<T> ReadListReadable<T>(int version = 0) where T : IReadable, new() => [];
                }
                public partial class GbxWriter
                {
                    public void WriteWritable<T>(T? value, int version = 0) where T : IWritable { }
                    public void WriteArrayWritable<T>(T[]? value, int version = 0) where T : IWritable { }
                    public void WriteListWritable<T>(System.Collections.Generic.List<T>? value, int version = 0) where T : IWritable { }
                }
                public partial class GbxReaderWriter
                {
                    public void ReadableWritable<T>(ref T? value, int version = 0) where T : class, IReadableWritable, new() { }
                    public void ArrayReadableWritable<T>(ref T[]? value, int version = 0) where T : IReadableWritable, new() { }
                    public void ListReadableWritable<T>(ref System.Collections.Generic.List<T>? value, int version = 0) where T : IReadableWritable, new() { }
                }
            }
            """;
        var (result, compilation) = Run(source, true,
        [
            new Text("Engines/Game/Shared.chunkl", """
                Shared 0x0310D000
                archive Spawn
                  version
                  int Value
                """),
            new Text("Engines/Game/Example.chunkl", """
                Example 0x03043000
                0x001
                  version
                  Shared.Spawn Data
                  Shared.Spawn[] Entries
                  Shared.Spawn[] ListEntries (list)
                0x002
                  version
                  Shared.Spawn Data
                  Shared.Spawn[] Entries
                  Shared.Spawn[] ListEntries (list)
                archive Container
                  Shared.Spawn[][] Entries
                """)
        ]);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var generated = result.GeneratedSources.Single(x => x.HintName == "Engines/Game/Example.g.cs").SourceText.ToString();
        await Assert.That(generated).Contains("public Shared.Spawn? Data");
        await Assert.That(generated).Contains("public Shared.Spawn[]? Entries");
        await Assert.That(generated).Contains("rw.ReadableWritable<Shared.Spawn>(ref n.data, version: Version)");
        await Assert.That(generated).Contains("rw.ArrayReadableWritable<Shared.Spawn>(ref n.entries!, version: Version)");
        await Assert.That(generated).Contains("rw.ListReadableWritable<Shared.Spawn>(ref n.listEntries!, version: Version)");
        await Assert.That(generated).Contains("rw.JaggedArrayReadableWritable<Shared.Spawn>(ref this.entries!, version: v)");
        await Assert.That(generated).Contains("n.data = r.ReadReadable<Shared.Spawn>(version: Version)");
        await Assert.That(generated).Contains("n.entries = r.ReadArrayReadable<Shared.Spawn>(version: Version)");
        await Assert.That(generated).Contains("n.listEntries = r.ReadListReadable<Shared.Spawn>(version: Version)");
        await Assert.That(generated).Contains("w.WriteWritable<Shared.Spawn>(n.data, version: Version)");
        await Assert.That(generated).Contains("w.WriteArrayWritable<Shared.Spawn>(n.entries, version: Version)");
        await Assert.That(generated).Contains("w.WriteListWritable<Shared.Spawn>(n.listEntries, version: Version)");
    }

    [Test]
    [Arguments("Missing.Spawn")]
    [Arguments("Shared.Missing")]
    public async Task ReportsUnknownSharedArchive(string typeName)
    {
        var (result, _) = Run("", new Text("Engines/Game/Shared.chunkl", """
            Shared 0x0310D000
            archive Spawn
              int Value
            """), new Text("Engines/Game/Example.chunkl", $$"""
            Example 0x03043000
            0x001
              {{typeName}}[] Entries
            """));

        await Assert.That(result.Diagnostics).Contains(x => x.Id == "GBXNETGEN200" &&
            x.GetMessage().Contains("Unknown archive: " + typeName));
    }

    [Test]
    public async Task EmitsEncapsulatedBlocksForCombinedAndSeparateSerialization()
    {
        const string source = """
            namespace GBX.NET.Engines.Game;
            public partial class Example
            {
                [GBX.NET.Attributes.ChunkGenerationOptions(StructureKind = StructureKind.SeparateReadAndWrite)]
                public partial class Chunk03043002 { }
            }
            """;
        var (result, _) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              block (encapsulated)
                int Value
            0x002
              block (encapsulated)
                int Other
            """));

        await Assert.That(result.Diagnostics).IsEmpty();
        var generated = Engine(result).ToString();
        await Assert.That(generated).Contains("rw.Encapsulated(rw =>");
        await Assert.That(generated).Contains("r.ReadEncapsulated(r =>");
        await Assert.That(generated).Contains("w.WriteEncapsulated(w =>");
    }

    [Test]
    public async Task RequiresVersionSourceWhenAnArchiveHasTwoVersions()
    {
        var (result, _) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            archive Named
              version
              v1+
                int Value
            """));

        await Assert.That(result.Diagnostics).Contains(x => x.Id == "GBXNETGEN200" &&
            x.GetMessage().Contains("must select (archive) or (chunk)"));
    }

    [Test]
    public async Task ReportsInvalidOverlapAndDuplicateLayoutsWithoutCrashingSiblingGenerationOrManagers()
    {
        var (result, _) = Run("namespace GBX.NET.Engines.Game; public class Bad { }",
            new Text("Engines/Game/Bad.chunkl", "Bad 0x03042000"),
            new Text("Engines/Game/Good.chunkl", "Good 0x03043000"),
            new Text("Engines/Plug/Good.chunkl", "Good 0x09001000"),
            new Text("Resources/CollectionId.txt", "1 Stadium"));
        await Assert.That(result.Exception).IsNull();
        await Assert.That(result.Diagnostics.Length).IsEqualTo(2);
        foreach (var diagnostic in result.Diagnostics)
        {
            await Assert.That(diagnostic.Id).IsEqualTo("GBXNETGEN200");
        }
        await Assert.That(result.GeneratedSources.Where(x => x.HintName.StartsWith("Engines/"))).HasSingleItem();
        await Assert.That(result.GeneratedSources).Contains(x => x.HintName == "Managers/ClassManager.g.cs");
    }

    [Test]
    public async Task KeepsNotRemappedChunkIdsOutOfClassRemapping()
    {
        var (result, compilation) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03078000
            0x000
              int Value
            0x0307B000 (not-remapped)
              int Value
            0x24062000 (not-remapped, base: 0x0307B000)
              base
            """), new Text("Resources/CollectionId.txt", "1 Stadium"));

        await Assert.That(result.Diagnostics).IsEmpty();
        var manager = result.GeneratedSources.Single(x => x.HintName == "Managers/ClassManager.g.cs").SyntaxTree;
        var method = manager.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>()
            .Single(x => x.Identifier.ValueText == "IsChunkIdRemapped");
        await Assert.That(method.ToString()).Contains("0x0307B000 => false");
        await Assert.That(method.ToString()).Contains("0x24062000 => false");
        await Assert.That(method.ToString()).DoesNotContain("0x03078000 => false");

        var executableMethod = method.WithModifiers(SyntaxFactory.TokenList(
            SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword)));
        var executable = CSharpCompilation.Create("ChunkRemappingFixture",
            [CSharpSyntaxTree.ParseText("public static class Mapping { " + executableMethod.NormalizeWhitespace() + " }")],
            compilation.References, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        await AssertNoErrors(executable);
        using var stream = new MemoryStream();
        var emitted = executable.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var mapping = System.Reflection.Assembly.Load(stream.ToArray()).GetType("Mapping")!
            .GetMethod("IsChunkIdRemapped")!;
        await Assert.That(mapping.Invoke(null, [0x0307B000u])).IsEqualTo(false);
        await Assert.That(mapping.Invoke(null, [0x24062000u])).IsEqualTo(false);
        await Assert.That(mapping.Invoke(null, [0x03078000u])).IsEqualTo(true);
        await Assert.That(mapping.Invoke(null, [0x03078001u])).IsEqualTo(true);
    }

    [Test]
    public async Task PreservesChunkOffsetsInRemappingAndReadsExtensionsAfterClassNames()
    {
        var (result, _) = Run("",
            new Text("Engines/Game/Example.chunkl", "Example 0x03043000"),
            new Text("Resources/CollectionId.txt", "1 Stadium"),
            new Text("Resources/Wrap.txt", "03043000 24003000"),
            new Text("Resources/Unwrap.txt", "24003000 03043000"),
            new Text("Resources/Extensions.txt", "03043000 Example Challenge.Gbx Map.Gbx"));
        var manager = result.GeneratedSources.Single(x => x.HintName == "Managers/ClassManager.g.cs").SourceText.ToString();
        await Assert.That(manager).Contains("classId & 0xFFF");
        await Assert.That(manager).Contains("}) | chunkPart");
        await Assert.That(manager).Contains("0x24003000 => 0x03043000");
        await Assert.That(manager).Contains("new string[] { \"Challenge.Gbx\", \"Map.Gbx\" }");
        await Assert.That(manager).DoesNotContain("\"Example Challenge.Gbx Map.Gbx\"");
    }

    [Test]
    public async Task ExposesBuiltInCollectionsAndFallsBackToCustomCollectionNames()
    {
        var (result, output) = Run("", new Text("Resources/CollectionId.txt", "0 Speed\n6 Stadium\n"));
        await Assert.That(result.Diagnostics).IsEmpty();
        var generated = result.GeneratedSources.Single(x => x.HintName == "Managers/CollectionManager.g.cs").SyntaxTree;
        var handwritten = CSharpSyntaxTree.ParseText("""
            namespace GBX.NET.Managers;
            public static partial class CollectionManager
            {
                public static System.Collections.Generic.IDictionary<int, string> CustomCollections { get; } = new System.Collections.Generic.Dictionary<int, string>();
                public static partial string? GetName(int id);
            }
            """, (CSharpParseOptions)generated.Options);
        var compilation = CSharpCompilation.Create("CollectionFixture", new[] { generated, handwritten }, output.References,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        await AssertNoErrors(compilation);
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Managers.CollectionManager")!;
        var collections = (await Assert.That(type.GetProperty("Collections")!.GetValue(null))
            .IsTypeOf<System.Collections.Immutable.ImmutableDictionary<int, string>>())!;
        await Assert.That(collections.Count).IsEqualTo(2);
        await Assert.That(collections[6]).IsEqualTo("Stadium");
        var custom = (IDictionary<int, string>)type.GetProperty("CustomCollections")!.GetValue(null)!;
        custom.Add(6, "Custom Stadium");
        custom.Add(999, "Custom Environment");
        var getName = type.GetMethod("GetName")!;
        await Assert.That(getName.Invoke(null, new object[] { 6 })).IsEqualTo("Stadium");
        await Assert.That(getName.Invoke(null, new object[] { 999 })).IsEqualTo("Custom Environment");
        await Assert.That(getName.Invoke(null, new object[] { 1000 })).IsNull();
        await Assert.That(collections.ContainsKey(999)).IsFalse();
    }

    [Test]
    public async Task CompilesConstructorAndAccessorsAndStoresValueInsteadOfAssigningTheSetterParameter()
    {
        var (result, compilation) = Run("", new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            constructor
              Value = 42
            property int Double
              get = Value * 2
              set
                Value = value / 2
            0x001
              int Value = 1
            """), compile: true);
        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var assembly = System.Reflection.Assembly.Load(stream.ToArray());
        var type = assembly.GetType("GBX.NET.Engines.Game.Example")!;
        var instance = Activator.CreateInstance(type)!;
        await Assert.That(type.GetProperty("Double")!.GetValue(instance)).IsEqualTo(84);
        type.GetProperty("Double")!.SetValue(instance, 100);
        await Assert.That(type.GetProperty("Value")!.GetValue(instance)).IsEqualTo(50);
    }

    [Test]
    public async Task UsesLayoutAccessorsForSerializedPropertiesWithNullableBackingFields()
    {
        const string source = """
            namespace GBX.NET.Engines.Game;
            public partial class Example
            {
                private bool? isNight;

                public static string Verify()
                {
                    var node = new Example { DayTime = 50 };
                    var inferred = node.IsNight;
                    var writer = new GBX.NET.Serialization.GbxWriter();
                    var chunk = new Chunk03043002();
                    chunk.ReadWrite(node, new GBX.NET.Serialization.GbxReaderWriter(writer));
                    var afterWrite = node.IsNight;
                    chunk.ReadWrite(node, new GBX.NET.Serialization.GbxReaderWriter(new GBX.NET.Serialization.GbxReader(0)));
                    var explicitFalse = node.IsNight;
                    node.IsNight = true;
                    node.DayTime = 0;
                    return inferred + ":" + string.Join(",", writer.Values) + ":" + afterWrite + ":" + explicitFalse + ":" + node.IsNight;
                }
            }
            """;
        var (result, compilation) = Run(source, new Text("Engines/Game/Example.chunkl", """
            Example 0x03043000
            0x001
              int DayTime
            0x002
              bool IsNight
            0x003
              int Score
            property bool IsNight
              get = isNight == true || (isNight == null && DayTime > 25 && DayTime < 75)
              set
                isNight = value
            property int Score
              get = score * 2
              set
                score = value / 2
            """), compile: true);

        await Assert.That(result.Diagnostics).IsEmpty();
        await AssertNoErrors(compilation);
        var generated = Engine(result).ToString();
        var property = Engine(result).GetRoot().DescendantNodes().OfType<PropertyDeclarationSyntax>()
            .Single(x => x.Identifier.ValueText == "IsNight");
        await Assert.That(property.Type.ToString()).IsEqualTo("bool");
        await Assert.That(property.AttributeLists.ToString()).Contains("AppliedWithChunk<Chunk03043001>");
        await Assert.That(property.AttributeLists.ToString()).Contains("AppliedWithChunk<Chunk03043002>");
        await Assert.That(generated).Contains("get => isNight== true || (isNight== null && DayTime> 25 && DayTime< 75);");
        await Assert.That(generated).Contains("isNight = value;");
        await Assert.That(generated).Contains("rw.Boolean(ref n.isNight);");
        await Assert.That(generated).DoesNotContain("private bool isNight;");
        await Assert.That(generated).Contains("private int score;");
        await Assert.That(generated).Contains("get => score* 2;");
        await Assert.That(generated).Contains("rw.Int32(ref n.score);");

        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        await Assert.That(emitted.Success).IsTrue().Because(string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Engines.Game.Example")!;
        await Assert.That(type.GetMethod("Verify")!.Invoke(null, null)).IsEqualTo("True:0:True:False:True");
    }

    [Test]
    public async Task SupportingGeneratorsHandleUnrelatedCompilations()
    {
        var compilation = CSharpCompilation.Create("Empty");
        var generators = new IIncrementalGenerator[] { new GbxReaderWriterGenerator(), new GbxReaderAndWriterArrayGenerator(), new InputWithTimeGenerator(), new CScriptTraitsMetadataMethodGenerator() };
        var result = CSharpGeneratorDriver.Create(generators.Select(x => x.AsSourceGenerator())).RunGenerators(compilation).GetRunResult();
        await Assert.That(result.Results).All(x => x.Exception == null);
        await Assert.That(result.Diagnostics).IsEmpty();
    }

    private static SyntaxTree Engine(GeneratorRunResult result) => result.GeneratedSources.Single(x => x.HintName.StartsWith("Engines/")).SyntaxTree;
    private static async Task AssertNoErrors(Compilation compilation) => await Assert.That(compilation.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error)).IsEmpty();

    private static (GeneratorRunResult Result, Compilation Compilation) Run(string source, Text file, bool compile = false) => Run(source, compile, new[] { file });
    private static (GeneratorRunResult Result, Compilation Compilation) Run(string source, params Text[] files) => Run(source, false, files);
    private static (GeneratorRunResult Result, Compilation Compilation) Run(string source, bool compile, Text[] files)
    {
        var options = new CSharpParseOptions(LanguageVersion.Preview);
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator).Select(x => MetadataReference.CreateFromFile(x));
        var compilation = CSharpCompilation.Create("Fixture", new[] { CSharpSyntaxTree.ParseText(source, options), CSharpSyntaxTree.ParseText(compile ? Support : "", options) }, references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new[] { new GbxGenerator().AsSourceGenerator() }, additionalTexts: files, parseOptions: options);
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var output, out _);
        return (driver.GetRunResult().Results.Single(), output);
    }

    private sealed class Text(string path, string source) : AdditionalText
    {
        public override string Path => path;
        public override SourceText GetText(CancellationToken cancellationToken = default) => SourceText.From(source);
    }

    private const string Support = """
        namespace TmEssentials { }
        namespace GBX.NET.Components { public class GbxRefTableFile { } }
        namespace GBX.NET
        {
            [System.Flags]
            public enum GameVersion { Unspecified = 0, TM10 = 1, TM2020 = 2 }
            public interface IClass { }
            public interface IVersionable { int Version { get; set; } }
            public class External<T> where T : GBX.NET.Engines.Game.CMwNod { }
        }
        namespace GBX.NET.Attributes
        {
            public class ClassAttribute(uint id) : System.Attribute { }
            public class ChunkAttribute(uint id) : System.Attribute { }
            public class ChunkGameVersionAttribute(GBX.NET.GameVersion game, params int[] versions) : System.Attribute { }
            public class HexadecimalAttribute : System.Attribute { }
            [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field, AllowMultiple = true)]
            public class GameVersionDefaultAttribute(GBX.NET.GameVersion game, object? defaultValue = null) : System.Attribute
            {
                public GBX.NET.GameVersion Game { get; } = game;
                public object? DefaultValue { get; } = defaultValue;
                public string? DefaultExpression { get; set; }
            }
            public class ChunkGenerationOptionsAttribute : System.Attribute
            {
                public int StructureKind { get; set; }
            }
            [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
            public class AppliedWithChunkAttribute<T> : System.Attribute
            {
                public AppliedWithChunkAttribute(int sinceVersion = 0) { }
                public AppliedWithChunkAttribute(int sinceVersion, int upToVersion) { }
            }
        }
        namespace GBX.NET.Serialization
        {
            public partial class GbxReader(params int[] values)
            {
                private readonly System.Collections.Generic.Queue<int> values = new(values);
                public int ReadInt32() => values.Dequeue();
                public T[] ReadArray<T>(int length) where T : struct => new T[length];
            }
            public partial class GbxWriter
            {
                public System.Collections.Generic.List<int> Values { get; } = new();
                public void Write(int value) => Values.Add(value);
                public void WriteArray<T>(T[]? value, int length) where T : struct { }
            }
            public interface IReadable { void Read(GbxReader reader, int version = 0); }
            public interface IWritable { void Write(GbxWriter writer, int version = 0); }
            public interface IReadableWritable { void ReadWrite(GbxReaderWriter readerWriter, int version = 0); }
            public interface IDeepCloneable { object DeepClone(DeepCloneContext context); }
            public class DeepCloneContext
            {
                public void Register(object source, object clone) { }
                public T Clone<T>(T source) => source;
                public T[] CloneArray<T>(T[] source) => source;
                public System.Collections.Generic.List<T> CloneList<T>(System.Collections.Generic.IEnumerable<T> source) => new(source);
                public System.Collections.Generic.Dictionary<TKey, TValue> CloneDictionary<TKey, TValue>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<TKey, TValue>> source) where TKey : notnull => new();
                public System.Collections.Generic.HashSet<T> CloneHashSet<T>(System.Collections.Generic.ISet<T> source) => new(source);
            }
            public partial class GbxReaderWriter : System.IDisposable
            {
                public GbxReaderWriter() { }
                public GbxReader? Reader { get; }
                public GbxWriter? Writer { get; }
                public GbxReaderWriter(GbxReader reader) { Reader = reader; }
                public GbxReaderWriter(GbxWriter writer) { Writer = writer; }
                public GbxReaderWriter(GbxReader reader, GbxWriter writer) { Reader = reader; Writer = writer; }
                public int Byte(int value) => Int32(value);
                public short Int16(short value) => (short)Int32(value);
                public T? NodeRef<T>(T? value, ref GBX.NET.Components.GbxRefTableFile? file) where T : GBX.NET.Engines.Game.CMwNod => value;
                public void Int32(ref int value) { value = Int32(value); }
                public void EnumInt32<T>(ref T value) where T : struct, System.Enum
                {
                    value = (T)System.Enum.ToObject(typeof(T), Int32(System.Convert.ToInt32(value)));
                }
                public void Boolean(ref bool? value)
                {
                    if (Reader is not null) value = Reader.ReadInt32() != 0;
                    Writer?.Write(value.GetValueOrDefault() ? 1 : 0);
                }
                public int Int32(int value)
                {
                    if (Reader is not null) value = Reader.ReadInt32();
                    Writer?.Write(value);
                    return value;
                }
                public void VersionInt32(GBX.NET.IVersionable value) { }
                public void VersionByte(GBX.NET.IVersionable value) { }
                public T[]? Array<T>(T[]? value, int length) where T : struct
                {
                    if (Reader is not null) value = Reader.ReadArray<T>(length);
                    Writer?.WriteArray(value, length);
                    return value;
                }
                public void ArrayReadableWritable<T>(ref T[]? value) where T : IReadableWritable, new() { }
                public void JaggedArrayReadableWritable<T>(ref T[][]? value, int? innerLength = null, int? outerLength = null, int version = 0)
                    where T : IReadable, IWritable, new() { }
                public void JaggedArray<T>(ref T[][]? value, int? innerLength = null, int? outerLength = null) where T : struct { }
                public void JaggedArrayId(ref string[][]? value, int? innerLength = null, int? outerLength = null) { }
                public void JaggedArrayString(ref string[][]? value, int? innerLength = null, int? outerLength = null) { }
                public void JaggedArrayNodeRef<T>(ref T?[][]? value, int? innerLength = null, int? outerLength = null) where T : GBX.NET.IClass { }
                public void JaggedArrayExternalNodeRef<T>(ref GBX.NET.External<T>[][]? value, int? innerLength = null, int? outerLength = null) where T : GBX.NET.Engines.Game.CMwNod { }
                public void Dispose() { }
            }
        }
        namespace GBX.NET.Serialization.Chunking
        {
            public interface IChunk { }
            public class Chunk : IChunk
            {
                internal virtual void DeepCloneFields(Chunk clone, GBX.NET.Serialization.DeepCloneContext context) { }
            }
            public class Chunk<T> : Chunk
            {
                public virtual uint Id => 0;
                public virtual GBX.NET.GameVersion GameVersion => GBX.NET.GameVersion.Unspecified;
                public virtual void ReadWrite(T node, GBX.NET.Serialization.GbxReaderWriter rw) { }
                public virtual void Read(T node, GBX.NET.Serialization.GbxReader r) { }
                public virtual void Write(T node, GBX.NET.Serialization.GbxWriter w) { }
            }
        }
        namespace GBX.NET.Engines.Game
        {
            public class CMwNod : GBX.NET.IClass
            {
                internal virtual GBX.NET.Serialization.Chunking.IChunk? NewChunk(uint id) => null;
                internal virtual void DeepCloneFields(CMwNod clone, GBX.NET.Serialization.DeepCloneContext context) { }
            }
        }
        """;
}
