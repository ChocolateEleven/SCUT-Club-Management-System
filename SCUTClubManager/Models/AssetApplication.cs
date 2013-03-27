using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class AssetApplication : Application
    {
        [Required]
        public int AssetId { get; set; }

        [Required]
        public int TimeId { get; set; }
        public int SubEventId { get; set; }

        [Required]
        public int Quantity { get; set; }
        public virtual Time Time { get; set; }
        public virtual SubEvent SubEvent { get; set; }
        public virtual Asset Asset { get; set; }
    }
}