using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyPiManager.Models.Factories
{
    public interface IAccessKeyFactory
    {
        string GetAccessKey();
        string GetAccessKey(string path);
    }
}
