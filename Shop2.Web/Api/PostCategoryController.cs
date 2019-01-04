using AutoMapper;
using Shop2.Model.Models;
using Shop2.Service;
using Shop2.Web.Infrastructure.Core;
using Shop2.Web.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Shop2.Web.Infrastructure.Extensions;
namespace Shop2.Web.Api
{
    [RoutePrefix("api/postcategory")]
    // kế thừa ApiControllerBase ở Infrastructure>Core
    public class PostCategoryController : ApiControllerBase
    {
        /*
         quy trình : đầu tiên chạy vào ApiControllerBase sau đó nó sẽ khởi tạo
          ApiControllerBase sau đó mới quay lại PostCategoryController
             */
        IPostCategoryService _postCategoryService;
        
        // kế thừa contructor
        public PostCategoryController(IErrorService errorService,IPostCategoryService postCategoryService) : base (errorService)
        {
            this._postCategoryService = postCategoryService;
        }
        [Route("getall")] // gọi theo route được điều hướng : http://localhost:50445/api/postcategory/getall
        // select dữ liệu
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request,
                () => {
                    HttpResponseMessage response = null;
                   
                        // không có lỗi sẽ dùng  Service để lưu đối tượng vào database
                       var listCategory = _postCategoryService.GetAll();

                    // sử dụng map và trả về đối tượng đó
                    //bất cứ đối tượng tạo ở Model (Shop.Web) sử dụng phương thức thì giá trị của nó sẽ tự động đẩy sang Model (Shop.Model) 
                    var listCateoryViewModel = Mapper.Map<List<PostCategoryViewModel>>(listCategory);
     
                        // trả về đối tượng đã dược lưu để xử lý
                        response = request.CreateResponse(HttpStatusCode.OK, listCateoryViewModel);
                    
                    return response;
                });
        }


        // thêm mới đối tượng
        [Route("add")]
        public HttpResponseMessage Post(HttpRequestMessage request, PostCategoryViewModel postCategoryViewModel)
        {
            return CreateHttpResponse(request, 
                () =>{
                          HttpResponseMessage response = null;
                    // kiểm tra ngoại lệ của dữ liệu truyền vào
                    if(ModelState.IsValid)
                    {
                        // nếu có lỗi thì truyền lỗi đó vào bảng lỗi trong database
                        request.CreateErrorResponse(HttpStatusCode.BadRequest,ModelState);
                    }
                    else
                    {
                        // không có lỗi sẽ dùng  Service để lưu đối tượng vào database
                       

                        PostCategory newPostCategory = new PostCategory();
                       // bất cứ đối tượng tạo ở Model(Shop.Web) sử dụng phương thức thì giá trị của nó sẽ tự động đẩy sang Model(Shop.Model)
                        newPostCategory.UpdatePostCategory(postCategoryViewModel);
                        var category = _postCategoryService.Add(newPostCategory);
                        _postCategoryService.Save();
                        // trả về đối tượng đã dược lưu để xử lý
                        response = request.CreateResponse(HttpStatusCode.Created, category);
                    }
                          return response;
                        });
        }
        // chỉnh sửa
        [Route("update")]
        public HttpResponseMessage Put(HttpRequestMessage request, PostCategoryViewModel postCategoryViewModel)
        {
            return CreateHttpResponse(request,
                () => {
                    HttpResponseMessage response = null;
                    // kiểm tra ngoại lệ của dữ liệu truyền vào
                    if (ModelState.IsValid)
                    {
                        // nếu có lỗi thì truyền lỗi đó vào bảng lỗi trong database
                        request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    }
                    else
                    {
                        var postcatagoryDb = _postCategoryService.GetById(postCategoryViewModel.ID);
                        postcatagoryDb.UpdatePostCategory(postCategoryViewModel);
                        // không có lỗi sẽ dùng  Service để lưu đối tượng vào database
                        _postCategoryService.Update(postcatagoryDb);
                        _postCategoryService.Save();
                        // trả về đối tượng đã dược lưu để xử lý
                        response = request.CreateResponse(HttpStatusCode.OK);
                    }
                    return response;
                });
        }
        // Xóa
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request,
                () => {
                    HttpResponseMessage response = null;
                    // kiểm tra ngoại lệ của dữ liệu truyền vào
                    if (ModelState.IsValid)
                    {
                        // nếu có lỗi thì truyền lỗi đó vào bảng lỗi trong database
                        request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    }
                    else
                    {
                        // không có lỗi sẽ dùng  Service để lưu đối tượng vào database
                        _postCategoryService.Delete(id);
                        _postCategoryService.Save();
                        // trả về đối tượng đã dược lưu để xử lý
                        response = request.CreateResponse(HttpStatusCode.OK);
                    }
                    return response;
                });
        }
    }
}