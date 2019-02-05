using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop2.Web.Models
{
    public class ContactDetailViewModel
    {
        [Key]
        public int ID { set; get; }

        [StringLength(250, ErrorMessage = "tên không vượt quá 250 ký tự")]
        [Required]
        public string Name { set; get; }

        [StringLength(250,ErrorMessage ="số điện thoại không vượt quá 250 ký tự")]
        public string Phone { set; get; }
        [StringLength(250, ErrorMessage = "mail không vượt quá 250 ký tự")]
        public string Email { set; get; }
        [StringLength(250, ErrorMessage = "tên website không vượt quá 250 ký tự")]
        public string Website { set; get; }
        [StringLength(250, ErrorMessage = "adress không vượt quá 250 ký tự")]
        public string Address { set; get; }

        public string Other { set; get; }

 
        public double? Lat { set; get; } // kinh độ 
        public double? Lng { set; get; } // vĩ độ

        public DateTime? CreatedDate { set; get; }


        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }


        public string UpdatedBy { set; get; }


        public string MetaKeyword { set; get; }

        public string MetaDescription { set; get; }

        [Required(ErrorMessage = "Yêu cầu nhập trạng thái")]
        public bool Status { set; get; }

    }
}