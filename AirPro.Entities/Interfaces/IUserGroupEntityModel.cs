using System;

namespace AirPro.Entities.Interfaces
{
    public interface IUserGroupEntityModel
    {
        Guid GroupGuid { get; set; }
        Guid UserGuid { get; set; }
    }
}