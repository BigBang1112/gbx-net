using GBX.NET.Components;

namespace GBX.NET;

public interface IClassVersion<T> where T : IClass
{
    GameVersion GameVersion { get; }
    bool IsGameVersion(GameVersion version, bool strict = false);
    bool CanBeGameVersion(GameVersion version);

    Gbx ToGbx(GbxHeaderBasic headerBasic);
    Gbx ToGbx();

    void Save(Stream stream, GbxWriteSettings settings = default);
    void Save(string fileName, GbxWriteSettings settings = default);
}