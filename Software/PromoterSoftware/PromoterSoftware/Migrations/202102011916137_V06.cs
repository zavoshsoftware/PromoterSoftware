namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V06 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DailyPromoterPlans", "StartLat", c => c.String());
            AlterColumn("dbo.DailyPromoterPlans", "StartLong", c => c.String());
            AlterColumn("dbo.DailyPromoterPlans", "FinishLat", c => c.String());
            AlterColumn("dbo.DailyPromoterPlans", "FinishLong", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DailyPromoterPlans", "FinishLong", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.DailyPromoterPlans", "FinishLat", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.DailyPromoterPlans", "StartLong", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.DailyPromoterPlans", "StartLat", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
