using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class EventDescription
    {
        public int EventId { get; set; }
        public string Description { get; set; }
        public virtual Event Event { get; set; }
    }
}