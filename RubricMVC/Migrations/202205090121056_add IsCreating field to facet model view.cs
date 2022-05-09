namespace RubricMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsCreatingfieldtofacetmodelview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Facets", "IsCreating", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Facets", "IsCreating");
        }
    }
}
