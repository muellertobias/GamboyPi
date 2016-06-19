using GameboyPiManager.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyPiManager.Models.SambaAccess
{
    public delegate void ConnectionHandler(bool isConnected);

    public interface IConnection
    {
        event ConnectionHandler ConnectionChanged;

        void SetDevice(IDevice device);
        IEnumerable<string> GetDirectories();
        IEnumerable<string> GetFiles(string directoryName);
        void UploadFile(string destination, string filepath);
        void DeleteFile(string destination, string filename);
        bool UploadDirectory(string source);
        bool DownloadDirectory(string destination);
    }
}
