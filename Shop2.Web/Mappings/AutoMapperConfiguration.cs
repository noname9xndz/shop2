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
            
            Mapper.CreateMap<Post, PostViewModel>();
            Mapper.CreateMap<PostCategory, PostCategoryViewModel>();
            Mapper.CreateMap<Tag, TagViewModel>();

            Mapper.CreateMap<ProductCategory, ProductCategoryViewModel>();
            Mapper.CreateMap<Product, ProductViewModel>();
            Mapper.CreateMap<ProductTag, ProductTagViewModel>();
            Mapper.CreateMap<Footer, FooterViewModel>();
#pragma warning disable CS0618 // Type or member is obsolete
            Mapper.CreateMap<Slide, SlideViewModel>();
#pragma warning restore CS0618 // Type or member is obsolete
            Mapper.CreateMap<Page, PageViewModel>();
        }
    }
}