namespace GBX.NET.Lua;

public enum LuaStatusCode
{
    Ok,
    Yield,
    ErrRun,
    ErrSyntax,
    ErrMem,
    ErrGcmm,
    ErrErr
}
