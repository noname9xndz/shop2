using AutoMapper;
using Shop2.Model.Models;
using Shop2.Service;
using Shop2.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop2.Web.Controllers
{
    public class HomeController : Controller
    {
        IProductCategoryService _productCategoryService;
        ICommonService _commonService;
        IProductService _productService;
        IPageService _pageService;

        public HomeController(IProductCategoryService productCategoryService, ICommonService commonService,
                        IProductService productService,IPageService pageService)
        {
            _productCategoryService = productCategoryService;
            _commonService = commonService;
            _productService = productService;
            _pageService = pageService;
        }
        [OutputCache(Duration = 60, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            var slideModel = _commonService.GetSlides();
            var slideView = Mapper.Map<IEnumerable<Slide> , IEnumerable<SlideViewModel> >(slideModel);
            var homeViewModel = new HomeViewModel();
            homeViewModel.Slides = slideView;

            var lastestProductModel = _productService.GetLastest(3);
            var topSaleProductModel = _productService.GetHotProduct(3);
            var lastestProductView = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lastestProductModel);
            var topSaleProductView = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(topSaleProductModel);

           

            homeViewModel.LastestProducts = lastestProductView;
            homeViewModel.TopSaleProducts = topSaleProductView;
         

            return View(homeViewModel);
        }

        [ChildActionOnly] //chỉ được nhúng
        [OutputCache(Duration =3600)]
        //OutputCache giúp giảm time load trang bằng cách giữ các nd thường hay truy cập để trên  ram server hay client
        // có thể setting trực tiếp trên phương thức như trên hoặc setting cache ở webconfig 
        //gg tìm hiển thêm các thuộc tính của OutputCache
        public ActionResult Footer()
        {   // footer view

            var footerModel = _commonService.GetFooter();
            var footerViewModel = Mapper.Map<Footer,FooterViewModel>(footerModel);
            return PartialView(footerViewModel);
        }


        [ChildActionOnly]
        public ActionResult Header()
        {
            var newPageModel = _pageService.GetAll();
            var newPageView = Mapper.Map<IEnumerable<Page>, IEnumerable<PageViewModel>>(newPageModel);
            return PartialView(newPageView);
        }


        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult Category()
        {    //Category view
            var model = _productCategoryService.GetAll();
            var listProductCategoryViewModel = Mapper.Map<IEnumerable< ProductCategory>,IEnumerable<ProductCategoryViewModel>>(model);
            return PartialView(listProductCategoryViewModel);
        }
    }
}