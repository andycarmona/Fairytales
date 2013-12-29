namespace BookWriterTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExternalUserDEtails : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ExternalUserDetails");
            DropTable("dbo.UserProfile");
        }
    }
}
