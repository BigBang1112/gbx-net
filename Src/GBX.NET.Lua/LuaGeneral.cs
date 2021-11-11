using System;
using System.Collections.Generic;
using System.Text;
using Neo.IronLua;

namespace GBX.NET.Lua;

public class LuaGeneral
{
    public LuaGeneral()
    {
        using var lua = new Neo.IronLua.Lua();
        var env = lua.CreateEnvironment();
    }
}
