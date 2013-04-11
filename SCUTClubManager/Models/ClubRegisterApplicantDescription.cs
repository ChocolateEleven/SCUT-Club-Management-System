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
        [Required(ErrorMessage = "请输入申请人描述")]
        public string Description { get; set; }
    }
}