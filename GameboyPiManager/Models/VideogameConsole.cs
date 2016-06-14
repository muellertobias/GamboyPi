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

        public VideogameConsole(String pathOrName, bool isName = false)
        {
            if (isName)
                this.Name = pathOrName;
            else
                this.Name = ExtractName(pathOrName);

            FileExtensions = FileExtensionsFactory.Instance.GetFileExtensions(Name);
            this.VideogameList = new List<Videogame>();
            loadVideogames();
        }

        private void loadVideogames()
        {
            try
            {
                //this.VideogameList.Clear();

                var files = SambaConnection.Instance.GetFiles(Name);
                foreach (String file in files)
                {
                    if (!VideogameList.Exists(v => v.Name.Equals(ExtractName(file))))
                    {
                        this.VideogameList.Add(new Videogame(file));
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public void Reload()
        {
            loadVideogames();
        }

        public static String ExtractName(String path)
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

        public bool UploadFile(string filepath)
        {
            try
            {
                SambaConnection.Instance.CopyFile(Name, filepath);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj == this)
                return true;
            VideogameConsole that = (VideogameConsole) obj;
            return this.Name == that.Name
                && this.VideogameList.Equals(that.VideogameList);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode() + this.FileExtensions.GetHashCode() + this.VideogameList.GetHashCode();
        }
    }
}
