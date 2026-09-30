namespace GBX.NET.Tool;

/// <summary>
/// Attribute to mark a config property to be serialized as an external file.
/// </summary>
/// <param name="fileNameWithoutExtension">The name of the file to be created and read from, without its extension.</param>
[AttributeUsage(AttributeTargets.Property)]
public class ExternalFileAttribute(string fileNameWithoutExtension) : Attribute
{
    public string FileNameWithoutExtension { get; } = fileNameWithoutExtension;
}
