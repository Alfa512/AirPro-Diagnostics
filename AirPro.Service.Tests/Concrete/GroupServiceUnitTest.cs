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
using UniMatrix.Common.Extensions;

namespace AirPro.Service.Tests.Concrete
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class GroupServiceUnitTests : ServiceUnitTestsBase, IServiceUnitTests
    {
        private GroupEntityModel _testGroup;

        [TestInitialize]
        public override void TestInit()
        {
            base.TestInit();

            // Add Test Group.
            _testGroup = AddGroup("Unit Test Group");
        }

        [TestMethod]
        public void GetAllTest()
        {
            Trace.WriteLine("\nCheck Groups - No Membership.");
            Assert.AreEqual(0, Factory.GetAll<IGroupDto>().Count(), "Expected 0 Group w/o Membership.");
            Trace.WriteLine("Found 0 Groups.");

            Trace.WriteLine("\nCheck Groups - 1 Membership w/o View Role.");
            AddGroupAssignment(User, _testGroup);
            Assert.AreEqual(0, Factory.GetAll<IGroupDto>().Count(), "Expected 0 Group w/o View Role.");
            Trace.WriteLine("Found 0 Groups.");

            Trace.WriteLine("\nCheck Groups - 1 Membership w/ View Role.");
            AddUserRole(User, ApplicationRoles.GroupView);
            Assert.AreEqual(1, Factory.GetAll<IGroupDto>().Count(), "Expected 1 Group from Membership.");
            Trace.WriteLine("Found 1 Group.");

            Trace.WriteLine("\nCheck Groups - Show All Role.");
            AddUserRole(User, ApplicationRoles.GroupShowAll);
            var total = Context.Groups.Count();
            var accounts = Factory.GetAll<IGroupDto>().Count();
            Assert.AreEqual(total, accounts, $"Expected All Groups in System. -> {total} Groups");
            Trace.WriteLine($"Found {accounts} Groups -> Total {total} Groups.");
        }

        [TestMethod]
        public void GetByIdTest()
        {
            // Load All Groups.
            var groups = Context.Groups.ToList();

            Trace.WriteLine("\nLooking up All Groups w/o Assignment.");
            foreach (var group in groups)
            {
                // Load By ID.
                var test = Factory.GetById<IGroupDto>(group.GroupGuid.ToString());

                // Check Account.
                Assert.IsNull(test, "Group Found.");
                //Trace.WriteLine($"Group {group.Name} -> Not Found");
            }
            Trace.WriteLine($"Searched {groups.Count} -> Found 0 Groups.");

            Trace.WriteLine("\nLooking up All Groups w/o View Role.");
            AddGroupAssignment(User, _testGroup);
            foreach (var group in groups)
            {
                // Load By ID.
                var test = Factory.GetById<IGroupDto>(group.GroupGuid.ToString());

                // Check Group.
                Assert.IsNull(test, "Group Found.");
                //Trace.WriteLine($"Group {group.Name} -> Not Found");
            }
            Trace.WriteLine($"Searched {groups.Count} -> Found 0 Groups.");

            Trace.WriteLine("\nLooking up All Groups w/ View Role.");
            AddUserRole(User, ApplicationRoles.GroupView);
            foreach (var group in groups)
            {
                // Load By ID.
                var test = Factory.GetById<IGroupDto>(group.GroupGuid.ToString());

                // Should Only Find Created Group.
                if (group.CreatedBy.UserName == Identity.Name)
                {
                    Assert.IsNotNull(test, "Group Not Found.");
                    Trace.WriteLine($"Group {group.Name} -> Found");
                }
                else
                {
                    Assert.IsNull(test, "Group Found.");
                    //Trace.WriteLine($"Group {group.Name} -> Not Found");
                }
            }

            Trace.WriteLine("\nLooking up All Groups w/ Show All Role.");
            AddUserRole(User, ApplicationRoles.GroupShowAll);
            foreach (var group in groups)
            {
                // Load By ID.
                var test = Factory.GetById<IGroupDto>(group.GroupGuid.ToString());

                // Check Results.
                Assert.IsNotNull(test, "Group Not Found.");
                Trace.WriteLine($"Group {group.Name} -> Found");
            }
        }

        [TestMethod]
        public void GetDisplayNameTest()
        {
            // Try Invalid ID.
            Trace.WriteLine("\nLooking up Invalid ID.");
            var invalid = Factory.GetDisplayName<IGroupDto>(null);
            Assert.AreEqual("Invalid ID", invalid, "Should be 'Invalid ID'.");
            Trace.WriteLine("Bad ID returned 'Invalid ID'.");

            // Load All Accounts.
            Trace.WriteLine("\nLooking up All Groups.");
            foreach (var group in Context.Groups.ToList())
            {
                // Load By ID.
                var test = Factory.GetDisplayName<IGroupDto>(group.GroupGuid.ToString());
                Trace.WriteLine($"Group {group.GroupGuid} -> {test}");

                // Test Display Name.
                Assert.AreEqual(group.Name, test, "Names Don't Match.");
            }
        }

        [TestMethod]
        public void GetDisplayListTest()
        {
            Trace.WriteLine("\nCheck Groups - No Membership.");
            Assert.AreEqual(0, Factory.GetDisplayList<IGroupDto>().Count(), "Expected 0 Group w/o Membership.");
            Trace.WriteLine("Found 0 Groups.");

            Trace.WriteLine("\nCheck Groups - 1 Membership");
            AddGroupAssignment(User, _testGroup);
            Assert.AreEqual(1, Factory.GetDisplayList<IGroupDto>().Count(), "Expected 1 Group from Membership.");
            Trace.WriteLine("Found 1 Groups.");

            Trace.WriteLine("\nCheck Groups - Show All Role");
            var user = Context.Users.Find(Factory.User.UserGuid);
            AddUserRole(user, ApplicationRoles.GroupShowAll);
            var total = Context.Groups.Count();
            var groups = Factory.GetDisplayList<IGroupDto>().Count();
            Assert.AreEqual(total, groups, $"Expected All Groups in System. -> {total} Groups");
            Trace.WriteLine($"Found {groups} Groups -> Total {total} Groups");
        }

        [TestMethod]
        public void SaveTest()
        {
            // Test Account.
            var group = new GroupTestDto()
            {
                Name = "Unit Test Group Create",
                Description = "Test Group Description"
            };

            Trace.WriteLine("\nCreate Group - No Create Role.");
            {
                var result = Factory.Save((IGroupDto) group);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsFalse(result?.UpdateResult?.Success ?? true, "Missing Update or Successful.");
            }

            Trace.WriteLine("\nCreate Group - With Create Role.");
            AddUserRole(User, ApplicationRoles.GroupCreate);
            {
                group.Roles = new List<KeyValuePair<Guid, string>>
                {
                    new KeyValuePair<Guid, string>(ApplicationRoles.GroupCreate.GetEnumGuid(), ApplicationRoles.GroupCreate.ToString())
                };
                var result = Factory.Save((IGroupDto) group);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Group Create Failed.");
                Assert.IsTrue(Context.Groups.Find(result?.GroupGuid) != null, "Created Group Not Found!");
                group.GroupGuid = result.GroupGuid;
                Assert.AreEqual(1, group.CompareTo(result), "Objects Do NOT Match.");
            }

            Trace.WriteLine("\nUpdate Group - w/o Edit Role.");
            {
                group.Name = "Unit Test Group Update";
                var result = Factory.Save((IGroupDto)group);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsFalse(result?.UpdateResult?.Success ?? true, "Missing Update or Successful.");
            }

            Trace.WriteLine("\nUpdate Group - w/ Edit Role.");
            AddUserRole(User, ApplicationRoles.GroupEdit);
            {
                group.Roles = new List<KeyValuePair<Guid, string>>
                {
                    new KeyValuePair<Guid, string>(ApplicationRoles.GroupEdit.GetEnumGuid(), ApplicationRoles.GroupEdit.ToString())
                };
                var result = Factory.Save((IGroupDto)group);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Group Update Failed.");
                Assert.AreEqual(1, group.CompareTo(result), "Objects Do NOT Match.");
            }

            Trace.WriteLine("\nUpdate Group - w/o Group Membership w/ Edit Role.");
            {
                var groupUpdate = Context.Groups.FirstOrDefault(g => g.Name != group.Name && g.Name != _testGroup.Name);
                Debug.Assert(groupUpdate != null, "groupUpdate != null");
                var update = Factory.Save(new GroupTestDto {GroupGuid = groupUpdate.GroupGuid} as IGroupDto);
                Trace.WriteLine($"Update Result Message: {update?.UpdateResult?.Message}");
                Assert.IsFalse(update?.UpdateResult?.Success ?? true, "Group Updated w/o Access.");
            }

            Trace.WriteLine("\nUpdate Group - w/o Membership w/ Edit & Show All Role.");
            AddUserRole(User, ApplicationRoles.GroupShowAll);
            {
                var groupUpdate = Context.Groups.FirstOrDefault(g => g.Name != group.Name && g.Name != _testGroup.Name);
                Debug.Assert(groupUpdate != null, "groupUpdate != null");
                var update = Factory.Save(new GroupTestDto { GroupGuid = groupUpdate.GroupGuid } as IGroupDto);
                Trace.WriteLine($"Update Result Message: {update?.UpdateResult?.Message}");
                Assert.IsTrue(update?.UpdateResult?.Success ?? false, "Unable to Modify Group w/ Show All Access.");
            }

            Trace.WriteLine("\nUpdate Group - Add Role Test Sync.");
            {
                Assert.IsFalse(Context.UserRoles.Where(r => r.UserId == User.Id).Select(r => r.RoleId).ToList().Contains(ApplicationRoles.GroupDelete.GetEnumGuid()), "User has GroupDelete Role.");
                group.Roles = new List<KeyValuePair<Guid, string>>
                {
                    new KeyValuePair<Guid, string>(ApplicationRoles.GroupDelete.GetEnumGuid(), ApplicationRoles.GroupDelete.ToString()),
                    new KeyValuePair<Guid, string>(ApplicationRoles.GroupEdit.GetEnumGuid(), ApplicationRoles.GroupEdit.ToString())
                };
                var result = Factory.Save((IGroupDto)group);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Unable to Modify Group Roles.");
                Assert.IsTrue(Context.UserRoles.Where(r => r.UserId == User.Id).Select(r => r.RoleId).ToList().Contains(ApplicationRoles.GroupDelete.GetEnumGuid()), "User Does NOT have GroupDelete Role.");
                Assert.AreEqual(1, group.CompareTo(result), "Objects Do NOT Match.");
                Trace.WriteLine("User Roles Assigned.");
            }

            Trace.WriteLine("\nUpdate Group - Remove Role Test Sync.");
            {
                Assert.IsTrue(Context.UserRoles.Where(r => r.UserId == User.Id).Select(r => r.RoleId).ToList().Contains(ApplicationRoles.GroupDelete.GetEnumGuid()), "User Does NOT have GroupDelete Role.");
                group.Roles = new List<KeyValuePair<Guid, string>>
                {
                    new KeyValuePair<Guid, string>(ApplicationRoles.GroupEdit.GetEnumGuid(), ApplicationRoles.GroupEdit.ToString())
                };
                var result = Factory.Save((IGroupDto)group);
                Trace.WriteLine($"Update Result Message: {result?.UpdateResult?.Message}");
                Assert.IsTrue(result?.UpdateResult?.Success ?? false, "Unable to Modify Group Roles.");
                Assert.IsFalse(Context.UserRoles.Where(r => r.UserId == User.Id).Select(r => r.RoleId).ToList().Contains(ApplicationRoles.GroupDelete.GetEnumGuid()), "User has GroupDelete Role.");
                Assert.AreEqual(1, group.CompareTo(result), "Objects Do NOT Match.");
                Trace.WriteLine("User Roles Removed.");
            }
        }

        private class GroupTestDto : IGroupDto, IComparable
        {
            public Guid GroupGuid { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public ICollection<KeyValuePair<Guid, string>> Roles { get; set; }
            public ICollection<IUserDto> Users { get; set; }

            public IUpdateResultDto UpdateResult { get; set; }

            public int CompareTo(object obj)
            {
                var group = obj as IGroupDto;
                if (group == null) return 0;

                Assert.AreEqual(GroupGuid, group.GroupGuid, "Group Guid Does NOT Match.");
                Assert.AreEqual(Name, group.Name, "Group Name Does NOT Match.");
                Assert.AreEqual(Description, group.Description, "Group Description Does NOT Match.");
                Assert.IsTrue(group.Roles?.All(r => Roles.Contains(r)) ?? true, "Group Roles Do NOT Match.");

                return 1;
            }
        }
    }
}
