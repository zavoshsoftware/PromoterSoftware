namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProvinceId = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        IsCenter = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Provinces", t => t.ProvinceId, cascadeDelete: true)
                .Index(t => t.ProvinceId);
            
            CreateTable(
                "dbo.Provinces",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        CityId = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.ProjectDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        StoreId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        ProjectId = c.Guid(nullable: false),
                        StartHour = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FinishHour = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalaryPerHour = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransportationAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Stores", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId)
                .Index(t => t.UserId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        CustomerId = c.Guid(nullable: false),
                        Body = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        ContactPersonFullName = c.String(),
                        ContactPersonCellNumber = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectAttachments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FileUrl = c.String(),
                        ProjectId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.ProjectProducts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProjectId = c.Guid(nullable: false),
                        ProductTitle = c.String(),
                        ImageUrl = c.String(),
                        ProductDescription = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.DailyPromoterProductSales",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Count = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProjectProductId = c.Guid(nullable: false),
                        DailyPromoterPlanId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DailyPromoterPlans", t => t.DailyPromoterPlanId, cascadeDelete: true)
                .ForeignKey("dbo.ProjectProducts", t => t.ProjectProductId, cascadeDelete: true)
                .Index(t => t.ProjectProductId)
                .Index(t => t.DailyPromoterPlanId);
            
            CreateTable(
                "dbo.DailyPromoterPlans",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProjectDetailPromoterId = c.Guid(nullable: false),
                        ShiftDate = c.DateTime(nullable: false),
                        StartHour = c.Decimal(precision: 18, scale: 2),
                        FinishHour = c.Decimal(precision: 18, scale: 2),
                        StartLat = c.Decimal(precision: 18, scale: 2),
                        StartLong = c.Decimal(precision: 18, scale: 2),
                        FinishLat = c.Decimal(precision: 18, scale: 2),
                        FinishLong = c.Decimal(precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectDetailPromoters", t => t.ProjectDetailPromoterId, cascadeDelete: true)
                .Index(t => t.ProjectDetailPromoterId);
            
            CreateTable(
                "dbo.DailyPromoterPlanAttachments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FileUrl = c.String(),
                        DailyPromoterPlanId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DailyPromoterPlans", t => t.DailyPromoterPlanId, cascadeDelete: true)
                .Index(t => t.DailyPromoterPlanId);
            
            CreateTable(
                "dbo.ProjectDetailPromoters",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        ProjectDetailId = c.Guid(),
                        IsFullTime = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProjectDetails", t => t.ProjectDetailId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ProjectDetailId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FullName = c.String(nullable: false),
                        CellNum = c.String(nullable: false),
                        Password = c.String(maxLength: 150),
                        NationalCode = c.String(),
                        NationalCardFileUrl = c.String(),
                        Code = c.Int(),
                        RoleId = c.Guid(nullable: false),
                        CityId = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserBankCards",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        CardNumber = c.String(),
                        BankTitle = c.String(),
                        ShebaNumber = c.String(),
                        FullName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectDetails", "StoreId", "dbo.Stores");
            DropForeignKey("dbo.ProjectProducts", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.DailyPromoterProductSales", "ProjectProductId", "dbo.ProjectProducts");
            DropForeignKey("dbo.UserBankCards", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.ProjectDetails", "UserId", "dbo.Users");
            DropForeignKey("dbo.ProjectDetailPromoters", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "CityId", "dbo.Cities");
            DropForeignKey("dbo.ProjectDetailPromoters", "ProjectDetailId", "dbo.ProjectDetails");
            DropForeignKey("dbo.DailyPromoterPlans", "ProjectDetailPromoterId", "dbo.ProjectDetailPromoters");
            DropForeignKey("dbo.DailyPromoterProductSales", "DailyPromoterPlanId", "dbo.DailyPromoterPlans");
            DropForeignKey("dbo.DailyPromoterPlanAttachments", "DailyPromoterPlanId", "dbo.DailyPromoterPlans");
            DropForeignKey("dbo.ProjectDetails", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectAttachments", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Stores", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Cities", "ProvinceId", "dbo.Provinces");
            DropIndex("dbo.UserBankCards", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "CityId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.ProjectDetailPromoters", new[] { "ProjectDetailId" });
            DropIndex("dbo.ProjectDetailPromoters", new[] { "UserId" });
            DropIndex("dbo.DailyPromoterPlanAttachments", new[] { "DailyPromoterPlanId" });
            DropIndex("dbo.DailyPromoterPlans", new[] { "ProjectDetailPromoterId" });
            DropIndex("dbo.DailyPromoterProductSales", new[] { "DailyPromoterPlanId" });
            DropIndex("dbo.DailyPromoterProductSales", new[] { "ProjectProductId" });
            DropIndex("dbo.ProjectProducts", new[] { "ProjectId" });
            DropIndex("dbo.ProjectAttachments", new[] { "ProjectId" });
            DropIndex("dbo.Projects", new[] { "CustomerId" });
            DropIndex("dbo.ProjectDetails", new[] { "ProjectId" });
            DropIndex("dbo.ProjectDetails", new[] { "UserId" });
            DropIndex("dbo.ProjectDetails", new[] { "StoreId" });
            DropIndex("dbo.Stores", new[] { "CityId" });
            DropIndex("dbo.Cities", new[] { "ProvinceId" });
            DropTable("dbo.UserBankCards");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.ProjectDetailPromoters");
            DropTable("dbo.DailyPromoterPlanAttachments");
            DropTable("dbo.DailyPromoterPlans");
            DropTable("dbo.DailyPromoterProductSales");
            DropTable("dbo.ProjectProducts");
            DropTable("dbo.ProjectAttachments");
            DropTable("dbo.Customers");
            DropTable("dbo.Projects");
            DropTable("dbo.ProjectDetails");
            DropTable("dbo.Stores");
            DropTable("dbo.Provinces");
            DropTable("dbo.Cities");
        }
    }
}
