using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyPiManager.Models
{
    public class Gameboy
    {
        public List<VideogameConsole> Consoles { get; private set; }

        public String Name { get; private set; }
        private String gameboyAccessStr;

        public Gameboy(String Name)
        {
            this.Name = Name;
            gameboyAccessStr = ConfigurationManager.AppSettings.Get("SambaAccess") + Name + ConfigurationManager.AppSettings.Get("ROMsDir");
            this.Consoles = new List<VideogameConsole>();
            loadVideogameConsoles();
        }

        private void loadVideogameConsoles()
        {
            try
            {
                var dirs = Directory.EnumerateDirectories(gameboyAccessStr);
                foreach (String str in dirs)
                {
                    //Console.Out.WriteLine(str);
                    this.Consoles.Add(new VideogameConsole(str));
                }
            }
            catch (IOException)
            {

            }
            
        }
    }
}
