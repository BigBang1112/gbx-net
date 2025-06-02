﻿using GBX.NET.Managers;
#if NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace GBX.NET.Components;

public sealed class GbxHeader<T>(GbxHeaderBasic basic) : GbxHeader(basic) where T : IClass
{
#if !NET8_0_OR_GREATER
    private uint? classId;
#endif

    public override uint ClassId =>
#if NET8_0_OR_GREATER
        T.Id;
#else
        classId ??= ClassManager.GetId<T>() ?? throw new Exception("Class ID not available");
#endif

#if NET8_0_OR_GREATER
    [Experimental("GBXNET10001")]
#endif
#if NETSTANDARD2_0
    public override GbxHeader DeepClone() => new GbxHeader<T>(Basic);
#else
    public override GbxHeader<T> DeepClone() => new(Basic);
#endif

    public override string ToString()
    {
        return $"GbxHeader ({ClassManager.GetName(ClassId)}, 0x{ClassId:X8}, known)";
    }
}