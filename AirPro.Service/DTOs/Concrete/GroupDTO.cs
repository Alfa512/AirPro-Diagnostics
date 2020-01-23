using System;
using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    [Serializable]
    internal class GroupDto : IGroupDto
    {
        public Guid GroupGuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<KeyValuePair<Guid, string>> Roles { get; set; }
        public ICollection<IUserDto> Users { get; set; }
        public IUpdateResultDto UpdateResult { get; set; }
    }
}
