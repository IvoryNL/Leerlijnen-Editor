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
    public class NiveaudoelenViewModelTestCase
    {
        [Test]
        public void NiveaudoelenViewModelDoelPropertyTest()
        {
            NiveaudoelenViewModel nvm = new NiveaudoelenViewModel();
            Assert.NotNull(nvm.Doel);
        }

        [Test]
        public void NiveaudoelenViewModelNiveauPropertyTest()
        {
            NiveaudoelenViewModel nvm = new NiveaudoelenViewModel();
            Assert.NotNull(nvm.Niveau);
        }
    }
}
