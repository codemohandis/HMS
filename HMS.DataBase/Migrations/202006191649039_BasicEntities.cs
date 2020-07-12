namespace HMS.DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class BasicEntities : DbMigration
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
                .ForeignKey("dbo.Accomodation", t => t.AccomodationPackageID, cascadeDelete: true)
                .Index(t => t.AccomodationPackageID);

            CreateTable(
                "dbo.Accomodation",
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
                "dbo.AccomodationTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);

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

        }

        public override void Down()
        {
            DropForeignKey("dbo.Bookings", "AccomodationID", "dbo.Accomodations");
            DropForeignKey("dbo.Accomodations", "AccomodationPackageID", "dbo.Accomodation");
            DropForeignKey("dbo.Accomodation", "AccomodationTypeID", "dbo.AccomodationTypes");
            DropIndex("dbo.Bookings", new[] { "AccomodationID" });
            DropIndex("dbo.Accomodation", new[] { "AccomodationTypeID" });
            DropIndex("dbo.Accomodations", new[] { "AccomodationPackageID" });
            DropTable("dbo.Bookings");
            DropTable("dbo.AccomodationTypes");
            DropTable("dbo.Accomodation");
            DropTable("dbo.Accomodations");
        }
    }
}
