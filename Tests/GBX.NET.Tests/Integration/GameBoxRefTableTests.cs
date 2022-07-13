using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

#if NET6_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;
#endif

namespace GBX.NET.Tests.Integration;

public class GameBoxRefTableTests
{

#if NET6_0_OR_GREATER

    private readonly JsonSerializerOptions jsonSeralizerOptions;

    public static IEnumerable<string> ExampleGbxs { get; } = Directory.GetFiles(Environment.CurrentDirectory, "*.Gbx", SearchOption.AllDirectories);

    public GameBoxRefTableTests()
    {
        jsonSeralizerOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
    }

    [Fact(DisplayName = "Read/Write - should equal")]
    public void ReadWrite_ShouldEqual()
    {
        foreach (var fileName in ExampleGbxs)
        {
            var gbx = GameBox.ParseHeader(Path.Combine("Files", fileName));
            var refTable = gbx.RefTable;
            var header = gbx.Header;

            using var ms = new MemoryStream();
            using var writer = new GameBoxWriter(ms);

            var expectedNull = refTable is null;
            var expectedAncestorLevel = default(int);
            var expectedFiles = default(string);
            var expectedFolders = default(string);

            if (refTable is null)
            {
                writer.Write(0);
            }
            else
            {
                expectedAncestorLevel = refTable.AncestorLevel;
                expectedFiles = JsonSerializer.Serialize(refTable.Files, jsonSeralizerOptions);
                expectedFolders = JsonSerializer.Serialize(refTable.Folders, jsonSeralizerOptions);

                refTable.Write(header, writer);
            }

            ms.Seek(0, SeekOrigin.Begin);

            using var reader = new GameBoxReader(ms);

            var newRefTable = GameBoxRefTable.Parse(header, reader);
            var actualNull = newRefTable is null;

            if (newRefTable is null)
            {
                Assert.Equal(expectedNull, actualNull);
                return;
            }

            var actualAncestorLevel = newRefTable.AncestorLevel;
            var actualFiles = JsonSerializer.Serialize(newRefTable.Files, jsonSeralizerOptions);
            var actualFolders = JsonSerializer.Serialize(newRefTable.Folders, jsonSeralizerOptions);

            Assert.Equal(expectedAncestorLevel, actualAncestorLevel);
            Assert.Equal(expectedFiles, actualFiles);
            Assert.Equal(expectedFolders, actualFolders);
        }
    }

#endif

}
