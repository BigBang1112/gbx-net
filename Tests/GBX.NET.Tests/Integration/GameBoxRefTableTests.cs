using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace GBX.NET.Tests.Integration;

public class GameBoxRefTableTests
{
    private readonly JsonSerializerOptions jsonSeralizerOptions;

    public GameBoxRefTableTests()
    {
        jsonSeralizerOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
    }

    [Theory(DisplayName = "Read/Write should equal")]
    [InlineData("CCP#04 - ODYSSEY.Map.Gbx")]
    [InlineData("IslandSeaWaySupport.TMEDRoad.Gbx")]
    [InlineData("RallyBase45x45Sunrise.TMDecoration.Gbx")]
    public void ReadWriteShouldEqual(string fileName)
    {
        var gbx = GameBox.ParseHeader(Path.Combine("Files", fileName));
        var refTable = gbx.GetRefTable();
        var header = gbx.GetHeader();

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

        var newRefTable = GameBox.RefTable.Parse(header, reader);
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
