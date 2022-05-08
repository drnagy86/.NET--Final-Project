namespace RubricMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnumberOfCriteriafieldtorubrictable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rubrics", "NumberOfCriteria", c => c.Int(nullable: false));
            AddColumn("dbo.Facets", "RubricVM_RubricID", c => c.Int());
            CreateIndex("dbo.Facets", "RubricVM_RubricID");
            AddForeignKey("dbo.Facets", "RubricVM_RubricID", "dbo.Rubrics", "RubricID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Facets", "RubricVM_RubricID", "dbo.Rubrics");
            DropIndex("dbo.Facets", new[] { "RubricVM_RubricID" });
            DropColumn("dbo.Facets", "RubricVM_RubricID");
            DropColumn("dbo.Rubrics", "NumberOfCriteria");
        }
    }
}
