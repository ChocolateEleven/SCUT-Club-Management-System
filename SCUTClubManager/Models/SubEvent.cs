using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class SubEvent
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int TimeId { get; set; }
        public SubEventDescription Description { get; set; }
        public Time Time { get; set; }
        public Event Event { get; set; }
        public IEnumerable<LocationApplication> LocationApplications { get; set; }
        public IEnumerable<AssetApplication> AssetApplications { get; set; }
        public FundApplication FundApplication { get; set; }
    }
}