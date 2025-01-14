namespace OnlineJobPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResumeFileToCandidateProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidate_Profile", "ResumeFile", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidate_Profile", "ResumeFile");
        }
    }
}
