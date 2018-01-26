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
    public class LeerlijnenPakketViewModelTestCase
    {
        [Test]
        public void LeerlijnenPakketViewModelPropertyTest()
        {
            LeerlijnenPakketViewModel leerlijnenPakketViewModel = new LeerlijnenPakketViewModel();
            Assert.NotNull(leerlijnenPakketViewModel.Naam);
        }

        [Test]
        public void LeerlijnenPakketViewModelMethodTest()
        {
            LeerlijnenPakketViewModel leerlijnenPakketViewModel = new LeerlijnenPakketViewModel();
            Assert.DoesNotThrow(() => leerlijnenPakketViewModel.AddLeerlijnen(null));
        }
    }
}
