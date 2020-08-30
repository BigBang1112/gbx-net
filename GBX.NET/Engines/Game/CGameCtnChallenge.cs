using GBX.NET.Engines.GameData;
using GBX.NET.Engines.Hms;
using GBX.NET.Engines.Script;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// Class of a map (0x03043000)
    /// </summary>
    /// <remarks>A map. Known extensions: .Challenge.Gbx, .Map.Gbx</remarks>
    [Node(0x03043000)]
    public class CGameCtnChallenge : Node
    {
        #region Enums

        /// <summary>
        /// Map type in which the track was validated in.
        /// </summary>
        public enum TrackType : int
        {
            Race,
            Platform,
            Puzzle,
            Crazy,
            Shortcut,
            Stunts,
            /// <summary>
            /// Any custom map type script.
            /// </summary>
            Script
        }

        /// <summary>
        /// The track's intended use.
        /// </summary>
        public enum TrackKind : int
        {
            EndMarker,
            Campaign,
            Puzzle,
            Retro,
            TimeAttack,
            Rounds,
            InProgress,
            Campaign_7,
            Multi,
            Solo,
            Site,
            SoloNadeo,
            MultiNadeo
        }

        public enum PlayMode : int
        {
            Race,
            Platform,
            Puzzle,
            Crazy,
            Shortcut,
            Stunts
        }

        /// <summary>
        /// Used by Virtual Skipper.
        /// </summary>
        public enum BoatName : byte
        {
            Acc,
            Multi,
            Melges,
            OffShore
        }

        /// <summary>
        /// Used by Virtual Skipper.
        /// </summary>
        public enum RaceMode : byte
        {
            FleetRace,
            MatchRace,
            TeamRace
        }

        /// <summary>
        /// Used by Virtual Skipper.
        /// </summary>
        public enum WindDirection : byte
        {
            North,
            NorthEast,
            East,
            SouthEast,
            South,
            SouthWest,
            West,
            NorthWest
        }

        /// <summary>
        /// Used by Virtual Skipper.
        /// </summary>
        public enum Weather : byte
        {
            Sunny,
            Cloudy,
            Rainy,
            Stormy
        }

        /// <summary>
        /// Used by Virtual Skipper.
        /// </summary>
        public enum StartDelay : byte
        {
            Immediate,
            OneMin,
            TwoMin,
            FiveMin,
            EightMin
        }

        /// <summary>
        /// Used by Virtual Skipper.
        /// </summary>
        public enum AILevel : byte
        {
            Easy,
            Intermediate,
            Expert,
            Pro
        }

        #endregion

        #region Properties

        /// <summary>
        /// Time of the bronze medal.
        /// </summary>
        public TimeSpan? BronzeTime
        {
            get
            {
                return GetValue<Chunk002, Chunk005>(x => x.BronzeTime, x =>
                {
                    if (!string.IsNullOrEmpty(x.XML))
                    {
                        try
                        {
                            using var sr = new StringReader(x.XML);
                            var doc = new XPathDocument(sr);
                            var att = doc.CreateNavigator().SelectSingleNode("/header/times/@bronze");
                            if (att != null)
                            {
                                var bronze = Convert.ToInt32(att.Value);
                                if (bronze == -1) return null;
                                return TimeSpan.FromMilliseconds(bronze);
                            }
                        }
                        catch { }
                    }
                    return null;
                }) as TimeSpan?;
            }
            set
            {
                SetValue<Chunk002, Chunk005>(x => x.BronzeTime = value, x =>
                {
                    if (!string.IsNullOrEmpty(x.XML))
                    {
                        try
                        {
                            var doc = new XmlDocument();
                            doc.LoadXml(x.XML);
                            var att = doc.SelectSingleNode("/header/times/@bronze");
                            if (att != null)
                            {
                                att.Value = Convert.ToInt32(value.HasValue ? value.Value.TotalMilliseconds : -1).ToString();
                                x.XML = doc.OuterXml;
                            }
                        }
                        catch { }
                    }
                });
            }
        }

        /// <summary>
        /// Time of the silver medal.
        /// </summary>
        public TimeSpan? SilverTime
        {
            get
            {
                return GetValue<Chunk002, Chunk005>(x => x.SilverTime, x =>
                {
                    if (!string.IsNullOrEmpty(x.XML))
                    {
                        try
                        {
                            using var sr = new StringReader(x.XML);
                            var doc = new XPathDocument(sr);
                            var att = doc.CreateNavigator().SelectSingleNode("/header/times/@silver");
                            if (att != null)
                            {
                                var silver = Convert.ToInt32(att.Value);
                                if (silver == -1) return null;
                                return TimeSpan.FromMilliseconds(silver);
                            }
                        }
                        catch { }
                    }
                    return null;
                }) as TimeSpan?;
            }
            set
            {
                SetValue<Chunk002, Chunk005>(x => x.SilverTime = value, x =>
                {
                    if (!string.IsNullOrEmpty(x.XML))
                    {
                        try
                        {
                            var doc = new XmlDocument();
                            doc.LoadXml(x.XML);
                            var att = doc.SelectSingleNode("/header/times/@silver");
                            if (att != null)
                            {
                                att.Value = Convert.ToInt32(value.HasValue ? value.Value.TotalMilliseconds : -1).ToString();
                                x.XML = doc.OuterXml;
                            }
                        }
                        catch { }
                    }
                });
            }
        }

        /// <summary>
        /// Time of the gold medal.
        /// </summary>
        public TimeSpan? GoldTime
        {
            get
            {
                return GetValue<Chunk002, Chunk005>(x => x.GoldTime, x =>
                {
                    if (!string.IsNullOrEmpty(x.XML))
                    {
                        try
                        {
                            using var sr = new StringReader(x.XML);
                            var doc = new XPathDocument(sr);
                            var att = doc.CreateNavigator().SelectSingleNode("/header/times/@gold");
                            if (att != null)
                            {
                                var gold = Convert.ToInt32(att.Value);
                                if (gold == -1) return null;
                                return TimeSpan.FromMilliseconds(gold);
                            }
                        }
                        catch { }
                    }
                    return null;
                }) as TimeSpan?;
            }
            set
            {
                SetValue<Chunk002, Chunk005>(x => x.GoldTime = value, x =>
                {
                    if (!string.IsNullOrEmpty(x.XML))
                    {
                        try
                        {
                            var doc = new XmlDocument();
                            doc.LoadXml(x.XML);
                            var att = doc.SelectSingleNode("/header/times/@gold");
                            if (att != null)
                            {
                                att.Value = Convert.ToInt32(value.HasValue ? value.Value.TotalMilliseconds : -1).ToString();
                                x.XML = doc.OuterXml;
                            }
                        }
                        catch { }
                    }
                });
            }
        }

        /// <summary>
        /// Time of the author medal.
        /// </summary>
        public TimeSpan? AuthorTime
        {
            get
            {
                return GetValue<Chunk002, Chunk005>(x => x.AuthorTime, x =>
                {
                    if (!string.IsNullOrEmpty(x.XML))
                    {
                        try
                        {
                            using var sr = new StringReader(x.XML);
                            var doc = new XPathDocument(sr);
                            var att = doc.CreateNavigator().SelectSingleNode("/header/times/@authortime");
                            if (att != null)
                            {
                                var author = Convert.ToInt32(att.Value);
                                if (author == -1) return null;
                                return TimeSpan.FromMilliseconds(author);
                            }
                        }
                        catch { }
                    }
                    return null;
                }) as TimeSpan?;
            }
            set
            {
                SetValue<Chunk002, Chunk005>(x => x.AuthorTime = value, x =>
                {
                    if (!string.IsNullOrEmpty(x.XML))
                    {
                        try
                        {
                            var doc = new XmlDocument();
                            doc.LoadXml(x.XML);
                            var att = doc.SelectSingleNode("/header/times/@authortime");
                            if (att != null)
                            {
                                att.Value = Convert.ToInt32(value.HasValue ? value.Value.TotalMilliseconds : -1).ToString();
                                x.XML = doc.OuterXml;
                            }
                        }
                        catch { }
                    }
                });
            }
        }

        /// <summary>
        /// Display cost (or coppers) of the track.
        /// </summary>
        public int? Cost
        {
            get
            {
                return GetValue<Chunk002, Chunk005>(x => x.Cost, x =>
                {
                    if (!string.IsNullOrEmpty(x.XML))
                    {
                        try
                        {
                            using var sr = new StringReader(x.XML);
                            var doc = new XPathDocument(sr);
                            var nav = doc.CreateNavigator();
                            var att = nav.SelectSingleNode("/header/desc/@displaycost");
                            if (att == null)
                                att = nav.SelectSingleNode("/header/desc/@price");
                            if (att != null)
                                return Convert.ToInt32(att.Value);
                        }
                        catch { }
                    }
                    return null;
                }) as int?;
            }
            set
            {
                SetValue<Chunk002, Chunk005>(x => x.Cost = value, x =>
                {
                    if (!string.IsNullOrEmpty(x.XML))
                    {
                        try
                        {
                            var doc = new XmlDocument();
                            doc.LoadXml(x.XML);
                            var att = doc.SelectSingleNode("/header/desc/@displaycost");
                            if (att == null)
                                att = doc.SelectSingleNode("/header/desc/@price");
                            if (att == null)
                                att = doc.SelectSingleNode("/header/desc").Attributes.Append(doc.CreateAttribute("displaycost"));
                            else
                                att.Value = value.GetValueOrDefault().ToString();
                            x.XML = doc.OuterXml;
                        }
                        catch { }
                    }
                });
            }
        }

        /// <summary>
        /// If the track has multiple laps.
        /// </summary>
        public bool? IsMultilap
        {
            get
            {
                return GetValue<Chunk002, Chunk005>(x => x.IsMultilap, x =>
                {
                    if (!string.IsNullOrEmpty(x.XML))
                    {
                        try
                        {
                            using var sr = new StringReader(x.XML);
                            var doc = new XPathDocument(sr);
                            var att = doc.CreateNavigator().SelectSingleNode("/header/desc/@nblaps");
                            if(att != null)
                                return Convert.ToInt32(att.Value) != 0;
                        }
                        catch { }
                    }
                    return null;
                }) as bool?;
            }
            set
            {
                SetValue<Chunk002, Chunk005>(x => x.IsMultilap = value, x =>
                {
                    if (!string.IsNullOrEmpty(x.XML))
                    {
                        try
                        {
                            var doc = new XmlDocument();
                            doc.LoadXml(x.XML);
                            if(!value.GetValueOrDefault()) doc.SelectSingleNode("/header/desc/@nblaps").Value = "0";
                            x.XML = doc.OuterXml;
                        }
                        catch { }
                    }
                });
            }
        }

        /// <summary>
        /// Map type in which the track was validated in.
        /// </summary>
        public TrackType? Type
        {
            get
            {
                return GetValue<Chunk002, Chunk005>(x => x.Type, x =>
                {
                    if (!string.IsNullOrEmpty(x.XML))
                    {
                        try
                        {
                            using var sr = new StringReader(x.XML);
                            var doc = new XPathDocument(sr);
                            return (TrackType)Enum.Parse(typeof(TrackType), doc.CreateNavigator().SelectSingleNode("/header/desc/@type").Value);
                        }
                        catch { }
                    }
                    return null;
                }) as TrackType?;
            }
            set
            {
                SetValue<Chunk002, Chunk005>(x => x.Type = value, x =>
                {
                    if (!string.IsNullOrEmpty(x.XML))
                    {
                        try
                        {
                            var doc = new XmlDocument();
                            doc.LoadXml(x.XML);
                            doc.SelectSingleNode("/header/desc/@type").Value = Enum.GetName(typeof(TrackType), value);
                            x.XML = doc.OuterXml;
                        }
                        catch { }
                    }
                });
            }
        }

        /// <summary>
        /// Usually author time or stunt score.
        /// </summary>
        public int? AuthorScore
        {
            get
            {
                return GetValue<Chunk002, Chunk005>(x => x.AuthorScore, x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var times = header.Element("times");
                            if (times != null)
                            {
                                var author = times.Attribute("authorscore");
                                if (author != null && author.Value != "-1")
                                    return Convert.ToInt32(author.Value);
                            }
                        }
                    }
                    return null;
                }) as int?;
            }
            set
            {
                SetValue<Chunk002, Chunk005>(x => x.AuthorScore = value, x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var times = header.Element("times");
                            if (times != null)
                            {
                                var author = times.Attribute("authorscore");
                                if (author != null)
                                {
                                    author.Value = value.ToString();
                                    x.XML = header.ToString(SaveOptions.DisableFormatting);
                                }
                            }
                        }
                    }
                });
            }
        }

        public bool? CreatedWithSimpleEditor
        {
            get => GetValue<Chunk002>(x => x.CreatedWithSimpleEditor) as bool?;
            set => SetValue<Chunk002>(x => x.CreatedWithSimpleEditor = value);
        }

        public bool? HasGhostBlocks
        {
            get
            {
                return GetValue<Chunk002, Chunk005>(x => x.HasGhostBlocks, x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var desc = header.Element("desc");
                            if (desc != null)
                            {
                                var hasGhostBlocks = desc.Attribute("hasghostblocks");
                                if (hasGhostBlocks != null)
                                    return hasGhostBlocks.Value == "1";
                            }
                        }
                    }
                    return null;
                }) as bool?;
            }
            set
            {
                SetValue<Chunk002, Chunk005>(x => x.HasGhostBlocks = value, x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var desc = header.Element("desc");
                            if (desc != null)
                            {
                                var hasGhostBlocks = desc.Attribute("hasghostblocks");
                                if (hasGhostBlocks != null)
                                {
                                    if (value.GetValueOrDefault())
                                        hasGhostBlocks.Value = "1";
                                    else hasGhostBlocks.Value = "0";
                                    x.XML = header.ToString(SaveOptions.DisableFormatting);
                                }
                            }
                        }
                    }
                });
            }
        }

        public int? NbCheckpoints
        {
            get => GetValue<Chunk002>(x => x.NbCheckpoints) as int?;
            set => SetValue<Chunk002>(x => x.NbCheckpoints = value);
        }

        public int? NbLaps
        {
            get
            {
                return GetValue<Chunk002, Chunk005, Chunk018>(x => x.NbLaps, x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var desc = header.Element("desc");
                            if (desc != null)
                            {
                                var nbLaps = desc.Attribute("nblaps");
                                if (nbLaps != null)
                                    return Convert.ToInt32(nbLaps.Value);
                            }
                        }
                    }
                    return null;
                },
                x => x.Laps) as int?;
            }
            set
            {
                SetValue<Chunk002, Chunk005, Chunk018>(x => x.NbLaps = value, x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var desc = header.Element("desc");
                            if (desc != null)
                            {
                                var nbLaps = desc.Attribute("nblaps");
                                if (nbLaps != null)
                                {
                                    nbLaps.Value = value.ToString();
                                    x.XML = header.ToString(SaveOptions.DisableFormatting);
                                }
                            }
                        }
                    }
                },
                x => x.Laps = value.GetValueOrDefault());
            }
        }

        public string MapUid
        {
            get
            {
                return GetValue<Chunk003, Chunk005, Chunk013, Chunk01F>(x =>
                {
                    if (x.MapInfo == null) return null;
                    return x.MapInfo.ID;
                },
                x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var ident = header.Element("ident");
                            if (ident != null)
                            {
                                var uid = ident.Attribute("uid");
                                if (uid != null)
                                    return uid.Value;
                            }
                        }
                    }
                    return null;
                },
                x => x.Chunk01F.MapInfo.ID,
                x => x.MapInfo.ID) as string;
            }
            set
            {
                SetValue<Chunk003, Chunk005, Chunk013, Chunk01F>(x => x.MapInfo.ID = value, x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var ident = header.Element("ident");
                            if (ident != null)
                            {
                                var uid = ident.Attribute("uid");
                                if (uid != null)
                                {
                                    uid.Value = value;
                                    x.XML = header.ToString(SaveOptions.DisableFormatting);
                                }
                            }
                        }
                    }
                },
                x => x.Chunk01F.MapInfo.ID = value,
                x => x.MapInfo.ID = value);
            }
        }

        public string AuthorLogin
        {
            get
            {
                return GetValue<Chunk003, Chunk005, Chunk008, Chunk013, Chunk01F, Chunk042>(x =>
                {
                    if (x.MapInfo == null) return null;
                    return x.MapInfo.Author;
                },
                x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var ident = header.Element("ident");
                            if (ident != null)
                            {
                                var author = ident.Attribute("author");
                                if (author != null)
                                    return author.Value;
                            }
                        }
                    }
                    return null;
                },
                x => x.AuthorLogin,
                x => x.Chunk01F.MapInfo.Author,
                x => x.MapInfo.Author,
                x => x.AuthorLogin) as string;
            }
            set
            {
                SetValue<Chunk003, Chunk005, Chunk008, Chunk013, Chunk01F, Chunk042> (x => x.MapInfo.Author = value, x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var ident = header.Element("ident");
                            if (ident != null)
                            {
                                var author = ident.Attribute("author");
                                if (author != null)
                                {
                                    author.Value = value;
                                    x.XML = header.ToString(SaveOptions.DisableFormatting);
                                }
                            }
                        }
                    }
                },
                x => x.AuthorLogin = value,
                x => x.Chunk01F.MapInfo.Author = value,
                x => x.MapInfo.Author = value,
                x => x.AuthorLogin = value);
            }
        }

        public string MapName
        {
            get
            {
                return GetValue<Chunk003, Chunk005, Chunk013, Chunk01F>(x => x.MapName, x =>
                {
                    if (!string.IsNullOrEmpty(x.XML))
                    {
                        try
                        {
                            using var sr = new StringReader(x.XML);
                            var doc = new XPathDocument(sr);
                            return doc.CreateNavigator().SelectSingleNode("/header/ident/@name").Value;
                        }
                        catch { }
                    }
                    return null;
                },
                x => x.Chunk01F.MapName,
                x => x.MapName, DifferenceSolution.FirstChunk) as string;
            }
            set
            {
                SetValue<Chunk003, Chunk005, Chunk013, Chunk01F>(x => x.MapName = value, x =>
                {
                    if (!string.IsNullOrEmpty(x.XML))
                    {
                        try
                        {
                            var doc = new XmlDocument();
                            doc.LoadXml(x.XML);
                            doc.SelectSingleNode("/header/ident/@name").Value = value;
                            x.XML = doc.OuterXml;
                        }
                        catch { }
                    }
                },
                x => x.Chunk01F.MapName = value,
                x => x.MapName = value);
            }
        }

        /// <summary>
        /// The track's intended use.
        /// </summary>
        public TrackKind? Kind
        {
            get => GetValue<Chunk003, Chunk011>(x => x.Kind, x => x.Kind, DifferenceSolution.FirstChunk) as TrackKind?;
            set => SetValue<Chunk003, Chunk011>(x => x.Kind = value.GetValueOrDefault(), x => x.Kind = value.GetValueOrDefault());
        }

        public bool? Locked
        {
            get => GetValue<Chunk003>(x => x.Locked) as bool?;
            set => SetValue<Chunk003>(x => x.Locked = value);
        }

        /// <summary>
        /// Password of the map used by older tracks.
        /// </summary>
        public string Password
        {
            get => GetValue<Chunk003, Chunk014>(x => x.Password, x => x.Password) as string;
            set => SetValue<Chunk003, Chunk014>(x => x.Password = value, x => x.Password = value);
        }

        public string DecorationName
        {
            get => GetValue<Chunk013, Chunk01F>(x => x.Chunk01F.Decoration.ID, x => x.Decoration.ID) as string;
            set => SetValue<Chunk003, Chunk013, Chunk01F>(x => x.Decoration.ID = value, x => x.Chunk01F.Decoration.ID = value, x => x.Decoration.ID = value);
        }

        public string DecorationAuthor
        {
            get => GetValue<Chunk003, Chunk013, Chunk01F>(x => x.Decoration?.Author, x => x.Chunk01F.Decoration.Author, x => x.Decoration.Author) as string;
            set => SetValue<Chunk003, Chunk013, Chunk01F>(x => x.Decoration.Author = value, x => x.Chunk01F.Decoration.Author = value, x => x.Decoration.Author = value);
        }

        public string Collection
        {
            get => GetValue<Chunk003, Chunk013, Chunk01F, Chunk003, Chunk013, Chunk01F>(x => x.Decoration?.Collection, x => x.Chunk01F.Decoration.Collection, x => x.Decoration.Collection,
                x => x.MapInfo.Collection, x => x.Chunk01F.MapInfo.Collection, x => x.MapInfo.Collection) as string;
            set => SetValue<Chunk003, Chunk013, Chunk01F, Chunk003, Chunk013, Chunk01F>(x => x.Decoration.Collection = value, x => x.Chunk01F.Decoration.Collection = value, x => x.Decoration.Collection = value,
            x => x.MapInfo.Collection = value, x => x.Chunk01F.MapInfo.Collection = value, x => x.MapInfo.Collection = value);
        }

        /// <summary>
        /// Origin of the map.
        /// </summary>
        public Vector2? MapOrigin
        {
            get => GetValue<Chunk003>(x => x.MapOrigin) as Vector2?;
            set => SetValue<Chunk003>(x => x.MapOrigin = value);
        }

        /// <summary>
        /// Target of the map. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="4"/></c>.
        /// </summary>
        public Vector2? MapTarget
        {
            get => GetValue<Chunk003>(x => x.MapTarget) as Vector2?;
            set => SetValue<Chunk003>(x => x.MapTarget = value);
        }

        /// <summary>
        /// Name of the map type script.
        /// </summary>
        public string MapType
        {
            get
            {
                return GetValue<Chunk003, Chunk005>(x => x.MapType, x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var desc = header.Element("desc");
                            if (desc != null)
                            {
                                var mapType = desc.Attribute("maptype");
                                if (mapType != null)
                                    return mapType.Value;
                            }
                        }
                    }
                    return null;
                }) as string;
            }
            set
            {
                SetValue<Chunk003, Chunk005>(x =>
                {
                    if (x.Version < 6)
                        x.Version = 6;
                    x.MapType = value;
                }, x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var desc = header.Element("desc");
                            if (desc != null)
                            {
                                var mapType = desc.Attribute("maptype");
                                if (mapType != null)
                                {
                                    mapType.Value = value;
                                    x.XML = header.ToString(SaveOptions.DisableFormatting);
                                }
                            }
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Style of the map (Fullspeed, LOL, Tech), usually unused and defined by user.
        /// </summary>
        public string MapStyle
        {
            get
            {
                return GetValue<Chunk003, Chunk005>(x => x.MapStyle, x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var desc = header.Element("desc");
                            if (desc != null)
                            {
                                var mapStyle = desc.Attribute("mapstyle");
                                if (mapStyle != null)
                                    return mapStyle.Value;
                            }
                        }
                    }
                    return null;
                }) as string;
            }
            set
            {
                SetValue<Chunk003, Chunk005>(x => x.MapStyle = value, x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var desc = header.Element("desc");
                            if (desc != null)
                            {
                                var mapStyle = desc.Attribute("mapstyle");
                                if (mapStyle != null)
                                {
                                    mapStyle.Value = value.ToString();
                                    x.XML = header.ToString(SaveOptions.DisableFormatting);
                                }
                            }
                        }
                    }
                });
            }
        }

        public ulong? LightmapCacheUID
        {
            get => GetValue<Chunk003>(x => x.LightmapCacheUID) as ulong?;
            set => SetValue<Chunk003>(x => x.LightmapCacheUID = value);
        }

        /// <summary>
        /// Version of the lightmap calculation.
        /// </summary>
        public byte? LightmapVersion
        {
            get
            {
                return GetValue<Chunk003, Chunk005>(x => x.LightmapVersion, x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var lightmap = header.Attribute("lightmap");
                            if (lightmap != null)
                                return Convert.ToByte(lightmap.Value);
                        }
                    }
                    return null;
                }) as byte?;
            }
            set
            {
                SetValue<Chunk003, Chunk005>(x => x.LightmapVersion = value, x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var lightmap = header.Attribute("lightmap");
                            if (lightmap != null)
                            {
                                lightmap.Value = value.ToString();
                                x.XML = header.ToString(SaveOptions.DisableFormatting);
                            }
                        }
                    }
                });
            }
        }

        public string Version
        {
            get
            {
                return GetValue<Chunk005>(x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var version = header.Attribute("version");
                            if (version != null)
                                return version.Value;
                        }
                    }
                    return null;
                }) as string;
            }
            set
            {
                SetValue<Chunk005>(x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var version = header.Attribute("version");
                            if (version != null)
                            {
                                version.Value = value;
                                x.XML = header.ToString(SaveOptions.DisableFormatting);
                            }
                        }
                    }
                });
            }
        }

        public string ExeBuild
        {
            get
            {
                return GetValue<Chunk005>(x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var exeBuild = header.Attribute("exebuild");
                            if (exeBuild != null)
                                return exeBuild.Value;
                        }
                    }
                    return null;
                }) as string;
            }
            set
            {
                SetValue<Chunk005>(x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var exeBuild = header.Attribute("exebuild");
                            if (exeBuild != null)
                            {
                                exeBuild.Value = value;
                                x.XML = header.ToString(SaveOptions.DisableFormatting);
                            }
                        }
                    }
                });
            }
        }

        public string ExeVersion
        {
            get
            {
                return GetValue<Chunk005>(x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var exeVer = header.Attribute("exever");
                            if (exeVer != null)
                                return exeVer.Value;
                        }
                    }
                    return null;
                }) as string;
            }
            set
            {
                SetValue<Chunk005>(x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var exeVer = header.Attribute("exever");
                            if (exeVer != null)
                            {
                                exeVer.Value = value;
                                x.XML = header.ToString(SaveOptions.DisableFormatting);
                            }
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Title pack the map was built in.
        /// </summary>
        public string TitleUID
        {
            get
            {
                return GetValue<Chunk003, Chunk005>(x => x.TitleUID, x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var title = header.Attribute("title");
                            if (title != null)
                                return title.Value;
                        }
                    }
                    return null;
                }) as string;
            }
            set
            {
                SetValue<Chunk003, Chunk005>(x =>
                {
                    if (x.Version < 11)
                        x.Version = 11;
                    x.TitleUID = value;
                }, x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var title = header.Attribute("title");
                            if (title != null)
                            {
                                title.Value = value;
                                x.XML = header.ToString(SaveOptions.DisableFormatting);
                            }
                        }
                    }
                });
            }
        }

        /// <summary>
        /// XML track information and dependencies.
        /// </summary>
        public string XML
        {
            get => GetValue<Chunk005>(x => x.XML) as string;
            set => SetValue<Chunk005>(x => x.XML = value);
        }

        public Dependency[] Dependencies
        {
            get
            {
                return GetValue<Chunk005>(x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var deps = header.Element("deps");
                            if (deps != null)
                            {
                                var depList = deps.Elements();
                                var dependencies = new Dependency[depList.Count()];

                                var i = 0;
                                foreach (var dep in depList)
                                {
                                    var file = "";
                                    var fileAtt = dep.Attribute("file");
                                    if (fileAtt != null) file = fileAtt.Value;

                                    var url = "";
                                    var urlAtt = dep.Attribute("url");
                                    if (urlAtt != null) url = urlAtt.Value;

                                    dependencies[i] = new Dependency(file, url);

                                    i++;
                                }

                                return dependencies;
                            }
                        }
                    }
                    return null;
                }) as Dependency[];
            }
        }

        [IgnoreDataMember]
        public Bitmap Thumbnail
        {
            get => GetValue<Chunk007>(x =>
            {
                if (x.Thumbnail == null) return null;
                return x.Thumbnail.Result;
            }) as Bitmap;
            set => SetValue<Chunk007>(x => x.Thumbnail = Task.Run(() => value));
        }

        public string Comments
        {
            get => GetValue<Chunk007, Chunk028>(x => x.Comments, x => x.Comments) as string;
            set => SetValue<Chunk007, Chunk028>(x => x.Comments = value, x => x.Comments = value);
        }

        public int? AuthorVersion
        {
            get => GetValue<Chunk008>(x => x.AuthorVersion) as int?;
            set => SetValue<Chunk008>(x => x.AuthorVersion = value.GetValueOrDefault());
        }

        public string AuthorNickname
        {
            get => GetValue<Chunk008>(x => x.AuthorNickname) as string;
            set => SetValue<Chunk008>(x => x.AuthorNickname = value);
        }

        public string AuthorZone
        {
            get => GetValue<Chunk008>(x => x.AuthorZone) as string;
            set => SetValue<Chunk008>(x => x.AuthorZone = value);
        }

        public string AuthorExtraInfo
        {
            get => GetValue<Chunk008>(x => x.AuthorExtraInfo) as string;
            set => SetValue<Chunk008>(x => x.AuthorExtraInfo = value);
        }

        public string PlayerModelID
        {
            get
            {
                return GetValue<Chunk005, Chunk00D>(x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var playermodel = header.Element("playermodel");
                            if (playermodel != null)
                            {
                                var id = playermodel.Attribute("id");
                                if (id != null)
                                    return id.Value;
                            }
                        }
                    }
                    return null;
                },
                x =>
                {
                    if (x.Vehicle == null) return null;
                    return x.Vehicle.ID;
                }) as string;
            }
            set
            {
                SetValue<Chunk005, Chunk00D>(x =>
                {
                    if (x.XML != "")
                    {
                        var header = XElement.Parse(x.XML);
                        if (header != null)
                        {
                            var playermodel = header.Element("playermodel");
                            if (playermodel != null)
                            {
                                var id = playermodel.Attribute("id");
                                if (id != null)
                                {
                                    id.Value = value;
                                    x.XML = header.ToString(SaveOptions.DisableFormatting);
                                }
                            }
                        }
                    }
                },
                x => x.Vehicle.ID = value);
            }
        }

        /// <summary>
        /// Map parameters.
        /// </summary>
        public CGameCtnChallengeParameters ChallengeParameters
        {
            get => GetValue<Chunk011>(x => x.ChallengeParameters) as CGameCtnChallengeParameters;
        }

        /// <summary>
        /// List of puzzle pieces.
        /// </summary>
        public CGameCtnCollectorList CollectorList
        {
            get => GetValue<Chunk011>(x => x.CollectorList) as CGameCtnCollectorList;
        }

        /// <summary>
        /// All checkpoints and their map coordinates. Used by older Trackmania.
        /// </summary>
        public Int3[] Checkpoints
        {
            get => GetValue<Chunk017>(x => x.Checkpoints) as Int3[];
            set => SetValue<Chunk017>(x => x.Checkpoints = value);
        }

        public FileRef ModPackDesc
        {
            get => GetValue<Chunk019>(x => x.ModPackDesc) as FileRef;
            set => SetValue<Chunk019>(x => x.ModPackDesc = value);
        }

        public PlayMode? Mode
        {
            get => GetValue<Chunk01C>(x => x.Mode) as PlayMode?;
            set => SetValue<Chunk01C>(x => x.Mode = value.GetValueOrDefault());
        }

        public Int3? Size
        {
            get => GetValue<Chunk013, Chunk01F>(x => x.Chunk01F.Size, x => x.Size) as Int3?;
            set => SetValue<Chunk013, Chunk01F>(x => x.Chunk01F.Size = value.GetValueOrDefault(), x => x.Size = value.GetValueOrDefault());
        }

        public bool? NeedUnlock
        {
            get => GetValue<Chunk013, Chunk01F>(x => x.Chunk01F.NeedUnlock, x => x.NeedUnlock) as bool?;
            set => SetValue<Chunk013, Chunk01F>(x => x.Chunk01F.NeedUnlock = value.GetValueOrDefault(), x => x.NeedUnlock = value.GetValueOrDefault());
        }

        /// <summary>
        /// Array of all blocks on the map.
        /// </summary>
        public List<Block> Blocks
        {
            get => GetValue<Chunk013, Chunk01F>(x => x.Chunk01F.Blocks, x => x.Blocks) as List<Block>;
            set => SetValue<Chunk013, Chunk01F>(x => x.Chunk01F.Blocks = value, x => x.Blocks = value);
        }

        public ReadOnlyCollection<FreeBlock> FreeBlocks
        {
            get => GetValue<Chunk05F>(x => x.FreeBlocks) as ReadOnlyCollection<FreeBlock>;
        }

        public CGameCtnMediaClip ClipIntro
        {
            get => GetValue<Chunk021, Chunk049>(x => x.ClipIntro, x => x.ClipIntro) as CGameCtnMediaClip;
        }

        public CGameCtnMediaClipGroup ClipGroupInGame
        {
            get => GetValue<Chunk021, Chunk049>(x => x.ClipGroupInGame, x => x.ClipGroupInGame) as CGameCtnMediaClipGroup;
        }

        public CGameCtnMediaClipGroup ClipGroupEndRace
        {
            get => GetValue<Chunk021, Chunk049>(x => x.ClipGroupEndRace, x => x.ClipGroupEndRace) as CGameCtnMediaClipGroup;
        }

        public FileRef CustomMusicPackDesc
        {
            get => GetValue<Chunk024>(x => x.CustomMusicPackDesc) as FileRef;
            set => SetValue<Chunk024>(x => x.CustomMusicPackDesc = value);
        }

        public Vector2? MapCoordOrigin
        {
            get => GetValue<Chunk025>(x => x.MapCoordOrigin) as Vector2?;
            set => SetValue<Chunk025>(x => x.MapCoordOrigin = value.GetValueOrDefault());
        }

        public Vector2? MapCoordTarget
        {
            get => GetValue<Chunk025>(x => x.MapCoordTarget) as Vector2?;
            set => SetValue<Chunk025>(x => x.MapCoordTarget = value.GetValueOrDefault());
        }

        public byte[] HashedPassword
        {
            get => GetValue<Chunk029>(x => x.HashedPassword) as byte[];
            set => SetValue<Chunk029>(x => x.HashedPassword = value);
        }

        public uint? CRC32
        {
            get => GetValue<Chunk029>(x => x.CRC32) as uint?;
            set => SetValue<Chunk029>(x => x.CRC32 = value.GetValueOrDefault());
        }

        /// <summary>
        /// Position of the thumnail camera.
        /// </summary>
        public Vector3? ThumbnailPosition
        {
            get => GetValue<Chunk036>(x => x.ThumbnailPosition) as Vector3?;
            set => SetValue<Chunk036>(x => x.ThumbnailPosition = value.GetValueOrDefault());
        }

        /// <summary>
        /// Pitch, yaw and roll of the thumbnail camera in radians.
        /// </summary>
        public Vector3? ThumbnailPitchYawRoll
        {
            get => GetValue<Chunk036>(x => x.ThumbnailPitchYawRoll) as Vector3?;
            set => SetValue<Chunk036>(x => x.ThumbnailPitchYawRoll = value.GetValueOrDefault());
        }

        /// <summary>
        /// Thumbnail camera FOV.
        /// </summary>
        public float? ThumbnailFOV
        {
            get => GetValue<Chunk036>(x => x.ThumbnailFOV) as float?;
            set => SetValue<Chunk036>(x => x.ThumbnailFOV = value.GetValueOrDefault());
        }

        public List<CGameCtnAnchoredObject> Items
        {
            get => GetValue<Chunk040>(x => x.Items) as List<CGameCtnAnchoredObject>;
            set => SetValue<Chunk040>(x => x.Items = value);
        }

        public CScriptTraitsMetadata MetadataTraits
        {
            get => GetValue<Chunk044>(x => x.MetadataTraits) as CScriptTraitsMetadata;
        }

        #endregion

        /// <summary>
        /// Creates a new <see cref="CGameCtnChallenge"/> instance with the latest node ID of 0x03043000.
        /// </summary>
        /// <param name="lookbackable">The parent of the node, usually <see cref="GameBoxHeader{T}"/> or <see cref="GameBoxBody{T}"/>.</param>
        public CGameCtnChallenge(ILookbackable lookbackable) : this(lookbackable, 0x03043000)
        {
            
        }

        /// <summary>
        /// Creates a new <see cref="CGameCtnChallenge"/> instance with a specified node ID used for compatibility.
        /// </summary>
        /// <param name="lookbackable">The parent of the node, usually <see cref="GameBoxHeader{T}"/> or <see cref="GameBoxBody{T}"/>.</param>
        /// <param name="classID">Whole ID of the node.</param>
        public CGameCtnChallenge(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {
            
        }

        #region Methods

        /// <summary>
        /// Exports the map's thumbnail.
        /// </summary>
        /// <param name="stream">Stream to export to.</param>
        /// <param name="format">Image format to use.</param>
        public void ExportThumbnail(Stream stream, ImageFormat format)
        {
            CallChunkMethod<Chunk007>(x => x.ExportThumbnail(stream, format));
        }

        /// <summary>
        /// Exports the map's thumbnail.
        /// </summary>
        /// <param name="fileName">File to export to.</param>
        /// <param name="format">Image format to use.</param>
        public void ExportThumbnail(string fileName, ImageFormat format)
        {
            CallChunkMethod<Chunk007>(x => x.ExportThumbnail(fileName, format));
        }

        /// <summary>
        /// Asynchronously imports (and replaces) a thumbnail to use for the map.
        /// </summary>
        /// <param name="stream">Stream to import from.</param>
        /// <returns>A task that processes the thumbnail.</returns>
        public Task<Bitmap> ImportThumbnailAsync(Stream stream)
        {
            return CallChunkMethod<Chunk007, Task<Bitmap>>(x => x.ImportThumbnailAsync(stream));
        }

        /// <summary>
        /// Asynchronously imports (and replaces) a thumbnail to use for the map.
        /// </summary>
        /// <param name="fileName">File to import from.</param>
        /// <returns>A task that processes the thumbnail.</returns>
        public Task<Bitmap> ImportThumbnailAsync(string fileName)
        {
            return CallChunkMethod<Chunk007, Task<Bitmap>>(x => x.ImportThumbnailAsync(fileName));
        }

        /// <summary>
        /// Imports (and replaces) a thumbnail to use for the map.
        /// </summary>
        /// <param name="stream">Stream to import from.</param>
        public void ImportThumbnail(Stream stream)
        {
            CallChunkMethod<Chunk007>(x => x.ImportThumbnail(stream));
        }

        /// <summary>
        /// Imports (and replaces) a thumbnail to use for the map.
        /// </summary>
        /// <param name="fileName">File to import from.</param>
        public void ImportThumbnail(string fileName)
        {
            CallChunkMethod<Chunk007>(x => ImportThumbnail(fileName));
        }

        /// <summary>
        /// Sets a new map password.
        /// </summary>
        /// <param name="password">Password that will be hashed.</param>
        public void NewPassword(string password)
        {
            CallChunkMethod<Chunk029>(x => x.NewPassword(password));
        }

        /// <summary>
        /// Cracks the map password.
        /// </summary>
        public void CrackPassword()
        {
            Body.GBX.RemoveBodyChunk<Chunk029>();
        }

        public void PlaceItem(Meta itemModel, Vector3 absolutePosition, Vector3 pitchYawRoll, Byte3 blockUnitCoord, Vector3 offsetPivot, int variant = 0)
        {
            var chunkItems = CreateChunk<Chunk040>();

            var it = new CGameCtnAnchoredObject(chunkItems);
            var itChunk = it.CreateChunk<CGameCtnAnchoredObject.Chunk002>();
            it.CreateChunk<CGameCtnAnchoredObject.Chunk004>();
            itChunk.ItemModel = itemModel;
            itChunk.AbsolutePositionInMap = absolutePosition;
            itChunk.PitchYawRoll = pitchYawRoll;
            itChunk.BlockUnitCoord = blockUnitCoord;
            itChunk.PivotPosition = offsetPivot;
            itChunk.Variant = variant;
            chunkItems.Items.Add(it);
        }

        public FreeBlock PlaceFreeBlock(string name, Vector3 position, Vector3 pitchYawRoll)
        {
            var block = new Block(name, Direction.North, (0, 0, 0), 0x20000000, null, null, null);
            var freeBlock = new FreeBlock(block)
            {
                Position = position,
                PitchYawRoll = pitchYawRoll
            };

            Blocks.Add(block);

            var freeBlockChunk = Body.GBX.CreateBodyChunk<Chunk05F>();
            freeBlockChunk.Vectors.Add(position);
            freeBlockChunk.Vectors.Add(pitchYawRoll);

            var freeBlockList = Resources.FreeBlock.Split('\n');
            var freeBlockSnapCount = freeBlockList.FirstOrDefault(x => x.StartsWith(name + ":"));

            if (freeBlockSnapCount != null)
            {
                if (int.TryParse(freeBlockSnapCount.Split(':')[1], out int v))
                {
                    if (v > 0)
                    {
                        throw new NotImplementedException("Cannot place a free block with snaps.");
                    }
                }
                else
                    throw new Exception("Wrong amount of snaps format.");
            }
            else
                throw new Exception("Cannot place a free block with an unknown amount of snaps.");

            return freeBlock;
        }

        /// <summary>
        /// Transfers the MediaTracker from <see cref="Chunk021"/> (up to TMUF) to <see cref="Chunk049"/> (ManiaPlanet and Trackmania®). If <see cref="Chunk049"/> is already presented, no action is performed.
        /// </summary>
        /// <param name="upsaleTriggerCoord">Defines how many times the same coord should repeat.</param>
        /// <returns>Returns <see cref="true"/> if any action was performed, otherwise <see cref="false"/>.</returns>
        public bool TransferMediaTrackerTo049(int upsaleTriggerCoord = 3)
        {
            var chunk021 = GetChunk<Chunk021>();
            var chunk049 = CreateChunk<Chunk049>();

            if (chunk021 == null) return false;

            chunk049.ClipIntro = chunk021.ClipIntro;
            chunk049.ClipGroupInGame = chunk021.ClipGroupInGame;
            chunk049.ClipGroupEndRace = chunk021.ClipGroupEndRace;

            if (chunk049.ClipIntro != null && chunk049.ClipIntro != null)
                ConvertMediaClip(chunk049.ClipIntro);

            if (chunk049.ClipGroupInGame != null && chunk049.ClipGroupInGame != null)
                ConvertMediaClipGroup(chunk049.ClipGroupInGame);

            if (chunk049.ClipGroupEndRace != null && chunk049.ClipGroupEndRace != null)
                ConvertMediaClipGroup(chunk049.ClipGroupEndRace);

            Chunks.Remove<Chunk021>();

            void ConvertMediaClip(CGameCtnMediaClip node)
            {
                foreach (var track in node.Tracks)
                    if (track != null)
                        ConvertMediaTrack(track);
            }

            void ConvertMediaClipGroup(CGameCtnMediaClipGroup node)
            {
                foreach(var trigger in node.Triggers)
                {
                    var coords = trigger.Coords.ToList();

                    for (var i = 0; i < trigger.Coords.Length; i++)
                    {
                        coords[i] = coords[i] * (upsaleTriggerCoord, 1, upsaleTriggerCoord);

                        for(var x = 0; x < upsaleTriggerCoord; x++)
                        {
                            for (var z = 0; z < upsaleTriggerCoord; z++)
                            {
                                coords.Add(coords[i] + new Int3(x, 0, z));
                            }
                        }
                    }

                    trigger.Coords = coords.ToArray();
                }

                foreach(var clip in node.Clips)
                {
                    ConvertMediaClip(clip);
                }
            }

            void ConvertMediaTrack(CGameCtnMediaTrack node)
            {
                var chunk001 = node.GetChunk<CGameCtnMediaTrack.Chunk001>();

                // Chunk 0x004 has to be removed so that ManiaPlanet accepts the entire map.
                node.Chunks.Remove<CGameCtnMediaTrack.Chunk004>();

                chunk001.Blocks.RemoveAll(x => x is CGameCtnMediaBlockGhost); // Some ghosts can crash the game
            }

            return true;
        }

        public void OffsetMediaTrackerCameras(Vector3 offset)
        {
            if (TryGetChunk(out Chunk021 c021))
            {
                OffsetCamerasInClip(c021.ClipIntro);
                OffsetCamerasInClipGroup(c021.ClipGroupInGame);
                OffsetCamerasInClipGroup(c021.ClipGroupEndRace);
            }
            else if (TryGetChunk(out Chunk049 c049))
            {
                OffsetCamerasInClip(c049.ClipIntro);
                OffsetCamerasInClip(c049.ClipPodium);
                OffsetCamerasInClipGroup(c049.ClipGroupInGame);
                OffsetCamerasInClipGroup(c049.ClipGroupEndRace);
                OffsetCamerasInClip(c049.ClipAmbiance);
            }

            void OffsetCamerasInClipGroup(CGameCtnMediaClipGroup group)
            {
                if (group == null) return;

                foreach (var clip in group.Clips)
                    OffsetCamerasInClip(clip);
            }

            void OffsetCamerasInClip(CGameCtnMediaClip clip)
            {
                if (clip == null) return;

                if (clip.Tracks != null)
                {
                    foreach (var track in clip.Tracks)
                    {
                        if (track.Blocks != null)
                        {
                            foreach (var block in track.Blocks)
                            {
                                if(block is CGameCtnMediaBlockCameraCustom c)
                                {
                                    if (c.Keys != null)
                                        foreach (var key in c.Keys)
                                            if(key.Anchor == -1)
                                                key.Position += offset;
                                }
                                else if(block is CGameCtnMediaBlockCameraPath p)
                                {
                                    if (p.Keys != null)
                                        foreach (var key in p.Keys)
                                            if(key.Anchor == -1)
                                                key.Position += offset;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void OffsetMediaTrackerTriggers(Int3 offset)
        {
            if (TryGetChunk(out Chunk021 c021))
            {
                OffsetTriggers(c021.ClipGroupInGame);
                OffsetTriggers(c021.ClipGroupEndRace);
            }
            else if (TryGetChunk(out Chunk049 c049))
            {
                OffsetTriggers(c049.ClipGroupInGame);
                OffsetTriggers(c049.ClipGroupEndRace);
            }

            void OffsetTriggers(CGameCtnMediaClipGroup group)
            {
                if (group == null) return;

                foreach(var trigger in group.Triggers)
                {
                    trigger.Coords = trigger.Coords.Select(x => x + offset).ToArray();
                }
            }
        }

        #endregion

        #region Chunks

        #region 0x001 chunk (virtual skipper)

        /// <summary>
        /// CGameCtnChallenge 0x001 chunk (virtual skipper)
        /// </summary>
        [Chunk(0x03043001)]
        public class Chunk001 : SkippableChunk
        {
            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public byte Version { get; set; }

            public Meta MapInfo { get; set; }

            public string MapName { get; set; }

            public BoatName BoatName { get; set; }

            public string Boat { get; set; }

            public string BoatAuthor { get; set; }

            public RaceMode RaceMode { get; set; }

            public WindDirection WindDirection { get; set; }

            public byte WindStrength { get; set; }

            public Weather Weather { get; set; }

            public StartDelay StartDelay { get; set; }

            public int StartTime { get; set; }

            public TimeSpan TimeLimit { get; set; }

            public bool NoPenalty { get; set; }

            public bool InflPenalty { get; set; }

            public bool FinishFirst { get; set; }

            public byte NbAIs { get; set; }

            public float CourseLength { get; set; }

            public int WindShiftDuration { get; set; }

            public int WindShiftAngle { get; set; }

            public bool ExactWind { get; set; }

            public int SpawnPoints { get; set; }

            public AILevel AILevel { get; set; }

            public bool SmallShifts { get; set; }

            public bool NoRules { get; set; }

            public bool StartSailUp { get; set; }

            public Chunk001(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
              
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Byte(Version);

                if (Version < 1)
                {
                    MapInfo = rw.Meta(MapInfo);
                    MapName = rw.String(MapName);
                }

                rw.Boolean(Unknown);
                rw.Int32(Unknown);

                if (Version < 1)
                    rw.Byte(Unknown);

                rw.Byte(Unknown);

                if (Version < 9)
                    BoatName = (BoatName)rw.Byte((byte)BoatName);

                if (Version >= 9)
                    Boat = rw.LookbackString(Boat);

                if (Version >= 12)
                    BoatAuthor = rw.LookbackString(BoatAuthor);

                RaceMode = (RaceMode)rw.Byte((byte)RaceMode);
                rw.Byte(Unknown);
                WindDirection = (WindDirection)rw.Byte((byte)RaceMode);
                WindStrength = rw.Byte(WindStrength);
                Weather = (Weather)rw.Byte((byte)Weather);
                rw.Byte(Unknown);
                StartDelay = (StartDelay)rw.Byte((byte)StartDelay);
                StartTime = rw.Int32(StartTime);

                if (Version >= 2)
                {
                    TimeLimit = TimeSpan.FromMilliseconds(rw.Int32(Convert.ToInt32(TimeLimit.TotalMilliseconds)));
                    NoPenalty = rw.Boolean(NoPenalty);
                    InflPenalty = rw.Boolean(InflPenalty);
                    FinishFirst = rw.Boolean(FinishFirst);

                    if (Version >= 3)
                    {
                        NbAIs = rw.Byte(NbAIs);

                        if (Version >= 4)
                        {
                            CourseLength = rw.Single(CourseLength);

                            if (Version >= 5)
                            {
                                WindShiftAngle = rw.Int32(WindShiftAngle);
                                rw.Byte(Unknown);

                                if (Version == 6 || Version == 7)
                                {
                                    rw.Boolean(Unknown);
                                    rw.String(Unknown);
                                }

                                if (Version >= 7)
                                {
                                    ExactWind = !rw.Boolean(!ExactWind);

                                    if (Version >= 10)
                                    {
                                        SpawnPoints = rw.Int32(SpawnPoints);

                                        if (Version >= 11)
                                        {
                                            AILevel = (AILevel)rw.Byte((byte)AILevel);

                                            if (Version >= 13)
                                            {
                                                SmallShifts = rw.Boolean(SmallShifts);

                                                if (Version >= 14)
                                                {
                                                    NoRules = rw.Boolean(NoRules);
                                                    StartSailUp = rw.Boolean(StartSailUp);
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

        #endregion

        #region 0x002 chunk (map info)

        /// <summary>
        /// CGameCtnChallenge 0x002 chunk (map info)
        /// </summary>
        [Chunk(0x03043002)]
        public class Chunk002 : SkippableChunk
        {
            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public byte Version { get; set; }

            /// <summary>
            /// Basic map information. <see cref="Meta.ID"/> is map UID, <see cref="Meta.Author"/> is the map author login. Can be <see cref="null"/> if <c><see cref="Version"/> &gt; <see cref="2"/></c>.
            /// </summary>
            public Meta MapInfo { get; set; }

            /// <summary>
            /// Formatted name of the map. Can be empty if <c><see cref="Version"/> &gt; <see cref="2"/></c>.
            /// </summary>
            public string MapName { get; set; }

            /// <summary>
            /// Time of the bronze medal. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="1"/></c>.
            /// </summary>
            public TimeSpan? BronzeTime { get; set; }

            /// <summary>
            /// Time of the silver medal. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="1"/></c>.
            /// </summary>
            public TimeSpan? SilverTime { get; set; }

            /// <summary>
            /// Time of the gold medal. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="1"/></c>.
            /// </summary>
            public TimeSpan? GoldTime { get; set; }

            /// <summary>
            /// Time of the author medal. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="1"/></c>.
            /// </summary>
            public TimeSpan? AuthorTime { get; set; }

            /// <summary>
            /// Display cost (or coppers) of the track. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="4"/></c>.
            /// </summary>
            public int? Cost { get; set; }

            /// <summary>
            /// If the track has multiple laps. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="5"/></c>.
            /// </summary>
            public bool? IsMultilap { get; set; }

            /// <summary>
            /// Map type in which the track was validated in. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="7"/></c>.
            /// </summary>
            public TrackType? Type { get; set; }

            /// <summary>
            /// Usually author time or stunt score. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="10"/></c>.
            /// </summary>
            public int? AuthorScore { get; set; }

            /// <summary>
            /// If the track was made using the simple editor. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="11"/></c>.
            /// </summary>
            public bool? CreatedWithSimpleEditor { get; set; }

            /// <summary>
            /// If the track uses ghost blocks. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="11"/></c>.
            /// </summary>
            public bool? HasGhostBlocks { get; set; }

            /// <summary>
            /// Number of checkpoints on the map. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="13"/></c>.
            /// </summary>
            public int? NbCheckpoints { get; set; }

            /// <summary>
            /// Number of laps. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="13"/></c>.
            /// </summary>
            public int? NbLaps { get; set; }

            public bool Unknown1 { get; set; }
            public byte Unknown2 { get; set; }
            public int Unknown3 { get; set; }
            public int Unknown4 { get; set; }
            public int Unknown5 { get; set; }

            public Chunk002(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Byte(Version);

                if (Version < 3)
                {
                    MapInfo = rw.Meta(MapInfo);
                    MapName = rw.String(MapName);
                }

                Unknown1 = rw.Boolean(Unknown1);

                if (Version >= 1)
                {
                    BronzeTime = rw.TimeSpan32(BronzeTime);
                    SilverTime = rw.TimeSpan32(SilverTime);
                    GoldTime = rw.TimeSpan32(GoldTime);
                    AuthorTime = rw.TimeSpan32(AuthorTime);

                    if (Version == 2)
                        Unknown2 = rw.Byte(Unknown2);

                    if (Version >= 4)
                    {
                        Cost = rw.Int32(Cost.GetValueOrDefault());

                        if (Version >= 5)
                        {
                            IsMultilap = rw.Boolean(IsMultilap.GetValueOrDefault());

                            if (Version == 6)
                                Unknown3 = rw.Int32(Unknown3);

                            if (Version >= 7)
                            {
                                Type = (TrackType)rw.Int32((int)Type.GetValueOrDefault());

                                if (Version >= 9)
                                {
                                    Unknown4 = rw.Int32(Unknown4);

                                    if (Version >= 10)
                                    {
                                        AuthorScore = rw.Int32(AuthorScore.GetValueOrDefault());

                                        if (Version >= 11)
                                        {
                                            if(rw.Mode == GameBoxReaderWriterMode.Read)
                                            {
                                                var editorMode = rw.Reader.ReadInt32();
                                                BitArray ba = new BitArray(new int[] { editorMode });
                                                CreatedWithSimpleEditor = ba.Get(0);
                                                HasGhostBlocks = ba.Get(1);
                                            }
                                            else if (rw.Mode == GameBoxReaderWriterMode.Write)
                                            {
                                                BitArray editorModeBit = new BitArray(32);
                                                editorModeBit.Set(0, CreatedWithSimpleEditor.GetValueOrDefault());
                                                editorModeBit.Set(1, HasGhostBlocks.GetValueOrDefault());

                                                var editorMode = new int[1];
                                                editorModeBit.CopyTo(editorMode, 0);
                                                rw.Writer.Write(editorMode[0]);
                                            }
                                            
                                            if (Version >= 12)
                                            {
                                                Unknown5 = rw.Int32(Unknown5);

                                                if (Version >= 13)
                                                {
                                                    NbCheckpoints = rw.Int32(NbCheckpoints.GetValueOrDefault());
                                                    NbLaps = rw.Int32(NbLaps.GetValueOrDefault());
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

        #endregion

        #region 0x003 chunk (common)

        /// <summary>
        /// CGameCtnChallenge 0x003 chunk (common)
        /// </summary>
        [Chunk(0x03043003)]
        public class Chunk003 : SkippableChunk
        {
            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public byte Version { get; set; }

            /// <summary>
            /// Basic map information. <see cref="Meta.ID"/> is map UID, <see cref="Meta.Author"/> is the map author login.
            /// </summary>
            public Meta MapInfo { get; set; }

            /// <summary>
            /// Formatted name of the map.
            /// </summary>
            public string MapName { get; set; }

            /// <summary>
            /// The track's intended use.
            /// </summary>
            public TrackKind Kind { get; set; }

            /// <summary>
            /// If the track is locked (used by Virtual Skipper to lock the map parameters). Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="1"/></c>.
            /// </summary>
            public bool? Locked { get; set; }

            /// <summary>
            /// Password of the map used by older tracks.
            /// </summary>
            public string Password { get; set; }

            /// <summary>
            /// Information about the used map decoration. <see cref="Meta.ID"/> is the map base ID, <see cref="Meta.Author"/> is the author of the map base.
            /// </summary>
            public Meta Decoration { get; set; }

            /// <summary>
            /// Origin of the map. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="3"/></c>.
            /// </summary>
            public Vector2? MapOrigin { get; set; }

            /// <summary>
            /// Target of the map. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="4"/></c>.
            /// </summary>
            public Vector2? MapTarget { get; set; }

            /// <summary>
            /// Name of the map type script.
            /// </summary>
            public string MapType { get; set; }

            /// <summary>
            /// Style of the map (Fullspeed, LOL, Tech), usually unused and defined by user.
            /// </summary>
            public string MapStyle { get; set; }

            /// <summary>
            /// Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="8"/></c>.
            /// </summary>
            public ulong? LightmapCacheUID { get; set; }

            /// <summary>
            /// Version of the lightmap calculation. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="9"/></c>.
            /// </summary>
            public byte? LightmapVersion { get; set; }

            /// <summary>
            /// Title pack the map was built in. Can be <see cref="null"/> if <c><see cref="Version"/> &lt; <see cref="11"/></c>.
            /// </summary>
            public string TitleUID { get; set; }

            public int Unknown1 { get; set; }

            public Chunk003(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Byte(Version);
                MapInfo = rw.Meta(MapInfo);
                MapName = rw.String(MapName);
                Kind = (TrackKind)rw.Byte((byte)Kind);

                if (Version >= 1)
                {
                    Locked = rw.UInt32(Convert.ToUInt32(Locked.GetValueOrDefault())) == 1;
                    Password = rw.String(Password);

                    if (Version >= 2)
                    {
                        Decoration = rw.Meta(Decoration);

                        if (Version >= 3)
                        {
                            MapOrigin = rw.Vec2(MapOrigin.GetValueOrDefault());

                            if (Version >= 4)
                            {
                                MapTarget = rw.Vec2(MapTarget.GetValueOrDefault());

                                if (Version >= 5)
                                {
                                    rw.Bytes(Unknown, 16);

                                    if (Version >= 6)
                                    {
                                        MapType = rw.String(MapType);
                                        MapStyle = rw.String(MapStyle);

                                        if (Version <= 8)
                                            Unknown1 = rw.Int32(Unknown1);

                                        if (Version >= 8)
                                        {
                                            LightmapCacheUID = rw.UInt64(LightmapCacheUID.GetValueOrDefault());

                                            if (Version >= 9)
                                            {
                                                LightmapVersion = rw.Byte(LightmapVersion.GetValueOrDefault());

                                                if (Version >= 11)
                                                    TitleUID = rw.LookbackString(TitleUID);
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

        #endregion

        #region 0x004 chunk (version)

        /// <summary>
        /// CGameCtnChallenge 0x004 chunk (version)
        /// </summary>
        [Chunk(0x03043004)]
        public class Chunk004 : SkippableChunk
        {
            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version { get; set; }

            public Chunk004(CGameCtnChallenge node, byte[] data) : base(node, data)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
            }
        }

        #endregion

        #region 0x005 chunk (xml)

        /// <summary>
        /// CGameCtnChallenge 0x005 chunk (xml)
        /// </summary>
        [Chunk(0x03043005)]
        public class Chunk005 : SkippableChunk
        {
            /// <summary>
            /// XML track information and dependencies.
            /// </summary>
            public string XML { get; set; }

            public Chunk005(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                XML = rw.String(XML);
            }
        }

        #endregion

        #region 0x007 chunk (thumbnail)

        /// <summary>
        /// CGameCtnChallenge 0x007 chunk (thumbnail)
        /// </summary>
        [Chunk(0x03043007)]
        public class Chunk007 : SkippableChunk
        {
            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version { get; set; }

            /// <summary>
            /// Thumbnail bitmap.
            /// </summary>
            [IgnoreDataMember]
            public Task<Bitmap> Thumbnail { get; set; }

            MemoryStream msT;

            /// <summary>
            /// Comments of the map.
            /// </summary>
            public string Comments { get; set; }

            public Chunk007(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                if(Version != 0)
                {
                    using var ms = new MemoryStream();
                    if (rw.Mode == GameBoxReaderWriterMode.Write)
                    {
                        Thumbnail.Result.RotateFlip(RotateFlipType.Rotate180FlipX);
                        ExportThumbnail(ms, ImageFormat.Jpeg);
                    }

                    var thumbnailSize = rw.Int32((int)ms.Length);
                    rw.Bytes(Encoding.UTF8.GetBytes("<Thumbnail.jpg>"), "<Thumbnail.jpg>".Length); // Because the string is purely ASCII anyway, Length is usable
                    var thumbnailData = rw.Bytes(ms.ToArray(), thumbnailSize);
                    rw.Bytes(Encoding.UTF8.GetBytes("</Thumbnail.jpg>"), "</Thumbnail.jpg>".Length);
                    rw.Bytes(Encoding.UTF8.GetBytes("<Comments>"), "<Comments>".Length);
                    Comments = rw.String(Comments);
                    rw.Bytes(Encoding.UTF8.GetBytes("</Comments>"), "</Comments>".Length);

                    if(rw.Mode == GameBoxReaderWriterMode.Read && thumbnailData.Length > 0)
                    {
                        Thumbnail = Task.Run(() =>
                        {
                            msT = new MemoryStream(thumbnailData);
                            var bitmap = (Bitmap)Image.FromStream(msT);
                            bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
                            return bitmap;
                        });
                    }
                }
            }

            /// <summary>
            /// Exports the map's thumbnail.
            /// </summary>
            /// <param name="stream">Stream to export to.</param>
            /// <param name="format">Image format to use.</param>
            public void ExportThumbnail(Stream stream, ImageFormat format)
            {
                if (Thumbnail == null) return;

                if (format == ImageFormat.Jpeg)
                {
                    var encoding = new EncoderParameters(1);
                    encoding.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                    var encoder = ImageCodecInfo.GetImageDecoders().Where(x => x.FormatID == ImageFormat.Jpeg.Guid).First();

                    Thumbnail.Result.Save(stream, encoder, encoding);
                }
                else
                    Thumbnail.Result.Save(stream, format);
            }

            /// <summary>
            /// Exports the map's thumbnail.
            /// </summary>
            /// <param name="fileName">File to export to.</param>
            /// <param name="format">Image format to use.</param>
            public void ExportThumbnail(string fileName, ImageFormat format)
            {
                if (Thumbnail == null) return;

                if (format == ImageFormat.Jpeg)
                {
                    var encoding = new EncoderParameters(1);
                    encoding.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                    var encoder = ImageCodecInfo.GetImageDecoders().Where(x => x.FormatID == ImageFormat.Jpeg.Guid).First();

                    Thumbnail.Result.Save(fileName, encoder, encoding);
                }
                else
                    Thumbnail.Result.Save(fileName, format);
            }

            /// <summary>
            /// Asynchronously imports (and replaces) a thumbnail to use for the map.
            /// </summary>
            /// <param name="stream">Stream to import from.</param>
            /// <returns>A task that processes the thumbnail.</returns>
            public Task<Bitmap> ImportThumbnailAsync(Stream stream)
            {
                Thumbnail = Task.Run(() => new Bitmap(stream));
                return Thumbnail;
            }

            /// <summary>
            /// Asynchronously imports (and replaces) a thumbnail to use for the map.
            /// </summary>
            /// <param name="fileName">File to import from.</param>
            /// <returns>A task that processes the thumbnail.</returns>
            public Task<Bitmap> ImportThumbnailAsync(string fileName)
            {
                Thumbnail = Task.Run(() => new Bitmap(fileName));
                return Thumbnail;
            }

            /// <summary>
            /// Imports (and replaces) a thumbnail to use for the map.
            /// </summary>
            /// <param name="stream">Stream to import from.</param>
            public void ImportThumbnail(Stream stream)
            {
                ImportThumbnailAsync(stream).Wait();
            }

            /// <summary>
            /// Imports (and replaces) a thumbnail to use for the map.
            /// </summary>
            /// <param name="fileName">File to import from.</param>
            public void ImportThumbnail(string fileName)
            {
                ImportThumbnailAsync(fileName).Wait();
            }
        }

        #endregion

        #region 0x008 chunk (author)

        /// <summary>
        /// CGameCtnChallenge 0x008 chunk (author)
        /// </summary>
        [Chunk(0x03043008)]
        public class Chunk008 : SkippableChunk
        {
            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version { get; set; }

            public int AuthorVersion { get; set; }

            /// <summary>
            /// Map author login.
            /// </summary>
            public string AuthorLogin { get; set; }

            /// <summary>
            /// Map author formatted nickname.
            /// </summary>
            public string AuthorNickname { get; set; }

            /// <summary>
            /// Map author zone.
            /// </summary>
            public string AuthorZone { get; set; }

            public string AuthorExtraInfo { get; set; }

            public Chunk008(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                AuthorVersion = rw.Int32(AuthorVersion);
                AuthorLogin = rw.String(AuthorLogin);
                AuthorNickname = rw.String(AuthorNickname);
                AuthorZone = rw.String(AuthorZone);
                AuthorExtraInfo = rw.String(AuthorExtraInfo);
            }
        }

        #endregion

        #region 0x00D chunk (vehicle)

        /// <summary>
        /// CGameCtnChallenge 0x00D chunk (vehicle)
        /// </summary>
        [Chunk(0x0304300D)]
        public class Chunk00D : Chunk
        {
            /// <summary>
            /// Vehicle metadata info.
            /// </summary>
            public Meta Vehicle { get; set; }

            public Chunk00D(CGameCtnChallenge node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Vehicle = rw.Meta(Vehicle);
            }
        }

        #endregion

        #region 0x00F chunk (old block data)

        /// <summary>
        /// CGameCtnChallenge 0x00F chunk (old block data)
        /// </summary>
        [Chunk(0x0304300F)]
        public class Chunk00F : Chunk
        {
            public Meta MapInfo { get; set; }
            public Int3 Size { get; set; }
            public int Unknown1 { get; set; }
            public CGameCtnBlock[] Blocks { get; set; }
            public int Unknown2 { get; set; }
            public Meta Unknown3 { get; set; }

            public Chunk00F(Node node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                MapInfo = rw.Meta(MapInfo);
                Size = rw.Int3(Size);
                Unknown1 = rw.Int32(Unknown1);
                Blocks = rw.Array(Blocks,
                    i => rw.Reader.ReadNodeRef<CGameCtnBlock>(),
                    x => rw.Writer.Write(x));
                Unknown2 = rw.Int32(Unknown2);
                Unknown3 = rw.Meta(Unknown3);
            }
        }

        #endregion

        #region 0x011 chunk

        /// <summary>
        /// CGameCtnChallenge 0x011 chunk
        /// </summary>
        [Chunk(0x03043011)]
        public class Chunk011 : Chunk
        {
            /// <summary>
            /// List of puzzle pieces.
            /// </summary>
            public CGameCtnCollectorList CollectorList { get; set; }

            public CGameCtnChallengeParameters ChallengeParameters { get; set; }

            /// <summary>
            /// The track's intended use.
            /// </summary>
            public TrackKind Kind { get; set; }

            public Chunk011(CGameCtnChallenge node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                CollectorList = rw.NodeRef<CGameCtnCollectorList>(CollectorList);
                ChallengeParameters = rw.NodeRef<CGameCtnChallengeParameters>(ChallengeParameters);
                Kind = (TrackKind)rw.Int32((int)Kind);
            }
        }

        #endregion

        #region 0x012 chunk

        /// <summary>
        /// CGameCtnChallenge 0x012 chunk
        /// </summary>
        [Chunk(0x03043012)]
        public class Chunk012 : Chunk
        {
            public Chunk012(CGameCtnChallenge node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.String(Unknown);
            }
        }

        #endregion

        #region 0x013 chunk (legacy block data)

        /// <summary>
        /// CGameCtnChallenge 0x013 chunk (legacy block data)
        /// </summary>
        [Chunk(0x03043013)]
        public class Chunk013 : Chunk
        {
            public Chunk01F Chunk01F { get; }

            public Chunk013(CGameCtnChallenge node) : base(node)
            {
                Chunk01F = new Chunk01F(node, this);
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Chunk01F.ReadWrite(rw);
            }
        }

        #endregion

        #region 0x014 skippable chunk (legacy password)

        /// <summary>
        /// CGameCtnChallenge 0x014 skippable chunk (legacy password)
        /// </summary>
        [Chunk(0x03043014)]
        public class Chunk014 : SkippableChunk
        {
            /// <summary>
            /// Legacy password string.
            /// </summary>
            public string Password { get; set; }

            public Chunk014(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                Password = rw.String(Password);
            }
        }

        #endregion

        #region 0x016 skippable chunk

        /// <summary>
        /// CGameCtnChallenge 0x016 skippable chunk
        /// </summary>
        [Chunk(0x03043016)]
        public class Chunk016 : SkippableChunk
        {
            public Chunk016(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x017 skippable chunk (checkpoints)

        /// <summary>
        /// CGameCtnChallenge 0x017 skippable chunk (checkpoints)
        /// </summary>
        [Chunk(0x03043017)]
        public class Chunk017 : SkippableChunk
        {
            /// <summary>
            /// All checkpoints and their map coordinates.
            /// </summary>
            public Int3[] Checkpoints { get; set; }

            public Chunk017(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Checkpoints = rw.Array(Checkpoints, i => rw.Reader.ReadInt3(), x => rw.Writer.Write(x));
            }
        }

        #endregion

        #region 0x018 skippable chunk (laps)

        /// <summary>
        /// CGameCtnChallenge 0x018 skippable chunk (laps)
        /// </summary>
        [Chunk(0x03043018)]
        public class Chunk018 : SkippableChunk
        {
            public bool IsLapRace { get; set; }
            /// <summary>
            /// Number of laps.
            /// </summary>
            public int Laps { get; set; }

            public Chunk018(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                IsLapRace = rw.Boolean(IsLapRace);
                Laps = rw.Int32(Laps);
            }
        }

        #endregion

        #region 0x019 skippable chunk (mod)

        /// <summary>
        /// CGameCtnChallenge 0x019 skippable chunk (mod)
        /// </summary>
        [Chunk(0x03043019)]
        public class Chunk019 : SkippableChunk
        {
            /// <summary>
            /// Used mod pack on the map.
            /// </summary>
            public FileRef ModPackDesc { get; set; }

            public Chunk019(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                ModPackDesc = rw.FileRef(ModPackDesc);
            }
        }

        #endregion

        #region 0x01C skippable chunk (play mode)

        /// <summary>
        /// CGameCtnChallenge 0x01C skippable chunk (play mode)
        /// </summary>
        [Chunk(0x0304301C)]
        public class Chunk01C : SkippableChunk
        {
            public PlayMode Mode { get; set; }

            public Chunk01C(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Mode = (PlayMode)rw.Int32((int)Mode);
            }
        }

        #endregion

        #region 0x01F chunk (block data)

        /// <summary>
        /// CGameCtnChallenge 0x01F chunk (block data)
        /// </summary>
        [Chunk(0x0304301F)]
        public class Chunk01F : Chunk
        {
            /// <summary>
            /// Map metadata with UID.
            /// </summary>
            public Meta MapInfo { get; set; }

            /// <summary>
            /// Formatted name of the map.
            /// </summary>
            public string MapName { get; set; }

            /// <summary>
            /// Map base metadata info.
            /// </summary>
            public Meta Decoration { get; set; }

            /// <summary>
            /// Size of the placeable area in block coordinates.
            /// </summary>
            public Int3 Size { get; set; }

            public bool NeedUnlock { get; set; }

            public int? Version { get; set; }

            public int NbBlocks
            {
                get => Blocks.Where(x => x.Flags != -1).Count();
            }

            /// <summary>
            /// Array of all blocks on the map.
            /// </summary>
            public List<Block> Blocks { get; set; }

            readonly bool is013;

            public Chunk01F(CGameCtnChallenge node) : this(node, null)
            {

            }

            public Chunk01F(CGameCtnChallenge node, Chunk chunk) : base(node)
            {
                is013 = chunk is Chunk013;
            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                MapInfo = r.ReadMeta();
                MapName = r.ReadString();
                Decoration = r.ReadMeta();
                Size = r.ReadInt3();
                NeedUnlock = r.ReadBoolean();

                if (!is013)
                    Version = r.ReadInt32();

                var nbBlocks = r.ReadInt32(); // It's maybe slower but better for the program to determine the count from the list

                List<Block> blocks = new List<Block>();

                while ((r.PeekUInt32() & 0xC0000000) > 0)
                {
                    var blockName = r.ReadLookbackString();
                    var dir = (Direction)r.ReadByte();
                    var coord = r.ReadByte3();
                    var flags = -1;

                    if (Version == null)
                        flags = r.ReadInt16();
                    else if (Version > 0)
                        flags = r.ReadInt32();

                    if (flags == -1)
                    {
                        blocks.Add(new Block(blockName, dir, (Int3)coord, flags, null, null, null));
                        continue;
                    }

                    string author = null;
                    CGameCtnBlockSkin skin = null;

                    if ((flags & (1 << 15)) != 0) // custom block
                    {
                        author = r.ReadLookbackString();
                        skin = r.ReadNodeRef<CGameCtnBlockSkin>();
                    }

                    CGameWaypointSpecialProperty parameters = null;

                    if ((flags & (1 << 20)) != 0)
                        parameters = r.ReadNodeRef<CGameWaypointSpecialProperty>();

                    if ((flags & (1 << 18)) != 0)
                    {

                    }

                    if ((flags & (1 << 17)) != 0)
                    {

                    }

                    blocks.Add(new Block(blockName, dir, (Int3)coord, flags, author, skin, parameters));
                }

                Blocks = blocks;
            }

            public override void Write(GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(MapInfo);
                w.Write(MapName);
                w.Write(Decoration);
                w.Write(Size);
                w.Write(NeedUnlock);

                if (!is013)
                    w.Write(Version.GetValueOrDefault());

                w.Write(NbBlocks);

                foreach (var x in Blocks)
                {
                    w.WriteLookbackString(x.Name);
                    w.Write((byte)x.Direction);
                    w.Write((Byte3)x.Coord);

                    if (Version == null)
                        w.Write((short)x.Flags);
                    else if (Version > 0)
                        w.Write(x.Flags);

                    if (x.Flags != -1)
                    {
                        if ((x.Flags & 0x8000) != 0) // custom block
                        {
                            w.WriteLookbackString(x.Author);
                            w.Write(x.Skin);
                        }

                        if ((x.Flags & 0x100000) != 0)
                            w.Write(x.Parameters);
                    }
                }
            }
        }

        #endregion

        #region 0x021 chunk (TMUF mediatracker)

        /// <summary>
        /// CGameCtnChallenge 0x021 chunk (TMUF mediatracker)
        /// </summary>
        [Chunk(0x03043021)]
        public class Chunk021 : Chunk
        {
            /// <summary>
            /// Reference to <see cref="CGameCtnMediaClip"/> intro.
            /// </summary>
            public CGameCtnMediaClip ClipIntro { get; set; }

            /// <summary>
            /// Reference to <see cref="CGameCtnMediaClipGroup"/> in game.
            /// </summary>
            public CGameCtnMediaClipGroup ClipGroupInGame { get; set; }

            /// <summary>
            /// Reference to <see cref="CGameCtnMediaClipGroup"/> end race.
            /// </summary>
            public CGameCtnMediaClipGroup ClipGroupEndRace { get; set; }

            public Chunk021(CGameCtnChallenge node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                ClipIntro = rw.NodeRef<CGameCtnMediaClip>(ClipIntro);
                ClipGroupInGame = rw.NodeRef<CGameCtnMediaClipGroup>(ClipGroupInGame);
                ClipGroupEndRace = rw.NodeRef<CGameCtnMediaClipGroup>(ClipGroupEndRace);
            }
        }

        #endregion

        #region 0x022 chunk

        /// <summary>
        /// CGameCtnChallenge 0x022 chunk
        /// </summary>
        [Chunk(0x03043022)]
        public class Chunk022 : Chunk
        {
            public Chunk022(CGameCtnChallenge node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Boolean(Unknown);
            }
        }

        #endregion

        #region 0x024 chunk (music)

        /// <summary>
        /// CGameCtnChallenge 0x024 chunk (music)
        /// </summary>
        [Chunk(0x03043024)]
        public class Chunk024 : Chunk
        {
            /// <summary>
            /// Reference to a music file.
            /// </summary>
            public FileRef CustomMusicPackDesc { get; set; }

            public Chunk024(CGameCtnChallenge node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                CustomMusicPackDesc = rw.FileRef(CustomMusicPackDesc);

                if(rw.Mode == GameBoxReaderWriterMode.Read) // TODO: check
                {
                    var idk = rw.Reader.ReadInt32();
                    if (idk != 0)
                        rw.Reader.BaseStream.Position -= sizeof(int);
                }
            }
        }

        #endregion

        #region 0x025 chunk

        /// <summary>
        /// CGameCtnChallenge 0x025 chunk
        /// </summary>
        [Chunk(0x03043025)]
        public class Chunk025 : Chunk
        {
            public Vector2 MapCoordOrigin { get; set; }
            public Vector2 MapCoordTarget { get; set; }

            public Chunk025(CGameCtnChallenge node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                MapCoordOrigin = rw.Vec2(MapCoordOrigin);
                MapCoordTarget = rw.Vec2(MapCoordTarget);
            }
        }

        #endregion

        #region 0x026 chunk

        /// <summary>
        /// CGameCtnChallenge 0x026 chunk
        /// </summary>
        [Chunk(0x03043026)]
        public class Chunk026 : Chunk
        {
            public Node ClipGlobal { get; set; }

            public Chunk026(CGameCtnChallenge node) : base(node)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                ClipGlobal = rw.NodeRef(ClipGlobal);
            }
        }

        #endregion

        #region 0x027 chunk

        /// <summary>
        /// CGameCtnChallenge 0x027 chunk
        /// </summary>
        [Chunk(0x03043027)]
        public class Chunk027 : Chunk
        {
            public bool ArchiveGmCamVal { get; set; }
            public Vector3? Vec1 { get; set; }
            public Vector3? Vec2 { get; set; }
            public Vector3? Vec3 { get; set; }

            public Chunk027(CGameCtnChallenge node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                ArchiveGmCamVal = rw.Boolean(ArchiveGmCamVal);

                if(ArchiveGmCamVal)
                {
                    rw.Byte(Unknown);

                    rw.Vec3(Unknown);
                    rw.Vec3(Unknown);
                    rw.Vec3(Unknown);

                    rw.Vec3(Unknown);
                    rw.Single(Unknown);
                    rw.Single(Unknown);
                    rw.Single(Unknown);
                }
            }
        }

        #endregion

        #region 0x028 chunk (comments)

        /// <summary>
        /// CGameCtnChallenge 0x028 chunk (comments)
        /// </summary>
        [Chunk(0x03043028)]
        public class Chunk028 : Chunk
        {
            public Chunk027 Chunk027 { get; }
            public string Comments { get; set; }

            public Chunk028(CGameCtnChallenge node) : base(node)
            {
                Chunk027 = new Chunk027(node);
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Chunk027.Unknown.Position = 0;
                Chunk027.ReadWrite(rw);
                Comments = rw.String(Comments);
            }
        }

        #endregion

        #region 0x029 skippable chunk (password)

        /// <summary>
        /// CGameCtnChallenge 0x029 skippable chunk (password)
        /// </summary>
        [Chunk(0x03043029)]
        public class Chunk029 : SkippableChunk
        {
            /// <summary>
            /// 128bit password MD5 hash.
            /// </summary>
            public byte[] HashedPassword { get; set; }
            public uint CRC32 { get; set; }

            public Chunk029(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                HashedPassword = rw.Bytes(HashedPassword, 16);
                CRC32 = rw.UInt32(CRC32);
            }

            /// <summary>
            /// Sets a new map password.
            /// </summary>
            /// <param name="password">Password that will be hashed.</param>
            public void NewPassword(string password)
            {
                var md5 = MD5.Create();
                HashedPassword = md5.ComputeHash(Encoding.UTF8.GetBytes(password));

                Crc32 crc32 = new Crc32();
                crc32.Update(Encoding.ASCII.GetBytes("0x" + BitConverter.ToInt16(HashedPassword).ToString() + "???" + (Node as CGameCtnChallenge).MapUid));
                CRC32 = Convert.ToUInt32(crc32.Value);
            }
        }

        #endregion

        #region 0x02A chunk

        /// <summary>
        /// CGameCtnChallenge 0x02A chunk
        /// </summary>
        [Chunk(0x0304302A)]
        public class Chunk02A : Chunk
        {
            public Chunk02A(CGameCtnChallenge node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Boolean(Unknown);
            }
        }

        #endregion

        #region 0x036 skippable chunk (realtime thumbnail)

        /// <summary>
        /// CGameCtnChallenge 0x036 skippable chunk (realtime thumbnail)
        /// </summary>
        [Chunk(0x03043036)]
        public class Chunk036 : SkippableChunk
        {
            /// <summary>
            /// Position of the thumnail camera.
            /// </summary>
            public Vector3 ThumbnailPosition { get; set; }

            /// <summary>
            /// Pitch, yaw and roll of the thumbnail camera in radians.
            /// </summary>
            public Vector3 ThumbnailPitchYawRoll { get; set; }

            /// <summary>
            /// Thumbnail camera FOV.
            /// </summary>
            public float ThumbnailFOV { get; set; }

            public Chunk036(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                ThumbnailPosition = rw.Vec3(ThumbnailPosition);
                ThumbnailPitchYawRoll = rw.Vec3(ThumbnailPitchYawRoll);
                ThumbnailFOV = rw.Single(ThumbnailFOV);

                rw.Bytes(Unknown, 31);
            }
        }

        #endregion

        #region 0x03D skippable chunk (lightmaps)

        /// <summary>
        /// CGameCtnChallenge 0x03D skippable chunk (lightmaps)
        /// </summary>
        [IgnoreChunk]
        [Chunk(0x0304303D)]
        public class Chunk03D : SkippableChunk
        {
            public int Version { get; set; }
            public Task<CHmsLightMapCache> LightmapCache { get; set; }

            public Chunk03D(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                
            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                unknownW.Write(r.ReadBoolean());
                Version = r.ReadInt32();

                int frames = 1;
                if (Version >= 5)
                    frames = r.ReadInt32();

                if (Version >= 2)
                {
                    int size = 0;

                    for (var i = 0; i < frames; i++)
                    {
                        size = r.ReadInt32();
                        var image = r.ReadBytes(size);

                        if (Version >= 3)
                        {
                            var size1 = r.ReadInt32();
                            if (size1 > 0)
                            {
                                var image1 = r.ReadBytes(size1);
                            }
                        }

                        if (Version >= 6)
                        {
                            var size2 = r.ReadInt32();
                            if (size2 > 0)
                            {
                                var image2 = r.ReadBytes(size2);
                            }
                        }
                    }

                    if (size != 0)
                    {
                        var uncompressedSize = r.ReadInt32();
                        var compressedSize = r.ReadInt32();
                        var data = r.ReadBytes(compressedSize);

                        LightmapCache = Task.Run(() =>
                        {
                            using var ms = new MemoryStream(data);
                            using var zlib = new InflaterInputStream(ms);
                            using var gbxr = new GameBoxReader(zlib);
                            return (CHmsLightMapCache)Node.Parse(Node.Lookbackable, 0x06022000, gbxr);
                        });
                    }
                }
            }
        }

        #endregion

        #region 0x040 skippable chunk (items)

        /// <summary>
        /// CGameCtnChallenge 0x040 skippable chunk (items)
        /// </summary>
        [Chunk(0x03043040)]
        public class Chunk040 : SkippableChunk, ILookbackable
        {
            int? ILookbackable.LookbackVersion { get; set; }
            List<string> ILookbackable.LookbackStrings { get; set; } = new List<string>();
            bool ILookbackable.LookbackWritten { get; set; }

            public int Version { get; set; } = 4;
            public List<CGameCtnAnchoredObject> Items { get; set; } = new List<CGameCtnAnchoredObject>();

            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; } = 10;
            public int Unknown3 { get; set; }

            public Chunk040(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                
            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                if (Version != 0)
                {
                    Unknown1 = r.ReadInt32();
                    var size = r.ReadInt32();
                    Unknown2 = r.ReadInt32(); // 10

                    Items = ParseArray<CGameCtnAnchoredObject>(this, r).ToList();
                    Unknown3 = r.ReadInt32(); // 0
                }
            }

            public override void Write(GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);

                if(Version != 0)
                {
                    w.Write(Unknown1);

                    using var itemMs = new MemoryStream();
                    using var wr = new GameBoxWriter(itemMs);

                    wr.Write(Unknown2);
                    wr.Write(Items.Count);

                    foreach (var item in Items)
                    {
                        wr.Write(item.ID);
                        item.Write(wr);
                    }

                    wr.Write(Unknown3);

                    w.Write((int)itemMs.Length);
                    w.Write(itemMs.ToArray(), 0, (int)itemMs.Length);
                }
            }
        }

        #endregion

        #region 0x042 skippable chunk (author)

        /// <summary>
        /// CGameCtnChallenge 0x042 skippable chunk (author)
        /// </summary>
        [Chunk(0x03043042)]
        public class Chunk042 : SkippableChunk
        {
            /// <summary>
            /// Version of the chunk.
            /// </summary>
            public int Version { get; set; }

            public int AuthorVersion { get; set; }

            /// <summary>
            /// Map author login.
            /// </summary>
            public string AuthorLogin { get; set; }

            /// <summary>
            /// Map author formatted nickname.
            /// </summary>
            public string AuthorNickname { get; set; }

            /// <summary>
            /// Map author zone.
            /// </summary>
            public string AuthorZone { get; set; }

            public string AuthorExtraInfo { get; set; }

            public Chunk042(CGameCtnChallenge node, byte[] data) : base(node, data)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                AuthorVersion = rw.Int32(AuthorVersion);
                AuthorLogin = rw.String(AuthorLogin);
                AuthorNickname = rw.String(AuthorNickname);
                AuthorZone = rw.String(AuthorZone);
                AuthorExtraInfo = rw.String(AuthorExtraInfo);
            }
        }

        #endregion

        #region 0x043 skippable chunk

        /// <summary>
        /// CGameCtnChallenge 0x043 skippable chunk
        /// </summary>
        [Chunk(0x03043043)]
        public class Chunk043 : SkippableChunk, ILookbackable
        {
            int? ILookbackable.LookbackVersion { get; set; }
            List<string> ILookbackable.LookbackStrings { get; set; } = new List<string>();
            bool ILookbackable.LookbackWritten { get; set; }

            public int Version { get; set; }
            public new byte[] Data { get; set; }

            public Task<CGameCtnZoneGenealogy[]> Genealogies { get; set; }

            public Chunk043(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                
            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                var sizeOfNodeWithClassID = r.ReadInt32();
                Data = r.ReadBytes(sizeOfNodeWithClassID);

                Genealogies = Task.Run(() =>
                {
                    using var ms = new MemoryStream(Data);
                    using var r2 = new GameBoxReader(ms, this);

                    return ParseArray<CGameCtnZoneGenealogy>(this, r2);
                });
            }

            public override void Write(GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);

                using var ms = new MemoryStream();
                using var w2 = new GameBoxWriter(ms);

                w2.Write(Genealogies.Result, x =>
                {
                    w2.Write(0x0311D000);
                    x.Write(w2);
                });

                w.Write((int)ms.Length);
                w.Write(ms.ToArray());
            }
        }

        #endregion

        #region 0x044 skippable chunk (metadata)

        /// <summary>
        /// CGameCtnChallenge 0x044 skippable chunk (metadata)
        /// </summary>
        [Chunk(0x03043044)]
        public class Chunk044 : SkippableChunk
        {
            public int Version { get; set; }
            public CScriptTraitsMetadata MetadataTraits { get; }

            public Chunk044(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                MetadataTraits = new CScriptTraitsMetadata();
            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                var size = r.ReadInt32();

                MetadataTraits.Read(r);
            }

            public override void Write(GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);

                using (var ms = new MemoryStream())
                {
                    using (var wm = new GameBoxWriter(ms))
                        MetadataTraits.Write(wm);

                    w.Write((int)ms.Length);
                    w.Write(ms.ToArray(), 0, (int)ms.Length);
                }
            }
        }

        #endregion

        #region 0x049 chunk (mediatracker)

        /// <summary>
        /// CGameCtnChallenge 0x049 chunk (mediatracker)
        /// </summary>
        [Chunk(0x03043049)]
        public class Chunk049 : Chunk
        {
            public int Version { get; set; } = 2;

            public CGameCtnMediaClip ClipIntro { get; set; }

            public CGameCtnMediaClipGroup ClipGroupInGame { get; set; }

            public CGameCtnMediaClipGroup ClipGroupEndRace { get; set; }

            public CGameCtnMediaClip ClipPodium { get; set; }
            public CGameCtnMediaClip ClipAmbiance { get; set; }

            public int Unknown1 { get; set; } = 3;
            public int Unknown2 { get; set; } = 1;
            public int Unknown3 { get; set; } = 3;

            public Chunk049(CGameCtnChallenge node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                ClipIntro = rw.NodeRef<CGameCtnMediaClip>(ClipIntro);
                ClipPodium = rw.NodeRef<CGameCtnMediaClip>(ClipPodium); //
                ClipGroupInGame = rw.NodeRef<CGameCtnMediaClipGroup>(ClipGroupInGame);
                ClipGroupEndRace = rw.NodeRef<CGameCtnMediaClipGroup>(ClipGroupEndRace);

                if(Version >= 2)
                    ClipAmbiance = rw.NodeRef<CGameCtnMediaClip>(ClipAmbiance);

                Unknown1 = rw.Int32(Unknown1);
                Unknown2 = rw.Int32(Unknown2);
                Unknown3 = rw.Int32(Unknown3);
            }
        }

        #endregion

        #region 0x04B skippable chunk (objectives)

        /// <summary>
        /// CGameCtnChallenge 0x04B skippable chunk (objectives)
        /// </summary>
        [Chunk(0x0304304B)]
        public class Chunk04B : SkippableChunk
        {
            public string ObjectiveTextAuthor { get; set; }
            public string ObjectiveTextGold { get; set; }
            public string ObjectiveTextSilver { get; set; }
            public string ObjectiveTextBronze { get; set; }

            public Chunk04B(CGameCtnChallenge node, byte[] data) : base(node, data)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                ObjectiveTextAuthor = rw.String(ObjectiveTextAuthor);
                ObjectiveTextGold = rw.String(ObjectiveTextGold);
                ObjectiveTextSilver = rw.String(ObjectiveTextSilver);
                ObjectiveTextBronze = rw.String(ObjectiveTextBronze);
            }
        }

        #endregion

        #region 0x051 skippable chunk (title info)

        /// <summary>
        /// CGameCtnChallenge 0x051 skippable chunk (title info)
        /// </summary>
        [Chunk(0x03043051)]
        public class Chunk051 : SkippableChunk
        {
            public int Version { get; set; }
            public string TitleID { get; set; }
            public string BuildVersion { get; set; }

            public Chunk051(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                TitleID = rw.LookbackString(TitleID);
                BuildVersion = rw.String(BuildVersion);
            }
        }

        #endregion

        #region 0x059 skippable chunk

        /// <summary>
        /// CGameCtnChallenge 0x059 skippable chunk
        /// </summary>
        [Chunk(0x03043059)]
        public class Chunk059 : SkippableChunk
        {
            public int Version { get; set; }

            public Chunk059(CGameCtnChallenge node, byte[] data) : base(node, data)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version); // 3

                rw.Vec3(Unknown);

                if (Version != 0)
                {
                    rw.Boolean(Unknown);

                    if (Version >= 3)
                    {
                        rw.Single(Unknown);
                        rw.Single(Unknown);
                    }
                }
            }
        }

        #endregion

        #region 0x05A skippable chunk

        /// <summary>
        /// CGameCtnChallenge 0x05A skippable chunk [TM®️]
        /// </summary>
        [Chunk(0x0304305A)]
        public class Chunk05A : SkippableChunk
        {
            public Chunk05A(CGameCtnChallenge node, byte[] data) : base(node, data)
            {

            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                rw.Int32(Unknown);
                rw.Int32(Unknown);
            }
        }

        #endregion

        #region 0x05F skippable chunk (free blocks) [TM®️]

        /// <summary>
        /// CGameCtnChallenge 0x05F skippable chunk (free blocks) [TM®️]
        /// </summary>
        [Chunk(0x0304305F)]
        public class Chunk05F : SkippableChunk
        {
            public int Version { get; set; }

            /// <summary>
            /// List of vectors that can't be directly figured out without information from <see cref="Chunk01F"/>.
            /// </summary>
            public List<Vector3> Vectors { get; set; }

            [IgnoreDataMember]
            public ReadOnlyCollection<FreeBlock> FreeBlocks
            {
                get
                {
                    List<FreeBlock> freeBlocks = new List<FreeBlock>();

                    var enumerator = Vectors.GetEnumerator();

                    foreach(var b in ((CGameCtnChallenge)Node).Blocks.Where(x => x.IsFree))
                    {
                        enumerator.MoveNext();
                        var position = enumerator.Current;

                        enumerator.MoveNext();
                        var pitchYawRoll = enumerator.Current;

                        var fb = new FreeBlock(b)
                        {
                            Position = position,
                            PitchYawRoll = pitchYawRoll
                        };

                        freeBlocks.Add(fb);
                    }

                    return new ReadOnlyCollection<FreeBlock>(freeBlocks);
                }
            }

            public Chunk05F(CGameCtnChallenge node, byte[] data) : base(node, data)
            {
                Vectors = new List<Vector3>();
            }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                Vectors.Clear();
                while (r.BaseStream.Position < r.BaseStream.Length)
                    Vectors.Add(new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle()));
            }

            public override void Write(GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);
                foreach (var v in Vectors)
                    w.Write(v);
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Dependency
        {
            public string File { get; }
            public string Url { get; }

            public Dependency(string file, string url)
            {
                File = file;
                Url = url;
            }
        }
    #endregion
    }
}