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
        private IDevice device;

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

        public void SetDevice(IDevice gameboy)
        {
            if (gameboy == null)
            {
                throw new ArgumentNullException();
            }
            this.device = gameboy;
        }

        public string GetAccessKey()
        {
            if (device == null)
            {
                throw new InvalidOperationException("Gerät wurde nicht spezifiziert. Es ist kein Zugriff möglich!");
            }
            return ConfigurationManager.AppSettings.Get("SambaAccess") + device.Name + ConfigurationManager.AppSettings.Get("ROMsDir");
        }

        public string GetAccessKey(string path)
        {
            return GetAccessKey() + "\\" + path;
        }
    }
}
