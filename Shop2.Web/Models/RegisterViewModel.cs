using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop2.Web.Models
{
    // viết dựa trên ApplicationUser 
    public class RegisterViewModel
    {
        [MaxLength(256)]
        [Required(ErrorMessage ="bạn cần nhập tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "bạn cần nhập tên đăng nhập")]
        public string UserName { set; get; }

        [Required(ErrorMessage = "bạn cần nhập mật khẩu")]
        [MinLength(6,ErrorMessage ="mật khẩu ít nhất 6 ký tự")]
        public string Password { set; get; }

        [Required(ErrorMessage = "bạn cần nhập email")]
        [EmailAddress(ErrorMessage ="địa chỉ Email không đúng")]
        public string Email { set; get; }

        public string Address { set; get; }

        [Required(ErrorMessage = "bạn cần nhập số điện thoại")]
        public string PhoneNumber { set; get; }

    }
}