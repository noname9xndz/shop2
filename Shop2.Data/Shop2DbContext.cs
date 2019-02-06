﻿using Microsoft.AspNet.Identity.EntityFramework;
using Shop2.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop2.Data
{

    //public class Shop2DbContext : DbContext
    // thay vì add DbConText chúng ta sẽ cho kế thừa từ IdentityDbContext<ApplicationUser> để tích hợp 
    // ASP.NET Identity (1 công nghệ giúp chúng ta trong bài toán xác thực và phân quyền người dùng trên website)
    public class Shop2DbContext : IdentityDbContext<ApplicationUser>
    {
        //internal object ApplicationUser;

        public Shop2DbContext() : base("Shop2Connection")
            {
                this.Configuration.LazyLoadingEnabled = false;
            }
        // code first bằng tay
            public DbSet<Footer> Footers { set; get; }
            public DbSet<Menu> Menus { set; get; }
            public DbSet<MenuGroup> MenuGroups { set; get; }
            public DbSet<Order> Orders { set; get; }
            public DbSet<OrderDetail> OrderDetails { set; get; }
            public DbSet<Page> Pages { set; get; }
            public DbSet<Post> Posts { set; get; }
            public DbSet<PostCategory> PostCategories { set; get; }
            public DbSet<PostTag> PostTags { set; get; }
            public DbSet<Product> Products { set; get; }
            public DbSet<ProductCategory> ProductCategories { set; get; }
            public DbSet<ProductTag> ProductTags { set; get; }
            public DbSet<Slide> Slides { set; get; }
            public DbSet<SupportOnline> SupportOnlines { set; get; }
            public DbSet<SystemConfig> SystemConfigs { set; get; }
            public DbSet<Tag> Tags { set; get; }
            public DbSet<VisitorStatistic> VisitorStatistics { set; get; }
            public DbSet<Error> Errors { set; get; }
            public DbSet<ContactDetail> ContactDetails { set; get; }
            public DbSet<Feedback> Feedbacks { set; get; }

        // phương thức để tạo mới Identity 
        public static Shop2DbContext Create()
        {
            return new Shop2DbContext();
        }

        // ghi đè phương thức có sẵn OnModelCreating(DbModelBuilder modelBuilder) của DbContext
        protected override void OnModelCreating(DbModelBuilder builder)
            {
            // câu hình key cho Identity
            builder.Entity<IdentityUserRole>().HasKey(x => new { x.UserId, x.RoleId });
            builder.Entity<IdentityUserLogin>().HasKey(x => x.UserId);
            

        }
        }
    
}
