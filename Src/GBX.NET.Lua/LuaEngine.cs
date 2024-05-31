using GBX.NET.Lua.Libs;
using static GBX.NET.Lua.LuaCTypes;

namespace GBX.NET.Lua;

public class LuaEngine
{
    private readonly lua_State state;

    public static readonly lua_CFunction DefaultPrintFunc = state =>
    {
        var n = LuaC.lua_gettop(state);

        for (var i = 1; i <= n; i++)
        {
            var s = LuaCAux.luaL_tolstring(state, i);

            Console.Write(s);

            if (i < n)
            {
                Console.Write(' ');
            }
        }

        Console.WriteLine();

        return 0;
    };

    public lua_CFunction PrintFunc { get; init; } = DefaultPrintFunc;

    public LogLib LogLib { get; init; } = new();

    public LuaEngine()
    {
        state = LuaCAux.luaL_newstate();

        LuaCAux.luaL_openlibs(state);

        Register();
    }

    private void Register()
    {
        LuaC.lua_register(state, "print", PrintFunc);
        LogLib.Register(state);
    }

    public void Run(string code)
    {
        var statusCode = LuaCAux.luaL_dostring(state, code);

        if (statusCode != LuaStatusCode.Ok)
        {
            throw new LuaException(LuaC.lua_tostring(state) ?? "Unknown error");
        }
    }
}
