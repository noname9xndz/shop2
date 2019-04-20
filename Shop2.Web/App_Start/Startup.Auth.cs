using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Shop2.Common;
using Shop2.Data;
using Shop2.Model.Models;
using Shop2.Service;
using Shop2.Web.Infrastructure.Core;
using static Shop2.Web.App_Start.IdentityConfig;

[assembly: OwinStartup(typeof(Shop2.Web.App_Start.Startup))]

namespace Shop2.Web.App_Start
{

    //cơ chế clamis là cơ chế giống như lưu session(tk,mk,tên,mail,phân quyền..) trong ứng dụng 
    //còn clamis mở rộng nó sẽ mã hóa chứa nhiều thông tin của user và có thể trao đổi qua lại với client
    // mỗi lân đăng nhập nó sẽ có token khác nhau, mỗi request gửi lên(query với phần quản trị) phải kèm theo token này vào header của request 
    // Token sẽ dc lưu sử dụng localStorageModule ,logout thì token này sẽ được xóa đi 
    //  Nếu không có token xem như user chưa đăng nhập 

    public partial class Startup
    {
        // cấu hình đăng nhập trong  ASP.NET Identity, nhớ gọi nó bên Startup
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(Shop2DbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // viết lại cơ chế login, mặc định la cơ chế login và lưu vào cookies ở đây ta dùng token



            // add PerOwinContext(Open web interface for dotnet) quản lý usermanger tượng tác với bảng user
            // PerOwinContext giúp giảm sự phụ thuôc giữa serve và application , cho phép quản lý user độc lập không phụ thuộc vào server
            app.CreatePerOwinContext<UserManager<ApplicationUser>>(CreateManager);

            // đăng nhập bằng token sử dụng cho admin
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                // tất cả request đăng nhâp đều thông qua TokenEndpointPath 
                TokenEndpointPath = new PathString("/oauth/token"),
                Provider = new AuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                // có thể validiton qua các ứng dụng client sử dụng http uri
                AllowInsecureHttp = true,

            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());


            // đăng nhập bằng cookies ,sử dụng cho client
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/dang-nhap.html"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager, DefaultAuthenticationTypes.ApplicationCookie)) // DefaultAuthenticationTypes.ApplicationCookie dùng trong đăng nhập MXH
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // chưa login thì sẽ chuyển qua authen để chứng thực

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            // add nuget : Microsoft.Owin.Security.Facebook
            app.UseFacebookAuthentication(
               appId: "394781941300120",
               appSecret: "cc0a3cce87ae425c9ff251e217979104");


            // add nuget : Microsoft.Owin.Security.Google

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "413684325958-invhtr9uthksetmpgnh2lldcm4t1n3o1.apps.googleusercontent.com",
                ClientSecret = "g9JzXCXFKOKjJOw3_4THW6EF"
            });
        }

        // viết class kế thừa từ OAuthAuthorizationServerProvider
        public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
        {

            //kiểm tra,xác thực tất cả các request gửi lên server
            public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
            {
                context.Validated();
            }
            // cho phép đăng nhập từ các domain khác
            public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
            {
                var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

                if (allowedOrigin == null) allowedOrigin = "*";

                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

                // mới 1 user theo Identity tìm kiếm user đăng nhập, password trong database
                UserManager<ApplicationUser> userManager = context.OwinContext.GetUserManager<UserManager<ApplicationUser>>();
                ApplicationUser user;
                try
                {
                    user = await userManager.FindAsync(context.UserName, context.Password);
                }
                catch
                {
                    // Could not retrieve the user due to error.
                    context.SetError("server_error");
                    context.Rejected();
                    return;
                }
                if (user != null)
                {
                    // đăng nhập thành công tạo ra 1 userclaims(chứa tất cả thông tin user) gán vào session
                    //ClaimsIdentity identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ExternalBearer);
                    //context.Validated(identity);

                    // check quyền admin
                    var applicationGroupService = ServiceFactory.Get<IApplicationGroupService>();
                    var listGroup = applicationGroupService.GetListGroupByUserId(user.Id);
                    if (listGroup.Any(x => x.Name == CommonConstants.Administrator))
                    {
                       // đăng nhập thành công tạo ra 1 userclaims(chứa tất cả thông tin user) gán vào session
                       ClaimsIdentity identity = await userManager.CreateIdentityAsync(
                                      user,
                                      DefaultAuthenticationTypes.ExternalBearer);
                        context.Validated(identity);
                    }
                    else
                    {
                        context.Rejected();
                        context.SetError("invalid_group", "Bạn không phải là admin");
                    }

                }
                else
                {

                    context.Rejected();
                    context.SetError("invalid_grant", "Tài khoản hoặc mật khẩu không đúng -__- ");
                }
            }
        }

        // phương thức tạo usermanager
        private static UserManager<ApplicationUser> CreateManager(IdentityFactoryOptions<UserManager<ApplicationUser>> options, IOwinContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context.Get<Shop2DbContext>());
            var owinManager = new UserManager<ApplicationUser>(userStore);
            return owinManager;
        }

       


        
       
     

    }
}
