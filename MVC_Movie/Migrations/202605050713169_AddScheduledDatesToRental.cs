namespace MVC_Movie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddScheduledDatesToRental : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rentals", "ScheduledRentalDate", c => c.DateTime());
            AddColumn("dbo.Rentals", "ScheduledReturnDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rentals", "ScheduledReturnDate");
            DropColumn("dbo.Rentals", "ScheduledRentalDate");
        }
    }
}
