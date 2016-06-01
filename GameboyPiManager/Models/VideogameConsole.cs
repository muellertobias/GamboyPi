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

        private List<String> FileEndings;

        public VideogameConsole(String path)
        {
            this.Name = extractName(path);
            loadFileEnding();
            loadVideogames();
        }

        private void loadFileEnding()
        {
            throw new NotImplementedException();
        }

        private void loadVideogames()
        {
            this.VideogameList = new List<Videogame>();

            var files = Directory.EnumerateFiles(this.Name);
            foreach (String file in files)
            {
                this.VideogameList.Add(new Videogame(file));
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

        private String extractName(String path)
        {
            return path.Split('\\').Last();
        }

        internal bool checkFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}
