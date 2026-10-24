namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockToneMapping
{
    public partial class Key
    {
        [Obsolete("Use ExposureBias instead.")]
        public float Exposure { get => ExposureBias; set => ExposureBias = value; }

        [Obsolete("Use FilmCurve instead.")]
        public int U01 { get => (int)FilmCurve; set => FilmCurve = (EFilmCurve)value; }
    }
}
