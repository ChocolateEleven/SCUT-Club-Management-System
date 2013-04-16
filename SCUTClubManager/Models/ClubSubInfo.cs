using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubSubInfo
    {
        public int Id { get; set; }

        [MaxLength(200)]
        [Display(Name = "宗旨")]
        [Required(ErrorMessage = "请输入宗旨")]
        public string Principle { get; set; }

        [MaxLength(100)]
        [Display(Name = "目的")]
        [Required(ErrorMessage = "请输入目的")]
        public string Purpose { get; set; }

        [MaxLength(100)]
        [Display(Name = "活动范围")]
        [Required(ErrorMessage = "请输入活动范围")]
        public string Range { get; set; }

        [MaxLength(40)]
        [Display(Name = "地址")]
        [Required(ErrorMessage = "请输入地址")]
        public string Address { get; set; }

        [MaxLength(256)]
        [Display(Name = "海报")]
        public string PosterUrl { get; set; }

        [Display(Name = "规章制度")]
        [Required(ErrorMessage = "请输入规章制度")]
        public string Regulation { get; set; }

        public static bool operator ==(ClubSubInfo obj1, ClubSubInfo obj2)
        {
            if (Object.ReferenceEquals(obj1, null) ^ Object.ReferenceEquals(obj2, null))
                return false;

            if (Object.ReferenceEquals(obj1, null) && Object.ReferenceEquals(obj2, null))
                return true;

            return obj1.Principle == obj2.Principle && obj1.Purpose == obj2.Purpose
                && obj1.Range == obj2.Range && obj1.Address == obj2.Address &&
                obj1.Regulation == obj2.Regulation;
        }

        public static bool operator !=(ClubSubInfo obj1, ClubSubInfo obj2)
        {
            return !(obj1 == obj2);
        }
    }
}