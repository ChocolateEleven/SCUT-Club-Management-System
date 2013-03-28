using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class AssignedAsset : AssetBase
    {
        public int AssetId { get; set; }
        public int AssetAssignmentId { get; set; }

        public virtual Asset Asset { get; set; }
        public virtual AssetAssignment AssetAssignment { get; set; }
    }
}