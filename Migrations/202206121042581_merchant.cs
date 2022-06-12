namespace GrabFit2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class merchant : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        merchantName = c.String(),
                        itemName = c.String(),
                        imageURL = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Merchants",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        merchantName = c.String(),
                        imageURL = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Merchants");
            DropTable("dbo.Menus");
        }
    }
}
