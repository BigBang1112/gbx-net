namespace GBX.NET.Engines.Game;

/// <remarks>ID: 0x03028000</remarks>
[Class(0x03028000)]
public partial class CGameNetOnlineMessage : CMwNod, IClass
{
    [Hexadecimal] public static new uint Id => 0x03028000;




    private string? receiverLogin;
    [AppliedWithChunk<Chunk03028000>]
    public string? ReceiverLogin { get => receiverLogin; set => receiverLogin = value; }

    private string? senderLogin;
    [AppliedWithChunk<Chunk03028000>]
    public string? SenderLogin { get => senderLogin; set => senderLogin = value; }

    private string? subject;
    [AppliedWithChunk<Chunk03028000>]
    public string? Subject { get => subject; set => subject = value; }

    private int donation;
    [AppliedWithChunk<Chunk03028000>]
    public int Donation { get => donation; set => donation = value; }

    private DateTime? date;
    [AppliedWithChunk<Chunk03028000>]
    public DateTime? Date { get => date; set => date = value; }


    /// <summary>
    /// CGameNetOnlineMessage 0x000 chunk
    /// </summary>
    [Chunk(0x03028000)]
    public partial class Chunk03028000 : Chunk<CGameNetOnlineMessage>
    {
        /// <inheritdoc />
        public override uint Id => 0x03028000;

        public bool U01;

        public override void ReadWrite(CGameNetOnlineMessage n, GbxReaderWriter rw)
        {
            rw.String(ref n.receiverLogin);
            rw.String(ref n.senderLogin);
            rw.String(ref n.subject);
            rw.String(ref n.message);
            rw.Int32(ref n.donation);
            rw.Boolean(ref U01);
            rw.SystemTime(ref n.date);
        }
    }




    internal override IChunk? NewChunk(uint chunkId) => chunkId switch
    {
        0x03028000 => new Chunk03028000(),
        _ => base.NewChunk(chunkId),
    };
}
