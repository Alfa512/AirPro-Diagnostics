using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Site.Areas.Admin.Models.Access
{
    public class GroupViewModel : IGroupDto
    {
        public Guid GroupGuid { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<KeyValuePair<Guid, string>> Roles { get; set; }
        public ICollection<IUserDto> Users { get; set; }

        [Required, Display(Name = "Roles")]
        public ICollection<Guid> RoleGuids { get; set; }

        public IUpdateResultDto UpdateResult { get; set; }

        public bool AllowEntry { get; set; }
    }
}