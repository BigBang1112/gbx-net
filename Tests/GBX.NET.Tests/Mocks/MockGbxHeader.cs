using GBX.NET.Components;

namespace GBX.NET.Tests.Mocks;

public sealed class MockGbxHeader(GbxHeaderBasic basic) : GbxHeader(basic)
{
    public override uint ClassId => throw new NotImplementedException();
}
