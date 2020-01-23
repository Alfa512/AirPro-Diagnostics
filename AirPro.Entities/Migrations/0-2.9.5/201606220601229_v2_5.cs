using System.Reflection;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Repair.InsuranceCompanies",
                c => new
                    {
                        InsuranceCompanyID = c.Int(nullable: false, identity: true),
                        InsuranceCompanyName = c.String(),
                    })
                .PrimaryKey(t => t.InsuranceCompanyID);

            Sql("SET IDENTITY_INSERT Repair.InsuranceCompanies ON;");
            Sql("INSERT INTO Repair.InsuranceCompanies (InsuranceCompanyID, InsuranceCompanyName) VALUES(1, 'Other');");
            Sql("SET IDENTITY_INSERT Repair.InsuranceCompanies OFF;");

            AddColumn("Repair.Orders", "InsuranceCompanyID", c => c.Int(nullable: true));
            Sql("UPDATE Repair.Orders SET InsuranceCompanyID = 1;");
            AlterColumn("Repair.Orders", "InsuranceCompanyID", c => c.Int(nullable: false));

            CreateIndex("Repair.Orders", "InsuranceCompanyID");
            AddForeignKey("Repair.Orders", "InsuranceCompanyID", "Repair.InsuranceCompanies", "InsuranceCompanyID", cascadeDelete: false);

            SqlFile($"{ Assembly.GetExecutingAssembly().GetExecutingAssemblyPath() }/Migrations/201606220601229_v2_5.sql");

            RenameColumn("Repair.Orders", "InsuranceCompany", "InsuranceCompanyOther");
        }
        
        public override void Down()
        {
            DropForeignKey("Repair.Orders", "InsuranceCompanyID", "Repair.InsuranceCompanies");
            DropIndex("Repair.Orders", new[] { "InsuranceCompanyID" });
            DropColumn("Repair.Orders", "InsuranceCompanyID");
            DropTable("Repair.InsuranceCompanies");

            RenameColumn("Repair.Orders", "InsuranceCompanyOther", "InsuranceCompany");
        }
    }
}
