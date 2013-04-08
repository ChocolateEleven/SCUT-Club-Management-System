using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCUTClubManager.Models
{
    public abstract class BranchModification
    {
        public int Id { get; set; }

        //[Required]
        public int? BranchId { get; set; }

        //[Required]
        public int ApplicationId { get; set; }

        [MaxLength(20)]
        //[Required]
        public string BranchName { get; set; }

        [ForeignKey("BranchId")]
        public virtual ClubBranch OrigBranch { get; set; }

        public virtual Application Application { get; set; }
    }
}