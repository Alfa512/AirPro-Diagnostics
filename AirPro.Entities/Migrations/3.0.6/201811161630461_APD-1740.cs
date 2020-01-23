namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using UniMatrix.Common.Enumerations;
    using UniMatrix.Common.Extensions;

    public partial class APD1740 : DbMigration
    {
        public override void Up()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveShop");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetShops");

            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Access", "udt_ShopContacts");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Access", "udt_ShopInsuranceCompaniesEstimatePlans");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Access", "udt_ShopInsuranceCompaniesPricingPlans");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Access", "udt_ShopVehicleMakesPricing");
        }
        
        public override void Down()
        {
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_SaveShop");
            this.DropObjectIfExists(DropObjectType.Procedure, "Access", "usp_GetShops");

            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Access", "udt_ShopContacts");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Access", "udt_ShopInsuranceCompaniesEstimatePlans");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Access", "udt_ShopInsuranceCompaniesPricingPlans");
            this.DropObjectIfExists(DropObjectType.UserDefinedType, "Access", "udt_ShopVehicleMakesPricing");
        }
    }
}
