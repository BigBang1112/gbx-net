namespace GBX.NET.Engines.Game;

public partial class CGamePlayerProfileChunk_AccountSettings
{
    public bool LoginValidated
    {
        get => BitHelper.GetBit(flags, 0);
        set => flags = BitHelper.SetBit(flags, 0, value);
    }

    public bool RememberOnlinePassword
    {
        get => BitHelper.GetBit(flags, 1);
        set => flags = BitHelper.SetBit(flags, 1, value);
    }

    public bool AutoConnect
    {
        get => BitHelper.GetBit(flags, 2);
        set => flags = BitHelper.SetBit(flags, 2, value);
    }

    public bool AskForAccountConversion
    {
        get => BitHelper.GetBit(flags, 3);
        set => flags = BitHelper.SetBit(flags, 3, value);
    }

    public bool UnlockAllCheats
    {
        get => BitHelper.GetBit(flags2, 0);
        set => flags2 = BitHelper.SetBit(flags2, 0, value);
    }

    public bool FriendsCheat
    {
        get => BitHelper.GetBit(flags2, 1);
        set => flags2 = BitHelper.SetBit(flags2, 1, value);
    }

    public partial class Chunk0312C005 : IVersionable
    {
        public int Version { get; set; }

        public bool U01;
        public ulong U02;

        public override void ReadWrite(CGamePlayerProfileChunk_AccountSettings n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            rw.Boolean(ref U01);

            if (Version < 2)
            {
                rw.UInt64(ref U02);
                rw.ArrayNodeRef_deprec<CGameNetOnlineMessage>(ref n.inboxMessages!);
                rw.ArrayNodeRef_deprec<CGameNetOnlineMessage>(ref n.readMessages!);
                rw.ArrayNodeRef_deprec<CGameNetOnlineMessage>(ref n.outboxMessages!);
            }
            else
            {
                if (rw.Reader is not null)
                {
                    var r = rw.Reader;

                    var size = r.ReadInt32();

                    using var _ = new Encapsulation(r);

                    U02 = r.ReadUInt64();
                    n.inboxMessages = r.ReadArrayNodeRef_deprec<CGameNetOnlineMessage>();
                    n.readMessages = r.ReadArrayNodeRef_deprec<CGameNetOnlineMessage>();
                    n.outboxMessages = r.ReadArrayNodeRef_deprec<CGameNetOnlineMessage>();
                }

                if (rw.Writer is not null)
                {
                    var w = rw.Writer;

                    using var ms = new MemoryStream();
                    using var wBuffer = new GbxWriter(ms);
                    using var _ = new Encapsulation(wBuffer);

                    wBuffer.Write(U02);
                    wBuffer.WriteArrayNodeRef_deprec(n.inboxMessages);
                    wBuffer.WriteArrayNodeRef_deprec(n.readMessages);
                    wBuffer.WriteArrayNodeRef_deprec(n.outboxMessages);

                    w.Write((int)ms.Length);
                    ms.WriteTo(w.BaseStream);
                }
            }
        }
    }
}
