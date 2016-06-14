using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Shapes;
using WPFUtilities.MVVM;

namespace GameboyPiManager.ViewModels
{
    public class MenuItemViewModel : ViewModel
    {
        public ObservableCollection<MenuItemViewModel> Childrens { get; private set; }

        private string menuHeader;
        public string MenuHeader
        {
            get { return menuHeader; }
            set
            {
                if (menuHeader != value)
                {
                    menuHeader = value;
                    OnPropertyChanged(() => MenuHeader);
                }
            }
        }

        private Path icon;
        public Path Icon
        {
            get { return icon; }
            set
            {
                if (Icon != value)
                {
                    icon = value;
                    OnPropertyChanged(() => Icon);
                }
            }
        }

        private ICommand menuCommand;
        public ICommand MenuCommand
        {
            get { return menuCommand; }
            set
            {
                if (menuCommand != value)
                {
                    menuCommand = value;
                    OnPropertyChanged(() => MenuCommand);
                }
            }
        }

        public MenuItemViewModel(string menuHeader, Path icon)
        {
            this.menuHeader = menuHeader;
            this.icon = icon;
            this.Childrens = new ObservableCollection<MenuItemViewModel>();
        }

        public MenuItemViewModel(string menuHeader, Path icon, ICommand menuCommand) 
            : this(menuHeader, icon)
        {
            this.menuCommand = menuCommand;
        }
    }
}
