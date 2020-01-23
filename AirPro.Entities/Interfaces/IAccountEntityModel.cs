using System;

namespace AirPro.Entities.Interfaces
{
    public interface IAccountEntityModel
    {
        Guid AccountGuid { get; set; }
        bool ActiveInd { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        string City { get; set; }
        int DiscountPercentage { get; set; }
        string Fax { get; set; }
        string Name { get; set; }
        string Phone { get; set; }
        int StateId { get; set; }
        string Zip { get; set; }
        Guid? EmployeeGuid { get; set; }
    }
}