namespace GBX.NET.Lua;

public interface ILib
{
    void Register(LuaCTypes.lua_State state);
}
