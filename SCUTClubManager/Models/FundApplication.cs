using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCUTClubManager.Models
{
    public class FundApplication : Application
    {
        [Required]
        public Decimal Quantity { get; set; }

        public virtual Event Event { get; set; }
    }
}