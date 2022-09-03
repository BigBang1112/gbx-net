using GBX.NET.Utils;
using System.IO;
using Xunit;

namespace GBX.NET.Tests.Unit.Utils;

public class ObjFileExporterTests
{
    // test constructor
    [Fact]
    public void Constructor_OptionalNull()
    {
        using var objStream = new MemoryStream();
        using var mtlStream = new MemoryStream();
        using var exporter = new ObjFileExporter(objStream, mtlStream);
    }
}
