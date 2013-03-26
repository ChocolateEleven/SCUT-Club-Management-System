using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class Student
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public string Department { get; set; }
        public string Major { get; set; }
        public string Grade { get; set; }
        public string Degree { get; set; }
        public string PoliticalId { get; set; }
        public virtual User User { get; set; }
        public virtual ContactInfo ContactInfo { get; set; }
        public virtual IEnumerable<Application> Applications { get; set; }
        public virtual IEnumerable<ClubMember> MemberShips { get; set; }
        public virtual IEnumerable<Poll> Polls { get; set; }
        public virtual IEnumerable<Message> Messages { get; set; }
        public virtual IEnumerable<Event> Events { get; set; }
    }
}