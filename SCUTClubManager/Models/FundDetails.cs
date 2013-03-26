using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class FundDetails
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public DateTime Date { get; set; }
        public string ApplicantUserName { get; set; }
        public Decimal Quantity { get; set; }
        public virtual Student Applicant { get; set; }
    }
}