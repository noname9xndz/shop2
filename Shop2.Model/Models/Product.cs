using Shop2.Model.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Shop2.Model.Models
{
    [Table("Products")]
    public class Product : Auditable
    {
        [Key]
        // set id tự động tăng
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        //public virtual IEnumerable<OrderDetail> OrderDetails { set; get; }
        //public virtual IEnumerable<ProductTag> ProductTags { set; get; }


        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [Required]
        [MaxLength(256)]
        [Column(TypeName ="varchar")]
        public string Alias { set; get; }

        [Required]
        public string CategoryID { set; get; }
        [ForeignKey("CategoryID")]
        public virtual ProductCategory ProductCategory { set; get; }

       
        [MaxLength(256)]
        public string Image { set; get; }
        // thuộc tính này dạng xml trong csdl
        public XElement MoreImages { set; get; }
        public decimal Price { set; get; }
        public decimal? PromotionPrice { set; get; }
        public int? Warranty { set; get; }
       
        [MaxLength(500)]
        public string Description { set; get; }
        public string Content { set; get; }

        public bool? HomeFlag { set; get; }
        public bool? HotFlag { set; get; }
        public int? ViewCount { set; get; }
        
       

    }
}