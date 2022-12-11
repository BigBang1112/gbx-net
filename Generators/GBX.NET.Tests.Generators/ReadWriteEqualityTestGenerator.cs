using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Text;
using System.IO.Compression;
using GBX.NET.Tests.Generators.Extensions;

namespace GBX.NET.Tests.Generators;

[Generator]
public class ReadWriteEqualityTestGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
#if DEBUG
        if (!Debugger.IsAttached)
        {
            //Debugger.Launch();
        }
#endif
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // GBX.NET assembly
        var gbxnet = context.Compilation.SourceModule.ReferencedAssemblySymbols.First(x => x.Name == "GBX.NET");
        var enginesNamespace = gbxnet.GlobalNamespace.NavigateToNamespace("GBX.NET.Engines") ?? throw new Exception("GBX.NET.Engines namespace not found.");
        var engineTypes = enginesNamespace.GetNamespaceMembers().SelectMany(x => x.GetTypeMembers());
        var testsProjectDirectory = Path.GetDirectoryName(context.Compilation.SyntaxTrees.First(x => Path.GetDirectoryName(x.FilePath).EndsWith("GBX.NET.Tests")).FilePath);

        var testsNamespace = context.Compilation
            .GlobalNamespace
            .NavigateToNamespace("GBX.NET.Tests") ?? throw new Exception("GBX.NET.Tests namespace not found."); ;

        var gameVersionsType = testsNamespace
            .GetTypeMembers()
            .First(x => x.Name == "GameVersions");

        var versions = gameVersionsType.GetMembers()
            .OfType<IFieldSymbol>()
            .Where(x => x.IsConst && x.ConstantValue is string)
            .ToArray();

        var existingTestsNamespace = testsNamespace.NavigateToNamespace($"Integration.Engines");

        foreach (var type in engineTypes)
        {
            var anyVersionExists = false;

            foreach (var version in versions)
            {
                if (File.Exists(Path.Combine(testsProjectDirectory, "TestData", "Chunks", $"{type.Name}.{version.ConstantValue}.zip")))
                {
                    anyVersionExists = true;
                    break;
                }
            }

            if (!anyVersionExists)
            {
                continue;
            }

            var existingClassTestsType = existingTestsNamespace?
                .NavigateToNamespace(type.ContainingNamespace.Name)?
                .GetTypeMembers().FirstOrDefault(x => x.Name == $"{type.Name}Tests");

            var chunkCode = GenerateChunkCode(type, existingClassTestsType, testsProjectDirectory, versions);

            context.AddSource($"{type.Name}Tests.g.cs", $@"using Xunit;
using GBX.NET.Engines.{type.ContainingNamespace.Name};

namespace GBX.NET.Tests.Integration.Engines.{type.ContainingNamespace.Name};

public partial class {type.Name}Tests
{{
    {string.Join("\n\n    ", chunkCode)}
}}
");
        }

        //var collector = engineTypes.First(x => x.Name == "CGameCtnCollector");
        //var nestedTypes = collector.GetTypeMembers();
        //var nestedChunks = nestedTypes.Where(x => x.BaseType?.Name == "Chunk" || x.BaseType?.Name == "HeaderChunk");
    }

    private IEnumerable<string> GenerateChunkCode(INamedTypeSymbol type,
                                                  INamedTypeSymbol? existingClassTestsType,
                                                  string testsProjectDirectory,
                                                  IFieldSymbol[] versions)
    {
        foreach (var typeMember in type.GetTypeMembers())
        {
            // Does not work good for inheritance
            if (typeMember.BaseType?.Name != "Chunk"
             && typeMember.BaseType?.Name != "HeaderChunk"
             && typeMember.BaseType?.Name != "SkippableChunk")
            {
                continue;
            }

            if (typeMember.GetAttributes().Any(x => x.AttributeClass?.Name == "IgnoreChunkAttribute"))
            {
                continue;
            }

            var chunkType = typeMember;
            
            var existingChunkTestsType = existingClassTestsType?.GetTypeMembers()
                .FirstOrDefault(x => x.Name == $"{chunkType.Name}Tests");

            yield return $@"public partial class {chunkType.Name}Tests
    {{
        {string.Join("\n\n        ", GenerateVersionMethods(type, existingChunkTestsType, chunkType, testsProjectDirectory, versions))}
    }}";
        }
    }

    private IEnumerable<string> GenerateVersionMethods(INamedTypeSymbol type,
                                                       INamedTypeSymbol? existingChunkTestsType,
                                                       INamedTypeSymbol chunkType,
                                                       string testsProjectDirectory,
                                                       IFieldSymbol[] versions)
    {

        var ignoreReadWriteEqualityTestFullyAtt = existingChunkTestsType?.GetAttributes()
            .FirstOrDefault(x => x.AttributeClass?.Name == "IgnoreReadWriteEqualityTestAttribute");
        
        foreach (var version in versions)
        {
            var zipPath = Path.Combine(testsProjectDirectory, "TestData", "Chunks", $"{type.Name}.{version.ConstantValue}.zip");

            if (!File.Exists(zipPath))
            {
                continue;
            }
            
            using var zip = ZipFile.OpenRead(zipPath);

            if (zip.Entries.FirstOrDefault(x => x.Name == $"{chunkType.Name}.dat") is null)
            {
                continue;
            }

            var methodName = $"ReadAndWrite_{version.Name.Replace('-', '_')}_DataShouldEqual";

            var manualTestMethod = existingChunkTestsType?.GetMembers()
                .OfType<IMethodSymbol>()
                .FirstOrDefault(x => x.Name == methodName);

            var ignoreReadWriteEqualityTestAtt = default(AttributeData);
            var readWriteEqualityTestAtt = default(AttributeData);

            if (manualTestMethod is not null)
            {
                if (!manualTestMethod.IsPartialDefinition)
                {
                    continue;
                }

                var manualTestMethodAttributes = manualTestMethod.GetAttributes();

                ignoreReadWriteEqualityTestAtt ??= manualTestMethodAttributes
                    .FirstOrDefault(x => x.AttributeClass?.Name == "IgnoreReadWriteEqualityTestAttribute");
                readWriteEqualityTestAtt = manualTestMethodAttributes
                    .FirstOrDefault(x => x.AttributeClass?.Name == "ReadWriteEqualityTestAttribute");
            }

            var idVersion = (int?)3;
            var idStrings = default(string);

            if (readWriteEqualityTestAtt is not null)
            {
                var attArguments = readWriteEqualityTestAtt.NamedArguments
                    .ToDictionary(x => x.Key, x => x.Value);

                if (attArguments.TryGetValue("FirstIdOccurance", out TypedConstant firstIdOccuranceValue)
                    && (firstIdOccuranceValue.Value is bool firstIdOccuranceVal) && firstIdOccuranceVal)
                {
                    idVersion = null;
                }
            }

            yield return $@"[Fact{((ignoreReadWriteEqualityTestFullyAtt is null && ignoreReadWriteEqualityTestAtt is null) ? "" : "(Skip = \"This chunk was ignored from the automatic test.\")")}]
        public{(manualTestMethod is null ? "" : " partial")} void {methodName}()
        {{
            // Arrange
            using ChunkReadWriteEqualityTester<{type.Name}, {type.Name}.{chunkType.Name}> chunkTester
                = new(GameVersions.{version.Name});
            {(idVersion != 3 && idStrings is null ? $"\n            chunkTester.SetIdState(version: {idVersion?.ToString() ?? "null"}); // idStrings usage missing\n" : "")}
            // Act
            chunkTester.ReadWrite();

            // Assert
            Assert.Equal(expected: chunkTester.InputData, actual: chunkTester.OutputStream.ToArray());
        }}";
        }
    }
}
