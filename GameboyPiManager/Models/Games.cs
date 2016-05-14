using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyPiManager.Models
{
    public class VideogameConsole
    {
        public String Name { get; private set; }

        public List<Videogame> VideogameList { get; private set; }

        public VideogameConsole(String Name)
        {
            this.Name = Name;
            this.VideogameList = new List<Videogame>();
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
