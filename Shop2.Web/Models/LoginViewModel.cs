using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop2.Web.Models
{    // viết dựa trên ApplicationUser chia làm 2 phần là login và register
    public class LoginViewModel
    {
        [Required(ErrorMessage = "bạn cần nhập tài khoản")]
        public string UserName { set; get; }

        [Required(ErrorMessage = "bạn cần nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { set; get; }

        public bool RememberMe { set; get; }
    }
}