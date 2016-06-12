using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameboyPiManager.Models.Interfaces;
using GameboyPiManager.Models.Factories;
using System.Xml;

namespace GameboyPiManager.Models
{
    public class Gameboy : IDevice
    {
        public Dictionary<string, VideogameConsole> Consoles { get; private set; }

        public string Name { get; set; }

        #region Constructor
        public Gameboy(String Name)
        {
            this.Name = Name;
            SambaAccessKeyFactory.Instance.SetDevice(this);
            this.Consoles = new Dictionary<string, VideogameConsole>();
            load();
        }
        #endregion

        private void load()
        {
            loadVideogameConsoles();
        }

        private void loadVideogameConsoles()
        {
            try
            {
                string accessKey = SambaAccessKeyFactory.Instance.GetAccessKey();
                IEnumerable<string> directories;
                directories = Directory.EnumerateDirectories(accessKey);
                foreach (string directory in directories)
                {
                    var videoGameConsole = new VideogameConsole(directory);

                    this.Consoles.Add(videoGameConsole.Name, videoGameConsole);
                }
            }
            catch (Exception)
            {
                this.Consoles.Clear();
                IEnumerable<string> consoleNames = FileExtensionsFactory.Instance.GetConsoleNames(String.Empty);
                foreach (string consoleName in consoleNames)
                {
                    var videoGameConsole = new VideogameConsole(consoleName, FileExtensionsFactory.Instance.GetFileExtensions(consoleName));
                    this.Consoles.Add(videoGameConsole.Name, videoGameConsole);
                }
            }
        }
        
        public List<string> FindConsole(string filepath)
        {
            string fileExtension = filepath.Split('.').Last().ToLower();
            List<string> consoleNames = FileExtensionsFactory.Instance.GetConsoleNames(fileExtension) as List<string>;
            return consoleNames;
        }

        public bool UploadFileToConsole(string filepath, string consoleName)
        {
            try
            {
                Consoles[consoleName].UploadFile(filepath);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
