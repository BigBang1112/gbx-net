using GBX.NET;
using GBX.NET.Engines.Game;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamlDotNet.Serialization;

namespace IslandConverter
{
    public partial class ConverterForm : Form
    {
        public Dictionary<string, GameBox<CGameCtnChallenge>> Maps { get; set; }
        public List<string> MapsLoading { get; set; }

        Random random = new Random();

        public ConverterForm()
        {
            InitializeComponent();

            Maps = new Dictionary<string, GameBox<CGameCtnChallenge>>();
            MapsLoading = new List<string>();

            UpdateSelectedMap();

            Log.OnLogEvent += Log_OnLogEvent;

            ilThumbnails.Images.Add("Loading", Resources.Loading);

            tsmiASuperSecretOption.Click += TsmiASuperSecretOption_Click;
            addAMapToolStripMenuItem.Click += AddAMapToolStripMenuItem_Click;
            toolStripMenuItem2.Click += AboutToolStripMenuItem_Click;
            tsmiChangeManiaPlanetUserdataLocation.Click += TsmiChangeManiaPlanetUserdataLocation_Click;
        }

        private void TsmiChangeManiaPlanetUserdataLocation_Click(object sender, EventArgs e)
        {
            SetOpenFolder();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ConverterAboutBox().ShowDialog();
        }

        private void Log_OnLogEvent(string text, ConsoleColor color)
        {
            Invoke(new Action(() =>
            {
                lbLog.Items.Add(text);
                lbLog.SelectedIndex = lbLog.Items.Count - 1;
            }));
        }

        private void ShowHideMapPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = !groupBox1.Visible;
        }

        private void ShowHideCommandLineOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void LoadMaps(params string[] fileNames)
        {
            foreach (string f in fileNames)
            {
                var item = lvMaps.Items.Add(Path.GetFileName(f), "Loading");

                MapsLoading.Add(f);
                bConvertAll.Enabled = false;
                bConvertSelected.Enabled = false;

                Task.Run(() =>
                {
                    try
                    {
                        var gbx = IslandConverter.LoadGBX(f, out TimeSpan? completionTime);

                        if (gbx != null)
                        {
                            var containsMap = true;

                            Invoke(new Action(() =>
                            {
                                var map = gbx.MainNode;
                                var mapUid = map.MapUid;

                                if (Maps.ContainsKey(mapUid))
                                    containsMap = false;

                                Maps.TryAdd(mapUid, gbx);

                                item.Name = mapUid;

                                if (map.Thumbnail == null)
                                    item.ImageKey = "";
                                else
                                {
                                    lvMaps.LargeImageList.Images.Add(mapUid, map.Thumbnail);
                                    item.ImageKey = mapUid;
                                }

                                item.Text = Formatter.Deformat(map.MapName);
                            }));

                            return containsMap;
                        }
                        else return false;
                    }
                    catch (Exception e)
                    {
                        Log.Write(e.ToString());
                        MessageBox.Show(e.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    return false;
                }).ContinueWith(successful =>
                {
                    Invoke(new Action(() =>
                    {
                        MapsLoading.RemoveAll(x => x == f);

                        UpdateSelectedMap();
                        if (!successful.Result)
                            item.Remove();

                        if (MapsLoading.Count == 0 && Maps.Count > 0)
                            bConvertAll.Enabled = true;
                    }));
                });
            }
        }

        private void LvMaps_DragDrop(object sender, DragEventArgs e)
        {
            if (lvMaps.View == View.Tile)
            {
                lvMaps.Items.Clear();
                lvMaps.View = View.LargeIcon;
            }

            LoadMaps((string[])e.Data.GetData(DataFormats.FileDrop, false));
        }

        private void LvMaps_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void XSizeButton_CheckedChanged(object sender, EventArgs e)
        {
            if (x45Button.Checked)
            {
                cbCutoff.Checked = false;
                cbCutoff.Enabled = false;
            }
            else
            {
                cbCutoff.Enabled = true;
            }
        }

        private void TsmiASuperSecretOption_Click(object sender, EventArgs e)
        {
            // All credits to Arkady
            Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.NoneEnabled;
        }

        private void LvMaps_KeyDown(object sender, KeyEventArgs e)
        {
            if (lvMaps.View != View.Tile && e.KeyCode == Keys.Delete)
            {
                foreach (ListViewItem item in lvMaps.SelectedItems)
                {
                    Maps.Remove(item.Name);

                    lvMaps.Items.Remove(item);
                }

                UpdateSelectedMap();
            }
        }

        private void LvMaps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(MapsLoading.Count == 0)
                UpdateSelectedMap();
        }

        Int3 blockRange = default;
        Int3? minCoord = default;

        void UpdateSelectedMap()
        {
            if (lvMaps.View == View.Tile || lvMaps.SelectedItems.Count == 0 || lvMaps.SelectedItems.Count > 1)
            {
                lMapName.Text = "";
                lUID.Text = "";
                lAuthor.Text = "";
                lSize.Text = "";
                lDecoration.Text = "";
                lMedals.Text = "";
                lAuthorM.Text = "";
                lGoldM.Text = "";
                lSilverM.Text = "";
                lBronzeM.Text = "";
                lMapType.Text = "";
                lBlockRange.Text = "";
                lBlockRange.BackColor = Color.Transparent;
                ttBlockRange.SetToolTip(lBlockRange, "");

                pbThumbnail.Image = null;
            }

            if (Maps.Count == 0)
                bConvertAll.Enabled = false;
            else if(MapsLoading.Count == 0)
                bConvertAll.Enabled = true;

            bConvertSelected.Enabled = false;

            if (lvMaps.SelectedItems.Count > 0)
            {
                if (MapsLoading.Count == 0)
                    bConvertSelected.Enabled = true;

                bConvertSelected.Text = "Convert selected maps";

                if (lvMaps.SelectedItems.Count == 1)
                {
                    bConvertSelected.Text = "Convert selected map";

                    var item = lvMaps.SelectedItems[0];
                    if (Maps.TryGetValue(item.Name, out GameBox<CGameCtnChallenge> gbx))
                    {
                        var map = gbx.MainNode;

                        lMapName.Text = Formatter.Deformat(map.MapName);
                        lUID.Text = "UID: " + map.MapUid;
                        lAuthor.Text = "Author: " + Formatter.Deformat(map.AuthorLogin);

                        lSize.Text = "Size: " + map.Size.ToString();
                        if (map.Size == (45, 36, 45)) lSize.Text += " (made in TMUF)";
                        else if (map.Size == (36, 36, 36)) lSize.Text += " (made in TMU or Sunrise)";
                        else lSize.Text += " (made in China)";

                        lDecoration.Text = "Decoration: " + map.DecorationName;
                        lMedals.Text = "Medals:";
                        lAuthorM.Text = "Author: " + (map.AuthorTime.HasValue ? map.AuthorTime.Value.ToString("m':'ss':'fff") : "None");
                        lGoldM.Text = "Gold: " + (map.GoldTime.HasValue ? map.GoldTime.Value.ToString("m':'ss':'fff") : "None");
                        lSilverM.Text = "Silver: " + (map.SilverTime.HasValue ? map.SilverTime.Value.ToString("m':'ss':'fff") : "None");
                        lBronzeM.Text = "Bronze: " + (map.BronzeTime.HasValue ? map.BronzeTime.Value.ToString("m':'ss':'fff") : "None");
                        lMapType.Text = "Map type: " + (map.Type.HasValue ? map.Type.Value.ToString() : "Unknown");

                        blockRange = IslandConverter.DefineMapRange(map.Blocks.ToArray(), out minCoord);

                        if (blockRange.X <= 32 && blockRange.Z <= 32)
                        {
                            lBlockRange.BackColor = Color.Lime;
                            ttBlockRange.SetToolTip(lBlockRange, "This map fits the default Stadium64x64 base! You can choose your map base size just fine without issues.");

                            x32Button.Text = "32x32 area with normal border with Island background, doesn\'t require OpenPlanet to work (recommended)";
                            x45Button.Text = "45x45 area with small border with Island background, requires OpenPlanet to work (also recommended)";
                        }
                        else
                        {
                            lBlockRange.BackColor = Color.Yellow;
                            ttBlockRange.SetToolTip(lBlockRange, "This map is bigger than the default Stadium64x64 base! Choose the map base size wisely.");

                            x32Button.Text = "32x32 area with normal border with Island background, doesn\'t require OpenPlanet to work";
                            x45Button.Text = "45x45 area with small border with Island background, requires OpenPlanet to work (recommended)";
                        }

                        lBlockRange.Text = "Block range: " + blockRange;

                        pbThumbnail.Image = map.Thumbnail;
                    }
                }
            }
        }

        bool converting = false;

        void ConvertMaps(IEnumerable<GameBox<CGameCtnChallenge>> maps)
        {
            if (converting) return;

            MapSize size = MapSize.X45WithSmallBorder;

            if (x31Button.Checked)
                size = MapSize.X31WithSmallBorder;
            else if (x32Button.Checked)
                size = MapSize.X32WithBigBorder;
            else if (x45Button.Checked)
                size = MapSize.X45WithSmallBorder;

            var cutoff = cbCutoff.Checked;
            var ignoreMediaTracker = cbIgnoreMediaTracker.Checked;

            converting = true;

            Enabled = false;

            Task.Run(() =>
            {
                try
                {
                    foreach (var gbx in maps)
                    {
                        var map = gbx.MainNode;

                        IslandConverter.ConvertToTM2Island(gbx, null, gbx.FileName, "output", size, blockRange, minCoord.GetValueOrDefault(), random, cutoff, ignoreMediaTracker);
                    }
                }
                catch (Exception e)
                {
                    Log.Write(e.ToString());
                    MessageBox.Show(e.ToString(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }).ContinueWith(x =>
            {
                converting = false;

                Invoke(new Action(() =>
                {
                    Enabled = true;

                    Invoke(new Action(() =>
                    {
                        foreach (var map in maps)
                        {
                            var mapUid = map.MainNode.MapUid;
                            Maps.Remove(mapUid);
                            foreach(ListViewItem item in lvMaps.Items)
                            {
                                if(item.Name == mapUid)
                                {
                                    lvMaps.Items.Remove(item);
                                    break;
                                }
                            }
                        }
                        UpdateSelectedMap();
                    }));

                    Process.Start("explorer.exe", Environment.CurrentDirectory + "\\output\\");
                }));
            });
        }

        private void bConvertAll_Click(object sender, EventArgs e)
        {
            ConvertMaps(Maps.Values);
        }

        private void bOpenFolder_Click(object sender, EventArgs e)
        {
            if(File.Exists("Config.yaml"))
            {
                Dictionary<string, string> config;

                using (var r = new StreamReader("Config.yaml"))
                {
                    Deserializer de = new Deserializer();
                    config = de.Deserialize<Dictionary<string, string>>(r);
                }

                if(config.TryGetValue("ManiaPlanetMapsFolder", out string value))
                {
                    if(value == null)
                        value = SetOpenFolder();
                    
                    if(value != null)
                        Process.Start("explorer.exe", value);
                }
            }
            else
            {
                SetOpenFolder();
            }
        }

        string SetOpenFolder()
        {
            string folderName = null;

            MessageBox.Show("Please select your ManiaPlanet Maps folder.", "Select Maps folder", MessageBoxButtons.OK, MessageBoxIcon.Information);

            using (var ofd = new CommonOpenFileDialog())
            {
                ofd.IsFolderPicker = true;

                if (ofd.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    folderName = ofd.FileName;

                    Serializer s = new Serializer();
                    var output = s.Serialize(new Dictionary<string, string>(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("ManiaPlanetMapsFolder", folderName) }));
                    File.WriteAllText("Config.yaml", output);
                }
            }

            return folderName;
        }

        private void AddAMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "GBX (*.Gbx)|*.Gbx|All files (*.*)|*.*";
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (lvMaps.View == View.Tile)
                {
                    lvMaps.Items.Clear();
                    lvMaps.View = View.LargeIcon;
                }

                LoadMaps(ofd.FileNames);
            }
        }

        private void bConvertSelected_Click(object sender, EventArgs e)
        {
            List<GameBox<CGameCtnChallenge>> maps = new List<GameBox<CGameCtnChallenge>>();

            foreach (ListViewItem item in lvMaps.SelectedItems)
            {
                if (Maps.TryGetValue(item.Name, out GameBox<CGameCtnChallenge> gbx))
                    maps.Add(gbx);
            }

            ConvertMaps(maps);
        }
    }
}
