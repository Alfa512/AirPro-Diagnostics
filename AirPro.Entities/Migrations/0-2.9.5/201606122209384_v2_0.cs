namespace AirPro.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_0 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Scan.Uploads", "CreatedDt", c => c.DateTimeOffset(precision: 7));
            AddColumn("Scan.Uploads", "CreatedBy_Id", c => c.String(maxLength: 128));

            Sql("UPDATE Scan.Uploads SET CreatedDt = UPLOAD_DT;");
            Sql("UPDATE u SET u.CreatedBy_Id = l.UserId FROM Scan.Uploads u INNER JOIN Access.Users l ON l.Email = 'sandersmw@mac.com';");

            AlterColumn("Scan.Uploads", "CreatedDt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("Scan.Uploads", "CreatedBy_Id", c => c.String(nullable: false, maxLength: 128));

            AddColumn("Scan.Uploads", "UpdatedDt", c => c.DateTimeOffset(precision: 7));
            AddColumn("Scan.Uploads", "UpdatedBy_Id", c => c.String(maxLength: 128));

            CreateIndex("Scan.Uploads", "CreatedBy_Id");
            CreateIndex("Scan.Uploads", "UpdatedBy_Id");

            AddForeignKey("Scan.Uploads", "CreatedBy_Id", "Access.Users", "UserId", cascadeDelete: true);
            AddForeignKey("Scan.Uploads", "UpdatedBy_Id", "Access.Users", "UserId");

            DropColumn("Scan.Uploads", "UPLOAD_DT");
        }
        
        public override void Down()
        {
            AddColumn("Scan.Uploads", "UPLOAD_DT", c => c.DateTime(nullable: false));
            DropForeignKey("Scan.Uploads", "UpdatedBy_Id", "Access.Users");
            DropForeignKey("Scan.Uploads", "CreatedBy_Id", "Access.Users");
            DropIndex("Scan.Uploads", new[] { "UpdatedBy_Id" });
            DropIndex("Scan.Uploads", new[] { "CreatedBy_Id" });
            DropColumn("Scan.Uploads", "UpdatedBy_Id");
            DropColumn("Scan.Uploads", "CreatedBy_Id");
            DropColumn("Scan.Uploads", "UpdatedDt");
            DropColumn("Scan.Uploads", "CreatedDt");
        }
    }
}
