namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V04 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "AvatarImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "AvatarImageUrl");
        }
    }
}
