using System;

namespace AirPro.Entities.Interfaces
{
    public interface IGroupRoleEntityModel
    {
        Guid GroupGuid { get; set; }
        Guid RoleGuid { get; set; }
    }
}