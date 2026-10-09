using GBX.NET.LZO;

namespace GBX.NET.Tests.Infrastructure;

public static class TestSetup
{
    [Before(Assembly)]
    public static void ConfigureGbx()
    {
        Gbx.LZO = new Lzo();
        Gbx.ZLib = new TestZLib();
        Gbx.StrictBooleans = true;
    }
}
