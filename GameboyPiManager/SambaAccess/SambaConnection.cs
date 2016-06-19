using GameboyPiManager.Models.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPFUtilities.Annotations;
using WPFUtilities.Logging;

namespace GameboyPiManager.Models.SambaAccess
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

        public bool wasLastDownloadSuccessful { get; protected set; }
        public bool wasLastUploadSuccessful { get; protected set; }

        private SambaConnection()
        {
            PermanentConnectionCheckTimer = new Timer(x => OnConnectionChanged(checkConnection()), null, 0, 1000);
        }

        #endregion
        private readonly string dirSeparator = "\\";
        private IDevice device;
        private bool isConnected;
        private Timer PermanentConnectionCheckTimer;
        private SambaAccessStringBuilder accessBuilder;
        private ILogger Logger;

        public event ConnectionHandler ConnectionChanged;

        public void SetDevice(IDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException();
            }
            this.device = device;
            this.accessBuilder = new SambaAccessStringBuilder(device.Name);
            this.Logger = new FileLogger();
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
                string accessKey = accessBuilder.Build(Access.Roms);
                return Directory.EnumerateDirectories(accessKey);
            }
            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> GetFiles(string directoryName)
        {
            if (isConnected)
            {
                string path = accessBuilder.Build(Access.Roms, directoryName);
                return Directory.EnumerateFiles(path);
            }
            return Enumerable.Empty<string>();
        }

        public void UploadFile(string destination, string filepath)
        {
            string pathToConsole = accessBuilder.Build(Access.Roms, destination);
            File.Copy(filepath, getFilepath(pathToConsole, cutPathFromFilename(filepath)));
        }

        public void DeleteFile(string destination, string filename)
        {
            string pathToConsole = accessBuilder.Build(Access.Roms, destination);
            File.Delete(getFilepath(pathToConsole, filename));
        }

        public void DownloadFile(string source, string destination)
        {
            File.Copy(source, destination);
        }

        public void DownloadAllFiles(string source, string destination)
        {
            string pathToConsole = accessBuilder.Build(Access.Roms, destination);
            IEnumerable<string> files = Directory.EnumerateFiles(pathToConsole);
            files.ToList().ForEach(file => File.Copy(file, getFilepath(destination, cutPathFromFilename(file))));
        }
        
        public bool UploadDirectory(string source)
        {
            bool successful = true;
            successful = copyFiles(source + accessBuilder.Build(Access.Splashscreens, true), accessBuilder.Build(Access.Root));
            successful = copyFiles(source + accessBuilder.Build(Access.Bios, true), accessBuilder.Build(Access.Root));
            successful = copyFiles(source + accessBuilder.Build(Access.Configs, true), accessBuilder.Build(Access.Root));
            successful = copyFiles(source + accessBuilder.Build(Access.Roms, true), accessBuilder.Build(Access.Root));
            return successful;
        }

        public bool DownloadDirectory(string destination)
        {
            bool successful = true;
            successful = copyFiles(accessBuilder.Build(Access.Bios), destination);
            successful = copyFiles(accessBuilder.Build(Access.Configs), destination);
            successful = copyFiles(accessBuilder.Build(Access.Roms), destination);
            successful = copyFiles(accessBuilder.Build(Access.Splashscreens), destination);
            return successful;
        }

        private bool copyFiles(string source, string destinationRoot)
        {
            bool successful = true;
            try {
                string destinationDirectory = String.Format("{0}{1}", destinationRoot, SambaAccessStringBuilder.SplitDirectoryNameFromPath(source));

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                foreach (string sourceFilename in Directory.EnumerateFiles(source))
                {
                    string destinationFilename = String.Format("{0}{1}", destinationDirectory, SambaAccessStringBuilder.SplitDirectoryNameFromPath(sourceFilename));
                    try
                    {
                        File.Copy(sourceFilename, destinationFilename, true);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        successful = false;
                        Logger.Log(String.Format("Can not copy file {0} to {1}! Not authorized!\n{2}", sourceFilename, destinationFilename, ex.Message));
                    }
                }
                var sourceSubDirectories = Directory.EnumerateDirectories(source);
                foreach (string subDirectory in sourceSubDirectories)
                {
                    bool downloadSuccessful = copyFiles(subDirectory, destinationDirectory);
                    if (!downloadSuccessful)
                        successful = false;
                }
            }
            catch (Exception ex)
            {
                successful = false;
                Logger.Log(String.Format("Can not access to {0}! \n{1}", source, ex.Message));
            }
            return successful;
        }
    }

    interface IAccessStringBuilder
    {
        string Build(Access access, bool offline);
        string Build(Access access, string path, bool offline = false);
        string Build(Access access, string path, string filename, bool offline = false);
    }

    class SambaAccessStringBuilder : IAccessStringBuilder
    {
        private string deviceName;
        private static Hashtable cache = new Hashtable();
        public static readonly string DIR_SEPARATOR = "\\";

        public SambaAccessStringBuilder(string deviceName)
        {
            this.deviceName = deviceName;
        }

        public string Build(Access access, bool offline = false)
        {
            int hashCode = access.GetHashCode() + offline.GetHashCode();
            if (cache.Contains(hashCode))
            {
                return cache[hashCode] as string;
            }
            else
            {
                string directory = string.Empty;
                if (!offline)
                {
                    directory = String.Format("{0}{1}", parse(Access.Root), deviceName);
                }
                if (access != Access.Root)
                {
                    directory = String.Format("{0}{1}", directory, parse(access));
                }
                cache.Add(hashCode, directory);
                return directory;
            }
        }

        public string Build(Access access, string path, bool offline = false)
        {
            int hashCode = access.GetHashCode() + path.GetHashCode();
            if (cache.Contains(hashCode))
            {
                return cache[hashCode] as string;
            }
            else
            {
                string directory = String.Format("{0}{1}{2}", Build(access, offline), DIR_SEPARATOR, path);
                cache.Add(hashCode, directory);
                return directory;
            }
        }

        public string Build(Access access, string path, string filename, bool offline = false)
        {
            filename = SplitFilenameFromPath(filename);
            path = SplitFilenameFromPath(path);
            int hashCode = access.GetHashCode() + path.GetHashCode() + filename.GetHashCode();
            if (cache.Contains(hashCode))
            {
                return cache[hashCode] as string;
            }
            else
            {
                string directory = String.Format("{0}{1}{2}", Build(access, path, offline), DIR_SEPARATOR, filename);
                cache.Add(hashCode, directory);
                return directory;
            }
        }

        public static string SplitFilenameFromPath(string filename)
        {
            return filename.Split(DIR_SEPARATOR.ToCharArray()).Last();
        }

        public static string SplitDirectoryNameFromPath(string path)
        {
            return String.Format("{0}{1}", DIR_SEPARATOR, SplitFilenameFromPath(path));
        }

        private string parse(Access value)
        {
            string accessStringValue = StringValueAttribute.GetStringValue(value);
            return ConfigurationManager.AppSettings.Get(accessStringValue);
        }
    }

    enum Access
    {
        [StringValue("SambaAccess")]
        Root,
        [StringValue("ROMsDir")]
        Roms,
        [StringValue("ConfigsDir")]
        Configs,
        [StringValue("BIOSDir")]
        Bios,
        [StringValue("SplashscreeensDir")]
        Splashscreens
    }
}
