using System.Runtime.InteropServices;

using static GBX.NET.Lua.LuaCTypes;
using static GBX.NET.Lua.LuaCAuxTypes;

namespace GBX.NET.Lua;

internal static partial class LuaCAux
{
    public const string LibName = "lua";

#pragma warning disable IDE1006

    [LibraryImport(LibName)]
    public static partial lua_State luaL_newstate();

    [LibraryImport(LibName)]
    public static partial void luaL_openlibs(lua_State L);

    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    public static partial LuaStatusCode luaL_loadstring(lua_State L, string str);

    public static LuaStatusCode luaL_dostring(lua_State L, string str)
    {
        var result = luaL_loadstring(L, str);
        return result is not LuaStatusCode.Ok ? result : LuaC.lua_pcall(L, 0, -1, 0);
    }

    public static void luaL_newlibtable(lua_State L, luaL_Reg[] l) => LuaC.lua_createtable(L, 0, l.Length - 1);

    public static LuaStatusCode luaL_dofile(lua_State L, string filename)
    {
        luaL_loadfile(L, filename);
        return LuaC.lua_pcall(L, 0, -1, 0);
    }

    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    public static partial int luaL_loadfilex(lua_State L, string filename, string? mode);

    public static int luaL_loadfile(lua_State L, string filename) => luaL_loadfilex(L, filename, mode: null);

    [LibraryImport(LibName)]
    public static partial void luaL_where(lua_State L, int lvl);

    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    public static partial int luaL_error(lua_State L, string fmt);

    [LibraryImport(LibName)]
    public static partial nint luaL_tolstring(lua_State L, int index, out int len);

    public static string? luaL_tolstring(lua_State L, int index = -1)
    {
        var strPtr = luaL_tolstring(L, index, out var len);

        if (strPtr == nint.Zero)
        {
            return null;
        }

        unsafe
        {
            return len == 0 ? "" : new string((sbyte*)strPtr, 0, len, System.Text.Encoding.UTF8);
        }
    }

    [LibraryImport(LibName)]
    public static partial int luaL_ref(lua_State L, int t);
}