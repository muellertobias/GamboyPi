using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtilities.MVVM;
using GameboyPiManager.Models;
using System.Collections.ObjectModel;

namespace GameboyPiManager.ViewModels
{
    public class GameboyViewModel : ViewModel<Gameboy>
    {
        private ObservableCollection<VideogameConsoleViewModel> consolesVMs;

        public ObservableCollection<VideogameConsoleViewModel> ConsolesVMs
        {
            get { return consolesVMs; }
            private set
            {
                if (ConsolesVMs != value)
                {
                    consolesVMs = value;
                    this.OnPropertyChanged("ConsolesVMs");
                }
            }
        }

        public GameboyViewModel(Gameboy model) 
            : base(model)
        {
        }
    }
}
