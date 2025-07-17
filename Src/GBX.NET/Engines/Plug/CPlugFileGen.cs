﻿namespace GBX.NET.Engines.Plug;

public partial class CPlugFileGen
{
#if NET8_0_OR_GREATER
    static void IClass.Read<T>(T node, GbxReaderWriter rw)
    {
        node.ReadWrite(rw);
    }
#endif

    public override void ReadWrite(GbxReaderWriter rw)
    {
        throw new NotSupportedException("CPlugFileGen is not yet supported");
    }
}