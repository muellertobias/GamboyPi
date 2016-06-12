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

        public FileExtensions FileExtensions { get; private set; }

        public VideogameConsole(String path)
        {
            this.Name = extractName(path);
            FileExtensions = FileExtensionsFactory.Instance.GetFileExtensions(Name);
            loadVideogamesIfOnline();
        }

        public VideogameConsole(String name, FileExtensions fileExtensions)
        {
            this.Name = name;
            this.FileExtensions = fileExtensions;
            this.VideogameList = new List<Videogame>();
        }

        private void loadVideogamesIfOnline()
        {
            try
            {
                this.VideogameList = new List<Videogame>();

                string path = SambaAccessKeyFactory.Instance.GetAccessKey(Name);

                var files = Directory.EnumerateFiles(path);
                foreach (String file in files)
                {
                    this.VideogameList.Add(new Videogame(file));
                }
            }
            catch (Exception)
            {

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

        public bool UploadFile(string path)
        {
            try
            {
                string pathToConsole = SambaAccessKeyFactory.Instance.GetAccessKey(Name);
                File.Copy(path, pathToConsole);
                
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
