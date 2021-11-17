namespace GBX.NET.Engines.Game;

[Node(0x03028000), WritingNotSupported]
public class CGameNetOnlineMessage : CMwNod
{
    public string ReceiverLogin { get; set; }
    public string SenderLogin { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public int Donation { get; set; }
    public DateTime Date { get; set; }

    private CGameNetOnlineMessage()
    {
        ReceiverLogin = null!;
        SenderLogin = null!;
        Subject = null!;
        Message = null!;
    }

    public override string ToString()
    {
        return "Message from " + SenderLogin;
    }

    [Chunk(0x03028000)]
    public class Chunk03028000 : Chunk<CGameNetOnlineMessage>
    {
        public int U01 = 1;

        public override void Read(CGameNetOnlineMessage n, GameBoxReader r)
        {
            n.ReceiverLogin = r.ReadString();
            n.SenderLogin = r.ReadString();
            n.Subject = r.ReadString();
            n.Message = r.ReadString();
            n.Donation = r.ReadInt32();
            U01 = r.ReadInt32();

            var date = r.ReadInt64();
            var year = (int)(date & 0xFFFF);
            var month = (int)((date >> 16) & 0xF);
            var day = (int)((date >> 23) & 0xF);
            var hour = (int)((date >> 32) & 0x1F);
            var minute = (int)((date >> 37) & 0x3F);
            var second = (int)((date >> 43) & 0x3F);

            n.Date = new DateTime(year, month, day, hour, minute, second);
        }
    }
}
