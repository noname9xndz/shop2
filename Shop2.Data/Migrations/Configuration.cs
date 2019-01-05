namespace Shop2.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Shop2.Model.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Shop2.Data.Shop2DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Shop2.Data.Shop2DbContext context)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new Shop2DbContext()));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new Shop2DbContext()));

            var user = new ApplicationUser()
            {
                UserName = "noname",
                Email = "noname9xnd@gmail.com",
                EmailConfirmed = true,
                BirthDay = DateTime.Now,
                FullName = "nonamexx"
            };
            manager.Create(user, "12345$");

            if(!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = manager.FindByEmail("noname9xnd@gmail.com");
            manager.AddToRoles(adminUser.Id, new string[] {"Admin","User" });
        }
    }
}
