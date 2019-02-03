using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop2.Web.Models
{
    public class HomeViewModel
    {// có thể dùng viewbag thay thì  sử dụng gộp model kiểu này 
        public IEnumerable<SlideViewModel> Slides { set; get; }
        public IEnumerable<ProductViewModel> LastestProducts { set; get; }
        public IEnumerable<ProductViewModel> TopSaleProducts { set; get; }
        public IEnumerable<PageViewModel> NewPage { set; get; }
    }
}