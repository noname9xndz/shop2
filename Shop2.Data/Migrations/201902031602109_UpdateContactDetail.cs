namespace Shop2.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateContactDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactDetails", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.ContactDetails", "CreatedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.ContactDetails", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.ContactDetails", "UpdatedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.ContactDetails", "MetaKeyword", c => c.String(maxLength: 256));
            AddColumn("dbo.ContactDetails", "MetaDescription", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContactDetails", "MetaDescription");
            DropColumn("dbo.ContactDetails", "MetaKeyword");
            DropColumn("dbo.ContactDetails", "UpdatedBy");
            DropColumn("dbo.ContactDetails", "UpdatedDate");
            DropColumn("dbo.ContactDetails", "CreatedBy");
            DropColumn("dbo.ContactDetails", "CreatedDate");
        }
    }
}
