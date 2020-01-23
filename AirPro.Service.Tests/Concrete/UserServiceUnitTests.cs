using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AirPro.Common.Enumerations;
using AirPro.Entities.Access;
using AirPro.Service.DTOs.Interface;
using AirPro.Service.Tests.Abstract;
using AirPro.Service.Tests.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AirPro.Service.Tests.Concrete
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class UserServiceUnitTests : ServiceUnitTestsBase, IServiceUnitTests
    {
        private readonly List<AccountEntityModel> _testAccounts = new List<AccountEntityModel>();
        private readonly List<ShopEntityModel> _testShops = new List<ShopEntityModel>();

        [TestInitialize]
        public override void TestInit()
        {
            base.TestInit();

            // Create Test Account 1.
            _testAccounts.Add(AddAccount("Unit Test Account 1"));
            AddAccountMembership(AddUser("user@account1"), _testAccounts[0]);
            _testShops.Add(AddShop("Unit Test Shop 1a", _testAccounts[0]));
            AddShopMembership(AddUser("user@shop1a"), _testShops[0]);
            _testShops.Add(AddShop("Unit Test Shop 1b", _testAccounts[0]));
            AddShopMembership(AddUser("user@shop1b"), _testShops[1]);

            // Create Test Account 2.
            _testAccounts.Add(AddAccount("Unit Test Account 2"));
            AddAccountMembership(AddUser("user@account2"), _testAccounts[1]);
            _testShops.Add(AddShop("Unit Test Shop 2a", _testAccounts[1]));
            AddShopMembership(AddUser("user@shop2a"), _testShops[2]);
            _testShops.Add(AddShop("Unit Test Shop 2b", _testAccounts[1]));
            AddShopMembership(AddUser("user@shop2b"), _testShops[3]);
        }

        [TestMethod]
        public void GetAllTest()
        {
            Trace.WriteLine("\nCheck Users - No Membership.");
            Assert.AreEqual(0, Factory.GetAll<IUserDto>().Count(), "Expected 0 User w/o Membership.");
            Trace.WriteLine("Found 0 Users.");

            Trace.WriteLine("\nCheck Users - 1 Shop Membership & w/o View Role.");
            AddShopMembership(User, _testShops[0]);
            Assert.AreEqual(0, Factory.GetAll<IUserDto>().Count(), "Expected 0 User w/o View Role.");
            Trace.WriteLine("Found 0 Users.");

            Trace.WriteLine("\nCheck Users - 1 Shop Membership & w/ View Role.");
            AddUserRole(User, ApplicationRoles.UserView);
            Assert.AreEqual(2, Factory.GetAll<IUserDto>().Count(), "Expected 2 Users from Memberships.");
            Trace.WriteLine("Found 2 Users.");

            Trace.WriteLine("\nCheck Users - 1 Shop & 1 Account Membership & w/ View Role.");
            AddAccountMembership(User, _testAccounts[1]);
            Assert.AreEqual(5, Factory.GetAll<IUserDto>().Count(), "Expected 5 Users from Memberships.");
            Trace.WriteLine("Found 5 Users.");

            Trace.WriteLine("\nCheck Users - Show All Role.");
            AddUserRole(User, ApplicationRoles.UserShowAll);
            var totalCount = Context.Users.Count(user => user.Id != Guid.Empty);
            var userCount = Factory.GetAll<IUserDto>().Count();
            Assert.AreEqual(totalCount, userCount, $"Expected All Users in System. -> {totalCount} Users");
            Trace.WriteLine($"Found {userCount} Users = Total {totalCount} Users.");

            Trace.WriteLine("\nCheck Users - Find Passwords.");
            var passwordsFound = Factory.GetAll<IUserDto>().Count(u => u.PasswordHash != null);
            Assert.AreEqual(0, passwordsFound, "Found Passwords!");
            Trace.WriteLine("Found 0 Passwords.");
        }

        [TestMethod]
        public void GetByIdTest()
        {
            // Load All Users.
            var users = Context.Users.Where(user => user.Id != Guid.Empty).ToList();

            // Check Users w/o Membership.
            Trace.WriteLine("\nLooking up All Users w/o Membership.");
            foreach (var user in users)
            {
                // Load By ID.
                var test = Factory.GetById<IUserDto>(user.Id.ToString());

                // Check User.
                Assert.IsNull(test, "User Found.");
            }
            Trace.WriteLine($"Searched {users.Count} -> Found 0 Users.");

            // Check Users w/o Membership.
            Trace.WriteLine("\nLooking up All Users w/o View Role.");
            AddShopMembership(User, _testShops[0]);
            foreach (var user in users)
            {
                // Load By ID.
                var test = Factory.GetById<IUserDto>(user.Id.ToString());

                // Check User.
                Assert.IsNull(test, "User Found.");
            }
            Trace.WriteLine($"Searched {users.Count} -> Found 0 Users.");

            // Check Users By Membership.
            Trace.WriteLine("\nLooking up All Users w/ Shop Membership & View Role.");
            AddUserRole(User, ApplicationRoles.UserView);
            {
                // Load Valid Users.
                var validShop = _testShops[0].ShopGuid;
                var validUsers = Context.UserShops.Where(us => us.ShopGuid == validShop).Select(us => us.UserGuid);

                foreach (var user in users)
                {
                    // Load By ID.
                    var test = Factory.GetById<IUserDto>(user.Id.ToString());

                    // Should Only Find Created User.
                    if (validUsers.Contains(user.Id))
                    {
                        Assert.IsNotNull(test, "User Not Found.");
                        Assert.IsNull(test.PasswordHash, "Found Password!");
                        Trace.WriteLine($"User {user.UserName} -> Found");
                    }
                    else
                    {
                        Assert.IsNull(test, "User Found.");
                    }
                }
            }

            // Check Users By Membership.
            Trace.WriteLine("\nLooking up All Users w/ Shop & Account Membership & View Role.");
            AddAccountMembership(User, _testAccounts[1]);
            {
                // Load Valid Users.
                var validShop = _testShops.Where(s => s.Account == _testAccounts[1]).Concat(new [] { _testShops[0] }).Select(s => s.ShopGuid).ToList();
                var validAccount = _testAccounts[1].AccountGuid;
                var validUsers = Context.UserShops.Where(us => validShop.Contains(us.ShopGuid))
                        .Select(us => us.UserGuid)
                    .Union(Context.UserAccounts.Where(ua => ua.AccountGuid == validAccount)
                        .Select(ua => ua.UserGuid));

                foreach (var user in users)
                {
                    // Load By ID.
                    var test = Factory.GetById<IUserDto>(user.Id.ToString());

                    // Should Only Find Valid Users.
                    if (validUsers.Contains(user.Id))
                    {
                        Assert.IsNotNull(test, "User Not Found.");
                        Assert.IsNull(test.PasswordHash, "Found Password!");
                        Trace.WriteLine($"User {user.UserName} -> Found");
                    }
                    else
                    {
                        Assert.IsNull(test, "User Found.");
                    }
                }
            }

            // Check Users - Show All.
            Trace.WriteLine("\nLooking up All Users w/ Show All Role.");
            AddUserRole(User, ApplicationRoles.UserShowAll);
            foreach (var user in users)
            {
                // Load By ID.
                var test = Factory.GetById<IUserDto>(user.Id.ToString());

                // Check Results.
                Assert.IsNotNull(test, "User Not Found.");
                Assert.IsNull(test.PasswordHash, "Found Password!");
                Trace.WriteLine($"User {user.UserName} -> Found");
            }
        }

        [TestMethod]
        public void GetDisplayNameTest()
        {
            // Try Invalid ID.
            Trace.WriteLine("\nLooking up Invalid ID.");
            var invalid = Factory.GetDisplayName<IUserDto>(null);
            Assert.AreEqual("Invalid ID", invalid, "Should be 'Invalid ID'.");
            Trace.WriteLine("Bad ID returned 'Invalid ID'.");

            // Load All Users.
            Trace.WriteLine("\nLooking up All Users.");
            foreach (var user in Context.Users.ToList())
            {
                // Load By ID.
                var test = Factory.GetDisplayName<IUserDto>(user.Id.ToString());
                Trace.WriteLine($"User {user.Id} -> {test}");

                // Test Display Name.
                Assert.AreEqual($"{user.LastName}, {user.FirstName}", test, "Names Don't Match.");
            }
        }

        [TestMethod]
        public void GetDisplayListTest()
        {
            var list = Factory.GetDisplayList<IUserDto>();
            Trace.WriteLine($"Display List Count: { (list?.Count() ?? 0) }");
        }

        [TestMethod]
        public void SaveTest()
        {
            var group = AddGroup("Unit Test Group");
            AddGroupRole(group, ApplicationRoles.UserCreate);

            var user = new UserTestDto
            {
                Email = "unit@test.create",
                EmailConfirmed = true,
                PasswordHash = "0123456789",
                FirstName = "First",
                LastName = "Last",
                JobTitle = "Job Title",
                ContactNumber = "(000) 000-0000",
                PhoneNumber = "(111) 111-1111",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = true,
                AccessFailedCount = 10,
                ShopBillingNotification = true,
                ShopReportNotification = true,
                TimeZoneInfoId = "Eastern",
                AccountLocked = true
            };

            var expected = new UserTestDto
            {
                Email = user.Email,
                EmailConfirmed = false, // Email Can't be Confirmed on Create.
                PasswordHash = null, // Should Always be Null.
                FirstName = user.FirstName,
                LastName = user.LastName,
                JobTitle = user.JobTitle,
                ContactNumber = user.ContactNumber,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = false, // Phone Can't be Confirmed on Create.
                TwoFactorEnabled = false, // Two-Factor Can't be Enabled on Create.
                AccessFailedCount = 0, // No Failed Logins for Created User.
                ShopBillingNotification = true,
                ShopReportNotification = true,
                TimeZoneInfoId = "Eastern",
                AccountLocked = false // Account Can't be Locked on Create.
            };

            Trace.WriteLine("\nCreate User - w/o Create Role.");
            {
                var result = Factory.Save((IUserDto)user);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsFalse(result?.UpdateResult?.Success ?? true, "Missing Update or Successful.");
            }

            Trace.WriteLine("\nCreate User - w/ Create Role & w/o Group Membership.");
            AddUserRole(User, ApplicationRoles.UserCreate);
            user.GroupMemberships = expected.GroupMemberships = new List<Guid> {group.GroupGuid};
            {
                var result = Factory.Save((IUserDto)user);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsFalse(result?.UpdateResult?.Success ?? true, "Missing Update or Successful.");
            }

            Trace.WriteLine("\nCreate User - w/ Create Role & Group Membership.");
            AddGroupAssignment(User, group);
            {
                var result = Factory.Save((IUserDto)user);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Missing Update or Failed.");

                expected.UserGuid = result.UserGuid;
                Assert.AreEqual(1, expected.CompareTo(result));
            }

            Trace.WriteLine("\nCreate User - w/ Create Role & w/o Shop Membership.");
            user.ShopMemberships = expected.ShopMemberships = new List<Guid> {_testShops[0].ShopGuid};
            {
                user.Email = expected.Email = "test@create.shop";

                var result = Factory.Save((IUserDto)user);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsFalse(result?.UpdateResult?.Success ?? true, "Missing Update or Successful.");
            }

            Trace.WriteLine("\nCreate User - w/ Create Role & Shop Membership.");
            AddShopMembership(User, _testShops[0]);
            {
                user.UserGuid = expected.UserGuid = Guid.Empty;

                var result = Factory.Save((IUserDto)user);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Missing Update or Failed.");

                expected.UserGuid = result.UserGuid;
                Assert.AreEqual(1, expected.CompareTo(result));
            }

            Trace.WriteLine("\nCreate User - w/ Create Role & w/o Account Membership.");
            user.AccountMemberships = expected.AccountMemberships = new List<Guid> {_testAccounts[0].AccountGuid};
            {
                user.UserGuid = expected.UserGuid = Guid.Empty;
                user.Email = expected.Email = "test@create.account";

                var result = Factory.Save((IUserDto)user);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsFalse(result?.UpdateResult?.Success ?? true, "Missing Update or Successful.");
            }

            Trace.WriteLine("\nCreate User - w/ Create Role & Account Membership.");
            AddAccountMembership(User, _testAccounts[0]);
            {
                var result = Factory.Save((IUserDto)user);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Missing Update or Failed.");

                user.UserGuid = expected.UserGuid = result.UserGuid;
                Assert.AreEqual(1, expected.CompareTo(result));
            }

            Trace.WriteLine("\nCreate User - Duplicate UserName/Email.");
            {
                var result = Factory.Save((IUserDto)new UserTestDto { Email = user.Email });
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsFalse(result?.UpdateResult?.Success ?? true, "Missing Update or Successful.");
            }

            Trace.WriteLine("\nUpdate User - w/o Edit Role.");
            {
                var result = Factory.Save((IUserDto)user);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsFalse(result?.UpdateResult?.Success ?? true, "Missing Update or Successful.");
            }

            Trace.WriteLine("\nUpdate User - w/ Edit Role.");
            AddUserRole(User, ApplicationRoles.UserEdit);
            AddGroupRole(group, ApplicationRoles.UserEdit);
            {
                user.AccountLocked = expected.AccountLocked = false;

                var result = Factory.Save((IUserDto)user);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Missing Update or Failed.");

                Assert.AreEqual(1, expected.CompareTo(result));
            }

            Trace.WriteLine("\nUpdate User - Basic Update.");
            {

                var confirm = Context.Users.Find(user.UserGuid);
                user.EmailConfirmed = expected.EmailConfirmed = confirm.EmailConfirmed = true;
                user.PhoneNumberConfirmed = expected.PhoneNumberConfirmed = confirm.PhoneNumberConfirmed = true;
                user.TwoFactorEnabled = expected.TwoFactorEnabled = true;

                user.FirstName = expected.FirstName = "Test First";
                user.LastName = expected.LastName = "Test Last";
                user.JobTitle = expected.JobTitle = "Test Title";
                user.ContactNumber = expected.ContactNumber = "(222) 222-2222";
                user.TimeZoneInfoId = expected.TimeZoneInfoId = "Western";
                user.ShopBillingNotification = expected.ShopBillingNotification = false;
                user.ShopReportNotification = expected.ShopReportNotification = false;
                user.PasswordHash = "0000000000";

                var result = Factory.Save((IUserDto)user);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Missing Update or Failed.");
                Assert.AreEqual(Context.Users.Find(user.UserGuid).PasswordHash, user.PasswordHash, "Passwords Do NOT Match.");

                Assert.AreEqual(1, expected.CompareTo(result));
            }

            Trace.WriteLine("\nUpdate User - Email & Phone Update.");
            {
                user.Email = expected.Email = "somewhere@something.com";
                expected.EmailConfirmed = false;

                user.PhoneNumber = expected.PhoneNumber = "(333) 333-3333";
                expected.PhoneNumberConfirmed = false;

                expected.TwoFactorEnabled = false;

                var result = Factory.Save((IUserDto)user);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Missing Update or Failed.");

                Assert.AreEqual(1, expected.CompareTo(result));
            }

            Trace.WriteLine("\nUpdate User - Lock Account.");
            {
                user.AccountLocked = expected.AccountLocked = true;

                var result = Factory.Save((IUserDto)user);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Missing Update or Failed.");

                Assert.AreEqual(1, expected.CompareTo(result));
            }

            Trace.WriteLine("\nUpdate User - Unlock Account.");
            {
                user.AccountLocked = expected.AccountLocked = false;

                var result = Factory.Save((IUserDto)user);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Missing Update or Failed.");

                Assert.AreEqual(1, expected.CompareTo(result));
            }

            Trace.WriteLine("\nUpdate User - Add Membership Update.");
            {
                var groupTest = AddGroup("Unit Test Group 2");
                AddGroupAssignment(User, groupTest);
                user.GroupMemberships.Add(groupTest.GroupGuid);
                expected.GroupMemberships = user.GroupMemberships;

                AddShopMembership(User, _testShops[1]);
                user.ShopMemberships.Add(_testShops[1].ShopGuid);
                expected.ShopMemberships = user.ShopMemberships;

                AddAccountMembership(User, _testAccounts[1]);
                user.AccountMemberships.Add(_testAccounts[1].AccountGuid);
                expected.AccountMemberships = user.AccountMemberships;

                var result = Factory.Save((IUserDto)user);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Missing Update or Failed.");

                Assert.AreEqual(1, expected.CompareTo(result));
            }

            Trace.WriteLine("\nUpdate User - Remove Membership Update.");
            {
                user.ShopMemberships = expected.ShopMemberships = null;
                user.GroupMemberships = expected.GroupMemberships = null;
                user.AccountMemberships = expected.AccountMemberships = null;

                var result = Factory.Save((IUserDto)user);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Missing Update or Failed.");

                Assert.AreEqual(1, expected.CompareTo(result));
            }
        }

        private class UserTestDto : IUserDto, IComparable
        {
            public Guid UserGuid { get; set; }
            public string PasswordHash { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string DisplayName { get; set; }
            public string JobTitle { get; set; }
            public string ContactNumber { get; set; }
            public string Email { get; set; }
            public bool EmailConfirmed { get; set; }
            public string PhoneNumber { get; set; }
            public bool PhoneNumberConfirmed { get; set; }
            public bool TwoFactorEnabled { get; set; }
            public bool AccountLocked { get; set; }
            public int AccessFailedCount { get; set; }
            public bool ShopBillingNotification { get; set; }
            public bool ShopReportNotification { get; set; }
            public bool ShopStatementNotification { get; set; }
            public ICollection<Guid> GroupMemberships { get; set; }
            public ICollection<Guid> AccountMemberships { get; set; }
            public ICollection<Guid> ShopMemberships { get; set; }
            public string TimeZoneInfoId { get; set; }
            public bool EmployeeInd { get; set; }
            public bool EmployeeAssignedInd { get; set; }

            public IUpdateResultDto UpdateResult { get; set; }

            public int CompareTo(object obj)
            {
                var user = obj as IUserDto;
                if (user == null) return 0;

                Assert.AreEqual(UserGuid, user.UserGuid, "User Guid Does NOT Match.");
                Assert.AreEqual(PasswordHash, user.PasswordHash, "User Password Hash Does NOT Match.");
                Assert.AreEqual(FirstName, user.FirstName, "User First Name Does NOT Match.");
                Assert.AreEqual(LastName, user.LastName, "User Last Name Does NOT Match.");
                Assert.AreEqual(JobTitle, user.JobTitle, "User Job Title Does NOT Match.");
                Assert.AreEqual(ContactNumber, user.ContactNumber, "User Contact Number Does NOT Match.");
                Assert.AreEqual(Email, user.Email, "User Email Does NOT Match.");
                Assert.AreEqual(EmailConfirmed, user.EmailConfirmed, "User Email Confirmed Does NOT Match.");
                Assert.AreEqual(PhoneNumber, user.PhoneNumber, "User Phone Number Does NOT Match.");
                Assert.AreEqual(PhoneNumberConfirmed, user.PhoneNumberConfirmed, "User Phone Number Confirmed Does NOT Match.");
                Assert.AreEqual(TwoFactorEnabled, user.TwoFactorEnabled, "User Two Factor Enabled Does NOT Match.");
                Assert.AreEqual(AccountLocked, user.AccountLocked, "User Account Locked Does NOT Match.");
                Assert.AreEqual(AccessFailedCount, user.AccessFailedCount, "User Access Failed Count Does NOT Match.");
                Assert.AreEqual(ShopBillingNotification, user.ShopBillingNotification, "User Shop Billing Notification Does NOT Match.");
                Assert.AreEqual(ShopReportNotification, user.ShopReportNotification, "User Shop Report Notification Does NOT Match.");
                Assert.AreEqual(TimeZoneInfoId, user.TimeZoneInfoId, "User Time Zone Info Id Does NOT Match.");

                Assert.IsTrue(user.AccountMemberships?.All(r => AccountMemberships?.Contains(r) ?? false) ?? true, "User Account Memberships Do NOT Match.");
                Assert.IsTrue(user.ShopMemberships?.All(r => ShopMemberships?.Contains(r) ?? false) ?? true, "User Shop Memberships Do NOT Match.");
                Assert.IsTrue(user.GroupMemberships?.All(r => GroupMemberships?.Contains(r) ?? false) ?? true, "User Group Memberships Do NOT Match.");

                return 1;
            }
        }
    }
}
