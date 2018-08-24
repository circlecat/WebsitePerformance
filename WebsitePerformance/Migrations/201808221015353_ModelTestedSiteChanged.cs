namespace WebsitePerformance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelTestedSiteChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TestedSites", "MaxTime", c => c.Int());
            AlterColumn("dbo.TestedSites", "MinTime", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TestedSites", "MinTime", c => c.Int(nullable: false));
            AlterColumn("dbo.TestedSites", "MaxTime", c => c.Int(nullable: false));
        }
    }
}
