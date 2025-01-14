namespace OnlineJobPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admin_Actions",
                c => new
                    {
                        ActionID = c.Int(nullable: false, identity: true),
                        AdminID = c.Int(nullable: false),
                        ActionDescription = c.String(),
                        ActionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ActionID)
                .ForeignKey("dbo.Registrations", t => t.AdminID, cascadeDelete: true)
                .Index(t => t.AdminID);
            
            CreateTable(
                "dbo.Registrations",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        password = c.String(nullable: false),
                        your_Role = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.Applicationscs",
                c => new
                    {
                        Application_Id = c.Int(nullable: false, identity: true),
                        Job_Id = c.Int(nullable: false),
                        Candidate_Id = c.Int(nullable: false),
                        Status = c.String(),
                        AppliedDate = c.DateTime(nullable: false),
                        Candidate_Profile_Profile_Id = c.Int(),
                        Job_Listing_Job_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Application_Id)
                .ForeignKey("dbo.Candidate_Profile", t => t.Candidate_Profile_Profile_Id)
                .ForeignKey("dbo.Job_Listing", t => t.Job_Listing_Job_Id)
                .ForeignKey("dbo.Job_Listing", t => t.Job_Id, cascadeDelete: true)
                .ForeignKey("dbo.Registrations", t => t.Candidate_Id)
                .Index(t => t.Job_Id)
                .Index(t => t.Candidate_Id)
                .Index(t => t.Candidate_Profile_Profile_Id)
                .Index(t => t.Job_Listing_Job_Id);
            
            CreateTable(
                "dbo.Candidate_Profile",
                c => new
                    {
                        Profile_Id = c.Int(nullable: false, identity: true),
                        User_Id = c.Int(nullable: false),
                        Name = c.String(),
                        Gender = c.String(),
                        Contact_No = c.String(),
                        Address = c.String(),
                        Carrear_Prefrence = c.String(),
                        Highest_Qalification = c.String(),
                        Colleage_name = c.String(),
                        Per_Or_CGPA = c.Double(nullable: false),
                        Pass_Year = c.Int(nullable: false),
                        key_Skills = c.String(),
                        Project_Name = c.String(),
                        Details = c.String(),
                        Upload_Resume = c.String(),
                    })
                .PrimaryKey(t => t.Profile_Id)
                .ForeignKey("dbo.Registrations", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Job_Listing",
                c => new
                    {
                        Job_Id = c.Int(nullable: false, identity: true),
                        Recuriter_Id = c.Int(nullable: false),
                        Job_Tittle = c.String(),
                        Company_Name = c.String(),
                        Job_Location = c.String(),
                        Job_Type = c.String(),
                        Salary = c.Int(nullable: false),
                        Description = c.String(),
                        Job_PstedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Job_Id)
                .ForeignKey("dbo.Registrations", t => t.Recuriter_Id, cascadeDelete: true)
                .Index(t => t.Recuriter_Id);
            
            CreateTable(
                "dbo.Approved_Jobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Job_Tittle = c.String(),
                        Company_Name = c.String(),
                        Job_Location = c.String(),
                        Job_Type = c.String(),
                        Salary = c.Int(nullable: false),
                        Description = c.String(),
                        Job_PstedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JobResponseViews",
                c => new
                    {
                        JobId = c.Int(nullable: false, identity: true),
                        JobTitle = c.String(),
                        CompanyName = c.String(),
                        Location = c.String(),
                        JobType = c.String(),
                        Salary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        PostedDate = c.DateTime(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.JobId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Applicationscs", "Candidate_Id", "dbo.Registrations");
            DropForeignKey("dbo.Applicationscs", "Job_Id", "dbo.Job_Listing");
            DropForeignKey("dbo.Applicationscs", "Job_Listing_Job_Id", "dbo.Job_Listing");
            DropForeignKey("dbo.Job_Listing", "Recuriter_Id", "dbo.Registrations");
            DropForeignKey("dbo.Applicationscs", "Candidate_Profile_Profile_Id", "dbo.Candidate_Profile");
            DropForeignKey("dbo.Candidate_Profile", "User_Id", "dbo.Registrations");
            DropForeignKey("dbo.Admin_Actions", "AdminID", "dbo.Registrations");
            DropIndex("dbo.Job_Listing", new[] { "Recuriter_Id" });
            DropIndex("dbo.Candidate_Profile", new[] { "User_Id" });
            DropIndex("dbo.Applicationscs", new[] { "Job_Listing_Job_Id" });
            DropIndex("dbo.Applicationscs", new[] { "Candidate_Profile_Profile_Id" });
            DropIndex("dbo.Applicationscs", new[] { "Candidate_Id" });
            DropIndex("dbo.Applicationscs", new[] { "Job_Id" });
            DropIndex("dbo.Admin_Actions", new[] { "AdminID" });
            DropTable("dbo.JobResponseViews");
            DropTable("dbo.Approved_Jobs");
            DropTable("dbo.Job_Listing");
            DropTable("dbo.Candidate_Profile");
            DropTable("dbo.Applicationscs");
            DropTable("dbo.Registrations");
            DropTable("dbo.Admin_Actions");
        }
    }
}
