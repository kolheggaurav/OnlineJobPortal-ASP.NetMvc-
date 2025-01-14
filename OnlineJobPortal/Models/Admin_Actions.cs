using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnlineJobPortal.Models
{
    public class Admin_Actions
    {
        [Key] public int ActionID { get; set; }

        public int AdminID { get; set; } //(Foreign key from Registration table)
        // Navigation property
        [ForeignKey("AdminID")]
        public virtual Registration Registration { get; set; }
        public string ActionDescription { get; set; }
        public DateTime ActionDate { get; set; }
    }
}