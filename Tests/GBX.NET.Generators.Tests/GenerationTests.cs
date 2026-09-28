using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Xunit;

namespace GBX.NET.Generators.Tests;

public class GenerationTests
{
    [Fact]
    public void CompilesWithCustomPropertiesBackingFieldsOverloadsAndAliasedSerializationMethods()
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
        Assert.Empty(result.Diagnostics);
        AssertNoErrors(compilation);
        var root = Engine(result).GetRoot();
        Assert.Empty(root.DescendantNodes().OfType<ConstructorDeclarationSyntax>());
        Assert.DoesNotContain(root.DescendantNodes().OfType<PropertyDeclarationSyntax>(), x => x.Identifier.ValueText == "Custom");
        Assert.DoesNotContain(root.DescendantNodes().OfType<FieldDeclarationSyntax>(), x => x.Declaration.Variables.Any(v => v.Identifier.ValueText == "count"));
        var chunks = root.DescendantNodes().OfType<ClassDeclarationSyntax>().Where(x => x.Identifier.ValueText.StartsWith("Chunk")).ToArray();
        Assert.Empty(chunks[0].Members.OfType<MethodDeclarationSyntax>());
        Assert.Single(chunks[1].Members.OfType<MethodDeclarationSyntax>());
        Assert.Contains("rw.Int32(ref n.count)", Engine(result).ToString());
    }

    [Fact]
    public void UsesPropertiesWhenThereIsNoBackingFieldAndKeepsUnknownsInTheirChunks()
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
        AssertNoErrors(compilation);
        var generated = Engine(result).ToString();
        Assert.Contains("n.Value = rw.Int32(n.Value)", generated);
        Assert.Contains("public int U01;", generated);
        Assert.DoesNotContain("private int u01", generated);
        Assert.Contains("if (Version >= 2)", generated);
        Assert.Contains("rw.Int32(ref n.other)", generated);
    }

    [Fact]
    public void PreservesWireWidthsDataIdentifiersContextualArchivesAndGameVersions()
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
        Assert.Empty(result.Diagnostics);
        var generated = Engine(result).ToString();
        Assert.Contains("w.Write((short)n.Value)", generated);
        Assert.Contains("n.Value = (int)rw.Int16((short)n.Value)", generated);
        Assert.Contains("r.ReadIdent()", generated);
        Assert.DoesNotContain("IdAsStringent", generated);
        Assert.DoesNotContain("VisualIdAsString", generated);
        Assert.Contains("w.WriteWritable<Context, Example>(n.context, n)", generated);
        Assert.Contains("w.WriteData(n.bytes)", generated);
        Assert.Contains("private byte[]? bytes", generated);
        Assert.Contains("private DateTime? date", generated);
        Assert.Contains("[ChunkGameVersion(GameVersion.TM2020 | GameVersion.MP4, 3, -1)]", generated);
    }

    [Fact]
    public void EmitsArchiveInheritanceEnumsAndDeclaredThrowMessages()
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
        Assert.Empty(result.Diagnostics);
        var generated = Engine(result).ToString();
        Assert.Contains("partial class Derived : Base, IReadableWritable", generated);
        Assert.Contains("public override void ReadWrite(GbxReaderWriter rw, int v = 0)", generated);
        Assert.Contains("base.ReadWrite(rw, v)", generated);
        Assert.Contains("if (v >= 2)", generated);
        Assert.Contains("throw new InvalidOperationException(\"Unsupported format\")", generated);
        Assert.Contains("First = 1,", generated);
    }

    [Fact]
    public void ReportsInvalidOverlapAndDuplicateLayoutsWithoutCrashingSiblingGenerationOrManagers()
    {
        var (result, _) = Run("namespace GBX.NET.Engines.Game; public class Bad { }",
            new Text("Engines/Game/Bad.chunkl", "Bad 0x03042000"),
            new Text("Engines/Game/Good.chunkl", "Good 0x03043000"),
            new Text("Engines/Plug/Good.chunkl", "Good 0x09001000"),
            new Text("Resources/CollectionId.txt", "1 Stadium"));
        Assert.Null(result.Exception);
        Assert.Equal(2, result.Diagnostics.Length);
        Assert.All(result.Diagnostics, x => Assert.Equal("GBXNETGEN200", x.Id));
        Assert.Single(result.GeneratedSources.Where(x => x.HintName.StartsWith("Engines/")));
        Assert.Contains(result.GeneratedSources, x => x.HintName == "Managers/ClassManager.g.cs");
    }

    [Fact]
    public void PreservesChunkOffsetsInRemappingAndReadsExtensionsAfterClassNames()
    {
        var (result, _) = Run("",
            new Text("Engines/Game/Example.chunkl", "Example 0x03043000"),
            new Text("Resources/CollectionId.txt", "1 Stadium"),
            new Text("Resources/Wrap.txt", "03043000 24003000"),
            new Text("Resources/Unwrap.txt", "24003000 03043000"),
            new Text("Resources/Extensions.txt", "03043000 Example Challenge.Gbx Map.Gbx"));
        var manager = result.GeneratedSources.Single(x => x.HintName == "Managers/ClassManager.g.cs").SourceText.ToString();
        Assert.Contains("classId & 0xFFF", manager);
        Assert.Contains("}) | chunkPart", manager);
        Assert.Contains("0x24003000 => 0x03043000", manager);
        Assert.Contains("new string[] { \"Challenge.Gbx\", \"Map.Gbx\" }", manager);
        Assert.DoesNotContain("\"Example Challenge.Gbx Map.Gbx\"", manager);
    }

    [Fact]
    public void ExposesBuiltInCollectionsAndFallsBackToCustomCollectionNames()
    {
        var (result, output) = Run("", new Text("Resources/CollectionId.txt", "0 Speed\n6 Stadium\n"));
        Assert.Empty(result.Diagnostics);
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
        AssertNoErrors(compilation);
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        Assert.True(emitted.Success, string.Join(Environment.NewLine, emitted.Diagnostics));
        var type = System.Reflection.Assembly.Load(stream.ToArray()).GetType("GBX.NET.Managers.CollectionManager")!;
        var collections = Assert.IsType<System.Collections.Immutable.ImmutableDictionary<int, string>>(type.GetProperty("Collections")!.GetValue(null));
        Assert.Equal(2, collections.Count);
        Assert.Equal("Stadium", collections[6]);
        var custom = (IDictionary<int, string>)type.GetProperty("CustomCollections")!.GetValue(null)!;
        custom.Add(6, "Custom Stadium");
        custom.Add(999, "Custom Environment");
        var getName = type.GetMethod("GetName")!;
        Assert.Equal("Stadium", getName.Invoke(null, new object[] { 6 }));
        Assert.Equal("Custom Environment", getName.Invoke(null, new object[] { 999 }));
        Assert.Null(getName.Invoke(null, new object[] { 1000 }));
        Assert.False(collections.ContainsKey(999));
    }

    [Fact]
    public void CompilesConstructorAndAccessorsAndStoresValueInsteadOfAssigningTheSetterParameter()
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
        Assert.Empty(result.Diagnostics);
        AssertNoErrors(compilation);
        using var stream = new MemoryStream();
        var emitted = compilation.Emit(stream);
        Assert.True(emitted.Success, string.Join(Environment.NewLine, emitted.Diagnostics));
        var assembly = System.Reflection.Assembly.Load(stream.ToArray());
        var type = assembly.GetType("GBX.NET.Engines.Game.Example")!;
        var instance = Activator.CreateInstance(type)!;
        Assert.Equal(84, type.GetProperty("Double")!.GetValue(instance));
        type.GetProperty("Double")!.SetValue(instance, 100);
        Assert.Equal(50, type.GetProperty("Value")!.GetValue(instance));
    }

    [Fact]
    public void SupportingGeneratorsHandleUnrelatedCompilations()
    {
        var compilation = CSharpCompilation.Create("Empty");
        var generators = new IIncrementalGenerator[] { new GbxReaderWriterGenerator(), new GbxReaderAndWriterArrayGenerator(), new InputWithTimeGenerator(), new CScriptTraitsMetadataMethodGenerator() };
        var result = CSharpGeneratorDriver.Create(generators.Select(x => x.AsSourceGenerator())).RunGenerators(compilation).GetRunResult();
        Assert.All(result.Results, x => Assert.Null(x.Exception));
        Assert.Empty(result.Diagnostics);
    }

    private static SyntaxTree Engine(GeneratorRunResult result) => Assert.Single(result.GeneratedSources.Where(x => x.HintName.StartsWith("Engines/"))).SyntaxTree;
    private static void AssertNoErrors(Compilation compilation) => Assert.Empty(compilation.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error));

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
        return (Assert.Single(driver.GetRunResult().Results), output);
    }

    private sealed class Text(string path, string source) : AdditionalText
    {
        public override string Path => path;
        public override SourceText GetText(CancellationToken cancellationToken = default) => SourceText.From(source);
    }

    private const string Support = """
        namespace TmEssentials { }
        namespace GBX.NET
        {
            public interface IClass { }
            public interface IVersionable { int Version { get; set; } }
        }
        namespace GBX.NET.Attributes
        {
            public class ClassAttribute(uint id) : System.Attribute { }
            public class ChunkAttribute(uint id) : System.Attribute { }
            public class HexadecimalAttribute : System.Attribute { }
            public class AppliedWithChunkAttribute<T> : System.Attribute { }
        }
        namespace GBX.NET.Serialization
        {
            public class GbxReader { }
            public class GbxWriter { }
            public class GbxReaderWriter
            {
                public void Int32(ref int value) { }
                public int Int32(int value) => value;
                public void VersionInt32(GBX.NET.IVersionable value) { }
            }
        }
        namespace GBX.NET.Serialization.Chunking
        {
            public interface IChunk { }
            public class Chunk<T> : IChunk
            {
                public virtual uint Id => 0;
                public virtual void ReadWrite(T node, GBX.NET.Serialization.GbxReaderWriter rw) { }
            }
        }
        namespace GBX.NET.Engines.Game
        {
            public class CMwNod : GBX.NET.IClass
            {
                internal virtual GBX.NET.Serialization.Chunking.IChunk? NewChunk(uint id) => null;
            }
        }
        """;
}
