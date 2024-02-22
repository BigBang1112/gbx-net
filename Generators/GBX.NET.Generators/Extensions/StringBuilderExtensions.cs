using System.Text;

namespace GBX.NET.Generators.Extensions;

internal static class StringBuilderExtensions
{
    public static void AppendIndent(this StringBuilder sb, int indent)
    {
        for (var i = 0; i < indent; i++)
        {
            sb.Append("    ");
        }
    }

    public static void Append(this StringBuilder sb, int indent, string value)
    {
        sb.AppendIndent(indent);
        sb.Append(value);
    }

    public static void AppendLine(this StringBuilder sb, int indent, string value)
    {
        sb.AppendIndent(indent);
        sb.AppendLine(value);
    }
}
