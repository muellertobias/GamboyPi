using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GameboyPiManager.Models.Factories
{
    public class FileExtensionsFactory : IFileExtensionsFactory
    {
        private Dictionary<string, FileExtensions> consolesFileExtensionsDictionary;
        private Dictionary<string, List<string>> fileExtensionConsoleDictionary;

        #region Singleton
        private static IFileExtensionsFactory instance;
        private static Object padLock = new Object();
        public static IFileExtensionsFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padLock)
                    {
                        if (instance == null)
                        {
                            instance = new FileExtensionsFactory();
                        }
                    }
                }
                return instance;
            }
        }

        private FileExtensionsFactory()
        {
            consolesFileExtensionsDictionary = new Dictionary<string, FileExtensions>();
            fileExtensionConsoleDictionary = new Dictionary<string, List<string>>();
            load();
        }
        #endregion

        public FileExtensions GetFileExtensions(string consoleName)
        {
            try
            {
                return consolesFileExtensionsDictionary[consoleName];
            }
            catch (Exception)
            {

            }
            return new FileExtensions();
        }

        public IEnumerable<string> GetConsoleNames(string fileExtension)
        {
            if (fileExtension == String.Empty)
            {
                return consolesFileExtensionsDictionary.Keys;
            }

            try
            {
                
                return fileExtensionConsoleDictionary[fileExtension];
            }
            catch (Exception)
            {

            }
            return new List<string>();
        }
        private void load()
        {
            readFileExtensions();
            createFileExtensionsDirectory();
        }

        private void readFileExtensions()
        {
            using (XmlReader reader = XmlReader.Create("FileExtensions.xml"))
            {
                string consoleName = String.Empty;
                FileExtensions fileExtensions = new FileExtensions();

                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (!reader.Name.StartsWith("Ext") && !reader.Name.Equals("systems"))
                            {
                                if (consoleName != String.Empty)
                                {
                                    consolesFileExtensionsDictionary.Add(consoleName, fileExtensions);
                                    fileExtensions = new FileExtensions();
                                }
                                consoleName = reader.Name;
                            }
                            break;
                        case XmlNodeType.Text:
                            fileExtensions.Add(reader.Value);
                            break;
                        case XmlNodeType.EndElement:
                            if (reader.Name.Equals("systems"))
                            {
                                consolesFileExtensionsDictionary.Add(consoleName, fileExtensions);
                            }
                            break;
                    }
                }
            }
        }

        private void createFileExtensionsDirectory()
        {
            foreach (KeyValuePair<string, FileExtensions> pair in consolesFileExtensionsDictionary)
            {
                foreach (string fileExtension in pair.Value)
                {
                    if (!fileExtensionConsoleDictionary.Keys.Contains(fileExtension))
                    {
                        fileExtensionConsoleDictionary.Add(fileExtension, new List<string>());
                    }
                    fileExtensionConsoleDictionary[fileExtension].Add(pair.Key);
                }
            }
        }
    }

    public sealed class FileExtensions : List<string>
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj == this)
                return true;
            FileExtensions that = obj as FileExtensions;
            if (that == null)
                return false;
            var diff1 = this.Except(that);
            if (diff1.Count() != 0)
                return false;
            var diff2 = that.Except(this);
            if (diff2.Count() != 0)
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            int hash = 11;
            foreach (string str in this)
            {
                hash += str.GetHashCode();
            }
            return hash;
        }
    }

    public interface IFileExtensionsFactory
    {
        FileExtensions GetFileExtensions(string consoleName);
        IEnumerable<string> GetConsoleNames(string fileExtension);
    }
}
