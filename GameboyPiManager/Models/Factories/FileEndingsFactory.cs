using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyPiManager.Models.Factories
{
    public abstract class FileExtensionsFactory
    {
        public abstract FileExtensions GetFileExtensions();
    }

    public abstract class FileExtensions
    {
        public List<String> Extensions { get; protected set; }
    }

}
