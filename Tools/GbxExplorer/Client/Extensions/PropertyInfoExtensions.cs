using System.Reflection;

namespace GbxExplorer.Client.Extensions;

public static class PropertyInfoExtensions
{
    public static bool CannotWrite(this PropertyInfo property)
    {
        if (!property.CanWrite)
        {
            return true;
        }

        var setMethod = property.SetMethod;

        if (setMethod is null)
        {
            return true;
        }

        // Get the modifiers applied to the return parameter.
        var setMethodReturnParameterModifiers = setMethod.ReturnParameter.GetRequiredCustomModifiers();

        // Init-only properties are marked with the IsExternalInit type.
        return setMethodReturnParameterModifiers.Contains(typeof(System.Runtime.CompilerServices.IsExternalInit));
    }
}
