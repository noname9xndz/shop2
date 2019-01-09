using AutoMapper;
using Shop2.Model.Models;
using Shop2.Service;
using Shop2.Web.Infrastructure.Core;
using Shop2.Web.Models;
using System;
using System.Collections;
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
        public HttpResponseMessage Get(HttpRequestMessage request,int page,int pageSize=20)
        {
            return CreateHttpResponse(request,
                () => {
                int totalRow = 0;
                var listCategory = _productCategoryService.GetAll();

                // lấy ra số bản ghi và thực hiện phân trang
                totalRow = listCategory.Count();
                var query = listCategory.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                    var listproductCategory = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(query);

                    var paginationSet = new PaginationSet<ProductCategoryViewModel>()
                    {
                        Items = listproductCategory,
                        Page = page,
                        TotalCount = totalRow,
                        // làm tròn số trang (lên )
                        TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                    };
                   
                    var response = request.CreateResponse(HttpStatusCode.OK,paginationSet);
                    return response;
                });
        }
       

    }
}