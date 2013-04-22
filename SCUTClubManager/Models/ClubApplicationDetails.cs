using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubApplicationDetails
    {
        [Key]
        public int ApplicationId { get; set; }

        [Display(Name = "个人简介")]
        [MaxLength(600)]
        public string Reason { get; set; }
    }
}