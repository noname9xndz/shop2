using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop2.Web.Models
{
    [Serializable] // :giúp biến đổi và tái tạo các đối tượng để chúng có thể được lưu trữ và trao đổi giữa các ứng dụng
    // cụ thể ở đây ta sẽ gán đói tượng shoppingcartviewmodel vào session
    public class ShoppingCartViewModel
    {
        public int ProductId { set; get; }
        public ProductViewModel Product { set; get; }
        public int Quantity { set; get; }
    }
}