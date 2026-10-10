using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace GBX.NET.Benchmarks.Generation;

internal sealed class GeneratorAdditionalText : AdditionalText
{
    private readonly SourceText text;

    public override string Path { get; }

    public GeneratorAdditionalText(string path, string source)
    {
        Path = path;
        text = SourceText.From(source);
    }

    public override SourceText GetText(CancellationToken cancellationToken = default) => text;
}
