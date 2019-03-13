using BotDetect.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Shop2.Common;
using Shop2.Model.Models;
using Shop2.Web.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Shop2.Web.App_Start.IdentityConfig;

namespace Shop2.Web.Controllers
{
    public class AccountController : Controller
    {
        // xem lại bên api 
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: Account
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl; // trả về url trước khi đăng nhập
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                // tìm kiếm tài khoản
                ApplicationUser user =await _userManager.FindAsync(loginViewModel.UserName, loginViewModel.Password);

                if(user !=null)
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie); // nếu đã đăng nhập thì thoát và xóa cookie có sẵn
                     // tạo ClaimsIdentity chứa thông tin đăng nhập người dùng lưu vào cookies
                    ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
#pragma warning disable IDE0017 // Simplify object initialization
                    AuthenticationProperties properties = new AuthenticationProperties();
#pragma warning restore IDE0017 // Simplify object initialization
                               // nếu chọn ghi nhớ thì sẽ lưu lâu dài
                    properties.IsPersistent = loginViewModel.RememberMe;
                    // đăng nhập
                    authenticationManager.SignIn(properties, identity);
                    if(Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "tài khoản hoặc mật khẩu không đúng");
                }

            }
            
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }



        [HttpGet]
        public ActionResult Register()
        {
            
            return View();
        }

        [HttpPost]
        [CaptchaValidation("CaptchaCode","registerCaptcha", "Mã xác nhận không đúng!")] // adđ sử dụng capcha(botdetect- Nugetpacket) https://captcha.com/asp.net-captcha.html
        // viết bất đồng bộ để check nhanh hơn
        public async Task<ActionResult> Register(RegisterViewModel registerViewModel)
        {

            if (ModelState.IsValid)
            {
                // check thông tin trùng
                var userCheckEmail = await _userManager.FindByEmailAsync(registerViewModel.Email);
                if(userCheckEmail != null)
                {
                    ModelState.AddModelError("Email","Email đã tồn tại");
                    return View(registerViewModel);
                }

                var userCheckName = await _userManager.FindByNameAsync(registerViewModel.UserName);
                if (userCheckName != null)
                {
                    ModelState.AddModelError("UserName", "tài khoản đã tồn tại");
                    return View(registerViewModel);
                }

                // thêm mới tài khoản
                var user = new ApplicationUser()
                {
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Email,
                    EmailConfirmed = true,
                    FullName = registerViewModel.FullName,
                    Address = registerViewModel.Address
                };
                await _userManager.CreateAsync(user, registerViewModel.Password);


                // add tài khoản vào db
                var addUser = await _userManager.FindByIdAsync(registerViewModel.Email);
                if (addUser != null)
                {
                   await _userManager.AddToRolesAsync(addUser.Id, new string[] { "User" });
                }

                // gửi mail báo thành công
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/client/template/newUser.html"));
                content = content.Replace("{{UserName}}", addUser.FullName);
                // nhớ cấu hình lại CurrentLink trong appsetting 
                var LinkWeb = ConfigHelper.GetByKey("CurrentLink");
                content = content.Replace("{{Link}}", LinkWeb + "dang-nhap.html");
                MailHelper.SendMail(addUser.Email, "đăng ký thành công", content);

                TempData["Ketqua"] = "Đăng ký thành công";
              

            }
            else
            {
                MvcCaptcha.ResetCaptcha("registerCaptcha");
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }


    }
}