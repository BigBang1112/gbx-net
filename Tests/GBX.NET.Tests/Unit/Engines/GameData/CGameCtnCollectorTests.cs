using GBX.NET.Engines.GameData;
using GBX.NET.Managers;
using System.Drawing;
using System.IO;
using System.Text;
using Xunit;

namespace GBX.NET.Tests.Unit.Engines.GameData;

public class CGameCtnCollectorTests
{
    public class Chunk2E001004Tests
    {
        private const string IconWebP = "IconWebP";

        [Fact]
        public void Read_ManiaPlanet_Latest_ShouldReadIcon() // TODO: Contents is not tested yet
        {
            // Arrange
            using ChunkReadTester<CGameCtnCollector, CGameCtnCollector.Chunk2E001004> chunkTester
                = new(GameVersions.ManiaPlanet_Latest);

            var node = chunkTester.Node;

            // Act
            chunkTester.Read();

            // Assert
            Assert.NotNull(node.Icon);
            Assert.True(node.Icon!.GetLength(0) == 64);
            Assert.True(node.Icon!.GetLength(1) == 64);
        }

        [Fact]
        public void Read_Trackmania2020_2022_7_6_ShouldReadIconWebp()
        {
            // Arrange
            using ChunkReadTester<CGameCtnCollector, CGameCtnCollector.Chunk2E001004> chunkTester
                = new(GameVersions.Trackmania2020_2022_7_6);

            var node = chunkTester.Node;
            var chunk = chunkTester.Chunk;

            var expectedIconWebp = chunkTester.GetAdditionalEntryData(IconWebP);
            var expectedU01 = (short)1;

            // Act
            chunkTester.Read();

            // Assert
            Assert.NotNull(node.IconWebP);
            Assert.Equal(expectedIconWebp, actual: node.IconWebP);
            Assert.Equal(expectedU01, actual: chunk.U01);
            Assert.Null(node.Icon);
        }

        [Fact]
        public void Write_ManiaPlanet_Latest_ShouldWriteIcon() // TODO: Contents is not tested yet
        {
            // Arrange
            using ChunkWriteTester<CGameCtnCollector, CGameCtnCollector.Chunk2E001004> chunkTester
                = new(GameVersions.ManiaPlanet_Latest);

            chunkTester.Node.Icon = new Color[64, 64];

            // Act
            chunkTester.Write();

            // Assert
        }

        [Fact]
        public void Write_Trackmania2020_2022_7_6_ShouldWriteIconWebp()
        {
            // Arrange
            using ChunkWriteTester<CGameCtnCollector, CGameCtnCollector.Chunk2E001004> chunkTester
                = new(GameVersions.Trackmania2020_2022_7_6);

            chunkTester.Node.IconWebP = chunkTester.GetAdditionalEntryData(IconWebP);
            chunkTester.Chunk.U01 = 1;
            
            // Act
            chunkTester.Write();

            // Assert
            Assert.True(chunkTester.ExpectedDataEqualActualData());
        }
    }

    public class Chunk2E00100BTests
    {
        [Fact]
        public void ReadWrite_ManiaPlanet_Latest_ShouldRead()
        {
            // Arrange
            using ChunkReadTester<CGameCtnCollector, CGameCtnCollector.Chunk2E00100B> chunkTester
                = new(GameVersions.ManiaPlanet_Latest, idVersionWasWritten: true);

            // Act
            chunkTester.ReadWriteWithReader();

            // Assert
            Assert.Equal(expected: new("", "Stadium", "bigbang1112"), actual: chunkTester.Node.Author);
        }
        
        [Fact]
        public void ReadWrite_Trackmania2020_2022_7_6_ShouldRead()
        {
            // Arrange
            using ChunkReadTester<CGameCtnCollector, CGameCtnCollector.Chunk2E00100B> chunkTester
                = new(GameVersions.Trackmania2020_2022_7_6, idVersionWasWritten: true);

            // Act
            chunkTester.ReadWriteWithReader();

            // Assert
            Assert.Equal(expected: new("", new(26), "akPfIM0aSzuHuaaDWptBbQ"), actual: chunkTester.Node.Author);
        }

        [Fact]
        public void ReadWrite_ManiaPlanet_Latest_ShouldWrite()
        {
            // Arrange
            using ChunkWriteTester<CGameCtnCollector, CGameCtnCollector.Chunk2E00100B> chunkTester
                = new(GameVersions.ManiaPlanet_Latest, idVersionWasWritten: true);

            chunkTester.Node.Author = new("", "Stadium", "bigbang1112");

            // Act
            chunkTester.ReadWriteWithWriter();

            // Assert
            Assert.True(chunkTester.ExpectedDataEqualActualData());
        }

        [Fact]
        public void ReadWrite_Trackmania2020_2022_7_6_ShouldWrite()
        {
            // Arrange
            using ChunkWriteTester<CGameCtnCollector, CGameCtnCollector.Chunk2E00100B> chunkTester
                = new(GameVersions.Trackmania2020_2022_7_6, idVersionWasWritten: true);

            chunkTester.Node.Author = new("", new(26), "akPfIM0aSzuHuaaDWptBbQ");

            // Act
            chunkTester.ReadWriteWithWriter();

            // Assert
            Assert.True(chunkTester.ExpectedDataEqualActualData());
        }
    }
    
    public class Chunk2E00100CTests
    {
        [Fact]
        public void ReadWrite_ManiaPlanet_Latest_ShouldRead()
        {
            // Arrange
            using ChunkReadTester<CGameCtnCollector, CGameCtnCollector.Chunk2E00100C> chunkTester
                = new(GameVersions.ManiaPlanet_Latest, idVersionWasWritten: true);

            // Act
            chunkTester.ReadWriteWithReader();

            // Assert
            Assert.Equal(expected: "New Item", actual: chunkTester.Node.Name);
        }

        [Fact]
        public void ReadWrite_Trackmania2020_2022_7_6_ShouldRead()
        {
            // Arrange
            using ChunkReadTester<CGameCtnCollector, CGameCtnCollector.Chunk2E00100C> chunkTester
                = new(GameVersions.Trackmania2020_2022_7_6, idVersionWasWritten: true);

            // Act
            chunkTester.ReadWriteWithReader();

            // Assert
            Assert.Equal(expected: "Item.2022.7.6.1137", actual: chunkTester.Node.Name);
        }

        [Fact]
        public void ReadWrite_ManiaPlanet_Latest_ShouldWrite()
        {
            // Arrange
            using ChunkWriteTester<CGameCtnCollector, CGameCtnCollector.Chunk2E00100C> chunkTester
                = new(GameVersions.ManiaPlanet_Latest, idVersionWasWritten: true);

            chunkTester.Node.Name = "New Item";

            // Act
            chunkTester.ReadWriteWithWriter();

            // Assert
            Assert.True(chunkTester.ExpectedDataEqualActualData());
        }

        [Fact]
        public void ReadWrite_Trackmania2020_2022_7_6_ShouldWrite()
        {
            // Arrange
            using ChunkWriteTester<CGameCtnCollector, CGameCtnCollector.Chunk2E00100C> chunkTester
                = new(GameVersions.Trackmania2020_2022_7_6, idVersionWasWritten: true);

            chunkTester.Node.Name = "Item.2022.7.6.1137";

            // Act
            chunkTester.ReadWriteWithWriter();

            // Assert
            Assert.True(chunkTester.ExpectedDataEqualActualData());
        }
    }

    public class Chunk2E00100DTests
    {
        [Fact]
        public void ReadWrite_ManiaPlanet_Latest_ShouldRead()
        {
            // Arrange
            using ChunkReadTester<CGameCtnCollector, CGameCtnCollector.Chunk2E00100D> chunkTester
                = new(GameVersions.ManiaPlanet_Latest, idVersionWasWritten: true);

            // Act
            chunkTester.ReadWriteWithReader();

            // Assert
            Assert.Equal(expected: "No Description", actual: chunkTester.Node.Description);
        }

        [Fact]
        public void ReadWrite_Trackmania2020_2022_7_6_ShouldRead()
        {
            // Arrange
            using ChunkReadTester<CGameCtnCollector, CGameCtnCollector.Chunk2E00100D> chunkTester
                = new(GameVersions.Trackmania2020_2022_7_6, idVersionWasWritten: true);

            // Act
            chunkTester.ReadWriteWithReader();

            // Assert
            Assert.Equal(expected: "﻿Žádný popis", actual: chunkTester.Node.Description);
        }

        [Fact]
        public void ReadWrite_ManiaPlanet_Latest_ShouldWrite()
        {
            // Arrange
            using ChunkWriteTester<CGameCtnCollector, CGameCtnCollector.Chunk2E00100D> chunkTester
                = new(GameVersions.ManiaPlanet_Latest, idVersionWasWritten: true);

            chunkTester.Node.Description = "No Description";

            // Act
            chunkTester.ReadWriteWithWriter();

            // Assert
            Assert.True(chunkTester.ExpectedDataEqualActualData());
        }

        [Fact]
        public void ReadWrite_Trackmania2020_2022_7_6_ShouldWrite()
        {
            // Arrange
            using ChunkWriteTester<CGameCtnCollector, CGameCtnCollector.Chunk2E00100D> chunkTester
                = new(GameVersions.Trackmania2020_2022_7_6, idVersionWasWritten: true);

            chunkTester.Node.Description = "﻿Žádný popis";

            // Act
            chunkTester.ReadWriteWithWriter();

            // Assert
            Assert.True(chunkTester.ExpectedDataEqualActualData());
        }
    }

    public class Chunk2E00100ETests
    {
        [Fact]
        public void ReadWrite_ManiaPlanet_Latest_ShouldRead()
        {
            // Arrange
            using ChunkReadTester<CGameCtnCollector, CGameCtnCollector.Chunk2E00100E> chunkTester
                = new(GameVersions.ManiaPlanet_Latest, idVersionWasWritten: true);

            // Act
            chunkTester.ReadWriteWithReader();

            // Assert
            Assert.True(chunkTester.Node.IconUseAutoRender);
            Assert.Equal(expected: 2, actual: chunkTester.Node.IconQuarterRotationY);
        }

        // 2022.7.6.1137 no longer has 0x00E

        [Fact]
        public void ReadWrite_ManiaPlanet_Latest_ShouldWrite()
        {
            // Arrange
            using ChunkWriteTester<CGameCtnCollector, CGameCtnCollector.Chunk2E00100E> chunkTester
                = new(GameVersions.ManiaPlanet_Latest, idVersionWasWritten: true);

            chunkTester.Node.IconUseAutoRender = true;
            chunkTester.Node.IconQuarterRotationY = 2;

            // Act
            chunkTester.ReadWriteWithWriter();

            // Assert
            Assert.True(chunkTester.ExpectedDataEqualActualData());
        }

        // 2022.7.6.1137 no longer has 0x00E
    }
}
