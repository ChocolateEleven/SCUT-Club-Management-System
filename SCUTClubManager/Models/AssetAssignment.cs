using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCUTClubManager.Models
{
    public class AssetAssignment
    {
        public int Id { get; set; }

        [Required]
        //[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        public int ClubId { get; set; }

        [RegularExpression(@"^[a-z0-9A-Z]{1,20}$",
       ErrorMessage = "用户名只能是数字和字母的组合，长度不能超过20个字符")]
        public string ApplicantUserName { get; set; }

        public virtual AssetApplication AssetApplication { get; set; }
        public virtual ICollection<AssignedAsset> AssignedAssets { get; set; }
        public virtual ICollection<Time> Times { get; set; }
        public virtual Club Club { get; set; }
        public virtual Student Applicant { get; set; }
    }
}