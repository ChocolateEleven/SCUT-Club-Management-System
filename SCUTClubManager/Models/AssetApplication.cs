using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SCUTClubManager.Models
{
    public class AssetApplication : Application
    {
        public int? SubEventId { get; set; }

        public int? AssignmentId { get; set; }

        public DateTime ApplicatedDate { get; set; }

        [JsonIgnore]
        public virtual AssetAssignment Assignment { get; set; }

        [JsonIgnore]
        public virtual ICollection<Time> Times { get; set; }

        [JsonIgnore]
        public virtual SubEvent SubEvent { get; set; }
        public virtual ICollection<ApplicatedAsset> ApplicatedAssets { get; set; }
    }
}