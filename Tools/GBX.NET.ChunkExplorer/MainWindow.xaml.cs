using GBX.NET.ChunkExplorer.Exceptions;
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
        private Stream? fileStream;
        private Stream? currentStream;
        private ObservableCollection<MainNodeModel> mainNodes = new();
        private ChunkSet? chunks;

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

        public ChunkSet? Chunks
        {
            get => chunks;
            set
            {
                if (value != chunks)
                {
                    chunks = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Stream? FileStream
        {
            get => fileStream;
            set
            {
                if (value != fileStream)
                {
                    fileStream = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Stream? CurrentStream
        {
            get => currentStream;
            set
            {
                if (value != currentStream)
                {
                    currentStream = value;
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

        private async void ButtonOpenFile_Click(object sender, RoutedEventArgs eventArgs)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "GBX file|*.Gbx|All files|*.*"
            };

            if (ofd.ShowDialog() == true)
            {
                try
                {
                    var nodeModel = await Task.Run(() => ParseNode(ofd.FileName));
                    MainNodes.Add(nodeModel);
                }
                catch (Exception e)
                {
                    _ = MessageBox.Show(e.ToString(), "Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private MainNodeModel ParseNode(string fileName)
        {
            fileStream = File.OpenRead(fileName);

            var node = GameBox.ParseNode(fileStream);

            if (node is null)
                throw new NodeIsNullException();

            node.GBX!.FileName = fileName;
            return new MainNodeModel(node);
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is not NodeModel nodeModel)
            {
                CurrentStream = null;
                return;
            }

            Chunks = nodeModel.Chunks;
        }

        private void ComboBoxChunks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentStream = e.AddedItems.Count == 0 || e.AddedItems[0] is not Chunk chunk
                ? default(Stream?)
                : new MemoryStream(chunk.Debugger.RawData!);

            _ = TreeViewNodes.Focus();
        }
    }
}
