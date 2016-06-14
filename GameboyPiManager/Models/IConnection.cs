using GameboyPiManager.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyPiManager.Models
{
    public delegate void ConnectionHandler(bool isConnected);

    public interface IConnection
    {
        event ConnectionHandler ConnectionChanged;

        void SetDevice(IDevice device);
        //void CheckConnection(Func<object, bool> p);
        IEnumerable<string> GetDirectories();
        IEnumerable<string> GetFiles(string directoryName);
        void UploadFile(string destination, string filepath);
        void DeleteFile(string destination, string filename);
        void DownloadAllFiles(string source, string destination);
        void UploadDirectory(string source);
    }
}
