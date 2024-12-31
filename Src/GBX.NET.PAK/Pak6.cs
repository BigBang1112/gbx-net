namespace GBX.NET.PAK;

internal sealed class Pak6
{
    public static Task<Pak6> ParseAsync(Stream stream, byte[] key, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new Pak6());
    }
}
