using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineJobPortal.Models
{
    public class Job_Listing
    {
            [Key]
            public int Job_Id { get; set; }

            public int Recuriter_Id { get; set; } // Foreign key from Registration table

            [ForeignKey("Recuriter_Id")]
            public virtual Registration Registration { get; set; }

            public string Job_Tittle { get; set; }
            public string Company_Name { get; set; }
            public string Job_Location { get; set; }
            public string Job_Type { get; set; }
            public int Salary { get; set; }
            public string Description { get; set; }
            public DateTime Job_PstedDate { get; set; }
        }
}