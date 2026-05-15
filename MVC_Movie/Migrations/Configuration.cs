namespace MVC_Movie.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MVC_Movie.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MVC_Movie.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MVC_Movie.Models.AppDbContext context)
        {
            if (!context.MembershipTypes.Any())
            {
                context.MembershipTypes.AddRange(new List<MembershipType>
        {
            new MembershipType { Id = 0, Name = "Unknown", SignUpFee = 0, DurationInMonths = 0, DiscountRate = 0 },
            new MembershipType { Id = 1, Name = "Pay As You Go", SignUpFee = 0, DurationInMonths = 1, DiscountRate = 0 },
            new MembershipType { Id = 2, Name = "Monthly", SignUpFee = 30, DurationInMonths = 1, DiscountRate = 10 },
            new MembershipType { Id = 3, Name = "Quarterly", SignUpFee = 90, DurationInMonths = 3, DiscountRate = 15 },
            new MembershipType { Id = 4, Name = "Annual", SignUpFee = 300, DurationInMonths = 12, DiscountRate = 20 }
        });
                context.SaveChanges();
            }

            if (!context.Genres.Any())
            {
                context.Genres.AddRange(new List<Genre>
        {
            new Genre { Name = "Action" },
            new Genre { Name = "Comedy" },
            new Genre { Name = "Romance" },
            new Genre { Name = "Thriller" }
        });
                context.SaveChanges();
            }

            if (!context.Customers.Any())
            {
                context.Customers.AddRange(new List<Customer>
        {
            new Customer { Name = "John Smith", IsSubscribedToNewsletter = true, Birthdate = new DateTime(1990, 1, 1), MembershipTypeId = 1 },
            new Customer { Name = "Mary Williams", IsSubscribedToNewsletter = false, Birthdate = new DateTime(1985, 5, 15), MembershipTypeId = 2 }
        });
                context.SaveChanges();
            }

            // Seed Roles
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Tạo role CanManageMovies nếu chưa có
            if (!roleManager.RoleExists(RoleName.CanManageMovies))
            {
                roleManager.Create(new IdentityRole(RoleName.CanManageMovies));
            }

            // Tạo tài khoản Admin
            if (userManager.FindByEmail("admin@vidly.com") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@vidly.com",
                    Email = "admin@vidly.com",
                    DrivingLicense = "ADMIN-LICENSE-001",
                    Phone = "0123456789"
                };
                userManager.Create(admin, "Admin@123");
                userManager.AddToRole(admin.Id, RoleName.CanManageMovies);
            }

            // Tạo tài khoản User thường
            if (userManager.FindByEmail("user@vidly.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "user@vidly.com",
                    Email = "user@vidly.com",
                     DrivingLicense = "USER-LICENSE-001",
                    Phone = "0987654321"
                };
                userManager.Create(user, "User@123");
                // Không gán role → là user thường
            }
        }
    }
}