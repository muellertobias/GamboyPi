using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtilities.MVVM;
using GameboyPiManager.Models;

namespace GameboyPiManager.ViewModels
{
    public class GameboyViewModel : ViewModel<Gameboy>
    {
        public GameboyViewModel(Gameboy model) 
            : base(model)
        {
        }
    }
}
