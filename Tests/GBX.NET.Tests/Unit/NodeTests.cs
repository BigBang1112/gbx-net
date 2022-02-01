using GBX.NET.Engines.Game;
using Xunit;

namespace GBX.NET.Tests.Unit;

public class NodeTests
{
    [Fact]
    public void ToGbx_GameBoxHasEqualNode()
    {
        var node = CGameCtnMediaBlockSound.Create().ForTM2().Build();

        var gbx = node.ToGbx();

        Assert.Equal(node, gbx.Node);
    }

    [Fact]
    public void ToGbx_NonGenericParameter_NoExceptionIsThrown()
    {
        var node = CGameCtnMediaBlockSound.Create().ForTM2().Build();

        node.ToGbx(nonGeneric: true);
    }

    [Fact]
    public void ToGbx_NonGenericParameter_GameBoxHasEqualNode()
    {
        var node = CGameCtnMediaBlockSound.Create().ForTM2().Build();

        var gbx = node.ToGbx(nonGeneric: true);

        Assert.Equal(node, gbx.Node);
    }

    [Fact]
    public void ToGbx_Generic_GameBoxHasEqualNode()
    {
        var node = CGameCtnMediaBlockSound.Create().ForTM2().Build();

        var gbx = node.ToGbx<CGameCtnMediaBlockSound>();

        Assert.Equal(node, gbx.Node);
    }
}
