﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class EventOrganizer
    {
        public int EventId { get; set; }
        public int Id { get; set; }
        public virtual Event Event { get; set; }
        public virtual Student Student { get; set; }
    }
}