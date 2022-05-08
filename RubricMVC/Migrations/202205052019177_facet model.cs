namespace RubricMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class facetmodel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Criteria", "FacetID", "dbo.Facets");
            DropIndex("dbo.Criteria", new[] { "FacetID" });
            DropPrimaryKey("dbo.Facets");
            CreateTable(
                "dbo.FacetTypes",
                c => new
                    {
                        FacetTypeID = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        Active = c.Boolean(nullable: false),
                        FacetModelView_FacetID = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.FacetTypeID)
                .ForeignKey("dbo.Facets", t => t.FacetModelView_FacetID)
                .Index(t => t.FacetModelView_FacetID);
            
            AlterColumn("dbo.Criteria", "FacetID", c => c.String(maxLength: 100));
            AlterColumn("dbo.Facets", "FacetID", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Facets", "Description", c => c.String(nullable: false, maxLength: 255));
            AddPrimaryKey("dbo.Facets", "FacetID");
            CreateIndex("dbo.Criteria", "FacetID");
            AddForeignKey("dbo.Criteria", "FacetID", "dbo.Facets", "FacetID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Criteria", "FacetID", "dbo.Facets");
            DropForeignKey("dbo.FacetTypes", "FacetModelView_FacetID", "dbo.Facets");
            DropIndex("dbo.FacetTypes", new[] { "FacetModelView_FacetID" });
            DropIndex("dbo.Criteria", new[] { "FacetID" });
            DropPrimaryKey("dbo.Facets");
            AlterColumn("dbo.Facets", "Description", c => c.String());
            AlterColumn("dbo.Facets", "FacetID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Criteria", "FacetID", c => c.String(maxLength: 128));
            DropTable("dbo.FacetTypes");
            AddPrimaryKey("dbo.Facets", "FacetID");
            CreateIndex("dbo.Criteria", "FacetID");
            AddForeignKey("dbo.Criteria", "FacetID", "dbo.Facets", "FacetID");
        }
    }
}
