using System;
using System.Linq;
using System.Web.Mvc;
using AirPro.Common.Enumerations;

namespace AirPro.Site.Attributes
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute 
    {
        public AuthorizeRolesAttribute(params ApplicationRoles[] roles) : base()
        {
            var allowedRolesAsStrings = roles.Select(x => Enum.GetName(typeof(ApplicationRoles), x)).ToArray();
            Roles = string.Join(",", allowedRolesAsStrings);
        }
    }
}
