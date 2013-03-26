using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class FundApplication
    {
        public int SubEventId { get; set; }
        public Decimal Quantity { get; set; }
        public virtual SubEvent SubEvent { get; set; }
    }
}