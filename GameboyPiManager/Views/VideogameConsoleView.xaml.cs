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
    /// Interaction logic for VideogameConsoleView.xaml
    /// </summary>
    public partial class VideogameConsoleView : UserControl
    {
        public VideogameConsoleView()
        {
            InitializeComponent();

            var m = new VideogameConsole("Super Nintendo");
            m.VideogameList.Add(new Videogame("The Legend of Zelda"));
            m.VideogameList.Add(new Videogame("Super Mario"));
            this.DataContext = new VideogameConsoleViewModel(m);
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var f = e.Data.GetData(DataFormats.FileDrop, true) as String[];
                
            }
        }

        private void ListBox_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }
    }
}
