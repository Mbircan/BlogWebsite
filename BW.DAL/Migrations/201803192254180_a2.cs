namespace BW.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Articles", "Keywords");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articles", "Keywords", c => c.String());
        }
    }
}
