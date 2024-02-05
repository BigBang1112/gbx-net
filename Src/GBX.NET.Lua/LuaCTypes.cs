namespace GBX.NET.Lua;

internal static class LuaCTypes
{
    public struct lua_State
    {
        public nint pointer;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="L">State.</param>
    /// <returns>Number of results.</returns>
    public delegate int lua_CFunction(lua_State L);
}
