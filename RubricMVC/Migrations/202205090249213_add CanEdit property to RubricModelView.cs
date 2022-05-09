namespace RubricMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCanEditpropertytoRubricModelView : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rubrics", "CanEdit", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rubrics", "CanEdit");
        }
    }
}
