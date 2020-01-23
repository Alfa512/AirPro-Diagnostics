using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using AirPro.Service.Tests.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirPro.Service.Tests.Concrete
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ServiceFactoryUnitTests : ServiceUnitTestsBase
    {
        private class MockIdentity : IIdentity
        {
            public string Name { get; set; } = "test@umd.tech";
            public string AuthenticationType { get; } = "Mock";
            public bool IsAuthenticated { get; set; } = true;
        }

        [TestMethod]
        public void InitFactory_NullIdentityTest()
        {
            Trace.WriteLine("\nInitialize Service Factory -> Null User.\n");

            try
            {
                // Constructor with Null Context.
                var factory = new ServiceFactory(Context, null);
                var ident = factory.User;
                Assert.Fail("Expected 'NullReferenceException' Exception.");
            }
            catch (NullReferenceException ex)
            {
                Trace.WriteLine(ex.Message);
                Assert.IsNotNull(ex);
            }
        }

        [TestMethod]
        public void InitFactory_EmptyConnectionStringTest()
        {
            Trace.WriteLine("\nInitialize Service Factory -> Empty Connection String.\n");

            try
            {
                // Constructor with Empty Connection String.
                var factory = new ServiceFactory(string.Empty, Identity);
                var user = factory.User;
                Assert.Fail("Expected 'NullReferenceException' Exception.");
            }
            catch (NullReferenceException ex)
            {
                Trace.WriteLine(ex.Message);
                Assert.IsNotNull(ex);
            }
        }

        [TestMethod]
        public void InitFactory_UserNotFoundTest()
        {
            Trace.WriteLine("\nInitialize Service Factory -> User Not Found.\n");

            try
            {
                // Set Authenticated on Bad User.
                var user = new MockIdentity
                {
                    Name = "notfound@somewhere.com"
                };

                // Constructor with Bad User.
                var factory = new ServiceFactory(Context, user);
                var factoryUser = factory.User;
                Assert.Fail("Expected 'Exception' -> User Account Not Found.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Assert.IsTrue(ex.Message == @"User Account Not Found.");
            }
        }

        [TestMethod]
        public void InitFactory_UserLockedoutTest()
        {
            Trace.WriteLine("\nInitialize Service Factory -> User Locked Out.\n");

            try
            {
                // Lockout User.
                User.LockoutEnabled = true;
                User.LockoutEndDateUtc = DateTime.UtcNow.AddDays(1);
                Context.Entry(User).State = EntityState.Modified;
                Context.SaveChanges();

                // Constructor with Lockedout User.
                var factory = new ServiceFactory(Context, Identity);
                var user = factory.User;
                Assert.Fail("Expected 'Exception' -> User Account is Locked Out.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Assert.IsTrue(ex.Message == @"User Account is Locked Out.");
            }
        }

        [TestMethod]
        public void InitFactory_ValidContextIdentityTest()
        {
            Trace.WriteLine("\nInitialize Service Factory -> Valid Identity & Context.\n");

            // Construct Valid Factory.
            var factory = new ServiceFactory(Context, Identity);

            // Check User.
            Assert.AreEqual(Identity.Name, factory.User.UserName);
        }

        [TestMethod]
        public void Factory_NotImplemented()
        {
            Trace.WriteLine("\nService Factory -> Not Implemented Interface.\n");

            try
            {
                var factory = new ServiceFactory(Context, Identity);
                var test = factory.GetAll<IIdentity>();
                Assert.Fail("Expected 'NotImplementedException'.");
            }
            catch (NotImplementedException ex)
            {
                Trace.WriteLine(ex.Message);
                Assert.IsTrue(ex.Message == @"The method or operation is not implemented.");
            }
        }
    }
}