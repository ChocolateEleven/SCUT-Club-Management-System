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
        public virtual SubEventDescription Description { get; set; }
        public virtual Time Time { get; set; }
        public virtual Event Event { get; set; }
        public virtual IEnumerable<LocationApplication> LocationApplications { get; set; }
        public virtual IEnumerable<AssetApplication> AssetApplications { get; set; }
        public virtual FundApplication FundApplication { get; set; }
    }
}