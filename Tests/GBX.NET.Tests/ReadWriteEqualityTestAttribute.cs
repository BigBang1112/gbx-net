using System;

namespace GBX.NET.Tests;

[AttributeUsage(AttributeTargets.Method)]
public class ReadWriteEqualityTestAttribute : Attribute
{
    public bool FirstIdOccurance { get; set; }
}
