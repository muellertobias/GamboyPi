using GameboyPiManager.Models;
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
            this.DataContext = new GameboyViewModel(model);
            //this.DataContext = new VideogameConsoleViewModel(b.Consoles.First(c => c.Name == "\\\\GAMEBOYPI\\roms\\snes"));
        }

        private void DropzoneDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var paths = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                var viewModel = DataContext as GameboyViewModel;
                viewModel.UploadFiles(paths);
            }
        }

        private void DropzoneDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                e.Effects = DragDropEffects.Move;
            }
        }
    }
}
