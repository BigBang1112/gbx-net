namespace GBX.NET.Lua;

#pragma warning disable IDE1006

public static class LuaCTypes
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
