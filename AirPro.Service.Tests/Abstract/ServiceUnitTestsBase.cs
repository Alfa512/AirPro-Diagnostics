using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Transactions;
using AirPro.Common.Enumerations;
using AirPro.Entities;
using AirPro.Entities.Access;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniMatrix.Common.Extensions;

namespace AirPro.Service.Tests.Abstract
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public abstract class ServiceUnitTestsBase
    {
        private TransactionScope _tran;
        protected UserEntityModel User { get; private set; }
        protected ServiceFactory Factory { get; private set; }
        protected EntityDbContext Context { get; } = new EntityDbContext();
        protected GenericIdentity Identity { get; } = new GenericIdentity("unit@test.com");

        [TestInitialize]
        public virtual void TestInit()
        {
            // Setup Test.
            Trace.WriteLine("\nTest Initialize:");

            // Start Transaction.
            _tran = new TransactionScope();

            // Add Default User.
            User = AddUser(Identity.Name);

            // Create Factory.
            Factory = new ServiceFactory(Context, Identity);
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            // Rollback.
            _tran.Dispose();
        }

        protected UserEntityModel AddUser(string userName)
        {
            // Create Test User.
            var user = new UserEntityModel
            {
                Email = userName,
                UserName = userName,
                CreatedByUserGuid = Guid.Empty
            };
            Context.Users.Add(user);
            Context.SaveChanges();
            Trace.WriteLine($"Added Test User -> {user.UserName}");
            return Context.Users.FirstOrDefault(u => u.UserName == userName);
        }

        protected AccountEntityModel AddAccount(string accountName)
        {
            // Create New Account.
            var account = new AccountEntityModel
            {
                Name = accountName,
                StateId = 1,
                CreatedBy = User
            };
            Context.Accounts.Add(account);
            Context.SaveChanges();
            Trace.WriteLine($"Added Test Account -> {account.Name}");
            return Context.Accounts.FirstOrDefault(u => u.Name == accountName);
        }

        protected ShopEntityModel AddShop(string shopName, AccountEntityModel account)
        {
            var shop = new ShopEntityModel
            {
                Name = shopName,
                StateId = 1,
                CurrencyId = 1,
                Account = account,
                CreatedBy = User
            };
            Context.Shops.Add(shop);
            Context.SaveChanges();
            Trace.WriteLine($"Added Test Shop -> {shop.Name}");
            return Context.Shops.FirstOrDefault(s => s.Name == shopName);
        }

        protected void AddAccountMembership(UserEntityModel user, AccountEntityModel account)
        {
            // Add Account Membership.
            var userAccount = new UserAccountEntityModel
            {
                User = user,
                Account = account,
                CreatedBy = User
            };
            Context.UserAccounts.Add(userAccount);
            Context.SaveChanges();
            Trace.WriteLine($"Added Account Membership -> {user.UserName} => {account.Name}");
        }

        protected GroupEntityModel AddGroup(string groupName)
        {
            // Create New Group.
            var group = new GroupEntityModel
            {
                Name = groupName,
                CreatedBy = User
            };
            Context.Groups.Add(group);
            Context.SaveChanges();
            Trace.WriteLine($"Added Test Group -> {group.Name}");
            return Context.Groups.FirstOrDefault(g => g.Name == groupName);
        }

        protected void AddGroupAssignment(UserEntityModel user, GroupEntityModel group)
        {
            // Add Group Assignment.
            var groupAssign = new UserGroupEntityModel
            {
                User = user,
                Group = group,
                CreatedBy = User
            };
            Context.UserGroups.Add(groupAssign);
            Context.SaveChanges();
            Trace.WriteLine($"Added Group Assignment -> {user.UserName} => {group.Name}");
        }

        protected void AddGroupRole(GroupEntityModel group, ApplicationRoles role)
        {
            // Add Group to Role.
            var groupRole = new GroupRoleEntityModel
            {
                Group = group,
                Role = Context.Roles.Find(role.GetEnumGuid()),
                CreatedBy = User
            };
            Context.GroupRoles.Add(groupRole);
            Context.SaveChanges();
            Trace.WriteLine($"Added Role to Group -> {role} => {group.Name}");
        }

        protected void AddShopMembership(UserEntityModel user, ShopEntityModel shop)
        {
            // Add Shop Membership.
            var userShop = new UserShopEntityModel
            {
                User = user,
                Shop = shop,
                CreatedBy = User
            };
            Context.UserShops.Add(userShop);
            Context.SaveChanges();
            Trace.WriteLine($"Added Shop Membership -> {user.UserName} => {shop.Name}");
        }

        protected void AddUserRole(UserEntityModel user, ApplicationRoles role)
        {
            // Add User Roles.
            var showAllRole = new UserRoleEntityModel
            {
                RoleId = role.GetEnumGuid(),
                User = user
            };
            Context.UserRoles.Add(showAllRole);
            Context.SaveChanges();

            // Reload Factory.
            Factory = new ServiceFactory(Context, Identity);

            Trace.WriteLine($"Added '{role}' Role to User -> {user.UserName}");
        }
    }
}
