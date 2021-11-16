using GBX.NET.Engines.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GBX.NET.Tests.Integration;

public class GameBoxTests
{
    [Theory]
    [InlineData("CCP#04 - ODYSSEY.Map.Gbx")]
    public void ParseMap(string fileName)
    {
        var node = GameBox.ParseNode(Path.Combine("Files", fileName));
    }

    [Theory]
    [InlineData("CCP#04 - ODYSSEY.Map.Gbx")]
    public void ParseSaveMap(string fileName)
    {
        var node = GameBox.ParseNode(Path.Combine("Files", fileName));
        using var ms = new MemoryStream();
        node.Save(ms);
    }

    [Theory(DisplayName = "Parse/Save map without exceptions")]
    [InlineData("CCP#04 - ODYSSEY.Map.Gbx")]
    [InlineData("Summer 2021 - 25.Map.Gbx")]
    public void ParseSaveMapWithoutExceptions(string fileName)
    {
        var node = GameBox.ParseNode(Path.Combine("Files", fileName));
        using var ms = new MemoryStream();
        node.Save(ms);
    }
}
