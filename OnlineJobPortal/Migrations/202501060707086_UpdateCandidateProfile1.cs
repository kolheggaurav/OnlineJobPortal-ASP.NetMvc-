namespace OnlineJobPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCandidateProfile1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidate_Profile", "ResumePdf", c => c.String());
            DropColumn("dbo.Candidate_Profile", "ResumePdfUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Candidate_Profile", "ResumePdfUrl", c => c.String());
            DropColumn("dbo.Candidate_Profile", "ResumePdf");
        }
    }
}
