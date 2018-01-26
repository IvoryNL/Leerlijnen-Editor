using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using DataCare.Tools.LeerlijnenEditor.ViewModels;

namespace DataCare.TestCases
{
    [TestFixture]
    public class DeellijnViewModelTestCase
    {
        [Test]
        public void DeellijnViewModelDeelgebiedPropertyTest()
        {
            DeellijnViewModel deellijnViewModel = new DeellijnViewModel();
            Assert.NotNull(deellijnViewModel.Deelgebied);
            Assert.NotNull(deellijnViewModel.Niveaudoelen);
            Assert.NotNull(deellijnViewModel.RenameDeellijnCommand);
        }
        [Test]
        public void DeellijnViewModelNiveaudoelenPropertyTest()
        {
            DeellijnViewModel deellijnViewModel = new DeellijnViewModel();
            Assert.NotNull(deellijnViewModel.Niveaudoelen);
        }
        [Test]
        public void DeellijnViewModelRenameDeellijnCommandPropertyTest()
        {
            DeellijnViewModel deellijnViewModel = new DeellijnViewModel();
            Assert.NotNull(deellijnViewModel.RenameDeellijnCommand);
        }

        [Test]
        public void DeellijnViewModelAddNiveaudoelenMethodTest()
        {
            DeellijnViewModel deellijnViewModel = new DeellijnViewModel();
            Assert.DoesNotThrow(() => deellijnViewModel.AddNiveaudoelen(null));
        }
        [Test]
        public void DeellijnViewModelRenameDeellijnMethodTest()
        {
            DeellijnViewModel deellijnViewModel = new DeellijnViewModel();
            Assert.DoesNotThrow(() => deellijnViewModel.RenameDeellijn(null));
            Assert.DoesNotThrow(() => deellijnViewModel.RenameDeellijn("test"));
            Assert.DoesNotThrow(() => deellijnViewModel.RenameDeellijn(" "));
        }
    }
}
