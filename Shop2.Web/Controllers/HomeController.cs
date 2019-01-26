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

        public HomeController(IProductCategoryService productCategoryService, ICommonService commonService)
        {
            _productCategoryService = productCategoryService;
            _commonService = commonService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly] //chỉ được nhúng
        public ActionResult Footer()
        {   // footer view

            var footerModel = _commonService.GetFooter();
            var footerViewModel = Mapper.Map<Footer,FooterViewModel>(footerModel);
            return PartialView(footerViewModel);
        }


        [ChildActionOnly] 
        public ActionResult Header()
        {   //Header view
            return PartialView();
        }


        [ChildActionOnly]
        public ActionResult Category()
        {    //Category view
            var model = _productCategoryService.GetAll();
            var listProductCategoryViewModel = Mapper.Map<IEnumerable< ProductCategory>,IEnumerable<ProductCategoryViewModel>>(model);
            return PartialView(listProductCategoryViewModel);
        }
    }
}