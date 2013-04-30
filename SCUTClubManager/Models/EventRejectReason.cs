using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class EventRejectReason
    {
        [Key]
        public int EventId { get; set; }

        public string Reason { get; set; }
    }
}