using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class AssetApplication
    {
        [Required]
        public int AssetId { get; set; }

        [Required]
        public int TimeId { get; set; }
        public int SubEventId { get; set; }

        [Required]
        public int Quantity { get; set; }
        public Time Time { get; set; }
        public SubEvent SubEvent { get; set; }
        public Asset Asset { get; set; }
    }
}