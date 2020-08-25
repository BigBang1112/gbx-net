using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace IslandConverter
{
    public partial class ConverterAboutBox : Form
    {
        public ConverterAboutBox()
        {
            InitializeComponent();

            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;

            lProgramName.Text = "Island Converter " + version;
        }

        public void OpenWeb(string url)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }

        private void llSourceCode_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenWeb("https://github.com/BigBang1112/gbx-net/IslandConverter");
        }

        private void llBigBang1112YouTube_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenWeb("https://www.youtube.com/c/BigBang1112tm");
        }

        private void llBigBang1112Discord_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenWeb("https://discord.gg/q9whS3c");
        }

        private void llBigBang1112Twitter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenWeb("https://twitter.com/BigBang1112tm");
        }

        private void llBigBang1112Facebook_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenWeb("https://www.facebook.com/BigBang1112tm");
        }

        private void llTM2IslandForum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenWeb("https://forum.maniaplanet.com/viewtopic.php?f=538&t=44207");
        }

        private void llTM2IslandDiscord_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenWeb("https://discord.gg/JEHzjy2");
        }

        private void llTM2IslandPatreon_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenWeb("https://www.patreon.com/tm2island");
        }

        private void llArkadyYouTube_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenWeb("https://www.youtube.com/channel/UCKv0IcNKxBallhHLuiZQm0A");
        }
    }
}
