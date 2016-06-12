using GameboyPiManager.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyPiManager.Models.Factories
{
    public interface IAccessKeyFactory
    {
        void SetDevice(IDevice device);
        string GetAccessKey();
        string GetAccessKey(string path);
        bool CheckConnection();
        void CheckConnection(Func<object, bool> p);
    }
}
