using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using AirPro.Entities;
using Moq;

namespace AirPro.Library.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BaseLibraryTests
    {
        private EntityDbContext Db { get; } = new EntityDbContext();

        private class MockIdentity : IIdentity
        {
            public string Name { get; set; } = "test@umd.tech";
            public string AuthenticationType { get; } = "Mock";
            public bool IsAuthenticated { get; set; } = true;
        }

        [TestMethod]
        public void InitBaseDefaultTest()
        {
            // Default Construction.
            var mock = new Mock<BaseLibrary>(MockBehavior.Loose) {CallBase = true};
            Assert.IsTrue(mock.Object != null);
        }

        [TestMethod]
        public void InitBaseNullContextTest()
        {
            try
            {
                // Constructor with Null Context.
                var mock = new Mock<BaseLibrary>(MockBehavior.Loose, null) {CallBase = true};
                Assert.IsNull(mock.Object);
            }
            catch (TargetInvocationException ex)
            {
                Assert.IsTrue(ex.InnerException is ArgumentNullException);
            }
        }

        [TestMethod]
        public void InitBaseNullIdentityTest()
        {
            try
            {
                // Constructor with Null Identity.
                var mock = new Mock<BaseLibrary>(MockBehavior.Loose, Db, null) {CallBase = true};
                Assert.IsNull(mock.Object);
            }
            catch (TargetInvocationException ex)
            {
                Assert.IsTrue(ex.InnerException is ArgumentNullException);
            }
        }

        [TestMethod]
        public void InitBaseUserNotAuthTest()
        {
            try
            {
                // Invalid User.
                var user = new MockIdentity {IsAuthenticated = false};

                // Constructor with Bad User.
                var mock = new Mock<BaseLibrary>(MockBehavior.Loose, Db, user) {CallBase = true};
                Assert.IsNull(mock.Object);
            }
            catch (TargetInvocationException ex)
            {
                Assert.IsTrue(ex.InnerException?.Message == @"Identity Not Authenticated.");
            }
        }

        [TestMethod]
        public void InitBaseUserNotFoundTest()
        {
            try
            {
                // Set Authenticated on Bad User.
                var user = new MockIdentity
                {
                    Name = "notfound@somewhere.com"
                };

                // Constructor with Bad User.
                var mock = new Mock<BaseLibrary>(MockBehavior.Loose, Db, user) {CallBase = true};
                Assert.IsNull(mock.Object);
            }
            catch (TargetInvocationException ex)
            {
                Assert.IsTrue(ex.InnerException?.Message == @"User Account Not Found.");
            }
        }

        [TestMethod]
        public void InitBaseUserLockedoutTest()
        {
            // Load User.
            var user = new MockIdentity();
            var accessUserEntityModel = Db.Users.FirstOrDefault(u => u.UserName == user.Name);
            if (accessUserEntityModel == null) Assert.Fail();

            try
            {
                // Lockout User.
                accessUserEntityModel.LockoutEndDateUtc = DateTime.UtcNow.AddDays(1);
                Db.Entry(accessUserEntityModel).State = EntityState.Modified;
                Db.SaveChanges();

                // Constructor with Lockedout User.
                var mock = new Mock<BaseLibrary>(MockBehavior.Loose, Db, user) {CallBase = true};
                Assert.IsNull(mock.Object);
            }
            catch (TargetInvocationException ex)
            {
                Assert.IsTrue(ex.InnerException?.Message == @"User Account is Locked Out.");
            }
            finally
            {
                // Unlock User.
                accessUserEntityModel.LockoutEndDateUtc = null;
                Db.Entry(accessUserEntityModel).State = EntityState.Modified;
                Db.SaveChanges();
            }
        }

        [TestMethod]
        public void InitBaseContextIdentityTest()
        {
            // Set Valid User.
            var user = new MockIdentity();

            // Construct Valid Base.
            var mock = new Mock<BaseLibrary>(MockBehavior.Loose, Db, user) { CallBase = true };

            // Check User.
            Assert.AreEqual(user.Name, mock.Object.User.UserName);
        }

        [TestMethod()]
        public void DisposeTest()
        {
            // Default Construction.
            var mock = new Mock<BaseLibrary>(MockBehavior.Loose) { CallBase = true };
            mock.Object.Dispose();
            GC.WaitForPendingFinalizers();
        }
    }
}