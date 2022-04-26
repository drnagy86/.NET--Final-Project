namespace RubricMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class name : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ScoreTypes",
                c => new
                    {
                        ScoreTypeID = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        RubricModelView_RubricID = c.Int(),
                    })
                .PrimaryKey(t => t.ScoreTypeID)
                .ForeignKey("dbo.Rubrics", t => t.RubricModelView_RubricID)
                .Index(t => t.RubricModelView_RubricID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScoreTypes", "RubricModelView_RubricID", "dbo.Rubrics");
            DropIndex("dbo.ScoreTypes", new[] { "RubricModelView_RubricID" });
            DropTable("dbo.ScoreTypes");
        }
    }
}
