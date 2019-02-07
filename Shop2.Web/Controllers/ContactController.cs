
using AutoMapper;
using Shop2.Model.Models;
using Shop2.Service;
using Shop2.Web.Infrastructure.Extensions;
using Shop2.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using System.Text;
using Shop2.Common;

namespace Shop2.Web.Controllers
{
    public class ContactController : Controller
    {
       
        IContactDetailService _contactDetailService;
        IFeedbackService _feedbackService;
        public ContactController(IContactDetailService contactDetailService,IFeedbackService feedbackService)
        {
            this._contactDetailService = contactDetailService;
            this._feedbackService = feedbackService;
        }

        public ActionResult Index()
        {
            // lấy api gg map nhớ nhúng  ~/Assets/client/controller/contact.js"

#pragma warning disable IDE0017 // Simplify object initialization
            FeedbackViewModel viewModel = new FeedbackViewModel();
#pragma warning restore IDE0017 // Simplify object initialization
            viewModel.ContactDetail = GetDetail(); // đẩy ngược vào contact view
            return View(viewModel);
        }

        private ContactDetailViewModel GetDetail()
        {
            var model = _contactDetailService.GetDefaultContact();
            var contactView = Mapper.Map<ContactDetail, ContactDetailViewModel>(model);
            return contactView;
        }

        // có thể đưa feedback và contact về 1 viewmodel  hoặc triển khai theo cách 
        // khởi tạo 1 ContactDetailViewModel trong FeedbackViewModel
        [HttpPost]
        [SimpleCaptchaValidation("CaptchaCode", "contactCaptcha", "Mã xác nhận không đúng!")] // adđ sử dụng capcha(botdetect- Nugetpacket) https://captcha.com/asp.net-captcha.html
        public ActionResult SendFeedback(FeedbackViewModel feedbackViewModel)
        {
            if(ModelState.IsValid)
            {
                Feedback feedback = new Feedback();
                feedback.UpdateFeedback(feedbackViewModel);
                _feedbackService.Create(feedback);
                _feedbackService.Save();

                ViewData["ThanhCong"] = "gửi phải hồi thành công";
               
                // đọc template mail ,ghi thông tin và gửi về website
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/client/template/ContactTemplate.html"));
                content = content.Replace("{{Name}}", feedbackViewModel.Name);
                content = content.Replace("{{Email}}", feedbackViewModel.Email);
                content = content.Replace("{{Message}}", feedbackViewModel.Message);
                // gửi mail về cho mail admin, cấu hình trong appsetting
                var adminEmail = ConfigHelper.GetByKey("AdminEmail");
                MailHelper.SendMail(adminEmail, "Thông tin liên hệ từ website", content);

                // sau khi phải hồi được gửi lên server => trả về input rỗng
                feedbackViewModel.Name = string.Empty;
                feedbackViewModel.Email = string.Empty;
                feedbackViewModel.Message = string.Empty;
            }
            feedbackViewModel.ContactDetail = GetDetail();

          

            // đẩy tiếp feedbackviewmodel vào index
            return View("Index", feedbackViewModel);
           
        }

       
    }
}