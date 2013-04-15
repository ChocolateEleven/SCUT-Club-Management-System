using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class ApplicatedAsset : AssetBase
    {
        public int AssetId { get; set; }
        //public int AssetApplicationId { get; set; }

        public virtual Asset Asset { get; set; }
        //public virtual AssetApplication AssetApplication { get; set; }
    }
}