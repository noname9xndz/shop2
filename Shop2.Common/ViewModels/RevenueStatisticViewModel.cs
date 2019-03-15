using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop2.Common.ViewModels
{
    public class RevenueStatisticViewModel //thống kê doanh thu
    {// trong db là float bên ngoài có thể để double,decimal ngoài là decimal
        public DateTime Date { set; get; }
        public decimal Revenues { set; get; } // doanh thu
        public decimal Benefit { set; get; } // lợi nhuận
    }
}
