using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCUTClubManager.Models
{
    public class LocationAssignment
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int ClubId { get; set; }

        [RegularExpression(@"^[a-z0-9A-Z]{1,20}$", ErrorMessage = "只能是数字和字母的组合，长度不能超过20个字符")]
        [MaxLength(20)]
        public string ApplicantName { get; set; }

        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<Time> Times { get; set; }
        public virtual Club Club { get; set; }

        public virtual LocationApplication LocationApplication { get; set; }

        [ForeignKey("ApplicantName")]
        public virtual Student Applicant { get; set; }
    }
}