using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCare.Tools.LeerlijnenEditor.ViewModels;

namespace DataCare.TestCases
{
    [TestFixture]
    public class MainViewModelTestCase
    {
        [Test]
        public void MainViewModelAuteurPropertyTest()
        {
            MainViewModel mainViewModel = new MainViewModel();
            Assert.NotNull(mainViewModel.Auteur);
        }
        [Test]
        public void MainViewModeAddLeerlijnCommandlPropertyTest()
        {
            MainViewModel mainViewModel = new MainViewModel();
            Assert.NotNull(mainViewModel.AddLeerlijnCommand);
        }
        [Test]
        public void MainViewModelDeleteLeerlijnCommandPropertyTest()
        {
            MainViewModel mainViewModel = new MainViewModel();
            Assert.NotNull(mainViewModel.DeleteLeerlijnCommand);
        }
        [Test]
        public void MainViewModelNewXMLCommandPropertyTest()
        {
            MainViewModel mainViewModel = new MainViewModel();
            Assert.NotNull(mainViewModel.NewXMLCommand);
        }
        [Test]
        public void MainViewModelSaveXMLCommandPropertyTest()
        {
            MainViewModel mainViewModel = new MainViewModel();
            Assert.NotNull(mainViewModel.SaveXMLCommand);
        }

        [Test]
        public void MainViewModelNewXMLMethodTest()
        {
            MainViewModel mainViewModel = new MainViewModel();
            Assert.DoesNotThrow(() => mainViewModel.NewXML());
        }
        [Test]
        public void MainViewModelImportFileMethodTest()
        {
            MainViewModel mainViewModel = new MainViewModel();
            Assert.DoesNotThrow(() => mainViewModel.importFile());
        }
        [Test]
        public void MainViewModelExecuteDeleteLeerlijnNullMethodTest()
        {
            MainViewModel mainViewModel = new MainViewModel();
            Assert.DoesNotThrow(() => mainViewModel.ExecuteDeleteLeerlijn(null));
        }
        [Test]
        public void MainViewModelExecuteDeleteLeerlijnMethodTest()
        {
            MainViewModel mainViewModel = new MainViewModel();
            LeerlijnViewModel leerlijnViewModel = new LeerlijnViewModel();
            leerlijnViewModel.Naam = "test";
            leerlijnViewModel.Deellijnen.Add(new DeellijnViewModel());
            Assert.DoesNotThrow(() => mainViewModel.ExecuteDeleteLeerlijn(leerlijnViewModel));
        }
    }
}
