using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GBX.NET.Generators.Tests;

public class GenerationTests
{
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
            public class Chunk : IChunk
            {
                internal virtual void DeepCloneFields(Chunk clone, GBX.NET.Serialization.DeepCloneContext context) { }
            }
            public class Chunk<T> : Chunk
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
                internal virtual void DeepCloneFields(CMwNod clone, GBX.NET.Serialization.DeepCloneContext context) { }
            }
        }
        """;
}
