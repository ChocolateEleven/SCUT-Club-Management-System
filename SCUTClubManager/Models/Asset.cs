using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SCUTClubManager.Models
{
    public class Asset : AssetBase
    {
        [MaxLength(20)]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<ApplicatedAsset> Applications { get; set; }

        [JsonIgnore]
        public ICollection<AssignedAsset> Assignments { get; set; }
    }
}