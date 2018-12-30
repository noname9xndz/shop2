// using thư viện
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop2.Model.Models
{
    [Table("Footers")] //giúp khi buid sẽ có tên bảng là footers
    public class Footer
    {
        [Key] // khóa chính của
              // set id tự động tăng
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [MaxLength(50)] // tránh việc tốn bộ nhớ
        public string ID { set; get; }

        [Required] // string và không cho phép null
                  // int? cho phép null 
        public string Content { set; get; }
    }
}