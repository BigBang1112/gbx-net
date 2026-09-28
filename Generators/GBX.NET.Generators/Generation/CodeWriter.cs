using System.Text;

namespace GBX.NET.Generators.Generation;

internal sealed class CodeWriter
{
    private readonly StringBuilder builder = new();
    public int Indent { get; set; }

    public void Line(string text = "")
    {
        if (text.Length > 0) builder.Append(' ', Indent * 4);
        builder.AppendLine(text.TrimEnd());
    }

    public void Open(string text)
    {
        Line(text);
        Line("{");
        Indent++;
    }

    public void Close(string suffix = "")
    {
        Indent--;
        Line("}" + suffix);
    }
    
    public override string ToString() => builder.ToString();
}
