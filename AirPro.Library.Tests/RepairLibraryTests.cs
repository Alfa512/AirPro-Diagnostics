using Microsoft.VisualStudio.TestTools.UnitTesting;
using AirPro.Library;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AirPro.Entities;

namespace AirPro.Library.Tests
{
    [ExcludeFromCodeCoverage]
    public class MockIdentity : IIdentity
    {
        public string Name { get; set; }
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; } = true;
    }

    [TestClass, ExcludeFromCodeCoverage]
    public class RepairLibraryTests
    {
        [TestMethod()]
        public void RepairLibraryTest()
        {
            var user = new MockIdentity()
            {
                Name = "sandersmw@mac.com",
                AuthenticationType = "Mock"
            };

            var context = new EntityDbContext();

            var lib = new RepairLibrary(context, user);

            Assert.IsTrue(lib != null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void RepairLibraryUnauthenticatedUserTest()
        {
            var user = new MockIdentity()
            {
                Name = "sandersmw@mac.com",
                AuthenticationType = "Mock",
                IsAuthenticated = false
            };

            var context = new EntityDbContext();

            var test = new RepairLibrary(context, user);

            Assert.IsFalse(test != null);
        }

        [TestMethod()]
        public void GetRepairsDashboardTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetRepairDetailsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetRepairEditViewModelTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CompleteRepairTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetInsuranceCompanySelectListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetRequestTypeSelectListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetShopSelectListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateRepairOrderTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetRepairVehicleTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateScanRequestTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateRepairOrderTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpsertRepairVehicleTest()
        {
            Assert.Fail();
        }
    }
}