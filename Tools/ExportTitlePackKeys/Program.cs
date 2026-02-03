using GBX.NET;
using GBX.NET.Crypto;
using GBX.NET.Engines.Game;
using GBX.NET.LZO;

Gbx.LZO = new Lzo();

var encryptedRSAPublicKey = "";
var checksumTitleIds = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
var checksumPackageInfos = new Dictionary<string, CGamePlayerProfileChunk_PackagesInfos.PackageInfo>(StringComparer.OrdinalIgnoreCase);

var profile = Gbx.ParseNode<CGamePlayerProfile>(args[0]);

var publicKeyDecryptionKey = MD5.Compute(profile.OnlineLogin!);

foreach (var profileChunk in profile.ProfileChunks ?? [])
{
    if (profileChunk is CGamePlayerProfileChunk_AccountSettings accountSettings)
    {
        encryptedRSAPublicKey = accountSettings.RSAPublicKey;
    }

    if (profileChunk is CGamePlayerProfileChunk_ManiaPlanetStations maniaPlanetStations)
    {
        foreach (var station in maniaPlanetStations.Stations ?? [])
        {
            if (string.IsNullOrEmpty(station.TitleId))
            {
                continue;
            }

            checksumTitleIds[Convert.ToHexString(station.Checksum!)] = station.TitleId!;
        }
    }

    if (profileChunk is CGamePlayerProfileChunk_PackagesInfos packagesInfos)
    {
        foreach (var packageInfo in packagesInfos.PackagesInfos ?? [])
        {
            checksumPackageInfos[Convert.ToHexString(packageInfo.Checksum!)] = packageInfo;
        }
    }
}

Console.WriteLine("Encrypted RSAPublicKey:");
Console.WriteLine(encryptedRSAPublicKey);
Console.WriteLine();
Console.WriteLine("RSAPublicKey decryption key:");
Console.WriteLine(Convert.ToHexString(publicKeyDecryptionKey));
Console.WriteLine();
foreach (var (checksum, packageInfo) in checksumPackageInfos)
{
    if (checksumTitleIds.TryGetValue(checksum, out var titleId))
    {
        Console.WriteLine(titleId);
    }
    else
    {
        Console.WriteLine(checksum);
    }

    Console.WriteLine(packageInfo.Key);
    Console.WriteLine(packageInfo.StartTimestamp);
    Console.WriteLine(packageInfo.EndTimestamp);
    Console.WriteLine(packageInfo.EndTimestamp - packageInfo.StartTimestamp);
    Console.WriteLine();
}
