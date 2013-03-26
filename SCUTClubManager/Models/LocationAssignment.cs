using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class LocationAssignment
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public DateTime Date { get; set; }
        public int TimeId { get; set; }
        public int ClubId { get; set; }
        public string ApplicantID { get; set; }
        public Location Location { get; set; }
        public Time Time { get; set; }
        public Club Club { get; set; }
        public Student Applicant { get; set; }
    }
}