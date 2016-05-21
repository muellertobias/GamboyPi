using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;


namespace GameboyPiManager.Models
{
    public class VideogameConsole
    {
        public String Name { get; private set; }

        public List<Videogame> VideogameList { get; private set; }

        public VideogameConsole(String path)
        {
            this.Name = path; // muss noch korrigiert werden, da kompletter Pfad übergeben wird
            this.VideogameList = new List<Videogame>();
            loadVideogames();
        }

        private void loadVideogames()
        {
            var files = Directory.EnumerateFiles(this.Name);
            foreach (String str in files)
            {
                //Console.Out.WriteLineAsync(str);
                this.VideogameList.Add(new Videogame(str));
            }
        }

        public void Add(Videogame game)
        {
            VideogameList.Add(game);
        }

        public void Remove(Videogame game)
        {
            VideogameList.Remove(game);
        }
    }

    public class Videogame
    {
        public String Name { get; set; }

        public Videogame(String Name)
        {
            this.Name = Name;
        }
    }
}
