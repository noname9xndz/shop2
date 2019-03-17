using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using Shop2.Data;
using Shop2.Data.Infrastructure;
using Shop2.Data.Repositories;
using Shop2.Model.Models;
using Shop2.Service;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using static Shop2.Web.App_Start.IdentityConfig;

[assembly: OwinStartup(typeof(Shop2.Web.App_Start.Startup))]


namespace Shop2.Web.App_Start
{// file này sẽ chạy song song với global.asax,web.config,... khi ứng dụng được chạy thì file này mặc định sẽ được chạy

    public partial class Startup
    {
        // gọi và chạy đối tượng , IAppBuilder sẽ giúp chúng ta khởi tạo đối tượng khi ứng dụng chạy
        public void Configuration(IAppBuilder app)
        {
          
            ConfigAutofac(app);
            // chạy class StartUp.Auth
            ConfigureAuth(app);
        }

        // setup các đối tượng, khi có bất cứ 1 đối tượng nào được gọi thì hàm này sẽ tự động khởi tạo nó
        public void ConfigAutofac(IAppBuilder app)
        {
            // khai báo đối tượng
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());//Register Controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()); //Register WebApi Controllers

            // khởi tạo đối tượng UnitOfWork và dbFactory
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();

            builder.RegisterType<Shop2DbContext>().AsSelf().InstancePerRequest();

            //khởi tạo đối tượng trong Asp.net Identity
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register(c => app.GetDataProtectionProvider()).InstancePerRequest();

            // khởi tạo các đối tượng có hậu tố Repository
            builder.RegisterAssemblyTypes(typeof(PostCategoryRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            // khởi tạo các đối tượng có hậu tố Service
            builder.RegisterAssemblyTypes(typeof(PostCategoryService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            // đưa tất cả vào 1 thùng chứa
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            // thay thế cơ chế mặc định
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            // set cho webapi
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container); //Set the WebApi DependencyResolver


        }
    }
}