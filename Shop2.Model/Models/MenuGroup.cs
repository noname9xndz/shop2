using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop2.Model.Models
{
    [Table("MenuGroups")]
    public class MenuGroup
    {
        [Key]
        // set id tự động tăng
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [Required]
        [MaxLength(50)]
        public string Name { set; get; }

        // chỉ ra khóa ngoại cho menu
        public virtual IEnumerable<Menu> Menus { set; get; }
    }
}