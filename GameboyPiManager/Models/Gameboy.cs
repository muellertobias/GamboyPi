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
        private Timer PermanentConnectionCheckTimer;
        public string Name { get; set; }

        public bool IsConnected { get; set; }

        #region Constructor
        public Gameboy(string Name)
        {
            this.Name = Name;
            SambaAccessKeyFactory.Instance.SetDevice(this);
            load();
            PermanentConnectionCheckTimer = new Timer((x) => checkConnection(), null, 1000, 3000);
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
                string accessKey = SambaAccessKeyFactory.Instance.GetAccessKey();
                IEnumerable<string> directories;
                directories = Directory.EnumerateDirectories(accessKey);
                foreach (string directory in directories)
                {
                    var videoGameConsole = new VideogameConsole(directory);

                    this.Consoles.Add(videoGameConsole.Name, videoGameConsole);
                }
                IsConnected = true;
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
                IsConnected = false;
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
                IsConnected = Consoles[consoleName].UploadFile(filepath);
            }
            catch
            {
                return false;
            }
            return true;
        }


        private Action<bool> onConnectionChanged;
        public void SetOnConnectionChanged(Action<bool> action)
        {
            this.onConnectionChanged = action;
        }

        private void checkConnection()
        {
            if (onConnectionChanged != null)
            {
                bool result = SambaAccessKeyFactory.Instance.CheckConnection();
                if (result != IsConnected)
                {
                    onConnectionChanged(result);
                }
            }
        }

        internal void LoadCompleteSave()
        {
            throw new NotImplementedException();
        }

        public bool CheckConnection()
        {
            SambaAccessKeyFactory.Instance.CheckConnection(b => IsConnected = (bool)b);
            return IsConnected;
        }

        internal void GenerateCompleteSave()
        {
            throw new NotImplementedException();
        }
    }
}
