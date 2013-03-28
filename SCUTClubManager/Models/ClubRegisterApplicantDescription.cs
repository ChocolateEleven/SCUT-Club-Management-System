using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubRegisterApplicantDescription
    {
        [Key]
        public int ClubRegisterApplicantId { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }
    }
}