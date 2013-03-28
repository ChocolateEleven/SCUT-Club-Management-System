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
        public DateTime Date { get; set; }

        [Required]
        public int ClubId { get; set; }

        [RegularExpression(@"^[a-z0-9A-Z]{1,20}$", ErrorMessage = "只能是数字和字母的组合，长度不能超过20个字符")]
        [MaxLength(20)]
        public string ApplicantUserName { get; set; }

        [Required]
        public int Quantity { get; set; }

        public virtual ICollection<Asset> Asset { get; set; }
        public virtual ICollection<Time> Time { get; set; }
        public virtual Club Club { get; set; }
        public virtual Student Applicant { get; set; }
    }
}