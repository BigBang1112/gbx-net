using System.Reflection.Emit;

namespace GBX.NET;

// Thanks! https://stackoverflow.com/a/42437504/3923447

public static class TypeSize<T> where T : struct
{
    public readonly static int Size;

    static TypeSize()
    {
        var dm = new DynamicMethod("SizeOfType", typeof(int), Array.Empty<Type>());
        ILGenerator il = dm.GetILGenerator();
        il.Emit(OpCodes.Sizeof, typeof(T));
        il.Emit(OpCodes.Ret);
        Size = (int)dm.Invoke(null, null)!;
    }
}
