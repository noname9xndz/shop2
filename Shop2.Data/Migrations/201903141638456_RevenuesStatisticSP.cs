namespace Shop2.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RevenuesStatisticSP : DbMigration
    {
        public override void Up()
        {// add store prc
            CreateStoredProcedure("GetRevenueStatistic",
              p => new
              {// add parameter
                  formDate = p.String(),
                  toDate = p.String()
              },
                // câu truy vấn
                @"
             select  o.CreatedDate as DateT,
                  SUM(od.Quantity * od.Price) as Revenues, --doanh thu
                  SUM((od.Quantity * od.Price) - (od.Quantity * p.OriginalPrice)) as Benefit -- lợi nhuận
              from Orders o
              inner join OrderDetails od
              on o.ID = od.OrderID
              inner join Products p
              on od.ProductID = p.ID
              where o.CreatedDate < = CAST(@toDate as date)  and o.CreatedDate >= CAST(@formDate as date)
              group by  o.CreatedDate "

                );
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.GetRevenueStatistic");
        }
    }
}
