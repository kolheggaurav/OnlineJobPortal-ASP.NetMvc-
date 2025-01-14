namespace OnlineJobPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCandidateProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidate_Profile", "ResumePdfUrl", c => c.String());
            DropColumn("dbo.Candidate_Profile", "Upload_Resume");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Candidate_Profile", "Upload_Resume", c => c.String());
            DropColumn("dbo.Candidate_Profile", "ResumePdfUrl");
        }
    }
}
