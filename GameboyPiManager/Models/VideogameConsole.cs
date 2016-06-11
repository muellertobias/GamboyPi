using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using GameboyPiManager.Models.Factories;
using GameboyPiManager.Models.Interfaces;

namespace GameboyPiManager.Models
{
    public class VideogameConsole : IUploadFile
    {
        public String Name { get; private set; }
        public List<Videogame> VideogameList { get; private set; }

        private FileExtensions FileExtensions;

        public VideogameConsole(String path)
        {
            this.Name = extractName(path);
            FileExtensions = FileExtensionsFactory.Instance.GetFileExtensions(Name);
            loadVideogames();
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

        public bool checkFile(string path)
        {
            foreach (String fileEnd in FileExtensions)
            {
                if (path.Split('.').Last().Equals(fileEnd))
                {
                    return true;
                }
            }

            return false;
        }

        public void UploadFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}
