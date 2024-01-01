namespace GBX.NET;

[Flags]
public enum GameVersion
{
    Undefined = 0,
    TM10 = 1,
    TMPU = 2,
    TMS = 4,
    TMO = 8,
    TMSX = 16,
    TMNESWC = 32,
    TMU = 64,
    TMF = 128,
    MP1 = 256,
    MP2 = 512,
    MP3 = 1024,
    TMT = 2048,
    MP4 = 4096,
    TM2020 = 8192
}