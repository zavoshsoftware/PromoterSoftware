namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V02 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectDetails", "StartMin", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProjectDetails", "FinishMin", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectDetails", "FinishMin");
            DropColumn("dbo.ProjectDetails", "StartMin");
        }
    }
}
