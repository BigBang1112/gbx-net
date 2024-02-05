using System.Runtime.InteropServices;

using static GBX.NET.Lua.LuaCTypes;

namespace GBX.NET.Lua;

internal static partial class LuaC
{
    public const string LibName = "lua";

    [LibraryImport(LibName)]
    public static partial void luaopen_base(lua_State L);

    [LibraryImport(LibName)]
    public static partial void luaopen_package(lua_State L);

    [LibraryImport(LibName)]
    public static partial void luaopen_coroutine(lua_State L);

    [LibraryImport(LibName)]
    public static partial void luaopen_string(lua_State L);

    [LibraryImport(LibName)]
    public static partial void luaopen_utf8(lua_State L);

    [LibraryImport(LibName)]
    public static partial void luaopen_table(lua_State L);

    [LibraryImport(LibName)]
    public static partial void luaopen_math(lua_State L);

    [LibraryImport(LibName)]
    public static partial void luaopen_io(lua_State L);

    [LibraryImport(LibName)]
    public static partial void luaopen_os(lua_State L);

    [LibraryImport(LibName)]
    public static partial void luaopen_debug(lua_State L);

    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    public static partial LuaType lua_getglobal(lua_State L, string name);

    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    public static partial void lua_setglobal(lua_State L, string name);

    [LibraryImport(LibName)]
    public static partial LuaStatusCode lua_pcallk(lua_State L, int nargs, int nresults, int errfunc, int ctx, nint k);

    public static LuaStatusCode lua_pcall(lua_State L, int nargs, int nresults, int errfunc)
        => lua_pcallk(L, nargs, nresults, errfunc, 0, nint.Zero);

    [LibraryImport(LibName)]
    public static partial nint lua_tolstring(lua_State L, int index, out int len);

    public static string? lua_tostring(lua_State L, int index = -1)
    {
        var strPtr = lua_tolstring(L, index, out var len);

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
    public static partial void lua_pushcclosure(lua_State L, lua_CFunction fn, int n);

    public static void lua_pushcfunction(lua_State L, lua_CFunction f)
    {
        lua_pushcclosure(L, f, 0);
    }

    public static void lua_register(lua_State L, string name, lua_CFunction f)
    {
        lua_pushcfunction(L, f);
        lua_setglobal(L, name);
    }

    [LibraryImport(LibName)]
    public static partial int lua_gettop(lua_State L);

    [LibraryImport(LibName)]
    public static partial void lua_close(lua_State L);

    [LibraryImport(LibName)]
    public static partial LuaType lua_type(lua_State l, int index);

    [LibraryImport(LibName)]
    public static partial int lua_isnumber(lua_State L, int index);

    [LibraryImport(LibName)]
    public static partial int lua_isinteger(lua_State L, int index);

    [LibraryImport(LibName)]
    public static partial int lua_isstring(lua_State L, int index);

    public static int lua_isboolean(lua_State L, int index) => lua_type(L, index) is LuaType.Boolean ? 1 : 0;

    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    public static partial nint lua_pushstring(lua_State L, string s);

    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    public static partial nint lua_pushlstring(lua_State L, string s, nint len);

    [LibraryImport(LibName)]
    public static partial void lua_pushnumber(lua_State L, double n);

    [LibraryImport(LibName)]
    public static partial void lua_pushinteger(lua_State L, int n);

    [LibraryImport(LibName)]
    public static partial void lua_pushboolean(lua_State L, int b);

    [LibraryImport(LibName)]
    public static partial void lua_pushnil(lua_State L);

    [LibraryImport(LibName)]
    public static partial int lua_error(lua_State L);

    [LibraryImport(LibName)]
    public static partial double lua_tonumberx(lua_State L, int index, nint isnum);

    public static double lua_tonumber(lua_State L, int index) => lua_tonumberx(L, index, nint.Zero);

    [LibraryImport(LibName)]
    public static partial int lua_tointegerx(lua_State L, int index, nint isnum);

    public static int lua_tointeger(lua_State L, int index) => lua_tointegerx(L, index, nint.Zero);

    [LibraryImport(LibName)]
    public static partial int lua_toboolean(lua_State L, int index);

    [LibraryImport(LibName)]
    public static partial void lua_createtable(lua_State L, int narray, int nrec);

    public static void lua_newtable(lua_State L) => lua_createtable(L, 0, 0);

    [LibraryImport(LibName, StringMarshalling = StringMarshalling.Utf8)]
    public static partial void lua_setfield(lua_State L, int index, string k);

    [LibraryImport(LibName)]
    public static partial void lua_settable(lua_State L, int index);

    [LibraryImport(LibName)]
    public static partial void lua_settop(lua_State L, int index);

    public static void lua_pop(lua_State L, int n) => lua_settop(L, -n - 1);

    [LibraryImport(LibName)]
    public static partial int lua_checkstack(lua_State L, int n);

    [LibraryImport(LibName)]
    public static partial nint lua_topointer(lua_State L, int index);
}
