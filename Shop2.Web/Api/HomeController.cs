using Shop2.Service;
using Shop2.Web.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Shop2.Web.Api
{
    [RoutePrefix("api/home")]
    [Authorize] // bắt buộc đăng nhập mói vô được
    public class HomeController : ApiControllerBase
    {
        IErrorService _errorService;
       public HomeController(IErrorService errorService):base(errorService)
        {
            this._errorService = errorService;
        }
        [HttpGet]
        [Route("TestMethod")]
        [Authorize] // bắt buộc đăng nhập mói vô được
        public string TestMethod()
        {
            return "Hello, Shop2 Member ";
        }
    }
}