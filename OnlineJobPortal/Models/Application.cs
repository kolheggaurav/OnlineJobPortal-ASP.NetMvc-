using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineJobPortal.Models
{
    public class Application
    {
        [Key]
        public int Application_Id { get; set; }

        // Foreign key to Approved_Jobs
        public int Job_Id { get; set; }
        [ForeignKey("Job_Id")]
        public virtual Approved_Jobs Job { get; set; }

        // Foreign key to Candidate_Profile
        public int Candidate_Id { get; set; }
        [ForeignKey("Candidate_Id")]
        public virtual Candidate_Profile Candidate_Profile { get; set; }

        public string Status { get; set; } // Applied / Shortlisted / Rejected
        public DateTime AppliedDate { get; set; }
    }
}