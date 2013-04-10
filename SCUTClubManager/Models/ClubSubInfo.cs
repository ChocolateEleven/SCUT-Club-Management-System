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
        public string Principle { get; set; }

        [MaxLength(100)]
        [Display(Name = "目的")]
        public string Purpose { get; set; }

        [MaxLength(100)]
        [Display(Name = "活动范围")]
        public string Range { get; set; }

        [MaxLength(40)]
        [Display(Name = "地址")]
        public string Address { get; set; }

        [MaxLength(256)]
        [Display(Name = "海报")]
        public string PosterUrl { get; set; }

        [Display(Name = "规章制度")]
        public string Regulation { get; set; }
    }
}