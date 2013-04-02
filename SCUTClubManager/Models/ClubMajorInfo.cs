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
        public string Name { get; set; }

        public string Instructor { get; set; }
    }
}