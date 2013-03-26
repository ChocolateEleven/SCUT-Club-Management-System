using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class FundApplication
    {
        public int SubEventId { get; set; }

        [Required]
        public Decimal Quantity { get; set; }
        public virtual SubEvent SubEvent { get; set; }
    }
}