namespace GrabFit2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gender : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Gender", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Gender");
        }
    }
}
