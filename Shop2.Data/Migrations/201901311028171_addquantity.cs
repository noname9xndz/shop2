namespace Shop2.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addquantity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Quantity", c => c.Int(nullable: false));
            //phải  update dữ liêu cho cột nếu not null
            Sql("update dbo.Products set Quantity =0");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Quantity");
        }
    }
}
