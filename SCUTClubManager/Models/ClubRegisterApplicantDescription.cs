using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubRegisterApplicantDescription
    {
        public int Id { get; set; }

        [Required]
        public int ApplicationId { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        public virtual ClubRegisterApplication Application { get; set; }
    }
}