namespace GBX.NET.Engines.Game;

public partial class CGamePlayerScore
{
    public class Score
    {
        public Ident MapInfo { get; }
        public int? U01 { get; }
        public TimeInt32? PersonalBest { get; }
        public int? U03 { get; }
        public bool? U04 { get; }
        public int U05 { get; }
        public int U06 { get; }
        public int U07 { get; }
        public int U08 { get; }
        public int U09 { get; }
        public int? U10 { get; } // Respawns?
        public int? U11 { get; }
        public int? U12 { get; }
        public ulong? U13 { get; }
        public ulong? U14 { get; }
        public LeagueScore[]? LeagueScores { get; }
        public int? U15 { get; }
        public int? U16 { get; }
        public int? U17 { get; }
        public byte? U18 { get; }
        public string? MapName { get; }
        public int? U20 { get; }
        public int? U21 { get; }
        public int? U22 { get; }
        public int? U23 { get; }
        public int? U24 { get; }
        public int? U25 { get; }
        public int? U26 { get; }
        public int? U27 { get; }
        public int? U28 { get; }
        public short? U29 { get; }
        public short? U30 { get; }

        public Score(Ident mapInfo, int? u01, TimeInt32? personalBest, int? u03, bool? u04, int u05, int u06, int u07,
            int u08, int u09, int? u10, int? u11, int? u12, ulong? u13, ulong? u14, LeagueScore[]? leagueScores,
            int? u15, int? u16, int? u17, byte? u18, string? mapName, int? u20, int? u21, int? u22, int? u23,
            int? u24, int? u25, int? u26, int? u27, int? u28, short? u29, short? u30)
        {
            MapInfo = mapInfo;
            U01 = u01;
            PersonalBest = personalBest;
            U03 = u03;
            U04 = u04;
            U05 = u05;
            U06 = u06;
            U07 = u07;
            U08 = u08;
            U09 = u09;
            U10 = u10;
            U11 = u11;
            U12 = u12;
            U13 = u13;
            U14 = u14;
            LeagueScores = leagueScores;
            U15 = u15;
            U16 = u16;
            U17 = u17;
            U18 = u18;
            MapName = mapName;
            U20 = u20;
            U21 = u21;
            U22 = u22;
            U23 = u23;
            U24 = u24;
            U25 = u25;
            U26 = u26;
            U27 = u27;
            U28 = u28;
            U29 = u29;
            U30 = u30;
        }

        public override string ToString()
        {
            return $"{MapName ?? MapInfo.ToString()}, PB: {PersonalBest.ToTmString(useHundredths: true)}";
        }

        public static Score Parse(GameBoxReader r, int version)
        {
            var mapInfo = r.ReadIdent();

            var u01 = default(int?);

            if (version == 0)
            {
                u01 = r.ReadInt32();
            }

            var personalBest = r.ReadTimeInt32Nullable(); // + 0xc

            var u03 = default(int?);
            var u04 = default(bool?);
            var u05 = default(int);
            var u06 = default(int);
            var u07 = default(int);
            var u08 = default(int);
            var u09 = default(int);

            var u10 = default(int?);
            var u11 = default(int?);
            var u12 = default(int?);
            var u13 = default(ulong?);
            var u14 = default(ulong?);
            var leagueScores = default(LeagueScore[]);
            var u15 = default(int?);
            var u16 = default(int?);
            var u17 = default(int?);
            var u18 = default(byte?);
            var mapName = default(string);
            var u20 = default(int?);
            var u21 = default(int?);
            var u22 = default(int?);
            var u23 = default(int?);
            var u24 = default(int?);
            var u25 = default(int?);
            var u26 = default(int?);
            var u27 = default(int?);
            var u28 = default(int?);
            var u29 = default(short?);
            var u30 = default(short?);

            if (version < 5)
            {
                if (version >= 1)
                {
                    u03 = r.ReadInt32();
                    u04 = r.ReadBoolean();

                    if (version >= 2)
                    {
                        u05 = r.ReadInt32(); // + 0x1c
                        u06 = r.ReadInt32(); // + 0x20
                        u07 = r.ReadInt32(); // + 0x28

                        if (version >= 3)
                        {
                            u08 = r.ReadInt32(); // + 0x2a

                            if (version >= 4)
                            {
                                u09 = r.ReadInt32(); // + 0x24
                            }
                        }
                    }
                }
            }

            if (version >= 5)
            {
                u05 = r.ReadInt32(); // + 0x1c
                u06 = r.ReadInt32(); // + 0x20
                u09 = r.ReadInt32(); // + 0x24
                u07 = r.ReadInt16(); // + 0x28
                u08 = r.ReadInt16(); // + 0x2a

                if (version >= 6)
                {
                    u10 = r.ReadInt32(); // + 0x10

                    if (version >= 7)
                    {
                        u11 = r.ReadInt32(); // + 0x14

                        if (version >= 8)
                        {
                            u12 = r.ReadInt32(); // + 0x18

                            if (version >= 9)
                            {
                                u13 = r.ReadUInt64(); // SSystemTime
                                u14 = r.ReadUInt64(); // SSystemTime

                                if (version < 16)
                                {
                                    leagueScores = r.ReadArray<LeagueScore>(r => LeagueScore.Parse(r));
                                }

                                if (version >= 10)
                                {
                                    u15 = r.ReadInt32();

                                    if (version >= 11)
                                    {
                                        u16 = r.ReadInt32(); // + 0x5c

                                        if (version >= 12)
                                        {
                                            u17 = r.ReadInt32();

                                            if (version >= 13)
                                            {
                                                u18 = r.ReadByte(); // + 0x3d
                                                mapName = r.ReadString(); // + 0x40

                                                if (version >= 14)
                                                {
                                                    u20 = r.ReadInt32(); // + 0x70

                                                    if (version >= 15)
                                                    {
                                                        u21 = r.ReadInt32(); // + 0x6c

                                                        if (version >= 17)
                                                        {
                                                            u22 = r.ReadInt32(); // + 0x60
                                                            u23 = r.ReadInt32(); // + 100
                                                            u24 = r.ReadInt32(); // + 0x68
                                                            u25 = r.ReadInt32(); // + 0x58

                                                            if (version >= 18)
                                                            {
                                                                u26 = r.ReadInt32(); // + 0x2c
                                                                u27 = r.ReadInt32(); // + 0x30
                                                                u28 = r.ReadInt32(); // + 0x34
                                                                u29 = r.ReadInt16(); // + 0x38
                                                                u30 = r.ReadInt16(); // + 0x3a
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return new Score(mapInfo, u01, personalBest, u03, u04, u05, u06, u07, u08, u09, u10, u11, u12, u13, u14,
                leagueScores, u15, u16, u17, u18, mapName, u20, u21, u22, u23, u24, u25, u26, u27, u28, u29, u30);
        }
    }
}
