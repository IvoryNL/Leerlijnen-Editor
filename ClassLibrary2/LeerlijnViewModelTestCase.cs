using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using DataCare.Tools.LeerlijnenEditor.ViewModels;
using DataCare.Model.Onderwijsinhoudelijk.Leerlijnen;

namespace DataCare.TestCases
{
    [TestFixture]
    class LeerlijnViewModelTestCase
    {
        [Test]
        public void LeerlijnViewModelAddDeellijnCommandPropertyTest()
        {
            LeerlijnViewModel leerlijnViewModel = new LeerlijnViewModel();
            Assert.NotNull(leerlijnViewModel.AddDeellijnCommand);
        }
        [Test]
        public void LeerlijnViewModelDeellijnenPropertyTest()
        {
            LeerlijnViewModel leerlijnViewModel = new LeerlijnViewModel();
            Assert.NotNull(leerlijnViewModel.Deellijnen);
        }
        [Test]
        public void LeerlijnViewModelDeleteDeellijnCommandPropertyTest()
        {
            LeerlijnViewModel leerlijnViewModel = new LeerlijnViewModel();
            Assert.NotNull(leerlijnViewModel.DeleteDeellijnCommand);
        }
        [Test]
        public void LeerlijnViewModelRenameLeerlijnCommandPropertyTest()
        {
            LeerlijnViewModel leerlijnViewModel = new LeerlijnViewModel();
            Assert.NotNull(leerlijnViewModel.RenameLeerlijnCommand);
        }

        [Test]
        public void LeerlijnViewModelAddDeellijnToCollectionMethodTest()
        {
            LeerlijnViewModel leerlijnViewModel = new LeerlijnViewModel();
            Assert.DoesNotThrow(() => leerlijnViewModel.AddDeellijnToCollection("Test"));
        }
        [Test]
        public void LeerlijnViewModelExecuteDeleteDeellijnMethodTest()
        {
            LeerlijnViewModel leerlijnViewModel = new LeerlijnViewModel();
            DeellijnViewModel deellijnViewModel = new DeellijnViewModel();
            deellijnViewModel.Deelgebied = "Test";
            Assert.DoesNotThrow(() => leerlijnViewModel.ExecuteDeleteDeellijn(deellijnViewModel));
        }
        [Test]
        public void LeerlijnViewModelExecuteAddDeellijnMethodTest()
        {
            LeerlijnViewModel leerlijnViewModel = new LeerlijnViewModel();
            Assert.DoesNotThrow(() => leerlijnViewModel.ExecuteAddDeellijn(null));
        }
        [Test]
        public void LeerlijnViewModelExecuteDeleteDeellijnNullMethodTest()
        {
            LeerlijnViewModel leerlijnViewModel = new LeerlijnViewModel();
            Assert.DoesNotThrow(() => leerlijnViewModel.ExecuteDeleteDeellijn(null));
        }
        [Test]
        public void LeerlijnViewModelRenameLeerlijnMethodTest()
        {
            LeerlijnViewModel leerlijnViewModel = new LeerlijnViewModel();
            Assert.DoesNotThrow(() => leerlijnViewModel.RenameLeerlijn("Test"));
            Assert.DoesNotThrow(() => leerlijnViewModel.RenameLeerlijn(null));
            Assert.DoesNotThrow(() => leerlijnViewModel.RenameLeerlijn(" "));
        }
        [Test]
        public void LeerlijnViewModelAddDeellijnMethodTest()
        {
            LeerlijnViewModel leerlijnViewModel = new LeerlijnViewModel();
            Assert.DoesNotThrow(() => leerlijnViewModel.AddDeellijn(null));
            Assert.DoesNotThrow(() => leerlijnViewModel.AddDeellijn("test"));
            Assert.DoesNotThrow(() => leerlijnViewModel.AddDeellijn(" "));
        }
    }
}
