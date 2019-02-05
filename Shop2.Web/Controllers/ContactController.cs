
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
    public class ContactController : Controller
    {
       
        IContactDetailService _contactDetailService;
        public ContactController(IContactDetailService contactDetailService)
        {
            this._contactDetailService = contactDetailService;
        }

        public ActionResult Index()
        {
            var model = _contactDetailService.GetDefaultContact();
            var contactView = Mapper.Map<ContactDetail, ContactDetailViewModel>(model);
            return View(contactView);
        }
    }
}