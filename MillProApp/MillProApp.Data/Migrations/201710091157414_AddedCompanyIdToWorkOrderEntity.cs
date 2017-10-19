namespace MillProApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCompanyIdToWorkOrderEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkOrders", "CreatedForCompanyId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkOrders", "CreatedForCompanyId");
        }
    }
}
