using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubInfoDetails
    {
        [Key]
        public int ClubInfoId { get; set; }
        public string Regulation { get; set; }

        public virtual ClubInfo Info { get; set; }
    }
}