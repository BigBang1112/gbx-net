namespace GBX.NET.Tool;

/// <summary>
/// Attribute to mark a config property to be serialized as an external file.
/// </summary>
/// <param name="fileName">The name of the file to be created and read from.</param>
[AttributeUsage(AttributeTargets.Property)]
public class ExternalFileAttribute(string fileNameWithoutExtension) : Attribute
{
    public string FileNameWithoutExtension { get; } = fileNameWithoutExtension;
}
