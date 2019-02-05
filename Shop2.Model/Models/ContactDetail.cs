using Shop2.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop2.Model.Models
{
    [Table("ContactDetails")]
    public class ContactDetail : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [StringLength(250)]
        [Required]
        public string Name { set; get; }

        [StringLength(250)]
        public string Phone { set; get; }
        [StringLength(250)]
        public string Email { set; get; }
        [StringLength(250)]
        public string Website { set; get; }
        [StringLength(250)]
        public string Address { set; get; }

        public string Other { set; get; }

        //[DefaultValue(0)] có thể set giá trị mặc định cho các trường
        public double? Lat { set; get; } // kinh độ 
        public double? Lng { set; get; } // vĩ độ

    }
}
