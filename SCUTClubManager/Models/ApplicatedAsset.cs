using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCUTClubManager.Models
{
    public class ApplicatedAsset : AssetBase
    {
        public int AssetId { get; set; }
        public int AssetApplicationId { get; set; }

        public virtual Asset Asset { get; set; }
        [ForeignKey("AssetApplicationId")]
        public virtual AssetApplication AssetApplication { get; set; }
    }
}