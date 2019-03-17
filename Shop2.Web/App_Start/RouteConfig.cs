using System.Web.Mvc;
using System.Web.Routing;

namespace Shop2.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" }); // route cho capcha

            routes.MapRoute(
              name: "Login",
              url: "dang-nhap.html",
              defaults: new
              {
                  controller = "Account",
                  action = "Login",
                  id = UrlParameter.Optional
              },
              namespaces: new string[] { "Shop2.Web.Controllers" } // giúp tránh lỗi trùng namespace với web api
          );

            routes.MapRoute(
              name: "Register",
              url: "dang-ky.html",
              defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional },
              namespaces: new string[] { "Shop2.Web.Controllers" } // giúp tránh lỗi trùng namespace với web api
          );

            routes.MapRoute(
                name: "Search",
                url: "tim-kiem.html",
                defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
              namespaces: new string[] { "Shop2.Web.Controllers" } // giúp tránh lỗi trùng namespace với web api
            );

            routes.MapRoute(
               name: "About",
               url: "gioi-thieu.html",
               defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "Shop2.Web.Controllers" } // giúp tránh lỗi trùng namespace với web api
           );

            routes.MapRoute(
               name: "Product Category",
               url: "{Alias}.pc-{id}.html",
               defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
                 namespaces: new string[] { "Shop2.Web.Controllers" } // giúp tránh lỗi trùng namespace với web api
           );

            routes.MapRoute(
               name: "Product",
               url: "{Alias}.p-{id}.html",
               defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
                 namespaces: new string[] { "Shop2.Web.Controllers" } // giúp tránh lỗi trùng namespace với web api
           );


            routes.MapRoute(
                  name: "TagList",
                  url: "tag-{tagID}.html",
                  defaults: new { controller = "Product", action = "ListProductByTag", tagID = UrlParameter.Optional },
                    namespaces: new string[] { "Shop2.Web.Controllers" }
              );


            routes.MapRoute(
               name: "trang chủ",
               url: "trang-chu.html",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "Shop2.Web.Controllers" } // giúp tránh lỗi trùng namespace với web api
           );

            routes.MapRoute(
             name: "Page",
             url: "trang/{Alias}.html",
             defaults: new { controller = "Page", action = "Index", Alias = UrlParameter.Optional },
             namespaces: new string[] { "Shop2.Web.Controllers" } // giúp tránh lỗi trùng namespace với web api
         );
            routes.MapRoute(
             name: "Cart",
             url: "gio-hang.html",
             defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "Shop2.Web.Controllers" } // giúp tránh lỗi trùng namespace với web api
         );
          

            routes.MapRoute(
               name: "Contact",
               url: "lien-he.html",
               defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "Shop2.Web.Controllers" } // giúp tránh lỗi trùng namespace với web api
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "Shop2.Web.Controllers" } // giúp tránh lỗi trùng namespace với web api
            );

            routes.MapRoute(
              name: "Confirm Order",
              url: "xac-nhan-don-hang.html",
              defaults: new { controller = "ShoppingCart", action = "ConfirmOrder", id = UrlParameter.Optional },
              namespaces: new string[] { "TeduShop.Web.Controllers" }
             );
            routes.MapRoute(
               name: "Cancel Order",
               url: "huy-don-hang.html",
               defaults: new { controller = "ShoppingCart", action = "CancelOrder", id = UrlParameter.Optional },
               namespaces: new string[] { "TeduShop.Web.Controllers" }
              );
        }
    }
}
