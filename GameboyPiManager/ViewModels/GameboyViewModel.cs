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
        public VideogameConsoleViewModel SelectedVideogameConsole
        {
            get;
            set;
        }

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

        private ObservableCollection<VideogameConsoleViewModel> searchedConsolesVMs;
        public ObservableCollection<VideogameConsoleViewModel> SearchedConsolesVMs
        {
            get { return searchedConsolesVMs; }
            private set
            {
                if (SearchedConsolesVMs != value)
                {
                    searchedConsolesVMs = value;
                    this.OnPropertyChanged("SearchedConsolesVMs");
                }
            }
        }

        public GameboyViewModel(Gameboy model) 
            : base(model)
        {
            ConsolesVMs = new ObservableCollection<VideogameConsoleViewModel>();
            foreach (VideogameConsole consoleModel in Model.Consoles.Values)
            {
                ConsolesVMs.Add(new VideogameConsoleViewModel(consoleModel));
            }
        }

        public void UploadFiles(string[] filepaths)
        {
            foreach (String filepath in filepaths)
            {
                List<string> consoleNames = Model.FindConsole(filepath);
                SearchedConsolesVMs = new ObservableCollection<VideogameConsoleViewModel>(ConsolesVMs.Where(c => consoleNames.Contains(c.Name)));
            }
        }
    }
}
