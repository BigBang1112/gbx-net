using GBX.NET.Extensions;
using Joveler.Compression.ZLib;
using System.Runtime.InteropServices;

namespace GBX.NET.ZLib;

public sealed class ZLib : IZLib
{
    static ZLib()
    {
        InitNativeLibrary();
    }

    public void Compress(Stream input, Stream output)
    {
        using var zlib = new ZLibStream(output, new ZLibCompressOptions { LeaveOpen = true });
        input.CopyTo(zlib);
    }

    public void Decompress(Stream input, Stream output)
    {
        using var zlib = new ZLibStream(input, new ZLibDecompressOptions { LeaveOpen = true });
        zlib.CopyTo(output);
    }

    private static void InitNativeLibrary()
    {
        string libDir = "runtimes";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            libDir = Path.Combine(libDir, "win-");
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.Create("Browser")))
            libDir = Path.Combine(libDir, "linux-");
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            libDir = Path.Combine(libDir, "osx-");

        switch (RuntimeInformation.ProcessArchitecture)
        {
            case Architecture.X86:
                libDir += "x86";
                break;
            case Architecture.X64:
                libDir += "x64";
                break;
            case Architecture.Arm:
                libDir += "arm";
                break;
            case Architecture.Arm64:
                libDir += "arm64";
                break;
        }
        libDir = Path.Combine(libDir, "native");

        string? libPath = null;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            libPath = Path.Combine(libDir, "zlib1.dll");
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.Create("Browser")))
            libPath = Path.Combine(libDir, "libz.so");
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            libPath = Path.Combine(libDir, "libz.dylib");

        if (libPath == null)
            throw new PlatformNotSupportedException($"Unable to find native library.");
        if (!File.Exists(libPath))
            throw new PlatformNotSupportedException($"Unable to find native library [{libPath}].");

        ZLibInit.GlobalInit(libPath, new ZLibInitOptions());
    }
}
