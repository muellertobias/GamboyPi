using GameboyPiManager.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyPiManager.Models.Factories
{
    public class SambaAccessKeyFactory : IAccessKeyFactory
    {
        #region Singleton
        private static IAccessKeyFactory instance;
        private static object padLock = new object();
        public static IAccessKeyFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padLock)
                    {
                        if (instance == null)
                        {
                            instance = new SambaAccessKeyFactory();
                        }
                    }
                }
                return instance;
            }
        }

        private SambaAccessKeyFactory() { }
        #endregion

        private IGameboy gameboy;
        public string GetAccessKey()
        {
            return ConfigurationManager.AppSettings.Get("SambaAccess") + gameboy.Name + ConfigurationManager.AppSettings.Get("ROMsDir");
        }

        public string GetAccessKey(string path)
        {
            return GetAccessKey() + "\\" + path;
        }
    }
}
