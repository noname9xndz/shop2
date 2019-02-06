using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop2.Web.Models
{
    public class FeedbackViewModel
    {
      
        public int ID { set; get; }

        [MaxLength(250,ErrorMessage ="tên không quá 250 ký tự")]
        [Required(ErrorMessage ="Tên phải nhập")]
        public string Name { set; get; }
        [MaxLength(250)]
        public string Email { set; get; }
        [MaxLength(700)]
        public string Message { set; get; }

        public DateTime CreatedDate { set; get; }
        [Required]
        public bool Status { set; get; }

        public ContactDetailViewModel ContactDetail { set; get; }
    }
}