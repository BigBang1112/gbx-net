namespace GBX.NET.Engines.Game;

public partial class CGameCtnCollectorList
{
    public class Collector
    {
        public Ident Ident { get; set; } = Ident.Empty;
        public int Count { get; set; } = 1;

        public override string ToString()
        {
            return $"{Count} {Ident.Id}";
        }
    }
}
