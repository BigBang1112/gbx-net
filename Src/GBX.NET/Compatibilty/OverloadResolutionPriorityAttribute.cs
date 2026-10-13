#if !NET9_0_OR_GREATER

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Property, Inherited = false)]
internal sealed class OverloadResolutionPriorityAttribute : Attribute
{
    public int Priority { get; }

    public OverloadResolutionPriorityAttribute(int priority) => Priority = priority;
}

#endif
