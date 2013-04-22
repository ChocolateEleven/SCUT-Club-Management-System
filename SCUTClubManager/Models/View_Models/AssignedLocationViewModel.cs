using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCUTClubManager.Models;

namespace SCUTClubManager.Models.View_Models
{
    public class AssignedLocationViewModel
    {
        public Club Club { get; set; }
        public Location Location { get; set; }
        public User Applicant { get; set; }
    }
}