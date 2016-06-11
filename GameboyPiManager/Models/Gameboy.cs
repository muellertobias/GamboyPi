using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameboyPiManager.Models.Interfaces;

namespace GameboyPiManager.Models
{
    public class Gameboy : IGameboy, IUploadFile
    {
        public List<VideogameConsole> Consoles { get; private set; }

        public string Name { get; set; }
        private string gameboyAccessStr;

        #region Constructor
        public Gameboy(String Name)
        {
            this.Name = Name;
            gameboyAccessStr = createAccessString();
            this.Consoles = new List<VideogameConsole>();
            load();
        }
        #endregion

        private string createAccessString()
        {
            return ConfigurationManager.AppSettings.Get("SambaAccess") + Name + ConfigurationManager.AppSettings.Get("ROMsDir");
        }

        private void load()
        {
            try
            {
                loadVideogameConsoles();
            }
            catch (IOException)
            {
                
            }
        }

        private void loadVideogameConsoles()
        {
            var directories = Directory.EnumerateDirectories(gameboyAccessStr);
            foreach (String directory in directories)
            {
                this.Consoles.Add(new VideogameConsole(directory));
            }
        }

        public void UploadFile(string path)
        {
            foreach (VideogameConsole console in Consoles)
            {
                if (console.checkFile(path))
                {
                    console.UploadFile(path);
                    return;
                }
            }
        }
    }
}
