using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace OnlineJobPortal.Models
{
    public class Candidate_Profile
    {
        [Key] 
        public int Profile_Id { get; set; }
        public int User_Id { get; set; }  //(Foreign key from registration table)
        // Navigation property
        [ForeignKey("User_Id")] public virtual Registration Registration { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Contact_No { get; set; }
        public string Address { get; set; }
        public string Carrear_Prefrence { get; set; }//job or Internshippublic string Preferd_Work_location { get; set; }
        public string Highest_Qalification { get; set; }
        public string Colleage_name { get; set; }
        public Double Per_Or_CGPA { get; set; }
        public int Pass_Year { get; set; }
        public string key_Skills { get; set; }
        public string Project_Name { get; set; }
        public string Details { get; set; }

        // Use HttpPostedFileBase for file handling
        public string ResumePdf { get; set; }
        public byte[] ResumeFile { get; set; }

    }
}