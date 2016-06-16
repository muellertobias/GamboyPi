using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameboyPiManager.Models.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameboyPiManager.Models.Factories.Tests
{
    [TestClass()]
    public class FileExtensionsFactoryTests
    {
        [TestMethod()]
        public void GetFileExtensionsTestStringEmpty()
        {
            IFileExtensionsFactory fileExtensionsFactory = FileExtensionsFactory.Instance;
            FileExtensions fileExtension = fileExtensionsFactory.GetFileExtensions(string.Empty);
            Assert.AreEqual(fileExtension.Count, new FileExtensions().Count);
        }
        [TestMethod()]
        public void GetFileExtensionsTest()
        {
            IFileExtensionsFactory fileExtensionsFactory = FileExtensionsFactory.Instance;
            FileExtensions fileExtension = new FileExtensions();
            fileExtension.Add("smc");
            fileExtension.Add("sfc");
            fileExtension.Add("fig");
            fileExtension.Add("swc");
            Assert.AreEqual(fileExtensionsFactory.GetFileExtensions("snes"), fileExtension);
        }
    }
}