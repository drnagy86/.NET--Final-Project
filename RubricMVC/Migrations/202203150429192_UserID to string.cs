namespace RubricMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIDtostring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "UserID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "UserID", c => c.Int());
        }
    }
}
