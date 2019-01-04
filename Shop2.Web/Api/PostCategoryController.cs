using Shop2.Model.Models;
using Shop2.Service;
using Shop2.Web.Infrastructure.Core;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
        [Route("getall")]
        // select dữ liệu
        public HttpResponseMessage Get(HttpRequestMessage request)
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
                       var listCategory = _postCategoryService.GetAll();
     
                        // trả về đối tượng đã dược lưu để xử lý
                        response = request.CreateResponse(HttpStatusCode.OK,listCategory);
                    }
                    return response;
                });
        }


        // thêm mới đối tượng
        public HttpResponseMessage Post(HttpRequestMessage request,PostCategory postCategory)
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
                        var category = _postCategoryService.Add(postCategory);
                        _postCategoryService.Save();
                        // trả về đối tượng đã dược lưu để xử lý
                        response = request.CreateResponse(HttpStatusCode.Created, category);
                    }
                          return response;
                        });
        }
        // chỉnh sửa
        public HttpResponseMessage Put(HttpRequestMessage request, PostCategory postCategory)
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
                        _postCategoryService.Add(postCategory);
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