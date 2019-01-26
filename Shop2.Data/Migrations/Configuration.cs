namespace Shop2.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Shop2.Common;
    using Shop2.Model.Models;
    using System;
    using System.Collections.Generic;
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
            CreateProductCategory(context);
            //CreateApplicationUser(context);
            CreateProduct(context);


        }
        private void CreateApplicationUser(Shop2.Data.Shop2DbContext context)
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

            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = manager.FindByEmail("noname9xnd@gmail.com");
            manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });

        }
        private void CreateProductCategory(Shop2.Data.Shop2DbContext context)
        {
            if (context.ProductCategories.Count() == 0)
            {
                List<ProductCategory> listProductCategory = new List<ProductCategory>()
            {

                new ProductCategory()
                {
                    Name="Điện Tử",
                    Alias="dien-tu",
                    Status=true
                },
                 new ProductCategory(){ Name = "Điện Lạnh",Alias = "dien-lanh",Status = false},
                 new ProductCategory(){ Name = "Viễn Thông",Alias = "vien-thong",Status = true},
                 new ProductCategory(){ Name = "Đồ Gia Dụng",Alias = "do-gia-dung",Status = true},
                 new ProductCategory(){ Name = "Mỹ Phẩm",Alias = "my-pham",Status = true},
            };
                context.ProductCategories.AddRange(listProductCategory);
                context.SaveChanges();
            }
        }
        private void CreateProduct(Shop2.Data.Shop2DbContext context)
        {
            if(context.Products.Count()==0)
            {
                List<Product> listProduct = new List<Product>()
                {
                    new Product(){Name="sam sung s7",Alias="sam-sung-s7",CategoryID=1,Price=3232,Status=true,Tags="samsung"},
                    new Product(){Name="xiaomi mi max",Alias="mi-max",CategoryID=3,Price=3434232,Status=false,Tags="xiaomi"}
                };
                context.Products.AddRange(listProduct);
                context.SaveChanges();
            }
        }
        //private void CreateFooter(Shop2.Data.Shop2DbContext context)
        //{
        //    if (context.Footers.Count(x=>x.ID==CommonConstants.DefaultFooterID) == 0)
        //    {
        //        string content = "";
        //        //context.Products.AddRange(content);
        //        //context.SaveChanges();
        //    }
        //}
    }
}
