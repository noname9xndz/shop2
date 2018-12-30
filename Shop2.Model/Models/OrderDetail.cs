using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop2.Model.Models
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        [Key]
        public int OrderID { set; get; }
        [ForeignKey("OrderID")]
        public virtual Order Order { set; get; }

       
        public int ProductID { set; get; }
        [ForeignKey("ProductID")]
        public virtual Product Product { set; get; }

        public int? Quantity { set; get; }
    }
}