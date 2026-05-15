namespace MVC_Movie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRentalPriceAndPricePaid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "RentalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Rentals", "PricePaid", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rentals", "PricePaid");
            DropColumn("dbo.Movies", "RentalPrice");
        }
    }
}
