using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OnlineJobPortal.Models
{
    public class Job_Portal_Context : DbContext
    {
        public Job_Portal_Context()
            : base("name=Job_Portal_Context") // Ensure it matches the connection string name
        {
        }
        public DbSet<JobResponseView> JobResponseViews { get; set; }

        public DbSet<Admin_Actions> admin_Actions { get; set; }
        public DbSet<Applicationscs> applicationscs { get; set; }
        public DbSet<Candidate_Profile> candidate_Profiles { get; set; }
        public DbSet<Job_Listing> job_Listing { get; set; }
        public DbSet<Registration> registrations { get; set; }

        public DbSet<Approved_Jobs> approved_Jobs { get; set; }
        public DbSet<RejectedJob> rejected_Jobs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Application> applications { get; set; }


        // Add the OnModelCreating method
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Disable cascade delete for Applicationscs -> Registration
            modelBuilder.Entity<Applicationscs>()
                .HasRequired(a => a.Registration)
                .WithMany()
                .HasForeignKey(a => a.Candidate_Id)
                .WillCascadeOnDelete(false);

            // Disable cascade delete for Candidate_Profile -> Registration
            modelBuilder.Entity<Candidate_Profile>()
                .HasRequired(cp => cp.Registration)
                .WithMany()
                .HasForeignKey(cp => cp.User_Id)
                .WillCascadeOnDelete(false);
        }
    }
}