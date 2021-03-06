﻿    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop2.Web.Models
{
    [Serializable] // :giúp biến đổi và tái tạo các đối tượng để chúng có thể được lưu trữ và trao đổi giữa các ứng dụng (bằng session)
    // ở đây shoppingcartviewmodel sử dụng đối tượng productviewmodel nên ta phải để [Serializable] 
    public class ProductViewModel
    {
        public int ID { set; get; }

        public string Name { set; get; }

        public string Alias { set; get; }

        public int CategoryID { set; get; }

        public string Image { set; get; }

        public string MoreImages { set; get; }

        public decimal Price { set; get; }

        public decimal? PromotionPrice { set; get; }

        public int? Warranty { set; get; }

        public string Description { set; get; }

        public string Content { set; get; }

        public bool? HomeFlag { set; get; }

        public bool? HotFlag { set; get; }

        public int? ViewCount { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public string MetaKeyword { set; get; }

        public string MetaDescription { set; get; }

        public bool Status { set; get; }

        public string Tags { set; get; }

        public int Quantity { set; get; }
        
        public decimal OriginalPrice { set; get; } // giá nhập

        public virtual ProductCategoryViewModel ProductCategory { set; get; }
    }
}