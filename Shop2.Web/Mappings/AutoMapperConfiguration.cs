using AutoMapper;
using Shop2.Model.Models;
using Shop2.Web.Models;

namespace Shop2.Web.Mappings
{
    // nhớ khai báo hàm này vào Global.asax
    public class AutoMapperConfiguration
    {
        //Bản chất ở đây chúng at sẽ map 1 class trong shop.Model với 1 ViewModel trong shop.Web
        public static void Configure()
        {


            Mapper.Initialize(cfg => {

                
                cfg.CreateMap<Post, PostViewModel>();
                cfg.CreateMap<PostCategory, PostCategoryViewModel>();
                cfg.CreateMap<Tag, TagViewModel>();
                cfg.CreateMap<ProductCategory, ProductCategoryViewModel>();
                cfg.CreateMap<Product, ProductViewModel>();
                cfg.CreateMap<ProductTag, ProductTagViewModel>();
                cfg.CreateMap<Footer, FooterViewModel>();
                cfg.CreateMap<Slide, SlideViewModel>();
                cfg.CreateMap<Page, PageViewModel>();
                cfg.CreateMap<ContactDetail, ContactDetailViewModel>();
                cfg.CreateMap<Feedback, FeedbackViewModel>();

                cfg.CreateMap<ApplicationGroup, ApplicationGroupViewModel>();
                cfg.CreateMap<ApplicationRole, ApplicationRoleViewModel>();
                cfg.CreateMap<ApplicationUser, ApplicationUserViewModel>();

            });

        }
    }
}