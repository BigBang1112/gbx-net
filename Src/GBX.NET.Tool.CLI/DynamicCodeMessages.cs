namespace GBX.NET.Tool.CLI;

internal static class DynamicCodeMessages
{
    internal const string MakeGenericTypeMessage = "This method uses reflection (MakeGenericType) to create collections for constructors. Though if the tool does not use collections in constructors, it could work. Please test the build.";
}
