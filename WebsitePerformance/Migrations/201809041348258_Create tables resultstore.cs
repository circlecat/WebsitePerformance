namespace WebsitePerformance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Createtablesresultstore : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PageResponses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        ResponseTime = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sites", t => t.SiteId, cascadeDelete: true)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        Site_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sites", t => t.Site_Id)
                .Index(t => t.Site_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pages", "Site_Id", "dbo.Sites");
            DropForeignKey("dbo.PageResponses", "SiteId", "dbo.Sites");
            DropIndex("dbo.Pages", new[] { "Site_Id" });
            DropIndex("dbo.PageResponses", new[] { "SiteId" });
            DropTable("dbo.Pages");
            DropTable("dbo.Sites");
            DropTable("dbo.PageResponses");
        }
    }
}
