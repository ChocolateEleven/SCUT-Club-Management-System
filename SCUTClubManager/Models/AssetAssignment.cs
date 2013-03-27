using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class AssetAssignment
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public DateTime Date { get; set; }
        public int TimeId { get; set; }
        public int ClubId { get; set; }
        public string ApplicantUserName { get; set; }
        public int Quantity { get; set; }
        public virtual Asset Asset { get; set; }
        public virtual Time Time { get; set; }
        public virtual Club Club { get; set; }
        public virtual Student Applicant { get; set; }
    }
}