using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class ClubRegisterApplicantDescription
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string Description { get; set; }
    }
}