using Microsoft.Extensions.Logging;
using static GBX.NET.Lua.LuaCTypes;

namespace GBX.NET.Lua.Libs;

public sealed class LogLib : ILib
{
    public static ILogger? Logger { get; set; }

    public lua_CFunction TraceFunc { get; init; } = state => Log(state, LogLevel.Trace);
    public lua_CFunction DebugFunc { get; init; } = state => Log(state, LogLevel.Debug);
    public lua_CFunction InfoFunc { get; init; } = state => Log(state, LogLevel.Information);
    public lua_CFunction WarnFunc { get; init; } = state => Log(state, LogLevel.Warning);
    public lua_CFunction ErrorFunc { get; init; } = state => Log(state, LogLevel.Error);

    public void Register(lua_State state)
    {
        LuaC.lua_createtable(state, 0, 5);
        LuaHelper.RegisterFieldFunction(state, TraceFunc, "trace");
        LuaHelper.RegisterFieldFunction(state, DebugFunc, "debug");
        LuaHelper.RegisterFieldFunction(state, InfoFunc, "info");
        LuaHelper.RegisterFieldFunction(state, WarnFunc, "warn");
        LuaHelper.RegisterFieldFunction(state, ErrorFunc, "error");
        LuaC.lua_setglobal(state, "log");
    }

    private static int Log(lua_State state, LogLevel level)
    {
        var n = LuaC.lua_gettop(state);

        if (n == 0)
        {
            // error
            return 0;
        }

        var message = LuaCAux.luaL_tolstring(state, 1);

        var args = new object?[n - 1];
        for (int i = 0; i < n - 1; i++)
        {
            args[i] = LuaCAux.luaL_tolstring(state, i + 2);
        }

        Logger?.Log(level, message, args);

        return 0;
    }
}
