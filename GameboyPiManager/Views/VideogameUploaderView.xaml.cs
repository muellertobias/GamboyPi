using GameboyPiManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace GameboyPiManager.Views
{
    /// <summary>
    /// Interaction logic for VideogameUploaderView.xaml
    /// </summary>
    public partial class VideogameUploaderView : UserControl
    {
        public VideogameUploaderView()
        {
            InitializeComponent();
        }

        private void DropzoneDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var paths = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                var viewModel = DataContext as VideogameUploaderViewModel;
                viewModel.UploadFiles(paths);
            }
        }

        private void DropzoneDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }
    }
}
