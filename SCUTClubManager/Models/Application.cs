using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class Application
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public int ClubId { get; set; }

        [MaxLength(1)]
        public string Status { get; set; }
        public virtual Club Club { get; set; }
        public virtual Student Applicatant { get; set; }

        [MaxLength(20)]
        public string ApplicantUserName { get; set; }
        public DateTime Date { get; set; }
        public virtual ApplicationRejectReason RejectReason { get; set; }
    }
}