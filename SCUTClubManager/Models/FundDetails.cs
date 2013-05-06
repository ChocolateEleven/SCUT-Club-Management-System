using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class FundDetails
    {
        public int Id { get; set; }

        [Required]
        public int ClubId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(200)]
        public string Purpose { get; set; }

        [RegularExpression(@"^[a-z0-9A-Z]{1,20}$", ErrorMessage = "只能是数字和字母的组合，长度不能超过20个字符")]
        [MaxLength(20)]
        public string ApplicantUserName { get; set; }

        [Required]
        public Decimal Quantity { get; set; }

        public virtual Student Applicant { get; set; }
        public virtual Club Club { get; set; }
    }
}