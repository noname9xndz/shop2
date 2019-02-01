using AutoMapper;
using Shop2.Common;
using Shop2.Model.Models;
using Shop2.Service;
using Shop2.Web.Infrastructure.Core;
using Shop2.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Shop2.Web.Controllers
{
    public class ProductController : Controller
    {
        IProductService _productService;
        IProductCategoryService _productCategoryService;
        

        public ProductController(IProductService productService, IProductCategoryService productCategoryService)
        {
            this._productService = productService;
            this._productCategoryService = productCategoryService;
        }
        // GET: Product
        public ActionResult Detail(int id)
        {
            var productModel = _productService.GetById(id);
            var productView = Mapper.Map<Product,ProductViewModel>(productModel);

            var relatedProduct = _productService.GetReatedProducts(id, 4);
            ViewBag.RelatedProducts = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(relatedProduct);

            var moreImages = productView.MoreImages;
            List<string> listImage = new JavaScriptSerializer().Deserialize<List<string>>(moreImages);
            ViewBag.MoreImages = listImage;

            var tags = _productService.GetListTagByProductId(id);
            ViewBag.Tags = Mapper.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(tags);

            return View(productView);
        }
       
        public ActionResult Category(int id, int page = 1,string sort="")
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var productModel = _productService.GetListProductByCategoryIdPaging(id, page, pageSize,out totalRow,sort);
            var productView = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);


            int totalPage = (int)(Math.Ceiling((double)totalRow / pageSize));
            var category = _productCategoryService.GetById(id);
            ViewBag.Category = Mapper.Map<ProductCategory, ProductCategoryViewModel>(category);
            var paginationSet = new PaginationSet<ProductViewModel>()
            {

                Items = productView,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage


            };

            return View(paginationSet);
        }
        public JsonResult GetListProductByName(string keyword)
        {
            var productModel = _productService.GetListProductByName(keyword);
            return Json(new
            {
                data = productModel

            },JsonRequestBehavior.AllowGet);

        }
        public ActionResult Search(string keyword, int page = 1, string sort = "")
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var productModel = _productService.SearchProductByName(keyword, page, pageSize, out totalRow, sort);
            var productView = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);


            int totalPage = (int)(Math.Ceiling((double)totalRow / pageSize));

            ViewBag.Keyword = keyword;
            var paginationSet = new PaginationSet<ProductViewModel>()
            {

                Items = productView,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage


            };

            return View(paginationSet);
        }
        public ActionResult ListProductByTag(string tagID, int page = 1, string sort = "")
        {

            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var productModel = _productService.GetListProductByTag(tagID, page, pageSize, out totalRow,sort);
            var productView = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);


            int totalPage = (int)(Math.Ceiling((double)totalRow / pageSize));

            var tag = _productService.GetTag(tagID);
            ViewBag.Tag= Mapper.Map<Tag,TagViewModel>(tag);

            var paginationSet = new PaginationSet<ProductViewModel>()
            {

                Items = productView,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage


            };

            return View(paginationSet);
        }
    }
}
