using AutoMapper;
using Shop2.Model.Models;
using Shop2.Service;
using Shop2.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop2.Web.Controllers
{
    public class PageController : Controller
    {
        IPageService _pageService;
        public PageController(IPageService pageService)
        {
            this._pageService = pageService;
        }
        // GET: Page
        public ActionResult Index(string Alias)
        {
            var page = _pageService.GetPageByAlias(Alias);
            var model = Mapper.Map<Page, PageViewModel>(page);
            return View(model);
        }
    }
}