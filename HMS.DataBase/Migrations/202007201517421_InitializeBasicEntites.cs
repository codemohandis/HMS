namespace HMS.DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitializeBasicEntites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accomodations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AccomodationPackageID = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AccomodationPackages", t => t.AccomodationPackageID, cascadeDelete: true)
                .Index(t => t.AccomodationPackageID);

            CreateTable(
                "dbo.AccomodationPackages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AccomodationTypeID = c.Int(nullable: false),
                        Name = c.String(),
                        NoOfRoom = c.Int(nullable: false),
                        FeePerNight = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AccomodationTypes", t => t.AccomodationTypeID, cascadeDelete: true)
                .Index(t => t.AccomodationTypeID);

            CreateTable(
                "dbo.AccomodationPackagePictures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AccomodationPackageID = c.Int(nullable: false),
                        PictureID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Pictures", t => t.PictureID, cascadeDelete: true)
                .ForeignKey("dbo.AccomodationPackages", t => t.AccomodationPackageID, cascadeDelete: true)
                .Index(t => t.AccomodationPackageID)
                .Index(t => t.PictureID);

            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        URL = c.String(),
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.AccomodationTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.AccomodationPictures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AccomodationID = c.Int(nullable: false),
                        PictureID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Pictures", t => t.PictureID, cascadeDelete: true)
                .ForeignKey("dbo.Accomodations", t => t.AccomodationID, cascadeDelete: true)
                .Index(t => t.AccomodationID)
                .Index(t => t.PictureID);

            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AccomodationID = c.Int(nullable: false),
                        FromDate = c.DateTime(nullable: false),
                        Duration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Accomodations", t => t.AccomodationID, cascadeDelete: true)
                .Index(t => t.AccomodationID);

            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        Country = c.String(),
                        City = c.String(),
                        Address = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Bookings", "AccomodationID", "dbo.Accomodations");
            DropForeignKey("dbo.AccomodationPictures", "AccomodationID", "dbo.Accomodations");
            DropForeignKey("dbo.AccomodationPictures", "PictureID", "dbo.Pictures");
            DropForeignKey("dbo.Accomodations", "AccomodationPackageID", "dbo.AccomodationPackages");
            DropForeignKey("dbo.AccomodationPackages", "AccomodationTypeID", "dbo.AccomodationTypes");
            DropForeignKey("dbo.AccomodationPackagePictures", "AccomodationPackageID", "dbo.AccomodationPackages");
            DropForeignKey("dbo.AccomodationPackagePictures", "PictureID", "dbo.Pictures");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Bookings", new[] { "AccomodationID" });
            DropIndex("dbo.AccomodationPictures", new[] { "PictureID" });
            DropIndex("dbo.AccomodationPictures", new[] { "AccomodationID" });
            DropIndex("dbo.AccomodationPackagePictures", new[] { "PictureID" });
            DropIndex("dbo.AccomodationPackagePictures", new[] { "AccomodationPackageID" });
            DropIndex("dbo.AccomodationPackages", new[] { "AccomodationTypeID" });
            DropIndex("dbo.Accomodations", new[] { "AccomodationPackageID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Bookings");
            DropTable("dbo.AccomodationPictures");
            DropTable("dbo.AccomodationTypes");
            DropTable("dbo.Pictures");
            DropTable("dbo.AccomodationPackagePictures");
            DropTable("dbo.AccomodationPackages");
            DropTable("dbo.Accomodations");
        }
    }
}
