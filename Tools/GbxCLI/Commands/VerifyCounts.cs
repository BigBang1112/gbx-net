namespace GbxCLI.Commands;

internal readonly record struct VerifyCounts(int Parsed, int Failed, int Skipped)
{
    public static VerifyCounts operator +(VerifyCounts left, VerifyCounts right)
        => new(left.Parsed + right.Parsed, left.Failed + right.Failed, left.Skipped + right.Skipped);

    public override string ToString() => $"Parsed {Parsed} Gbx files; {Failed} failed; {Skipped} non-Gbx files skipped.";
}
