using System;
using System.Diagnostics.CodeAnalysis;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Tests.Abstract;
using AirPro.Service.Tests.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirPro.Service.Tests.Concrete
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RepairServiceUnitTests : ServiceUnitTestsBase, IServiceUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetAllTest()
        {
            var repairs = Factory.GetAll<IRepairDto>();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetByIdTest()
        {
            var repair = Factory.GetById<IRepairDto>("1");
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetDisplayNameTest()
        {
            Factory.GetById<IRepairDto>("1");
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetDisplayListTest()
        {
            Factory.GetDisplayList<IRepairDto>();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SaveTest()
        {
            throw new NotImplementedException();
        }
    }
}
