
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Noname
-- Create date: 14/03/2019
-- Description:	Get Revenue, file này ch? xem con render ph?i dùng code first ?? render ra prc 
-- =============================================
CREATE PROCEDURE GetRevenueStatistic
	@formDate [nvarchar](max),
    @toDate [nvarchar](max)
AS
BEGIN
	
	SET NOCOUNT ON;

    select  o.CreatedDate as DateT,
    SUM(od.Quantity * od.Price) as Revenues, --doanh thu
    SUM((od.Quantity * od.Price) - (od.Quantity * p.OriginalPrice)) as Benefit -- l?i nhu?n
    from Orders o
    inner join OrderDetails od
    on o.ID = od.OrderID
    inner join Products p
    on od.ProductID = p.ID
    where o.CreatedDate < = CAST(@toDate as date)  and o.CreatedDate >= CAST(@formDate as date)
    group by  o.CreatedDate 
END
GO
