namespace GBX.NET.PAK;

public sealed class Pak
{
    public static Task<Pak> ParseAsync(Stream stream, byte[] key, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new Pak());
    }
}
