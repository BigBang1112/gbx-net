using GBX.NET.Serialization;

namespace GBX.NET.PAK;

internal sealed partial class Pak6 : Pak
{
    public byte[] Checksum { get; private set; } = [];
    public uint HeaderFlags { get; private set; }
    public int? HeaderMaxSize { get; private set; }
    public int AuthorVersion { get; private set; }
    public string AuthorLogin { get; private set; } = string.Empty;
    public string AuthorNickname { get; private set; } = string.Empty;
    public string AuthorZone { get; private set; } = string.Empty;
    public string AuthorExtraInfo { get; private set; } = string.Empty;
    public string Comments { get; private set; } = string.Empty;
    public string CreationBuildInfo { get; private set; } = string.Empty;
    public string AuthorUrl { get; private set; } = string.Empty;
    public string? ManialinkUrl { get; private set; }
    public string? DownloadUrl { get; private set; }
    public DateTime? CreationDate { get; private set; }
    public string? Xml { get; private set; }
    public string? TitleId { get; private set; }
    public string? UsageSubDir { get; private set; }
    public IncludedPackHeader[] IncludedPacks { get; private set; } = [];

    internal Pak6(Stream stream, byte[]? key, int version) : base(stream, key, version)
    {
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    internal async Task ReadUnencryptedHeaderAsync(GbxReader r, int version, CancellationToken cancellationToken)
    {
        Checksum = await r.ReadBytesAsync(32, cancellationToken);
        HeaderFlags = r.ReadUInt32();

        if (version >= 15)
        {
            HeaderMaxSize = r.ReadInt32();
        }

        if (version >= 7)
        {
            ReadAuthorInfo(r);

            if (version < 9)
            {
                Comments = r.ReadString();
                r.SkipData(16);

                if (version == 8)
                {
                    CreationBuildInfo = r.ReadString();
                    AuthorUrl = r.ReadString();
                }
            }
            else
            {
                ManialinkUrl = r.ReadString();

                if (version >= 13)
                {
                    DownloadUrl = r.ReadString();
                }

                CreationDate = r.ReadFileTime();
                Comments = r.ReadString();

                if (version >= 12)
                {
                    Xml = r.ReadString();
                    TitleId = r.ReadString();
                }

                UsageSubDir = r.ReadString();
                CreationBuildInfo = r.ReadString();
                r.SkipData(16);

                if (version >= 10)
                {
                    IncludedPacks = new IncludedPackHeader[r.ReadUInt32()];

                    for (var i = 0; i < IncludedPacks.Length; i++)
                    {
                        IncludedPacks[i] = await IncludedPackHeader.DeserializeAsync(r, version, cancellationToken);
                    }
                }
            }
        }
    }

    internal void ReadAuthorInfo(GbxReader r)
    {
        AuthorVersion = r.ReadInt32();
        AuthorLogin = r.ReadString();
        AuthorNickname = r.ReadString();
        AuthorZone = r.ReadString();
        AuthorExtraInfo = r.ReadString();
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

        [Zomp.SyncMethodGenerator.CreateSyncVersion]
        internal static async Task<IncludedPackHeader> DeserializeAsync(GbxReader r, int version, CancellationToken cancellationToken)
        {
            var includedPacksHeaders = new IncludedPackHeader
            {
                ContentsChecksum = await r.ReadBytesAsync(32, cancellationToken),
                Name = r.ReadString(),
                AuthorVersion = r.ReadInt32(),
                AuthorLogin = r.ReadString(),
                AuthorNickName = r.ReadString(),
                AuthorZone = r.ReadString(),
                AuthorExtraInfo = r.ReadString(),
                InfoManialinkUrl = r.ReadString(),
                CreationDate = r.ReadFileTime()
            };
            includedPacksHeaders.Name = r.ReadString();

            if (version >= 11)
            {
                includedPacksHeaders.IncludeDepth = r.ReadUInt32();
            }

            return includedPacksHeaders;
        }
    }
}
