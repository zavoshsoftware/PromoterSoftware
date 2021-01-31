namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V05 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DailyPromoterPlans", "StartMin", c => c.Int());
            AddColumn("dbo.DailyPromoterPlans", "FinishMin", c => c.Int());
            AlterColumn("dbo.DailyPromoterPlans", "StartHour", c => c.Int());
            AlterColumn("dbo.DailyPromoterPlans", "FinishHour", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DailyPromoterPlans", "FinishHour", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.DailyPromoterPlans", "StartHour", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.DailyPromoterPlans", "FinishMin");
            DropColumn("dbo.DailyPromoterPlans", "StartMin");
        }
    }
}
