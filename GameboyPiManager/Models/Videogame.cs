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

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj == this)
                return true;
            Videogame that = (Videogame)obj;
            return this.Name == that.Name;
        }

        public override int GetHashCode()
        {
            int hc = 12;
            return hc + this.Name.GetHashCode();
        }
    }
}
