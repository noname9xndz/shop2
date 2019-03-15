namespace Shop2.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletePromotionPrice_OrderDetail : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OrderDetails", "PromotionPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderDetails", "PromotionPrice", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
