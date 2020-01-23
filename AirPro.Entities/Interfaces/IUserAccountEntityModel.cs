using System;

namespace AirPro.Entities.Interfaces
{
    public interface IUserAccountEntityModel
    {
        Guid AccountGuid { get; set; }
        Guid UserGuid { get; set; }
    }
}