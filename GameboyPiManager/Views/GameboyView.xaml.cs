using GameboyPiManager.Models;
using GameboyPiManager.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameboyPiManager.Views
{
    /// <summary>
    /// Interaction logic for GameboyView.xaml
    /// </summary>
    public partial class GameboyView : UserControl
    {
        private Gameboy model;

        public GameboyView()
        {
            InitializeComponent();
            createModel();
            loadDataContext();
        }

        private void createModel()
        {
            model = new Gameboy("GAMEBOYPI");
        }

        private void loadDataContext()
        {
            this.DataContext = new GameboyMainViewModel(model);
        }

        private void ShowAboutPopup(object sender, RoutedEventArgs e)
        {
            
        }

        private void DownloadBackup(object sender, RoutedEventArgs e)
        {
            string selectedPath = getDestinationFolder();

            var viewModel = this.DataContext as GameboyMainViewModel;
            if (viewModel != null && selectedPath != string.Empty)
            {
                viewModel.DownloadBackupCmd.Execute(selectedPath);
            }
        }

        private void UploadBackup(object sender, RoutedEventArgs e)
        {
            string selectedPath = getDestinationFolder();

            var viewModel = this.DataContext as GameboyMainViewModel;
            if (viewModel != null && selectedPath != string.Empty)
            {
                viewModel.UploadBackupCmd.Execute(selectedPath);
            }
        }

        private string getDestinationFolder()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            return dialog.SelectedPath;
        }
    }
}
