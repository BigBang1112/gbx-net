namespace GBX.NET.PAK;

internal sealed partial class Pak6 : Pak
{
    public byte[] Checksum { get; private set; } = [];
    public uint HeaderFlags { get; private set; }
    public string Comments { get; private set; } = string.Empty;
    public string CreationBuildInfo { get; private set; } = string.Empty;
    public string? AuthorUrl { get; private set; }
    public string? ManialinkUrl { get; private set; }
    public string? DownloadUrl { get; private set; }
    public DateTime? CreationDate { get; private set; }
    public string? Xml { get; private set; }
    public string? TitleId { get; private set; }
    public string? UsageSubDir { get; private set; }
    public IncludedPackHeader[] IncludedPacks { get; private set; } = [];

    public byte[]? U01 { get; private set; }

    public bool IsHeaderPrivate => (HeaderFlags & 0x01) != 0;
    public bool UseDefaultHeaderKey => (HeaderFlags & 0x02) != 0;
    public override bool IsHeaderEncrypted => IsHeaderPrivate || UseDefaultHeaderKey;

    internal Pak6(Stream stream, byte[]? key, int version) : base(stream, key, version)
    {
    }

    internal override async Task ReadHeaderAsync(Stream stream, AsyncGbxReader r, int version, CancellationToken cancellationToken)
    {
        await ReadUnencryptedHeaderAsync(r, version, cancellationToken);
        await base.ReadHeaderAsync(stream, r, version, cancellationToken);
    }

    private async Task ReadUnencryptedHeaderAsync(AsyncGbxReader r, int version, CancellationToken cancellationToken)
    {
        Checksum = await r.ReadBytesAsync(32, cancellationToken);
        HeaderFlags = await r.ReadUInt32Async(cancellationToken);

        if (version >= 15)
        {
            HeaderMaxSize = await r.ReadInt32Async(cancellationToken);
        }

        if (version >= 7)
        {
            AuthorInfo = await ReadAuthorInfoAsync(r, cancellationToken);

            if (version < 9)
            {
                Comments = await r.ReadStringAsync(cancellationToken);
                U01 = await r.ReadBytesAsync(16, cancellationToken); // some blowfish key likely

                if (version == 8)
                {
                    CreationBuildInfo = await r.ReadStringAsync(cancellationToken);
                    AuthorUrl = await r.ReadStringAsync(cancellationToken);
                }
            }
            else
            {
                ManialinkUrl = await r.ReadStringAsync(cancellationToken);

                if (version >= 13)
                {
                    DownloadUrl = await r.ReadStringAsync(cancellationToken);
                }

                CreationDate = await r.ReadFileTimeAsync(cancellationToken);
                Comments = await r.ReadStringAsync(cancellationToken);

                if (version >= 12)
                {
                    Xml = await r.ReadStringAsync(cancellationToken);
                    TitleId = await r.ReadStringAsync(cancellationToken);
                }

                UsageSubDir = await r.ReadStringAsync(cancellationToken);
                CreationBuildInfo = await r.ReadStringAsync(cancellationToken);
                U01 = await r.ReadBytesAsync(16, cancellationToken); // some blowfish key likely

                if (version >= 10)
                {
                    IncludedPacks = new IncludedPackHeader[await r.ReadUInt32Async(cancellationToken)];

                    for (var i = 0; i < IncludedPacks.Length; i++)
                    {
                        IncludedPacks[i] = await IncludedPackHeader.DeserializeAsync(r, version, cancellationToken);
                    }
                }
            }
        }
    }

    public sealed partial class IncludedPackHeader
    {
        public byte[] ContentsChecksum { get; private set; } = [];
        public string Name { get; private set; } = string.Empty;
        public int AuthorVersion { get; private set; }
        public string AuthorLogin { get; private set; } = string.Empty;
        public string AuthorNickName { get; private set; } = string.Empty;
        public string AuthorZone { get; private set; } = string.Empty;
        public string AuthorExtraInfo { get; private set; } = string.Empty;
        public string InfoManialinkUrl { get; private set; } = string.Empty;
        public DateTime CreationDate { get; private set; }
        public uint IncludeDepth { get; private set; }

        internal static async Task<IncludedPackHeader> DeserializeAsync(AsyncGbxReader r, int version, CancellationToken cancellationToken)
        {
            var includedPacksHeaders = new IncludedPackHeader
            {
                ContentsChecksum = await r.ReadBytesAsync(32, cancellationToken),
                Name = await r.ReadStringAsync(cancellationToken),
                AuthorVersion = await r.ReadInt32Async(cancellationToken),
                AuthorLogin = await r.ReadStringAsync(cancellationToken),
                AuthorNickName = await r.ReadStringAsync(cancellationToken),
                AuthorZone = await r.ReadStringAsync(cancellationToken),
                AuthorExtraInfo = await r.ReadStringAsync(cancellationToken),
                InfoManialinkUrl = await r.ReadStringAsync(cancellationToken),
                CreationDate = await r.ReadFileTimeAsync(cancellationToken)
            };
            includedPacksHeaders.Name = await r.ReadStringAsync(cancellationToken);

            if (version >= 11)
            {
                includedPacksHeaders.IncludeDepth = await r.ReadUInt32Async(cancellationToken);
            }

            return includedPacksHeaders;
        }
    }
}
