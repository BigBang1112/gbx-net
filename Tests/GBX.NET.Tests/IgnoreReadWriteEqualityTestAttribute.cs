using System;

namespace GBX.NET.Tests;

[AttributeUsage(AttributeTargets.Method)]
public class IgnoreReadWriteEqualityTestAttribute : Attribute
{
}
