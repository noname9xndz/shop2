using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop2.Model.Models
{
    // kế thừa trực tiếp IdentityRole có sẵn của Identity
   public class ApplicationRole : IdentityRole // quản lý quyền của user : ví dụ admin này được edit bài viết,admin này được quản trị,....
    {
        public ApplicationRole():base()
        {

        }
        [StringLength(250)]
        public string Description { set; get; }
    }
}
