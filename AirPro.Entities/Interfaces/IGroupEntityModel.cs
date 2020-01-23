using System;

namespace AirPro.Entities.Interfaces
{
    public interface IGroupEntityModel
    {
        string Description { get; set; }
        Guid GroupGuid { get; set; }
        string Name { get; set; }
    }
}