using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtilities.MVVM;
using GameboyPiManager.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Windows;

namespace GameboyPiManager.ViewModels
{
    public class VideogameConsoleViewModel : ViewModel<VideogameConsole>
    {
        public String Name
        {
            get { return Model.Name; }
        }

        private String newVideogameName;
        public String NewVideogameName
        {
            get { return newVideogameName; }
            set
            {
                newVideogameName = value;
                OnPropertyChanged("NewVideogameName");
            }
        }

        private ObservableCollection<VideogameViewModel> gamesVMs;

        public ObservableCollection<VideogameViewModel> GamesVMs
        {
            get { return gamesVMs; }
            private set
            {
                if (GamesVMs != value)
                {
                    gamesVMs = value;
                    this.OnPropertyChanged("ConsolesVMs");
                }
            }
        }

        public ICommand AddVideogameCommand { get; private set; }

        public VideogameConsoleViewModel(VideogameConsole model) 
            : base(model)
        {
            gamesVMs = new ObservableCollection<VideogameViewModel>(model.VideogameList.Select(m => new VideogameViewModel(m, this)));
            gamesVMs.CollectionChanged += GamesVMs_CollectionChanged;

            AddVideogameCommand = new Command(consoleVM => addVideogame(consoleVM));
        }

        public void LoadNewVideogames(String[] paths)
        {
            foreach (String p in paths)
            {
                GamesVMs.Add(new VideogameViewModel(new Videogame(p), this));
            }
        }

        private void addVideogame(object consoleVM)
        {

        }


        public void RemoveVideogame(VideogameViewModel videogameViewModel)
        {
            if (GamesVMs.Contains(videogameViewModel))
            {
                MessageBoxResult result = MessageBox.Show("Möchten Sie " + videogameViewModel.Model.Name + " wirklich löschen?", "Spiel löschen?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Model.Remove(videogameViewModel.Model);
                    GamesVMs.Remove(videogameViewModel);
                }
            }
        }

        private void GamesVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (VideogameViewModel vm in e.NewItems)
                    {
                        Model.VideogameList.Add(vm.Model);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (VideogameViewModel vm in e.OldItems)
                    {
                        Model.VideogameList.Remove(vm.Model);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Model.VideogameList.Clear();
                    break;
            }
        }
    }
}
