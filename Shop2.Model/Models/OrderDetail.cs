using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop2.Model.Models
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        [Key]
        // xử lý để khi render nó nhận được các key này
        [Column(Order =1)]
        public int OrderID { set; get; }
        [ForeignKey("OrderID")]
        public virtual Order Order { set; get; }

        [Key]
        [Column(Order = 2)]
        public int ProductID { set; get; }
        [ForeignKey("ProductID")]
        public virtual Product Product { set; get; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }
        
    }
}