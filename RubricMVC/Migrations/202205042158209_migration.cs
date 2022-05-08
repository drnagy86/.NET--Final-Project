namespace RubricMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Facets", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Criteria", "FacetID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Criteria", "FacetID");
            AddForeignKey("dbo.Criteria", "FacetID", "dbo.Facets", "FacetID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Criteria", "FacetID", "dbo.Facets");
            DropIndex("dbo.Criteria", new[] { "FacetID" });
            AlterColumn("dbo.Criteria", "FacetID", c => c.String());
            DropColumn("dbo.Facets", "Discriminator");
        }
    }
}
