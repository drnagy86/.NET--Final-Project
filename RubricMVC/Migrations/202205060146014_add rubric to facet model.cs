namespace RubricMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrubrictofacetmodel : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Criteria");
            AlterColumn("dbo.Criteria", "CriteriaID", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Criteria", "Content", c => c.String(nullable: false, maxLength: 255));
            AddPrimaryKey("dbo.Criteria", "CriteriaID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Criteria");
            AlterColumn("dbo.Criteria", "Content", c => c.String());
            AlterColumn("dbo.Criteria", "CriteriaID", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Criteria", "CriteriaID");
        }
    }
}
