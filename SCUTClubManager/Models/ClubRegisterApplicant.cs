using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class ClubRegisterApplicant
    {
        public int Id { get; set; }
        public string ApplicantUserName { get; set; }
        public bool IsMainApplicant { get; set; }
        public ClubRegisterApplicantDescription Description { get; set; }
        public int ApplicationId { get; set; }
    }
}