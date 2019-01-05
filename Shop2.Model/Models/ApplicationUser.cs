using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shop2.Model.Models
{// tạo 1 ASP.NET Identity để xem tổ chức code và cách cài đặt
   // ASP.NET Identity là 1 công nghệ giúp chúng ta trong bài toán xác thực và phân quyền người dùng trên website
    public class ApplicationUser :IdentityUser
    {
        // thêm cái thuộc tính mình cần mà IdentityUser chưa có
        [MaxLength(256)]
        public string FullName { get; set; }
        [MaxLength(256)]
        public string Address { set; get; }
        public DateTime? BirthDay { set; get; }

        //  tạo danh tính người dùng 
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            //add cơ chế quản lý thông qua cookies
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
