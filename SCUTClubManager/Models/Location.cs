using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SCUTClubManager.Models
{
    public class Location
    {
        public int Id { get; set; }

        [RegularExpression(@"^[a-z0-9A-Z]{1,20}$", ErrorMessage = "只能是数字和字母的组合，长度不能超过20个字符")]
        [MaxLength(20)]
        [Display(Name = "名称")]
        public string Name { get; set; }

        [JsonIgnore]
        [Display(Name = "不可用时间段")]
        public virtual ICollection<LocationUnavailableTime> UnAvailableTimes { get; set; }

        [JsonIgnore]
        public virtual ICollection<LocationApplication> Applications { get; set; }

        [JsonIgnore]
        public virtual ICollection<LocationAssignment> Assignments { get; set; }
    }
}