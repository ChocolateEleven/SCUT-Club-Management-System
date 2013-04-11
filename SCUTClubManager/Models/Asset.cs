using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class Asset : AssetBase
    {
        [MaxLength(20)]
        public string Name { get; set; }

        public ICollection<ApplicatedAsset> Applications { get; set; }
        public ICollection<AssignedAsset> Assignments { get; set; }
    }
}