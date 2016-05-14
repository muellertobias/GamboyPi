using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtilities.MVVM;
using GameboyPiManager.Models;

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

        public VideogameViewModel(Videogame model) 
            : base(model)
        {
        }
    }
}
