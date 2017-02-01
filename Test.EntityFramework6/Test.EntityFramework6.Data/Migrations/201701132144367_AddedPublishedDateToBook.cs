namespace Test.EntityFramework6.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPublishedDateToBook : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "PublishedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "PublishedDate");
        }
    }
}
