using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class Application
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public string Status { get; set; }
        public virtual Club Club { get; set; }
        public virtual Student Applicatant { get; set; }
        public string ApplicantUserName { get; set; }
        public DateTime Date { get; set; }
        public virtual ApplicationRejectReason RejectReason { get; set; }
    }
}