using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineJobPortal.Models
{
    public class RejectedJob
    {
        public int Id { get; set; }
        public string Job_Tittle { get; set; }
        public string Company_Name { get; set; }
        public string Job_Location { get; set; }
        public string Job_Type { get; set; }
        public int Salary { get; set; }
        public string Description { get; set; }
        public DateTime Job_PstedDate { get; set; }
    }
}