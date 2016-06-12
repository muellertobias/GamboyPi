using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtilities.MVVM;
using GameboyPiManager.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading;
using System.Windows.Threading;
using System.Collections.Specialized;

namespace GameboyPiManager.ViewModels
{
    public class GameboyViewModel : ViewModel<Gameboy>
    {
        public MenuItemViewModel MenuVM { get; private set; }

        public ICommand UploadCmd { get; private set; }
        private string filepath;
        private string statusMessage;
        public string StatusMessage
        {
            get { return statusMessage; }
            set
            {
                if (StatusMessage != value)
                {
                    statusMessage = value;
                    this.OnPropertyChanged("StatusMessage");
                }
            }
        }

        public bool IsConnected
        {
            get { return Model.IsConnected; }
            set
            {
                if (IsConnected != value)
                {
                    Model.IsConnected = value;
                    this.OnPropertyChanged(() => IsConnected);
                }
            }
        }

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

        #region Contructor
        public GameboyViewModel(Gameboy model) 
            : base(model)
        {
            UploadCmd = new Command(p => upload());
            ConsolesVMs = new ObservableCollection<VideogameConsoleViewModel>();
            foreach (VideogameConsole consoleModel in Model.Consoles.Values)
            {
                ConsolesVMs.Add(new VideogameConsoleViewModel(consoleModel));
            }
            model.SetOnConnectionChanged(SetIsConnection);

            buildMenu();
            
        }

        private void buildMenu()
        {
            MenuVM = new MenuItemViewModel("_System");
            MenuVM.Childrens.Add(new MenuItemViewModel("Erstelle Sicherung", new Command(p => Model.GenerateCompleteSave())));
            MenuVM.Childrens.Add(new MenuItemViewModel("Lade Sicherung", new Command(p => Model.LoadCompleteSave())));
        }
        #endregion

        private void SetIsConnection(bool b)
        {
            IsConnected = b;
        }

        public void UploadFiles(string[] filepaths)
        {
            foreach (String filepath in filepaths)
            {
                List<string> consoleNames = Model.FindConsole(filepath);
                SearchedConsolesVMs = new ObservableCollection<VideogameConsoleViewModel>(ConsolesVMs.Where(c => consoleNames.Contains(c.Name)));
                this.filepath = filepath;
            }
        }

        private void upload()
        {
            if (SelectedVideogameConsole != null)
            {
                IsConnected = SelectedVideogameConsole.Model.UploadFile(filepath);
                SearchedConsolesVMs = null;
                SelectedVideogameConsole = null;
            }
        }
    }
}
