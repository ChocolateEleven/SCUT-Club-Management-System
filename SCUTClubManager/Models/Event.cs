using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ClubId { get; set; }
        public string ChiefEventOrganizerId { get; set; }
        public int Score { get; set; }
        public string PosterUrl { get; set; }
        public string PlanUrl { get; set; }
        public string Status { get; set; }
        public virtual Club Club { get; set; }
        public virtual EventDescription Description { get; set; }
        public virtual IEnumerable<EventOrganizer> Organizers { get; set; }
        public virtual IEnumerable<SubEvent> SubEvents { get; set; }
        public virtual EventOrganizer ChiefEventOrganizer { get; set; }
        public virtual DateTime Date { get; set; }
    }
}