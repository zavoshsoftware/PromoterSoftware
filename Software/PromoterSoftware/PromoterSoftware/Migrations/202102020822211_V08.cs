namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V08 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DailyPromoterProductSales", "Count", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DailyPromoterProductSales", "Count", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
