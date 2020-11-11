using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace GBX.NET.ZIP
{
    public class CustomStaticDataSource : IStaticDataSource
    {
        private Stream stream;
        public Stream GetSource() => stream;

        public void SetStream(Stream inputStream)
        {
            stream = inputStream;
            stream.Position = 0;
        }
    }
}