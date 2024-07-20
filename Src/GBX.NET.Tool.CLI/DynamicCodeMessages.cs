namespace GBX.NET.Tool.CLI;

internal static class DynamicCodeMessages
{
    internal const string MakeGenericTypeMessage = "This method uses reflection (MakeGenericType) to create collections for constructors.";
    internal const string JsonSerializeMessage = "If context is null, this method uses reflection for JSON serialization and deserialization.";
    internal const string DynamicRunMessage = "This method uses reflection (MakeGenericType) to create collections for constructors. Though if the tool does not use collections in constructors, it could work. ToolConsoleOptions.JsonSerializerContext should be also set for correct JSON serialization and deserialization. Please test the build.";
    internal const string UnreferencedRunMessage = "ToolConsoleOptions.JsonSerializerContext should be set for correct JSON serialization and deserialization.";
}
