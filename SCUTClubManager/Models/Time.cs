using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class Time
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(10)]
        public string TimeName { get; set; }
    }
}