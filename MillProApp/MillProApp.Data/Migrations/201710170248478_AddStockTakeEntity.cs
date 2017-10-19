namespace MillProApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStockTakeEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockTakes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Location = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(maxLength: 128),
                        CreatedForCompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CreatedForCompanyId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.CreatedForCompanyId);
            
            AddColumn("dbo.InventoryItems", "StockTake_Id", c => c.Int());
            CreateIndex("dbo.InventoryItems", "StockTake_Id");
            AddForeignKey("dbo.InventoryItems", "StockTake_Id", "dbo.StockTakes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockTakes", "CreatedByUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.InventoryItems", "StockTake_Id", "dbo.StockTakes");
            DropForeignKey("dbo.StockTakes", "CreatedForCompanyId", "dbo.Companies");
            DropIndex("dbo.StockTakes", new[] { "CreatedForCompanyId" });
            DropIndex("dbo.StockTakes", new[] { "CreatedByUserId" });
            DropIndex("dbo.InventoryItems", new[] { "StockTake_Id" });
            DropColumn("dbo.InventoryItems", "StockTake_Id");
            DropTable("dbo.StockTakes");
        }
    }
}
