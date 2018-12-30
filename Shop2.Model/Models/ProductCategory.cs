using Shop2.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop2.Model.Models
{
    [Table("ProductCategorys")]
    public class ProductCategory:Auditable
    {
        [Key]
        // set id tự động tăng
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        public virtual IEnumerable<Product> Products { set; get; }

        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [Required]
        [MaxLength(256)]
        [Column(TypeName ="varchar")]
        public string Alias { set; get; }

        public string Description { set; get; }
        public int? ParnetID { set; get; }
        public int? DisplayOrder { set; get; }
        
        [MaxLength(256)]
        public string Images { set; get; }


        public bool? HomeFlag { set; get; }

        
    }
}
