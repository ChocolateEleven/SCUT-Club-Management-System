using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class AssetApplication
    {
        public int AssetId { get; set; }
        public int TimeId { get; set; }
        public int SubEventId { get; set; }
        public int Quantity { get; set; }
        public Time Time { get; set; }
        public SubEvent SubEvent { get; set; }
        public Asset Asset { get; set; }
    }
}