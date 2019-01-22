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
    [RoutePrefix("api/productcategory")]
    [Authorize] // bắt buộc đăng nhập mói vô được
    public class ProductCategoryController : ApiControllerBase
    {
        IProductCategoryService _productCategoryService;

        public ProductCategoryController(IErrorService errorService, IProductCategoryService productCategoryService) : base (errorService)
        {
            this._productCategoryService = productCategoryService;
        }



        [Route("getall")]
        [HttpGet]
       
        public HttpResponseMessage Get(HttpRequestMessage request,string keyword,int page,int pageSize=20)
        {
            return CreateHttpResponse(request,
                () => {
                int totalRow = 0;
                    // tìm kiếm theo keyword
                var listCategory = _productCategoryService.GetAllByKeyWord(keyword);

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




        [Route("getallparents")]
        [HttpGet]
       
        public HttpResponseMessage GetAllParents(HttpRequestMessage request)
        {
            return CreateHttpResponse(request,
                () => {
                    
                    var listproductCategory = _productCategoryService.GetAll();
                    
                    var responseData = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(listproductCategory);

               
                    var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                    return response;
                });
        }




        [Route("create")]
        [HttpPost]
        [Authorize] // bắt buộc đăng nhập mói vô được
       
        public HttpResponseMessage Creat(HttpRequestMessage request,ProductCategoryViewModel productCategoryViewModel)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if(!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest,ModelState);
                }
                else
                {
                    var newProductCategory = new ProductCategory();
                    newProductCategory.UpdateProductCategory(productCategoryViewModel);
                    newProductCategory.CreatedDate = DateTime.Now;
                    newProductCategory.CreatedBy = User.Identity.Name;
                    _productCategoryService.Add(newProductCategory);
                    _productCategoryService.Save();

                    var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(newProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }
                

                return response;

             });
        }


        // lấy về id để update
        [Route("getbyid/{id:int}")]
        [HttpGet]
        
        public HttpResponseMessage GetByID(HttpRequestMessage request,int id)
        {
            return CreateHttpResponse(request,
                () => {

                    var listproductCategory = _productCategoryService.GetById(id);

                    var responseData = Mapper.Map<ProductCategory,ProductCategoryViewModel>(listproductCategory);


                    var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                    return response;
                });
        }



        [Route("update")]
        [HttpPut] 
        [AllowAnonymous]
      
        public HttpResponseMessage Update(HttpRequestMessage request, ProductCategoryViewModel productCategoryViewModel)
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
                    var dbProductCategory = _productCategoryService.GetById(productCategoryViewModel.ID);

                    dbProductCategory.UpdateProductCategory(productCategoryViewModel);
                    dbProductCategory.UpdatedDate = DateTime.Now;
                    dbProductCategory.CreatedBy = User.Identity.Name;

                    _productCategoryService.Update(dbProductCategory);
                    _productCategoryService.Save();

                    var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(dbProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }


                return response;

            });
        }

        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
        
        public HttpResponseMessage Delete(HttpRequestMessage request, ProductCategoryViewModel productCategoryViewModel,int id)
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

                   var dbProductCategory = _productCategoryService.Delete(id);
                    _productCategoryService.Save();

                    var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(dbProductCategory);
                    response = request.CreateResponse(HttpStatusCode.OK, responseData);
                }


                return response;

            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request,string checkedProductCategories)
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
                    // thư viện JavaScriptSerializer giúp ta dữ liệu text dưới dạng JSON qua một đối tượng tương ứng ở đây 
                    var listProductCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedProductCategories);
                    foreach(var item in listProductCategory)
                    {
                        _productCategoryService.Delete(item);
                    }
                   
                    _productCategoryService.Save();

          
                    response = request.CreateResponse(HttpStatusCode.OK,listProductCategory.Count);
                }


                return response;

            });
        }

    }
}