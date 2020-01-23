using System;
using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IGroupDto
    {
        Guid GroupGuid { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        ICollection<KeyValuePair<Guid,string>> Roles { get; set; }
        ICollection<IUserDto> Users { get; set; }
        IUpdateResultDto UpdateResult { get; set; }
    }
}