namespace RubricMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addoldfacetid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Facets", "OldFacetID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Facets", "OldFacetID");
        }
    }
}
