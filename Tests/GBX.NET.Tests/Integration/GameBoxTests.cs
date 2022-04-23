using System.Collections.Generic;
using System.IO;
using Xunit;

namespace GBX.NET.Tests.Integration;

public class GameBoxTests
{
    public static IEnumerable<string> ExampleMaps { get; } = Directory.GetFiles("Maps", "*.Gbx", SearchOption.AllDirectories);
    public static IEnumerable<string> ExampleReplays { get; } = Directory.GetFiles("Replays", "*.Gbx", SearchOption.AllDirectories);

#if !NET462_OR_GREATER

    [Fact(DisplayName = "Parse example maps - no exception is thrown")]
    public void ParseExampleMaps_NoExceptionIsThrown()
    {
        foreach (var mapFileName in ExampleMaps)
        {
            var node = GameBox.ParseNode(mapFileName);
        }
    }

    [Fact(DisplayName = "Parse and save example maps - no exception is thrown")]
    public void ParseAndSaveExampleMaps_NoExceptionIsThrown()
    {
        foreach (var mapFileName in ExampleMaps)
        {
            var node = GameBox.ParseNode(mapFileName)!;
            using var ms = new MemoryStream();

            node.Save(ms);
        }
    }

    [Fact(DisplayName = "Parse and save example maps, then parse again - no exception is thrown")]
    public void ParseAndSaveExampleMapsThenParseAgain_NoExceptionIsThrown()
    {
        foreach (var mapFileName in ExampleMaps)
        {
            var node = GameBox.ParseNode(mapFileName)!;
            using var ms = new MemoryStream();

            node.Save(ms);

            ms.Position = 0;

            var newNode = GameBox.ParseNode(ms);
        }
    }

    [Fact(DisplayName = "Parse example replays - no exception is thrown")]
    public void ParseExampleReplay_NoExceptionIsThrown()
    {
        foreach (var mapFileName in ExampleReplays)
        {
            var node = GameBox.ParseNode(mapFileName);
        }
    }

#endif

}
