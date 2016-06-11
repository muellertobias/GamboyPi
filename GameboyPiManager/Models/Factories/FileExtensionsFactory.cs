using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyPiManager.Models.Factories
{
    public class FileExtensionsFactory : IFileExtensionsFactory
    {
        private static IFileExtensionsFactory instance;
        private static Object padLock = new Object();
        public static IFileExtensionsFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padLock)
                    {
                        if (instance == null)
                        {
                            instance = new FileExtensionsFactory();
                        }
                    }
                }
                return instance;
            }
        }

        public FileExtensions GetFileExtensions(string consoleName)
        {
            return null;
        }
    }

    public sealed class FileExtensions : List<string>
    {
        
    }

    public interface IFileExtensionsFactory
    {
        FileExtensions GetFileExtensions(string consoleName);
    }
}
