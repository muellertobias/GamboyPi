using GameboyPiManager.Models;
using GameboyPiManager.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUtilities.MVVM;

namespace GameboyPiManager.ViewModels
{
    public delegate void ReloadViewModelHandler();

    public class VideogameUploaderViewModel : ViewModel<Gameboy>
    {
        public event ReloadViewModelHandler DoReload;

        private string Filepath;
        private IGameboyMain gameboyMainVM;

        private ObservableCollection<VideogameConsoleViewModel> searchedConsolesVMs;
        public ObservableCollection<VideogameConsoleViewModel> SearchedConsolesVMs
        {
            get { return searchedConsolesVMs; }
            set
            {
                if (SearchedConsolesVMs != value)
                {
                    searchedConsolesVMs = value;
                    this.OnPropertyChanged("SearchedConsolesVMs");
                }
            }
        }

        public VideogameConsoleViewModel SelectedVideogameConsole { get; set; }

        public ICommand UploadCmd { get; private set; }
        public ICommand CancelCmd { get; private set; }

        public VideogameUploaderViewModel(Gameboy model, IGameboyMain main) 
            : base(model)
        {
            gameboyMainVM = main;
            UploadCmd = new Command(p => upload());
            CancelCmd = new Command(p => cancelUpload());

            SearchedConsolesVMs = new ObservableCollection<VideogameConsoleViewModel>();
        }

        public void UploadFiles(string[] filepaths)
        {
            foreach (String filepath in filepaths)
            {
                List<string> consoleNames = Model.FindConsole(filepath);
                SearchedConsolesVMs = new ObservableCollection<VideogameConsoleViewModel>(gameboyMainVM.ConsolesVMs.Where(c => consoleNames.Contains(c.Name)));
                this.Filepath = filepath;
            }
        }

        private void upload()
        {
            if (SelectedVideogameConsole != null)
            {
                gameboyMainVM.IsConnected = SelectedVideogameConsole.Model.UploadFile(Filepath);
                cancelUpload();
                DoReload?.Invoke();
            }
        }

        private void cancelUpload()
        {
            SearchedConsolesVMs.Clear();
            SelectedVideogameConsole = null;
            Filepath = string.Empty;
        }
    }
}
