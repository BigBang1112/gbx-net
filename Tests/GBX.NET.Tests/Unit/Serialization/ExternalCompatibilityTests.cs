using GBX.NET.Serialization;
using System.Reflection;

namespace GBX.NET.Tests.Unit.Serialization;

[Category("Unit")]
public class ExternalCompatibilityTests
{
    [Test]
    [Arguments(typeof(IGbxReader))]
    [Arguments(typeof(IGbxWriter))]
    [Arguments(typeof(IGbxReaderWriter))]
    [Arguments(typeof(GbxReader))]
    [Arguments(typeof(GbxWriter))]
    [Arguments(typeof(GbxReaderWriter))]
    public async Task SerializationMethods_WithExternalReferences_AreObsolete(Type type)
    {
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(method => ContainsExternal(method.ReturnType) || method.GetParameters().Any(parameter => ContainsExternal(parameter.ParameterType)))
            .ToArray();

        await Assert.That(methods.Length).IsGreaterThan(0);
        foreach (var method in methods)
            await Assert.That(method.GetCustomAttribute<ObsoleteAttribute>()).IsNotNull().Because(type.Name + "." + method.Name);
    }

    private static bool ContainsExternal(Type type) => type.HasElementType
        ? ContainsExternal(type.GetElementType()!)
        : type.IsGenericType && (type.GetGenericTypeDefinition().FullName == "GBX.NET.External`1" || type.GetGenericArguments().Any(ContainsExternal));
}
