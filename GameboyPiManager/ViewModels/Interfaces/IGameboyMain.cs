using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyPiManager.ViewModels.Interfaces
{
    public interface IGameboyMain
    {
        bool IsConnected { get; set; }
        ObservableCollection<VideogameConsoleViewModel> ConsolesVMs { get; }
    }
}
