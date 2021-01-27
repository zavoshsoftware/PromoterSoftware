namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V03 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProjectDetails", "StartHour", c => c.Int(nullable: false));
            AlterColumn("dbo.ProjectDetails", "StartMin", c => c.Int(nullable: false));
            AlterColumn("dbo.ProjectDetails", "FinishHour", c => c.Int(nullable: false));
            AlterColumn("dbo.ProjectDetails", "FinishMin", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectDetails", "FinishMin", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProjectDetails", "FinishHour", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProjectDetails", "StartMin", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProjectDetails", "StartHour", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
