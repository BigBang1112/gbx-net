using System;

namespace GBX.NET.Tests;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class IgnoreReadWriteEqualityTestAttribute : Attribute
{
}
