using System.Text;

namespace GBX.NET.Generators.Generation;

internal sealed class CodeWriter
{
    private readonly StringBuilder builder = new();
    private string lastLine = "";
    public int Indent { get; set; }

    public void Line(string text = "")
    {
        if (text.Length > 0) builder.Append(' ', Indent * 4);
        builder.AppendLine(text.TrimEnd());
        lastLine = text.TrimEnd();
    }

    public void BlankLine()
    {
        if (builder.Length > 0 && lastLine.Length > 0 && lastLine != "{")
            Line();
    }

    public void Open(string text)
    {
        Line(text);
        Line("{");
        Indent++;
    }

    public void Close(string suffix = "")
    {
        if (lastLine.Length == 0)
        {
            builder.Length--;
            if (builder.Length > 0 && builder[builder.Length - 1] == '\r')
                builder.Length--;
        }
        Indent--;
        Line("}" + suffix);
    }
    
    public override string ToString() => builder.ToString();
}
