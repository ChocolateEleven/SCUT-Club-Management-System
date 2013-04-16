using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubMajorInfo
    {
        public int Id { get; set; }

        [MaxLength(20)]
        [Required(ErrorMessage = "请输入社团名称")]
        [Display(Name = "社团名称")]
        public string Name { get; set; }

        [Display(Name = "指导教师")]
        [Required(ErrorMessage = "请输入指导教师")]
        public string Instructor { get; set; }

        public static bool operator == (ClubMajorInfo obj1, ClubMajorInfo obj2)
        {
            if (Object.ReferenceEquals(obj1, null) ^ Object.ReferenceEquals(obj2, null))
                return false;

            if (Object.ReferenceEquals(obj1, null) && Object.ReferenceEquals(obj2, null))
                return true;

            return obj1.Name == obj2.Name && obj1.Instructor == obj2.Instructor;
        }

        public static bool operator != (ClubMajorInfo obj1, ClubMajorInfo obj2)
        {
            return !(obj1 == obj2);
        }
    }
}