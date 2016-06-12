using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyPiManager.Models
{
    public class Videogame
    {
        public String Name { get; set; }

        public Videogame(String path)
        {
            this.Name = path.Split('\\').Last();
        }
    }
}
