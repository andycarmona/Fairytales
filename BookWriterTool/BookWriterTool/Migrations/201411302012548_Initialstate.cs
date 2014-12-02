namespace BookWriterTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialstate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExternalUserDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        Name = c.String(),
                        PageLink = c.String(),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.ID);
            
            //CreateTable(
            //    "dbo.UserProfile",
            //    c => new
            //        {
            //            UserId = c.Int(nullable: false, identity: true),
            //            UserName = c.String(),
            //        })
            //    .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserProfile");
            DropTable("dbo.ExternalUserDetails");
        }
    }
}
