using GBX.NET.Tool.CLI;
#if (EnableCrc32)
using GBX.NET.Hashing;
#endif
#if (EnableZlib)
using GBX.NET.ZLib;
#endif
using MyGbxToolApp;

#if (EnableZlib)
Gbx.ZLib = new ZLib();
#endif
#if (EnableCrc32)
Gbx.CRC32 = new CRC32();
#endif

await ToolConsole<MyGbxToolAppTool>.RunAsync(args, new()
{
    // .. JsonContext etc.
});