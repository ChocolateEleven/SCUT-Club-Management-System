using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public abstract class RoleBase
    {
        public int Id { get; set; }

        [Required]
        // TODO: 想办法允许中文字符。
        //[RegularExpression(@"^[a-zA-Z]{1,10}$")]
        [MaxLength(10)]
        public string Name { get; set; }
    }
}