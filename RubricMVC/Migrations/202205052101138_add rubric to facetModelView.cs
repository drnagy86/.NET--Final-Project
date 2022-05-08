namespace RubricMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrubrictofacetModelView : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Facets", "Rubric_RubricID", c => c.Int());
            CreateIndex("dbo.Facets", "Rubric_RubricID");
            AddForeignKey("dbo.Facets", "Rubric_RubricID", "dbo.Rubrics", "RubricID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Facets", "Rubric_RubricID", "dbo.Rubrics");
            DropIndex("dbo.Facets", new[] { "Rubric_RubricID" });
            DropColumn("dbo.Facets", "Rubric_RubricID");
        }
    }
}
