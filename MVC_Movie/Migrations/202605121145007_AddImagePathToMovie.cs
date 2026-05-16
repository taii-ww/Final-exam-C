namespace MVC_Movie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImagePathToMovie : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "ImagePath");
        }
    }
}
