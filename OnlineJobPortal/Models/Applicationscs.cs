using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineJobPortal.Models
{
    public class Applicationscs
    {
        [Key]
        public int Application_Id { get; set; }

        public int Job_Id { get; set; }  // Foreign key from Job_Listing table
        [ForeignKey("Job_Id")]
        public virtual Job_Listing JobListing { get; set; }

        public int Candidate_Id { get; set; } // Foreign key from Registration table
        [ForeignKey("Candidate_Id")]
        public virtual Registration Registration { get; set; }
        public Candidate_Profile Candidate_Profile { get; set; }
        public Job_Listing Job_Listing { get; set; }
        public string Status { get; set; } // Applied / Shortlisted / Rejected
        public DateTime AppliedDate { get; set; }
    }
}