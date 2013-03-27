using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class SubEventDescription
    {
        [Key]
        public int SubEventId { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }

        public virtual SubEvent SubEvent { get; set; }
    }
}