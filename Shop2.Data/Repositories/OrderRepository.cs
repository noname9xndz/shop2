using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using Shop2.Data.Infrastructure;
using Shop2.Model.Models;
using Shop2.Common.ViewModels;

namespace Shop2.Data.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        // xử lý store procedure để thống kê doanh thu
        IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate);
    }

    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate)
        {
            var parameters = new SqlParameter[]{
                new SqlParameter("@fromDate",fromDate),
                new SqlParameter("@toDate",toDate)
            };
            // thực thi execute ten_prc @thamso,.. 
            return DbContext.Database.SqlQuery<RevenueStatisticViewModel>("GetRevenueStatistic @fromDate,@toDate", parameters);
        }
    }
}