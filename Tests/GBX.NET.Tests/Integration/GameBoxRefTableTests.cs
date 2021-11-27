using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace GBX.NET.Tests.Integration;

public class GameBoxRefTableTests
{
    private readonly JsonSerializerOptions jsonSeralizerOptions = new();

    [Theory(DisplayName = "Read/Write should equal")]
    [InlineData("CCP#04 - ODYSSEY.Map.Gbx")]
    [InlineData("IslandSeaWaySupport.TMEDRoad.Gbx")]
    [InlineData("RallyBase45x45Sunrise.TMDecoration.Gbx")]
    public void ReadWriteShouldEqual(string fileName)
    {
        var header = GameBox.ParseHeader(Path.Combine("Files", fileName));
        var refTable = header.RefTable;

        if (refTable is null) throw new Exception();

        var expectedAncestorLevel = refTable.AncestorLevel;
        var expectedFiles = JsonSerializer.Serialize(refTable.Files, jsonSeralizerOptions);
        var expectedFolders = JsonSerializer.Serialize(refTable.Folders, jsonSeralizerOptions);

        using var ms = new MemoryStream();

        using (var writer = new GameBoxWriter(ms))
        {
            refTable.Write(writer);
        }

        ms.Seek(0, SeekOrigin.Begin);

        using var reader = new GameBoxReader(ms);

        refTable.Read(reader);

        var actualAncestorLevel = refTable.AncestorLevel;
        var actualFiles = JsonSerializer.Serialize(refTable.Files, jsonSeralizerOptions);
        var actualFolders = JsonSerializer.Serialize(refTable.Folders, jsonSeralizerOptions);

        Assert.Equal(expectedAncestorLevel, actualAncestorLevel);
        Assert.Equal(expectedFiles, actualFiles);
        Assert.Equal(expectedFolders, actualFolders);
    }
}
