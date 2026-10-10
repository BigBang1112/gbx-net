namespace GBX.NET.Engines.System;

public partial class CSystemConfig
{
    [Obsolete("Use InstallCdKey instead.")]
    public string? Key
    {
        get => InstallCdKey;
        set => InstallCdKey = value;
    }

    [Obsolete("Use ProfileEnableMulti instead.")]
    public bool GameProfileEnableMulti
    {
        get => ProfileEnableMulti;
        set => ProfileEnableMulti = value;
    }

    [Obsolete("Use ProfileName instead.")]
    public string? GameProfileName
    {
        get => ProfileName;
        set => ProfileName = value;
    }

    [Obsolete("Use NetworkDownloadRate instead.")]
    public int NetworkDownload
    {
        get => NetworkDownloadRate;
        set => NetworkDownloadRate = value;
    }

    [Obsolete("Use NetworkUploadRate instead.")]
    public int NetworkUpload
    {
        get => NetworkUploadRate;
        set => NetworkUploadRate = value;
    }

    [Obsolete("Use NetworkProxyUrl instead.")]
    public string? NetworkProxyAddress
    {
        get => NetworkProxyUrl;
        set => NetworkProxyUrl = value;
    }

    [Obsolete("Use FileTransferEnableTagSkinDownload instead.")]
    public bool FileTransferEnableTagDownload
    {
        get => FileTransferEnableTagSkinDownload;
        set => FileTransferEnableTagSkinDownload = value;
    }

    [Obsolete("Use FileTransferEnableTagSkinUpload instead.")]
    public bool FileTransferEnableTagUpload
    {
        get => FileTransferEnableTagSkinUpload;
        set => FileTransferEnableTagSkinUpload = value;
    }

    [Obsolete("Use FileTransferEnableTagSkinLocators instead.")]
    public bool FileTransferEnableTagLocators
    {
        get => FileTransferEnableTagSkinLocators;
        set => FileTransferEnableTagSkinLocators = value;
    }
}
