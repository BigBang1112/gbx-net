using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GBX.NET.Tests.Integration
{
    public class GameBoxTests
    {
        [Theory(DisplayName = "Parse/Save map without exceptions")]
        [InlineData("CCP#04 - ODYSSEY.Map.Gbx")]
        public void ParseSaveMapWithoutExceptions(string fileName)
        {
            var node = GameBox.ParseNode(Path.Combine("Files", fileName));
            node.GBX.Header.CompressionOfBody = GameBoxCompression.Uncompressed;
            using var ms = new MemoryStream();
            node.Save(ms);
        }
    }
}
