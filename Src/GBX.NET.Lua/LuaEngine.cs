namespace GBX.NET.Lua;

public class LuaEngine
{
    public string? Open()
    {
        var state = LuaCAux.luaL_newstate();
        var test = LuaCAux.luaL_dostring(state, "function xyz() {\n" +
                                                           "   console.log(\"Hello world!\");\n" +
                                                           "}");
        return LuaC.lua_tostring(state);
    }
}
