using AutoMapper;
using Shop2.Service;
using Shop2.Web.Infrastructure.Core;
using Shop2.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Shop2.Web.Api
{
    [RoutePrefix("api/productcategory")]
    public class ProductCategoryController : ApiControllerBase
    {
        IProductCategoryService _productCategoryService;

        public ProductCategoryController(IErrorService errorService, IProductCategoryService productCategoryService) : base (errorService)
        {
            this._productCategoryService = productCategoryService;
        }
        [Route("getall")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request,
                () => {
                    HttpResponseMessage response = null;

                    var listCategory = _productCategoryService.GetAll();
                    var listCateoryViewModel = Mapper.Map<List<ProductCategoryViewModel>>(listCategory);
                    response = request.CreateResponse(HttpStatusCode.OK, listCateoryViewModel);

                    return response;
                });
        }
    }
}