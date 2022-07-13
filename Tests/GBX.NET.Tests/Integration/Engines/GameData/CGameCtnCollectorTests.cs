using GBX.NET.Engines.GameData;
using Xunit;

namespace GBX.NET.Tests.Integration.Engines.GameData;

public class CGameCtnCollectorTests
{
    public class Chunk2E001004Tests
    {
        [Fact]
        public void ReadAndWrite_ManiaPlanet_Latest_DataShouldEqual()
        {
            // Arrange
            using ChunkReadWriteEqualityTester<CGameCtnCollector, CGameCtnCollector.Chunk2E001004> chunkTester
                = new(GameVersions.ManiaPlanet_Latest, idWasWritten: true);

            // Act
            chunkTester.ReadWrite();

            // Assert
            Assert.True(chunkTester.ReadWriteIsEqual());
        }

        [Fact]
        public void ReadAndWrite_Trackmania2020_2022_7_6_DataShouldEqual()
        {
            // Arrange
            using ChunkReadWriteEqualityTester<CGameCtnCollector, CGameCtnCollector.Chunk2E001004> chunkTester
                = new(GameVersions.Trackmania2020_2022_7_6, idWasWritten: true);

            // Act
            chunkTester.ReadWrite();

            // Assert
            Assert.True(chunkTester.ReadWriteIsEqual());
        }
    }
}
