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
    public class ApplicationUser :IdentityUser  // quản lý user
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
            //cơ chế clamis là cơ chế giống như lưu session(tk,mk,tên,mail,phân quyền..) trong ứng dụng 
            //còn clamis mở rộng nó sẽ mã hóa chứa nhiều thông tin của user và có thể trao đổi qua lại với client
            // mỗi lân đăng nhập nó sẽ có token khác nhau, mỗi request gửi lên(query với phần quản trị) phải kèm theo token này vào header của request 
            // Token sẽ dc lưu sử dụng localStorageModule ,logout thì token này sẽ được xóa đi 
            //  Nếu không có token xem như user chưa đăng nhập 

            //add cơ chế quản lý thông qua cookies
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        //thuộc tính ảo tham chiếu đển bảng order để lấy thông tin người mua hàng nếu đăng nhập
        public virtual IEnumerable<Order> Orders { set; get; }
    }
}
