using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubInfoModificationApplication : Application
    {
        public virtual ICollection<BranchModification> ModificationBranches { get; set; }
        
        [Required]
        public int ClubInfoId { get; set; }
        public virtual ClubInfo Info { get; set; }
    }
}