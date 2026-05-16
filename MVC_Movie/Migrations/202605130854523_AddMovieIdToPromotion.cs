namespace MVC_Movie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMovieIdToPromotion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promotions", "MovieId", c => c.Int());
            CreateIndex("dbo.Promotions", "MovieId");
            AddForeignKey("dbo.Promotions", "MovieId", "dbo.Movies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Promotions", "MovieId", "dbo.Movies");
            DropIndex("dbo.Promotions", new[] { "MovieId" });
            DropColumn("dbo.Promotions", "MovieId");
        }
    }
}
