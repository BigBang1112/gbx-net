namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk_GameSettings : IReadableWritable
{
    private NodeRef<CGameCtnMediaShootParams>? oldShootParamsVideoRef;
    private NodeRef<CGameCtnMediaShootParams>? oldShootParams2Ref;

    public float OldU01 { get; set; }
    public bool OldU02 { get; set; }
    public bool OldU03 { get; set; } = true;
    public byte OldMaxPlayerCount { get; set; }
    public byte OldMaxSpectatorCount { get; set; }
    public byte OldNetStateQuality { get; set; }
    public byte OldLadderMode { get; set; }
    public byte OldFlags { get; set; }
    public byte OldUnlockFlags { get; set; }
    public byte OldProposeSimpleEditor { get; set; }
    public string?[] OldU04 { get; set; } = new string?[8];
    public string?[] OldU05 { get; set; } = new string?[8];
    public byte[]? OldU06 { get; set; }
    public int OldU07 { get; set; }
    public int OldU08 { get; set; }
    public float OldStereoscopyStrength01Multiplier { get; set; } = 1;
    public CGameCtnMediaShootParams? OldShootParams2 { get => oldShootParams2Ref?.Resolve(); set => NodeRef.SetEmbedded(ref oldShootParams2Ref, value); }
    public float OldMouseSensitivity { get; set; } = 1;
    public bool OldU09 { get; set; }

    /// <summary>
    /// Reads or writes the old archive (ArchiveOldVersion).
    /// </summary>
    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        OldU01 = rw.Single(OldU01);

        if (v >= 4) OldU02 = rw.Boolean(OldU02);
        if (v >= 5) OldU03 = rw.Boolean(OldU03);

        rw.String(ref serverName);
        rw.String(ref serverComment);
        rw.Int32(ref networkGameMode);
        OldMaxPlayerCount = rw.Byte(OldMaxPlayerCount);
        OldMaxSpectatorCount = rw.Byte(OldMaxSpectatorCount);
        OldNetStateQuality = rw.Byte(OldNetStateQuality);
        OldLadderMode = rw.Byte(OldLadderMode);
        OldFlags = rw.Byte(OldFlags);
        OldUnlockFlags = rw.Byte(OldUnlockFlags);

        if (v == 6) OldProposeSimpleEditor = rw.Byte(OldProposeSimpleEditor);

        for (var i = 0; i < 8; i++)
        {
            OldU04[i] = rw.String(OldU04[i]);
            OldU05[i] = rw.String(OldU05[i]);
        }

        OldU06 = rw.Data(OldU06, 5);
        OldU07 = rw.Int32(OldU07);
        OldU08 = rw.Int32(OldU08);
        rw.Single(ref stereoscopyStrength01);
        OldStereoscopyStrength01Multiplier = rw.Single(OldStereoscopyStrength01Multiplier);
        rw.Single(ref stereoscopyAdvancedScreenDist);

        if (v < 3)
        {
            if (rw.Writer is not null && oldShootParamsVideoRef is null)
            {
                NodeRef.SetEmbedded(ref oldShootParamsVideoRef, shootParamsVideo);
            }

            ReadWriteArchivedNode(rw, ref oldShootParamsVideoRef);
            shootParamsVideo = oldShootParamsVideoRef?.Resolve();

            // The buffer memory payload stores only the video parameters.
            if (v >= 2 && !IsBufferMemoryArchivation)
            {
                ReadWriteArchivedNode(rw, ref oldShootParams2Ref);
            }
        }
        else
        {
            rw.Node(ref shootParamsVideo);
            OldShootParams2 = rw.Node(OldShootParams2);
        }

        if (v >= 7) OldProposeSimpleEditor = rw.Byte(OldProposeSimpleEditor);

        if (v >= 8)
        {
            rw.Int32(ref opponentsVersionLast);
            rw.ArrayReadableWritable<SChallengeOpponents>(ref challengeOpponents!, version: opponentsVersionLast);
        }

        if (v >= 9)
        {
            OldMouseSensitivity = rw.Single(OldMouseSensitivity);
            OldU09 = rw.Boolean(OldU09);
            rw.Single(ref mouseAccelQuantity);
        }
    }
}
