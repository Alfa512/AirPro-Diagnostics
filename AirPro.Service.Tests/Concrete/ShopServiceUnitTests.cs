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
    public class ShopServiceUnitTests : ServiceUnitTestsBase, IServiceUnitTests
    {
        private readonly List<AccountEntityModel> _testAccounts = new List<AccountEntityModel>();
        private readonly List<ShopEntityModel> _testShops = new List<ShopEntityModel>();

        [TestInitialize]
        public override void TestInit()
        {
            base.TestInit();

            // Create Test Account 1.
            _testAccounts.Add(AddAccount("Unit Test Account 1"));
            _testShops.Add(AddShop("Unit Test Shop 1a", _testAccounts[0]));
            _testShops.Add(AddShop("Unit Test Shop 1b", _testAccounts[0]));

            // Create Test Account 2.
            _testAccounts.Add(AddAccount("Unit Test Account 2"));
            _testShops.Add(AddShop("Unit Test Shop 2a", _testAccounts[1]));
            _testShops.Add(AddShop("Unit Test Shop 2b", _testAccounts[1]));
        }

        [TestMethod]
        public void GetAllTest()
        {
            Trace.WriteLine("\nCheck Shops - No Membership.");
            Assert.AreEqual(0, Factory.GetAll<IShopDto>().Count(), "Expected 0 Shop w/o Membership.");
            Trace.WriteLine("Found 0 Shops.");

            Trace.WriteLine("\nCheck Shops - 1 Account & 1 Shop Membership w/o View Role");
            AddAccountMembership(User, _testAccounts[0]);
            AddShopMembership(User, _testShops[2]);
            Assert.AreEqual(0, Factory.GetAll<IShopDto>().Count(), "Expected 0 Shop w/o View Role.");
            Trace.WriteLine("Found 0 Shops.");

            Trace.WriteLine("\nCheck Shops - 1 Account & 1 Shop Membership w/ View Role");
            AddUserRole(User, ApplicationRoles.ShopView);
            Assert.AreEqual(3, Factory.GetAll<IShopDto>().Count(), "Expected 3 Shops from Memberships.");
            Trace.WriteLine("Found 3 Shops.");

            Trace.WriteLine("\nCheck Shops - Show All Role");
            AddUserRole(User, ApplicationRoles.ShopShowAll);
            var totalCount = Context.Shops.Count();
            var shopCount = Factory.GetAll<IShopDto>().Count();
            Assert.AreEqual(totalCount, shopCount, $"Expected All Shops in System. -> {totalCount} Shops");
            Trace.WriteLine($"Found {shopCount} Shops = Total {totalCount} Shops.");
        }

        [TestMethod]
        public void GetByIdTest()
        {
            // Load All Shops.
            var shops = Context.Shops.ToList();

            Trace.WriteLine("\nLooking up All Shops w/o Membership.");
            foreach (var shop in shops)
            {
                // Load By ID.
                var test = Factory.GetById<IShopDto>(shop.ShopGuid.ToString());

                // Check Shop.
                Assert.IsNull(test, "Shop Found.");
                //Trace.WriteLine($"Shop {shop.Name} -> Not Found");
            }
            Trace.WriteLine($"Searched {shops.Count} -> Found 0 Shops.");

            Trace.WriteLine("\nLooking up All Shops w/o View Role.");
            AddAccountMembership(User, _testAccounts[0]);
            AddShopMembership(User, _testShops[2]);
            var validShops = _testShops.Where(s => s.Account == _testAccounts[0]).Concat(new[] { _testShops[2] }).Select(s => s.ShopGuid).ToList();
            foreach (var shop in shops)
            {
                // Load By ID.
                var test = Factory.GetById<IShopDto>(shop.ShopGuid.ToString());

                // Check Shop.
                Assert.IsNull(test, "Shop Found.");
                //Trace.WriteLine($"Shop {shop.Name} -> Not Found");
            }
            Trace.WriteLine($"Searched {shops.Count} -> Found 0 Shops.");

            Trace.WriteLine("\nLooking up All Shops w/ Memberships & View Role.");
            AddUserRole(User, ApplicationRoles.ShopView);
            foreach (var shop in shops)
            {
                // Load By ID.
                var test = Factory.GetById<IShopDto>(shop.ShopGuid.ToString());

                // Should Only Find Valid Shops.
                if (validShops.Contains(shop.ShopGuid))
                {
                    Assert.IsNotNull(test, "Shop Not Found.");
                    Trace.WriteLine($"Shop {shop.Name} -> Found");
                }
                else
                {
                    Assert.IsNull(test, "Shop Found.");
                    //Trace.WriteLine($"Shop {shop.Name} -> Not Found");
                }
            }

            // Check Shops - Show All.
            Trace.WriteLine("\nLooking up All Shops w/ Show All Role.");
            AddUserRole(User, ApplicationRoles.ShopShowAll);
            foreach (var shop in shops)
            {
                // Load By ID.
                var test = Factory.GetById<IShopDto>(shop.ShopGuid.ToString());

                // Check Results.
                Assert.IsNotNull(test, "Shop Not Found.");
                Trace.WriteLine($"Shop {shop.Name} -> Found");
            }
        }

        [TestMethod]
        public void GetDisplayNameTest()
        {
            Trace.WriteLine("\nLooking up Invalid ID.");
            var invalid = Factory.GetDisplayName<IShopDto>(null);
            Assert.AreEqual("Invalid ID", invalid, "Should be 'Invalid ID'.");
            Trace.WriteLine("Bad ID returned 'Invalid ID'.");

            Trace.WriteLine("\nLooking up All Shops.");
            foreach (var shop in Context.Shops.ToList())
            {
                // Load By ID.
                var test = Factory.GetDisplayName<IShopDto>(shop.ShopGuid.ToString());
                Trace.WriteLine($"Shop {shop.ShopGuid} -> {test}");

                // Test Display Name.
                Assert.AreEqual(shop.DisplayName, test, "Names Don't Match.");
            }
        }

        [TestMethod]
        public void GetDisplayListTest()
        {
            Trace.WriteLine("\nCheck Shops - No Membership.");
            Assert.AreEqual(0, Factory.GetDisplayList<IShopDto>().Count(), "Expected 0 Shop w/o Membership.");
            Trace.WriteLine("Found 0 Shops.");

            Trace.WriteLine("\nCheck Shops - 1 Account & 1 Shop Membership.");
            AddAccountMembership(User, _testAccounts[0]);
            AddShopMembership(User, _testShops[2]);
            Assert.AreEqual(3, Factory.GetDisplayList<IShopDto>().Count(), "Expected 3 Shops from Memberships.");
            Trace.WriteLine("Found 3 Shops.");

            Trace.WriteLine("\nCheck Shops - Show All Role.");
            AddUserRole(User, ApplicationRoles.ShopShowAll);
            var total = Context.Shops.Count(s => s.ActiveInd && s.Account.ActiveInd);
            var shops = Factory.GetDisplayList<IShopDto>().Count();
            Assert.AreEqual(total, shops, $"Expected All Shops in System. -> {total} Shops");
            Trace.WriteLine($"Found {shops} Shops = Total {total} Shops.");
        }

        [TestMethod]
        public void SaveTest()
        {
            // Test Shop.
            var shop = new ShopTestDto
            {
                Name = "Unit Test Shop Create",
                AccountGuid = _testAccounts[0].AccountGuid,
                AccountName = _testAccounts[0].Name,
                Phone = "(000) 000-0000",
                Fax = "(111) 111-1111",
                Address1 = "Address 1",
                Address2 = "Address 2",
                City = "City",
                State = "FL",
                Zip = "12345"
            };

            Trace.WriteLine("\nCreate Shop - w/o Create Role & w/o Account Membership.");
            {
                var result = Factory.Save((IShopDto)shop);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsFalse(result?.UpdateResult?.Success ?? true, "Missing Update or Successful.");
            }

            Trace.WriteLine("\nCreate Shop - w/o Create Role & w/Account Membership.");
            AddAccountMembership(User, _testAccounts[0]);
            {
                var result = Factory.Save((IShopDto)shop);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsFalse(result?.UpdateResult?.Success ?? true, "Missing Update or Successful.");
            }

            Trace.WriteLine("\nCreate Shop - w/Create Role & Account Membership.");
            AddUserRole(User, ApplicationRoles.ShopCreate);
            {
                var result = Factory.Save((IShopDto)shop);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Shop Create Failed.");
                Assert.IsTrue(Context.Shops.Find(result?.ShopGuid) != null, "Created Shop Not Found!");

                shop.ShopGuid = result.ShopGuid;
                Assert.AreEqual(1, shop.CompareTo(result), "Objects Do NOT Match.");
            }

            Trace.WriteLine("\nCreate Shop - w/Create Role & w/o Account Membership.");
            {
                var result =
                    Factory.Save(new ShopTestDto
                    {
                        AccountGuid = _testAccounts[1].AccountGuid,
                        Name = $"Create Shop Test"
                    } as IShopDto);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsFalse(result?.UpdateResult?.Success ?? true, "Missing Update or Successful.");
            }

            Trace.WriteLine("\nUpdate Shop - w/o Edit Role.");
            shop.Name = "Unit Test Shop Update";
            {
                var result = Factory.Save((IShopDto)shop);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsFalse(result?.UpdateResult?.Success ?? true, "Missing Update or Successful.");
            }

            Trace.WriteLine("\nUpdate Shop - w/Edit Role & Account Membership.");
            AddUserRole(User, ApplicationRoles.ShopEdit);
            {
                var result = Factory.Save((IShopDto)shop);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Shop Update Failed.");
                Assert.AreEqual(1, shop.CompareTo(result), "Objects Do NOT Match.");
            }

            Trace.WriteLine("\nUpdate Shop - w/Edit Role & w/o Account Membership.");
            {
                var update = _testShops[2];
                var result =
                    Factory.Save(new ShopTestDto
                    {
                        ShopGuid = update.ShopGuid,
                        AccountGuid = update.AccountGuid,
                        Name = $"{update.Name} Test"
                    } as IShopDto);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsFalse(result?.UpdateResult?.Success ?? true, "Unable to Modify Shop w/o Access.");
            }

            Trace.WriteLine("\nCreate Shop - w/Create & Shop Show All Roles & w/o Account Membership.");
            AddUserRole(User, ApplicationRoles.ShopShowAll);
            {
                var result =
                    Factory.Save(new ShopTestDto
                    {
                        AccountGuid = _testAccounts[1].AccountGuid,
                        Name = $"Create Shop Test"
                    } as IShopDto);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsFalse(result?.UpdateResult?.Success ?? true, "Missing Update or Successful.");
            }

            Trace.WriteLine("\nUpdate Shop - w/Edit & Shop Show All Roles & w/o Account Membership.");
            {
                var update = new ShopTestDto
                {
                    ShopGuid = _testShops[2].ShopGuid,
                    AccountGuid = _testShops[2].AccountGuid,
                    Name = $"{_testShops[2].Name} Test",
                    State = "FL"
                };
                var result = Factory.Save((IShopDto)update);
                update.AccountName = _testAccounts[1].Name;
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Shop Update Failed.");
                Assert.AreEqual(1, update.CompareTo(result), "Objects Do NOT Match.");
            }

            Trace.WriteLine("\nCreate Shop - w/Create & Account Show All Roles & w/o Account Membership.");
            AddUserRole(User, ApplicationRoles.AccountShowAll);
            {
                var create = new ShopTestDto
                {
                    AccountGuid = _testAccounts[1].AccountGuid,
                    Name = "Create Shop Test",
                    State = "FL"
                };
                var result = Factory.Save((IShopDto)create);
                create.ShopGuid = result.ShopGuid;
                create.AccountName = _testAccounts[1].Name;
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Shop Create Failed.");
                Assert.AreEqual(1, create.CompareTo(result), "Objects Do NOT Match.");
            }
        }

        private class ShopTestDto : IShopDto, IComparable
        {
            public ShopTestDto()
            {
                ShopContacts = new List<IShopContactDto>();
                ShopInsuranceCompanyPricingPlans = new List<IShopInsuranceCompanyPlanDto>();
                ShopInsuranceCompanyEstimatePlans = new List<IShopInsuranceCompanyPlanDto>();
                ShopVehicleMakesPricingPlans = new List<IShopVehicleMakesPricingDto>();
                ShopVehicleMakes = new List<int>();
                ShopInsuranceCompanies = new List<int>();
                AirProTools = new List<IAirProToolDto>();
                AccountAirProTools = new List<IAirProToolDto>();
                Users = new List<IUserDto>();
                AccountUsers = new List<IUserDto>();
            }

            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public Guid ShopGuid { get; set; }
            public int ShopNumber { get; set; }
            public Guid? AccountGuid { get; set; }
            public Guid? AccountEmployeeGuid { get; set; }
            public string AccountName { get; set; }
            public string Phone { get; set; }
            public string Fax { get; set; }
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public string Notes { get; set; }
            public int DiscountPercentage { get; set; }
            public bool AllowScanAnalysisAutoClose { get; set; }
            public IUpdateResultDto UpdateResult { get; set; }
            public string CCCShopId { get; set; }
            public bool AllowAllRepairAutoClose { get; set; }
            public bool AllowSelfScan { get; set; }
            public bool AllowAutoRepairClose { get; set; }
            public bool AllowScanAnalysis { get; set; }
            public bool AllowSelfScanAssessment { get; set; }
            public bool AllowDemoScan { get; set; }
            public int DefaultInsuranceCompanyId { get; set; }
            public int? PricingPlanId { get; set; }
            public int? EstimatePlanId { get; set; }
            public int? BillingCycleId { get; set; }
            public int? AverageVehiclesPerMonth { get; set; }
            public bool ShopFixedPriceInd { get; set; }
            public bool HideFromReports { get; set; }
            public decimal FirstScanCost { get; set; }
            public decimal AdditionalScanCost { get; set; }
            public int? AutomaticRepairCloseDays { get; set; }
            public bool ActiveInd { get; set; }
            public int CurrencyId { get; set; } = 1;
            public bool SendToMitchellInd { get; set; }
            public bool DisableShopBillingNotification { get; set; }
            public bool DisableShopStatementNotification { get; set; }
            public ICollection<IUserDto> Users { get; set; }
            public ICollection<IUserDto> AccountUsers { get; set; }
            public ICollection<IShopInsuranceCompanyPlanDto> ShopInsuranceCompanyPricingPlans { get; set; }
            public ICollection<IShopInsuranceCompanyPlanDto> ShopInsuranceCompanyEstimatePlans { get; set; }
            public ICollection<IShopVehicleMakesPricingDto> ShopVehicleMakesPricingPlans { get; set; }
            public IEnumerable<int> ShopVehicleMakes { get; set; }
            public IEnumerable<int> ShopInsuranceCompanies { get; set; }
            public IEnumerable<IAirProToolDto> AirProTools { get; set; }
            public IEnumerable<IAirProToolDto> AccountAirProTools { get; set; }
            public IEnumerable<IShopContactDto> ShopContacts { get; set; }
            public IEnumerable<int> ShopRequestTypes { get; set; }
            public Guid? EmployeeGuid { get; set; }
            public string AccountRep { get; set; }
            public string ShopRep { get; set; }
            public bool AutomaticInvoicesInd { get; set; }

            public int CompareTo(object obj)
            {
                var shop = obj as IShopDto;
                if (shop == null) return 0;

                Assert.AreEqual(ShopGuid, shop.ShopGuid, "Shop Guid Does NOT Match.");
                Assert.AreEqual(AccountGuid, shop.AccountGuid, "Shop Account Guid Does NOT Match.");
                Assert.AreEqual(AccountName, shop.AccountName, "Shop Account Name Does NOT Match.");
                Assert.AreEqual(Name, shop.Name, "Shop Name Does NOT Match.");
                Assert.AreEqual(Phone, shop.Phone, "Shop Phone Does NOT Match.");
                Assert.AreEqual(Fax, shop.Fax, "Shop Fax Does NOT Match.");
                Assert.AreEqual(Address1, shop.Address1, "Shop Address1 Does NOT Match.");
                Assert.AreEqual(Address2, shop.Address2, "Shop Address2 Does NOT Match.");
                Assert.AreEqual(City, shop.City, "Shop City Does NOT Match.");
                Assert.AreEqual(State, shop.State, "Shop State Does NOT Match.");
                Assert.AreEqual(Zip, shop.Zip, "Shop Zip Does NOT Match.");
                Assert.AreEqual(Notes, shop.Notes, "Shop Notes Does NOT Match.");

                return 1;
            }
        }
    }
}
