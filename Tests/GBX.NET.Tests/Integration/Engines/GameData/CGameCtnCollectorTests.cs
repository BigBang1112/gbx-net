using GBX.NET.Engines.GameData;
using GBX.NET.Managers;
using System.Drawing;
using System.IO;
using System.Text;
using Xunit;

namespace GBX.NET.Tests.Integration.Engines.GameData;

public class CGameCtnCollectorTests
{
    public class Chunk2E001004Tests
    {
        [Fact]
        public void ReadAndWrite_ManiaPlanet_Latest_DataShouldEqual()
        {
            var gameVersion = GameVersions.ManiaPlanet_Latest;
            var node = NodeCacheManager.GetNodeInstance<CGameCtnCollector>();
            var chunk = node.CreateChunk<CGameCtnCollector.Chunk2E001004>();
            var nodeName = node.GetType().Name;
            var chunkName = chunk.GetType().Name;
            
            var inputData = File.ReadAllBytes(Path.Combine("TestData", "Chunks", $"{nodeName}.{gameVersion}", $"{nodeName}+{chunkName}.dat"));
            using var inputStream = new MemoryStream(inputData);
            using var outputStream = new MemoryStream();
            using var inputReader = new GameBoxReader(inputStream);
            using var outputWriter = new GameBoxWriter(outputStream);
            var rw = new GameBoxReaderWriter(inputReader, outputWriter);

            chunk.ReadWrite(node, rw);

            Assert.Equal(expected: inputData, actual: outputStream.ToArray());
        }

        [Fact]
        public void ReadAndWrite_Trackmania2020_2022_7_6_DataShouldEqual()
        {
            var gameVersion = GameVersions.Trackmania2020_2022_7_6;
            var node = NodeCacheManager.GetNodeInstance<CGameCtnCollector>();
            var chunk = node.CreateChunk<CGameCtnCollector.Chunk2E001004>();
            var nodeName = node.GetType().Name;
            var chunkName = chunk.GetType().Name;

            var inputData = File.ReadAllBytes(Path.Combine("TestData", "Chunks", $"{nodeName}.{gameVersion}", $"{nodeName}+{chunkName}.dat"));
            using var inputStream = new MemoryStream(inputData);
            using var outputStream = new MemoryStream();
            using var inputReader = new GameBoxReader(inputStream);
            using var outputWriter = new GameBoxWriter(outputStream);
            var rw = new GameBoxReaderWriter(inputReader, outputWriter);

            chunk.ReadWrite(node, rw);

            Assert.Equal(expected: inputData, actual: outputStream.ToArray());
        }
    }
}
