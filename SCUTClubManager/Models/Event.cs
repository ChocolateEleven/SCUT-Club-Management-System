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
        public Club Club { get; set; }
        public EventDescription Description { get; set; }
        public IEnumerable<EventOrganizer> Organizers { get; set; }
        public IEnumerable<SubEvent> SubEvents { get; set; }
        public EventOrganizer ChiefEventOrganizer { get; set; }
        public DateTime Date { get; set; }
    }
}