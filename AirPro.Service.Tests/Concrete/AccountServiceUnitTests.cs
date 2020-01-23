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
    public class AccountServiceUnitTests : ServiceUnitTestsBase, IServiceUnitTests
    {
        private AccountEntityModel _testAccount;

        [TestInitialize]
        public override void TestInit()
        {
            base.TestInit();

            // Create Test Account.
            _testAccount = AddAccount("Unit Test Account");
        }

        [TestMethod]
        public void GetAllTest()
        {
            Trace.WriteLine("\nCheck Accounts - No Membership.");
            Assert.AreEqual(0, Factory.GetAll<IAccountDto>().Count(), "Expected 0 Account w/o Membership.");
            Trace.WriteLine("Found 0 Accounts w/o Membership.");

            Trace.WriteLine("\nCheck Accounts - 1 Membership w/o View Role.");
            AddAccountMembership(User, _testAccount);
            Assert.AreEqual(0, Factory.GetAll<IAccountDto>().Count(), "Expected 0 Account w/o View Role.");
            Trace.WriteLine("Found 0 Accounts.");

            Trace.WriteLine("\nCheck Accounts - 1 Membership w/ View Role.");
            AddUserRole(User, ApplicationRoles.AccountView);
            Assert.AreEqual(1, Factory.GetAll<IAccountDto>().Count(), "Expected 1 Account from Membership.");
            Trace.WriteLine("Found 1 Accounts.");

            Trace.WriteLine("\nCheck Accounts - Show All Role.");
            AddUserRole(User, ApplicationRoles.AccountShowAll);
            var total = Context.Accounts.Count();
            var accounts = Factory.GetAll<IAccountDto>().Count();
            Assert.AreEqual(total, accounts, $"Expected All Accounts in System. -> {total} Accounts");
            Trace.WriteLine($"Found {accounts} Accounts -> Total {total} Accounts.");
        }

        [TestMethod]
        public void GetByIdTest()
        {
            // Load All Accounts.
            var accounts = Context.Accounts.ToList();

            // Check Accounts w/o Membership.
            Trace.WriteLine("\nLooking up All Accounts w/o Membership.");
            foreach (var account in accounts)
            {
                // Load By ID.
                var test = Factory.GetById<IAccountDto>(account.AccountGuid.ToString());

                // Check Account.
                Assert.IsNull(test, "Account Found.");
                //Trace.WriteLine($"Account {account.Name} -> Not Found");
            }
            Trace.WriteLine($"Searched {accounts.Count} -> Found 0 Accounts.");

            // Check Accounts w/o View Role.
            Trace.WriteLine("\nLooking up All Accounts w/o View Role.");
            AddAccountMembership(User, _testAccount);
            foreach (var account in accounts)
            {
                // Load By ID.
                var test = Factory.GetById<IAccountDto>(account.AccountGuid.ToString());

                // Check Account.
                Assert.IsNull(test, "Account Found.");
                //Trace.WriteLine($"Account {account.Name} -> Not Found");
            }
            Trace.WriteLine($"Searched {accounts.Count} -> Found 0 Accounts.");

            // Check Accounts By Membership.
            Trace.WriteLine("\nLooking up All Accounts w/ Membership & View Role.");
            AddUserRole(User, ApplicationRoles.AccountView);
            foreach (var account in accounts)
            {
                // Load By ID.
                var test = Factory.GetById<IAccountDto>(account.AccountGuid.ToString());

                // Should Only Find Created Account.
                if (account.CreatedBy.UserName == Identity.Name)
                {
                    Assert.IsNotNull(test, "Account Not Found.");
                    Trace.WriteLine($"Account {account.Name} -> Found");
                }
                else
                {
                    Assert.IsNull(test, "Account Found.");
                    //Trace.WriteLine($"Account {account.Name} -> Not Found");
                }
            }

            // Check Accounts - Show All.
            Trace.WriteLine("\nLooking up All Accounts w/ Show All Role.");
            AddUserRole(User, ApplicationRoles.AccountShowAll);
            foreach (var account in accounts)
            {
                // Load By ID.
                var test = Factory.GetById<IAccountDto>(account.AccountGuid.ToString());

                // Check Results.
                Assert.IsNotNull(test, "Account Not Found.");
                Trace.WriteLine($"Account {account.Name} -> Found");
            }
        }

        [TestMethod]
        public void GetDisplayNameTest()
        {
            // Try Invalid ID.
            Trace.WriteLine("\nLooking up Invalid ID.");
            var invalid = Factory.GetDisplayName<IAccountDto>(null);
            Assert.AreEqual("Invalid ID", invalid, "Should be 'Invalid ID'.");
            Trace.WriteLine("Bad ID returned 'Invalid ID'.");

            // Load All Accounts.
            Trace.WriteLine("\nLooking up All Accounts.");
            foreach (var account in Context.Accounts.ToList())
            {
                // Load By ID.
                var test = Factory.GetDisplayName<IAccountDto>(account.AccountGuid.ToString());
                Trace.WriteLine($"Account {account.AccountGuid} -> {test}");

                // Test Display Name.
                Assert.AreEqual(account.Name, test, "Names Don't Match.");
            }
        }

        [TestMethod]
        public void GetDisplayListTest()
        {
            Trace.WriteLine("\nCheck Accounts - No Membership.");
            Assert.AreEqual(0, Factory.GetDisplayList<IAccountDto>().Count(), "Expected 0 Account w/o Membership.");
            Trace.WriteLine("Found 0 Accounts.");

            Trace.WriteLine("\nCheck Accounts - 1 Membership.");
            AddAccountMembership(User, _testAccount);
            Assert.AreEqual(1, Factory.GetDisplayList<IAccountDto>().Count(), "Expected 1 Account from Membership.");
            Trace.WriteLine("Found 1 Accounts.");

            Trace.WriteLine("\nCheck Accounts - Show All Role.");
            var user = Context.Users.Find(Factory.User.UserGuid);
            AddUserRole(user, ApplicationRoles.AccountShowAll);
            var total = Context.Accounts.Count(a => a.ActiveInd);
            var accounts = Factory.GetDisplayList<IAccountDto>().Count();
            Assert.AreEqual(total, accounts, $"Expected All Accounts in System. -> {total} Accounts");
            Trace.WriteLine($"Found {accounts} Accounts -> Total {total} Accounts.");
        }

        [TestMethod]
        public void SaveTest()
        {
            // Test Account.
            var account = new AccountTestDto
            { 
                Name = "Unit Test Account Create",
                Phone = "(000) 000-0000",
                Fax = "(111) 111-1111",
                Address1 = "Address 1",
                Address2 = "Address 2",
                City = "City",
                State = "FL",
                Zip = "12345"
            };

            Trace.WriteLine("\nCreate Record - No Create Role.");
            {
                var result = Factory.Save((IAccountDto)account);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsFalse(result?.UpdateResult?.Success ?? true, "Missing Update or Successful.");
            }

            Trace.WriteLine("\nCreate Record - With Create Role.");
            AddUserRole(User, ApplicationRoles.AccountCreate);
            {
                var result = Factory.Save((IAccountDto)account);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Account Create Failed.");
                Assert.IsTrue(Context.Accounts.Find(result?.AccountGuid) != null, "Created Account Not Found!");

                account.AccountGuid = result.AccountGuid;
                Assert.AreEqual(1, account.CompareTo(result), "Objects Do NOT Match.");
            }

            Trace.WriteLine("\nUpdate Account - No Edit Role.");
            {
                account.Name = "Unit Test Account Update";
                var result = Factory.Save((IAccountDto)account);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsFalse(result?.UpdateResult?.Success ?? true, "Missing Update or Successful.");
            }

            Trace.WriteLine("\nUpdate Account - With Edit Role.");
            AddUserRole(User, ApplicationRoles.AccountEdit);
            {
                var result = Factory.Save((IAccountDto)account);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Account Update Failed.");
                Assert.AreEqual(1, account.CompareTo(result), "Objects Do NOT Match.");
            }

            Trace.WriteLine("\nUpdate Account - No Account Membership w/ Edit Role.");
            {
                var result = Context.Accounts.FirstOrDefault(a => a.Name != account.Name);
                Debug.Assert(result != null, "accountUpdate != null");
                var update =
                    Factory.Save(new AccountTestDto
                    {
                        AccountGuid = result.AccountGuid,
                        Name = $"{result.Name} Test"
                    } as IAccountDto);
                Trace.WriteLine($"Update Result Message: {update?.UpdateResult?.Message}");
                Assert.IsFalse(update?.UpdateResult?.Success ?? true, "Unable to Modify Account w/o Access.");
            }
        }

        private class AccountTestDto : IAccountDto, IComparable
        {
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public Guid AccountGuid { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Fax { get; set; }
            public int DiscountPercentage { get; set; }
            public bool ActiveInd { get; set; }

            public IUpdateResultDto UpdateResult { get; set; }
            public ICollection<IUserDto> Users { get; set; }
            public Guid? EmployeeGuid { get; set; }
            public string AccountRep { get; set; }

            public int CompareTo(object obj)
            {
                var account = obj as IAccountDto;
                if (account == null) return 0;

                Assert.AreEqual(AccountGuid, account.AccountGuid);
                Assert.AreEqual(Name, account.Name);
                Assert.AreEqual(Phone, account.Phone);
                Assert.AreEqual(Fax, account.Fax);
                Assert.AreEqual(Address1, account.Address1);
                Assert.AreEqual(Address2, account.Address2);
                Assert.AreEqual(City, account.City);
                Assert.AreEqual(State, account.State);
                Assert.AreEqual(Zip, account.Zip);

                return 1;
            }
        }
    }
}
