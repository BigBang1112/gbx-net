using GBX.NET.ChunkExplorer.Models;
using Mapster;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GBX.NET.ChunkExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Stream? stream;
        private ObservableCollection<MainNodeModel> mainNodes = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<MainNodeModel> MainNodes
        {
            get => mainNodes;
            set
            {
                if (value != mainNodes)
                {
                    mainNodes = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void ButtonOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "GBX file|*.Gbx|All files|*.*"
            };
            
            if (ofd.ShowDialog() == true)
            {
                stream = File.OpenRead(ofd.FileName);

                var nodeModel = await Task.Run(() =>
                {
                    var node = GameBox.ParseNode(stream);
                    node.GBX!.FileName = ofd.FileName;
                    return new MainNodeModel(node);
                });

                MainNodes.Add(nodeModel);
            }
        }
    }
}
