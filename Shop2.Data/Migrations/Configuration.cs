namespace Shop2.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Shop2.Common;
    using Shop2.Model.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Shop2DbContext>
    {
        public Configuration()
        {
            
            AutomaticMigrationsEnabled = true; // true lần đầu tiên chạy sẽ chạy cùng với phương thức Seed,update-database sẽ tự nhận được sự thay đổi nếu có
        }
        // tạo mới các dữ liệu mẫu
        protected override void Seed(Shop2DbContext context)
        {
            CreateProductCategory(context);
            CreateApplicationUser(context);
            CreateProduct(context);
            CreateSlide(context);
            CreatePage(context);
            CreateContact(context);
            CreateConfigTitle(context);
            CreateFooter(context);

        }
        private void CreateApplicationUser(Shop2DbContext context)
        {
            if(context.Users.Any())
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
            

        }
        private void CreateProductCategory(Shop2DbContext context)
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
        private void CreateProduct(Shop2DbContext context)
        {
            if (context.Products.Count() == 0)
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
        private void CreateSlide(Shop2DbContext context)
        {
            if (context.Slides.Count() == 0)
            {
                List<Slide> listSilde = new List<Slide>()
                {
                    new Slide() {
                        Name ="slide 1",
                        DisplayOrder =1,
                        Status =true,
                        URL ="#",
                        Image ="/Assets/client/images/bag.jpg" ,
                        Description="@@",
                        Content =@"<h2>FLAT 50% 0FF</h2>
                                <label>FOR ALL PURCHASE <b>VALUE</b></label>
                                <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et </p>
                                <span class=""on-get"">GET NOW</span>"},

                    new Slide() { Name="slide 2",DisplayOrder=2,Status=true,URL="#",Image="/Assets/client/images/bag1.jpg",Description="@@",Content=@"<h2>FLAT 50% 0FF</h2>
                                <label>FOR ALL PURCHASE <b>VALUE</b></label>
                                <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et </p>
                                <span class=""on-get"">GET NOW</span>"}
                };
                context.Slides.AddRange(listSilde);
                context.SaveChanges();
            }
        }

        private void CreatePage(Shop2DbContext context)
        {
            if (context.Pages.Count() == 0)
            {
                try
                {
                    var page = new Page()
                    {
                        Name = "tạo mới trang",
                        Alias = "trang-moi",
                        Content = @"test nôi dung mẫu : ểwrqwerweqrewrewrqwerqewrwer",
                        Status = true

                    };
                    context.Pages.Add(page);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        }
                    }
                }

            }
        }

        private void CreateContact(Shop2DbContext context)
        {
            if (context.ContactDetails.Count() == 0)
            {
                try
                {
                    var contactDetail = new Shop2.Model.Models.ContactDetail()
                    {

                        Name = "Shop xxdsf",
                        Address = "43 Nguyễn Chí Thanh",
                        Email = "noname9xnd@gmail.com",
                        Lat = 21.0633645,
                        Lng = 105.8053274,
                        Phone = "0969696969",
                        Website = "xxxx.com",
                        Other = "thông tin thêm",
                        Status = true

                    };
                    context.ContactDetails.Add(contactDetail);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        }
                    }
                }

            }
        }

        private void CreateConfigTitle(Shop2DbContext context)
        {
            try
            {
                if (context.SystemConfigs.Any(x => x.Code == "HomeTitle"))
                {
                    context.SystemConfigs.Add(new SystemConfig()
                    {
                        Code = "HomeTitle",
                        ValueString = "Trang chủ NonameShop"
                    });
                }
                if (context.SystemConfigs.Any(x => x.Code == "MetaKeyword"))
                {
                    context.SystemConfigs.Add(new SystemConfig()
                    {
                        Code = "HomeMetaKeyword",
                        ValueString = "Trang chủ NonameShop"
                    });
                }
                if (context.SystemConfigs.Any(x => x.Code == "MetaDescription"))
                {
                    context.SystemConfigs.Add(new SystemConfig()
                    {
                        Code = "HomeMetaDescription",
                        ValueString = "Trang chủ NonameShop"
                    });
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }
            }


        }
        private void CreateFooter(Shop2DbContext context)
        {
            if (context.Footers.Count(x => x.ID == CommonConstants.DefaultFooterId) == 0)
            {
                string content = @"Footer";

            //    < div class="footer-bottom-cate">
            //    <h6>CATEGORIES</h6>
            //    <ul>
            //        <li><a href = "#" > Curabitur sapien</a></li>
            //        <li><a href = "#" > Dignissim purus</a></li>
            //        <li><a href = "#" > Tempus pretium</a></li>
            //        <li><a href = "#" > Dignissim neque</a></li>
            //        <li><a href = "#" > Ornared id aliquet</a></li>
            //        <li><a href = "#" > Ultrices id du</a></li>
            //        <li><a href = "#" > Commodo sit</a></li>
            //        <li><a href = "#" > Urna ac tortor sc</a></li>
            //        <li><a href = "#" > Ornared id aliquet</a></li>
            //        <li><a href = "#" > Urna ac tortor sc</a></li>
            //        <li><a href = "#" > Eget nisi laoreet</a></li>
            //        <li><a href = "#" > Faciisis ornare</a></li>
            //    </ul>
            //</div>
            //<div class="footer-bottom-cate bottom-grid-cat">
            //    <h6>FEATURE PROJECTS</h6>
            //    <ul>
            //        <li><a href = "#" > Curabitur sapien</a></li>
            //        <li><a href = "#" > Dignissim purus</a></li>
            //        <li><a href = "#" > Tempus pretium</a></li>
            //        <li><a href = "#" > Dignissim neque</a></li>
            //        <li><a href = "#" > Ornared id aliquet</a></li>
            //        <li><a href = "#" > Ultrices id du</a></li>
            //        <li><a href = "#" > Commodo sit</a></li>
            //    </ul>
            //</div>
            //<div class="footer-bottom-cate">
            //    <h6>TOP BRANDS</h6>
            //    <ul>
            //        <li><a href = "#" > Curabitur sapien</a></li>
            //        <li><a href = "#" > Dignissim purus</a></li>
            //        <li><a href = "#" > Tempus pretium</a></li>
            //        <li><a href = "#" > Dignissim neque</a></li>
            //        <li><a href = "#" > Ornared id aliquet</a></li>
            //        <li><a href = "#" > Ultrices id du</a></li>
            //        <li><a href = "#" > Commodo sit</a></li>
            //        <li><a href = "#" > Urna ac tortor sc</a></li>
            //        <li><a href = "#" > Ornared id aliquet</a></li>
            //        <li><a href = "#" > Urna ac tortor sc</a></li>
            //        <li><a href = "#" > Eget nisi laoreet</a></li>
            //        <li><a href = "#" > Faciisis ornare</a></li>
            //    </ul>
            //</div>
            //<div class="footer-bottom-cate cate-bottom">
            //    <h6>OUR ADDERSS</h6>
            //    <ul>
            //        <li>Aliquam metus  dui. </li>
            //        <li>orci, ornareidquet</li>
            //        <li> ut, DUI.</li>
            //        <li>nisi, dignissim</li>
            //        <li>gravida at.</li>
            //        <li class="phone">PH : 6985792466</li>
            //        <li class="temp"> <p class="footer-class">Design by<a href= "http://w3layouts.com/" target= "_blank" > W3layouts </ a > </ p ></ li >
            //    </ ul >
            //</ div >

                context.Footers.Add(new Footer()
                {
                    ID = CommonConstants.DefaultFooterId,
                    Content = content
                });
                context.SaveChanges();
            }
        }
    }
}
