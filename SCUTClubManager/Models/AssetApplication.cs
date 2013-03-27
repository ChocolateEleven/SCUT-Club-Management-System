using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class AssetApplication : Application
    {
        public int AssetId { get; set; }
        public int TimeId { get; set; }
        public int SubEventId { get; set; }
        public int Quantity { get; set; }
        public virtual Time Time { get; set; }
        public virtual SubEvent SubEvent { get; set; }
        public virtual Asset Asset { get; set; }
    }
}