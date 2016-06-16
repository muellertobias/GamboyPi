using GameboyPiManager.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameboyPiManager.Models
{
    public class SambaConnection : IConnection
    { 
        #region Singleton
        private static IConnection instance;
        private static object padLock = new object();
        public static IConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padLock)
                    {
                        if (instance == null)
                        {
                            instance = new SambaConnection();
                        }
                    }
                }
                return instance;
            }
        }

        private SambaConnection()
        {
            PermanentConnectionCheckTimer = new Timer(x => OnConnectionChanged(checkConnection()), null, 0, 1000);
        }

        #endregion
        private readonly string dirSeparator = "\\";
        private IDevice device;
        private bool isConnected;
        private Timer PermanentConnectionCheckTimer;

        public event ConnectionHandler ConnectionChanged;

        public void SetDevice(IDevice gameboy)
        {
            if (gameboy == null)
            {
                throw new ArgumentNullException();
            }
            this.device = gameboy;
            this.device.IsConnected = this.isConnected = checkConnection();
        }

        protected virtual void OnConnectionChanged(bool isConnected)
        {
            if (this.isConnected != isConnected)
            {
                this.isConnected = isConnected;
                ConnectionChanged?.Invoke(isConnected);
            }
        }

        private string GetAccessKey()
        {
            if (device == null)
            {
                throw new InvalidOperationException("Gerät wurde nicht spezifiziert. Es ist kein Zugriff möglich!");
            }
            return string.Format("{0}{1}{2}", ConfigurationManager.AppSettings.Get("SambaAccess"), device.Name, ConfigurationManager.AppSettings.Get("ROMsDir"));
        }

        private string GetAccessKey(string path)
        {
            return GetAccessKey() + dirSeparator + path;
        }

        private bool checkConnection()
        {
            bool isConnected = false;
            try
            {
                IPHostEntry host;
                host = Dns.GetHostEntry(device.Name);
                PingReply replay = new Ping().Send(host.HostName, 100);
                if (replay.Status == IPStatus.Success)
                {
                    isConnected = true;
                }
            }
            catch { }
            return isConnected;
        }

        private void uploadFiles(string source, ref string sourceRoot)
        {
            foreach (string sourceFile in Directory.EnumerateFiles(source))
            {
                string destinationFile = GetAccessKey() + sourceFile.Remove(0, sourceRoot.Length);
                Console.WriteLine(destinationFile);
                File.Copy(sourceFile, destinationFile);
            }
            var sourceDirectories = Directory.EnumerateDirectories(source);
            foreach (string dir in sourceDirectories)
            {
                uploadFiles(dir, ref sourceRoot);
            }
        }

        private string cutPathFromFilename(string completeFilepath)
        {
            return completeFilepath.Split(dirSeparator.ToCharArray()).Last();
        }

        private string getFilepath(string path, string filename)
        {
            return string.Format("{0}{1}{2}", path, dirSeparator, filename);
        }

        public IEnumerable<string> GetDirectories()
        {
            if (isConnected)
            {
                string accessKey = GetAccessKey();
                return Directory.EnumerateDirectories(accessKey);
            }
            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> GetFiles(string directoryName)
        {
            if (isConnected)
            {
                string path = GetAccessKey(directoryName);
                return Directory.EnumerateFiles(path);
            }
            return Enumerable.Empty<string>();
        }

        public void UploadFile(string destination, string filepath)
        {
            string pathToConsole = GetAccessKey(destination);
            File.Copy(filepath, getFilepath(pathToConsole, cutPathFromFilename(filepath)));
        }

        public void DeleteFile(string destination, string filename)
        {
            string pathToConsole = GetAccessKey(destination);
            File.Delete(getFilepath(pathToConsole, filename));
        }

        public void DownloadFile(string source, string destination)
        {
            File.Copy(source, destination);
        }

        public void DownloadAllFiles(string source, string destination)
        {
            string pathToConsole = GetAccessKey(source);
            IEnumerable<string> files = Directory.EnumerateFiles(pathToConsole);
            files.ToList().ForEach(file => File.Copy(file, getFilepath(destination, cutPathFromFilename(file))));
        }
        
        public void UploadDirectory(string source)
        {
            // Argument wird 2 mal übegeben, da das 2. Argument die Wurzel (also die ursprüngliche Source ist) 
            // und das 1. Argument sich durch die Rekursion von uploadFiles ändert.
            uploadFiles(source, ref source);
        }
    }
}
