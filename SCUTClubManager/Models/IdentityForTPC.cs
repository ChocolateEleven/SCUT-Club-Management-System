using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class IdentityForTPC
    {
        [Key]
        [MaxLength(30)]
        public string BaseName { get; set; }

        [Required]
        public int Identity { get; set; }
    }
}