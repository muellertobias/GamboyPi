using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyPiManager.Models
{
    public class Gameboy
    {
        public List<VideogameConsole> Consoles { get; private set; }

        public String Name { get; private set; }

        public Gameboy(String Name)
        {
            this.Name = Name;
            this.Consoles = new List<VideogameConsole>();
            loadVideogameConsoles();
        }

        private void loadVideogameConsoles()
        {
            var dirs = Directory.EnumerateDirectories("\\\\" + Name + "\\ROMS");
            foreach (String str in dirs)
            {
                //Console.Out.WriteLine(str);
                this.Consoles.Add(new VideogameConsole(str));
            }
        }
    }
}
