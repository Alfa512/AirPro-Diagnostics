namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v281 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.RequestTypes", "ReportTemplateHtml", c => c.String());

            AddColumn("Scan.RequestTypes", "CreatedBy_Id", c => c.String(nullable: true, maxLength: 128));
            AddColumn("Scan.RequestTypes", "CreatedDt", c => c.DateTimeOffset(nullable: true, precision: 7));

            Sql(@"UPDATE rt SET rt.CreatedBy_Id = (SELECT TOP 1 UserId FROM access.users WHERE UserName LIKE 'sandersmw@%'),rt.CreatedDt = GETUTCDATE() FROM Scan.RequestTypes rt");

            AlterColumn("Scan.RequestTypes", "CreatedBy_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Scan.RequestTypes", "CreatedDt", c => c.DateTimeOffset(nullable: false, precision: 7));

            AddColumn("Scan.RequestTypes", "UpdatedBy_Id", c => c.String(maxLength: 128));
            AddColumn("Scan.RequestTypes", "UpdatedDt", c => c.DateTimeOffset(precision: 7));

            CreateIndex("Scan.RequestTypes", "CreatedBy_Id");
            CreateIndex("Scan.RequestTypes", "UpdatedBy_Id");

            AddForeignKey("Scan.RequestTypes", "CreatedBy_Id", "Access.Users", "UserId", cascadeDelete: false);
            AddForeignKey("Scan.RequestTypes", "UpdatedBy_Id", "Access.Users", "UserId");
        }

        public override void Down()
        {
            DropForeignKey("Scan.RequestTypes", "UpdatedBy_Id", "Access.Users");
            DropForeignKey("Scan.RequestTypes", "CreatedBy_Id", "Access.Users");
            DropIndex("Scan.RequestTypes", new[] { "UpdatedBy_Id" });
            DropIndex("Scan.RequestTypes", new[] { "CreatedBy_Id" });
            DropColumn("Scan.RequestTypes", "UpdatedBy_Id");
            DropColumn("Scan.RequestTypes", "CreatedBy_Id");
            DropColumn("Scan.RequestTypes", "UpdatedDt");
            DropColumn("Scan.RequestTypes", "CreatedDt");
            DropColumn("Scan.RequestTypes", "ReportTemplateHtml");
        }
    }
}
