using AutoMapper;
using Shop2.Model.Models;
using Shop2.Service;
using Shop2.Web.Infrastructure.Core;
using Shop2.Web.Infrastructure.Extensions;
using Shop2.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Shop2.Web.Api
{
    [RoutePrefix("api/product")]
    //[Authorize] // bắt buộc đăng nhập mói vô được
    public class ProductController : ApiControllerBase
    {
        IProductService _productService;

        public ProductController(IErrorService errorService, IProductService productService) : base(errorService)
        {
            this._productService = productService;
        }



        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request,
                () => {
                    int totalRow = 0;
                    // tìm kiếm theo keyword
                    var listCategory = _productService.GetAllByKeyWord(keyword);

                    // lấy ra số bản ghi và thực hiện phân trang
                    totalRow = listCategory.Count();

                    var query = listCategory.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                    var listproduct = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(query);

                    var paginationSet = new PaginationSet<ProductViewModel>()
                    {
                        Items = listproduct,
                        Page = page,
                        TotalCount = totalRow,
                        // làm tròn số trang (lên )
                        TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                    };

                    var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                    return response;
                });
        }




        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAllParents(HttpRequestMessage request)
        {
            return CreateHttpResponse(request,
                () => {

                    var listproduct = _productService.GetAll();

                    var responseData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(listproduct);


                    var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                    return response;
                });
        }




        [Route("create")]
        [HttpPost]
        [AllowAnonymous] // cho phép post vào nặc danh chưa cần đăng nhập
        public HttpResponseMessage Create(HttpRequestMessage request, ProductViewModel ProductViewModel)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var newProduct = new Product();
                    newProduct.UpdateProduct(ProductViewModel);
                    newProduct.CreatedDate = DateTime.Now;
                    newProduct.CreatedBy = User.Identity.Name;
                    _productService.Add(newProduct);
                    _productService.Save();

                    var responseData = Mapper.Map<Product, ProductViewModel>(newProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }


                return response;

            });
        }


        // lấy về id để update
        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetByID(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request,
                () => {

                    var listproduct = _productService.GetById(id);

                    var responseData = Mapper.Map<Product, ProductViewModel>(listproduct);


                    var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                    return response;
                });
        }



        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductViewModel ProductViewModel)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var dbProduct = _productService.GetById(ProductViewModel.ID);

                    dbProduct.UpdateProduct(ProductViewModel);
                    dbProduct.UpdatedDate = DateTime.Now;
                    dbProduct.CreatedBy = User.Identity.Name;

                    _productService.Update(dbProduct);
                    _productService.Save();

                    var responseData = Mapper.Map<Product, ProductViewModel>(dbProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }


                return response;

            });
        }

        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage Delete(HttpRequestMessage request, ProductViewModel ProductViewModel, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {

                    var dbProduct = _productService.Delete(id);
                    _productService.Save();

                    var responseData = Mapper.Map<Product, ProductViewModel>(dbProduct);
                    response = request.CreateResponse(HttpStatusCode.OK, responseData);
                }


                return response;

            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedProducts)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    // thư viện JavaScriptSerializer giúp ta dữ liệu text dưới dạng JSON qua một đối tượng tương ứng ở đây là lấy về list id
                    var listproduct = new JavaScriptSerializer().Deserialize<List<int>>(checkedProducts);
                    foreach (var item in listproduct)
                    {
                        _productService.Delete(item);
                    }

                    _productService.Save();


                    response = request.CreateResponse(HttpStatusCode.OK, listproduct.Count);
                }


                return response;

            });
        }

    }
}