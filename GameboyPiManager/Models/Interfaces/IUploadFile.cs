using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyPiManager.Models.Interfaces
{
    public interface IUploadFile
    {
        bool UploadFile(string path);
    }
}
