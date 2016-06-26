using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using StlViewer.Model;

namespace StlViewer.Demo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private async void OpenFile()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != true || dialog.FileName == null || !File.Exists(dialog.FileName))
            {
                return;
            }

            var file = dialog.FileName;
            var model = await Task.Run(() =>
            {
                using (var fs = File.Open(file, FileMode.Open))
                {
                    return new StlParser().Parse(fs);
                }
            });
            StlControl.StlModel = model;
        }
    }
}