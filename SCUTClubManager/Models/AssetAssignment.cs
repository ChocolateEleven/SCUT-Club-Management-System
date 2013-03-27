using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class AssetAssignment
    {
        public int Id { get; set; }

        [Required]
        public int AssetId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int TimeId { get; set; }

        [Required]
        public int ClubId { get; set; }


        [RegularExpression(@"^[a-z0-9A-Z]$",
      ErrorMessage = "用户名只能是数字和字母的组合")]
        [MaxLength(20, ErrorMessage = "用户名的长度不能超过20个字符")]
        public string ApplicantUserName { get; set; }

        [Required]
        public int Quantity { get; set; }

        public virtual Asset Asset { get; set; }
        public virtual Time Time { get; set; }
        public virtual Club Club { get; set; }
        public virtual Student Applicant { get; set; }
    }
}