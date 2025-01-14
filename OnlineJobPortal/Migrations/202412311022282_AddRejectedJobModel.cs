namespace OnlineJobPortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRejectedJobModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RejectedJobs",
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RejectedJobs");
        }
    }
}
