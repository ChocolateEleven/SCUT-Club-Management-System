using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class AssetApplication : Application
    {
        public int? SubEventId { get; set; }

        [Required]
        public int Quantity { get; set; }
        public virtual ICollection<Time> Time { get; set; }
        public virtual SubEvent SubEvent { get; set; }
        public virtual ICollection<Asset> Asset { get; set; }
    }
}