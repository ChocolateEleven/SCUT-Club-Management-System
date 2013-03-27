﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubRegisterApplication : Application
    {
        public virtual ICollection<ClubRegisterApplicant> Applicants { get; set; }
        public virtual ICollection<BranchModification> Branches { get; set; }
       
        [Required]
        public int ClubInfoId { get; set; }

        public virtual ClubInfo Info { get; set; }

        [MaxLength(256)]
        public string Material { get; set; }
    }
}