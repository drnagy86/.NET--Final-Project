namespace RubricMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rubrics",
                c => new
                    {
                        RubricID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 255),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        ScoreTypeID = c.String(),
                        Active = c.Boolean(nullable: false),
                        RowCount = c.Int(),
                        ColumnCount = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        RubricCreator_UserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RubricID)
                .ForeignKey("dbo.Users", t => t.RubricCreator_UserID)
                .Index(t => t.RubricCreator_UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                        GivenName = c.String(),
                        FamilyName = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.Criteria",
                c => new
                    {
                        CriteriaID = c.String(nullable: false, maxLength: 128),
                        RubricID = c.Int(nullable: false),
                        FacetID = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        Content = c.String(),
                        Score = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CriteriaID)
                .ForeignKey("dbo.Rubrics", t => t.RubricID, cascadeDelete: true)
                .Index(t => t.RubricID);
            
            CreateTable(
                "dbo.Facets",
                c => new
                    {
                        FacetID = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                        RubricID = c.Int(nullable: false),
                        FacetType = c.String(),
                    })
                .PrimaryKey(t => t.FacetID)
                .ForeignKey("dbo.Rubrics", t => t.RubricID, cascadeDelete: true)
                .Index(t => t.RubricID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Facets", "RubricID", "dbo.Rubrics");
            DropForeignKey("dbo.Criteria", "RubricID", "dbo.Rubrics");
            DropForeignKey("dbo.Rubrics", "RubricCreator_UserID", "dbo.Users");
            DropIndex("dbo.Facets", new[] { "RubricID" });
            DropIndex("dbo.Criteria", new[] { "RubricID" });
            DropIndex("dbo.Rubrics", new[] { "RubricCreator_UserID" });
            DropTable("dbo.Facets");
            DropTable("dbo.Criteria");
            DropTable("dbo.Users");
            DropTable("dbo.Rubrics");
        }
    }
}
