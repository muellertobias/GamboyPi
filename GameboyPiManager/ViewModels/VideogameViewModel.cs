using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtilities.MVVM;
using GameboyPiManager.Models;
using System.Windows.Input;

namespace GameboyPiManager.ViewModels
{
    public class VideogameViewModel : ViewModel<Videogame>
    {
        public String Name
        {
            get { return Model.Name; }
            set
            {
                if (Name != value)
                {
                    Model.Name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        public ICommand RemoveCmd { get; private set; }

        private VideogameConsoleViewModel parent;

        public VideogameViewModel(Videogame model, VideogameConsoleViewModel parent) 
            : base(model)
        {
            this.parent = parent;
            this.RemoveCmd = new Command(p => remove());
        }

        private void remove()
        {
            this.parent.RemoveVideogame(this);
        }
    }
}
