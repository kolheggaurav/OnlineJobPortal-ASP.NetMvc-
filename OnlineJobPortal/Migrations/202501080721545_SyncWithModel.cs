namespace OnlineJobPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SyncWithModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        Application_Id = c.Int(nullable: false, identity: true),
                        Job_Id = c.Int(nullable: false),
                        Candidate_Id = c.Int(nullable: false),
                        Status = c.String(),
                        AppliedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Application_Id)
                .ForeignKey("dbo.Candidate_Profile", t => t.Candidate_Id, cascadeDelete: true)
                .ForeignKey("dbo.Approved_Jobs", t => t.Job_Id, cascadeDelete: true)
                .Index(t => t.Job_Id)
                .Index(t => t.Candidate_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Applications", "Job_Id", "dbo.Approved_Jobs");
            DropForeignKey("dbo.Applications", "Candidate_Id", "dbo.Candidate_Profile");
            DropIndex("dbo.Applications", new[] { "Candidate_Id" });
            DropIndex("dbo.Applications", new[] { "Job_Id" });
            DropTable("dbo.Applications");
        }
    }
}
