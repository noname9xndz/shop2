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
        public int Count
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
*/
        public IEnumerable<T> Items { set; get; }
    }
}