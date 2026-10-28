namespace GbxCLI.Commands;

internal readonly record struct VerifyCounts(int Parsed, int Failed, int Skipped, int Unstored = 0)
{
    public static VerifyCounts operator +(VerifyCounts left, VerifyCounts right)
        => new(left.Parsed + right.Parsed, left.Failed + right.Failed, left.Skipped + right.Skipped, left.Unstored + right.Unstored);

    public override string ToString() => Unstored == 0
        ? $"Parsed {Parsed} Gbx files; {Failed} failed; {Skipped} non-Gbx files skipped."
        : $"Parsed {Parsed} Gbx files; {Failed} failed; {Skipped} non-Gbx files skipped; {Unstored} entries without stored data skipped.";
}
