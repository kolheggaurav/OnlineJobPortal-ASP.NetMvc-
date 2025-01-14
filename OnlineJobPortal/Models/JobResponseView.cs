using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineJobPortal.Models
{
    public class JobResponseView
    {
        [Key] // Mark this as the primary key
        public int JobId { get; set; }

        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string JobType { get; set; }
        public decimal Salary { get; set; }
        public string Description { get; set; }
        public DateTime PostedDate { get; set; }
        public string Status { get; set; } // Approved, Rejected, etc.
    }
}