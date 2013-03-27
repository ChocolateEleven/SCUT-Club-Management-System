using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubRole
    {
        public int Id { get; set; }

        [MaxLength(10)]
        public string Name { get; set; }
    }
}