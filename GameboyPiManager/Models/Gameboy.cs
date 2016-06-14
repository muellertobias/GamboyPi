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
using System.Net.NetworkInformation;
using System.Threading;

namespace GameboyPiManager.Models
{
    public class Gameboy : IDevice
    {
        public Dictionary<string, VideogameConsole> Consoles { get; private set; }
        
        public string Name { get; set; }

        public bool IsConnected { get; set; }

        #region Constructor
        public Gameboy(string Name)
        {
            this.Name = Name;
            SambaConnection.Instance.SetDevice(this);
            load();
        }

        #endregion

        private void load()
        {
            this.Consoles = new Dictionary<string, VideogameConsole>();
            loadVideogameConsoles();
        }

        private void loadVideogameConsoles()
        {
            try
            {

                IEnumerable<string> directories = SambaConnection.Instance.GetDirectories();

                foreach (string directory in directories)
                {
                    string consoleName = VideogameConsole.ExtractName(directory);
                    if (Consoles.Keys.Contains(consoleName))
                    {
                        Consoles[consoleName].Reload();
                    }
                    else
                    {
                        var videoGameConsole = new VideogameConsole(consoleName, true);
                        this.Consoles.Add(videoGameConsole.Name, videoGameConsole);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        
        public void Reload()
        {
            loadVideogameConsoles();
        }

        public List<string> FindConsole(string filepath)
        {
            string fileExtension = filepath.Split('.').Last().ToLower();
            List<string> consoleNames = FileExtensionsFactory.Instance.GetConsoleNames(fileExtension) as List<string>;
            return consoleNames;
        }

        public bool UploadFileToConsole(string filepath, string consoleName)
        {
            bool successfull = Consoles[consoleName].UploadFile(filepath);
            return successfull;
        }


        public void SetOnConnectionChanged(Action<bool> action)
        {
            SambaConnection.Instance.ConnectionChanged += new ConnectionHandler(action);
        }
        
        public void UploadBackup(string source)
        {
            SambaConnection.Instance.UploadDirectory(source);
        }

        public void DownloadBackup(string selectedPath)
        {
            IConnection connection = SambaConnection.Instance;
            foreach (VideogameConsole console in Consoles.Values)
            {
                var destination = selectedPath + "\\" + console.Name;
                connection.DownloadAllFiles(console.Name, destination);
            }
        }
    }
}
