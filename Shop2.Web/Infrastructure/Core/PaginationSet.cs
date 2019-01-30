using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop2.Web.Infrastructure.Core
{
    // class chung chứa thông tin phân trang cho toàn project
    public class PaginationSet<T>
    {
        public int Page { set; get; }
        public int Count // số item lấy ra
        {
            get
            {
                return (Items != null) ? Items.Count() : 0;
            }
        }
        public int TotalPages { set; get; } // tổng số trang
        public int TotalCount { set; get; } // tổng số bản ghi
        /*1 mảng IEnumerable có những thuộc tính
                 -Là một mảng read-only, chỉ có thể đọc, không thể thêm hay bớt phần tử.
                 -Chỉ duyệt theo một chiều, từ đầu tới cuối mảng
*/      public int MaxPage { set; get; } // max số trang hiển thị
        public IEnumerable<T> Items { set; get; } // list item lấy ra
    }
}
