namespace GBX.NET.PAK;

public sealed class EncryptionInitializer : IClassEncryptionInitializer
{
    private readonly BlowfishStream stream;
    private readonly int pakVersion;

    internal EncryptionInitializer(BlowfishStream stream, int pakVersion)
    {
        this.stream = stream;
        this.pakVersion = pakVersion;
    }

    public void InitializeClass(uint classId, uint parentClassId)
    {
        // TM2020 archives retain the legacy CFuncKeys dummy-write parent for the replacement curve node.
        if (pakVersion >= 18 && classId == 0x09185000)
        {
            parentClassId = 0x05002000;
        }

        // Legacy Hms emitters still had the CHmsPocEmitter base class.
        if (pakVersion < 12 && classId is 0x0600C000 or 0x0600D000)
        {
            parentClassId = 0x0600B000;
        }

        // CTrackMania inherited CGameCtnApp before CGameManiaPlanet existed.
        if (pakVersion <= 3 && classId == 0x24001000)
        {
            parentClassId = 0x24033000;
        }

        // These camera models inherited CMwNod before the Pak 18 generation.
        if (pakVersion < 18 && parentClassId == 0x0910C000
            && classId is 0x090EF000 or 0x090F6000 or 0x09112000)
        {
            parentClassId = 0x01001000;
        }

        Initialize(BitConverter.GetBytes(parentClassId), 0, 4);
    }

    public void Initialize(byte[] data, uint offset, uint count)
    {
        stream.Initialize(data, offset, count);
    }
}
