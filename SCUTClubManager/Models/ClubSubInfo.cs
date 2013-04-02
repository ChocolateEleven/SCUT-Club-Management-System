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
        public string Principle { get; set; }

        [MaxLength(100)]
        public string Purpose { get; set; }

        [MaxLength(100)]
        public string Range { get; set; }

        [MaxLength(40)]
        public string Address { get; set; }

        [MaxLength(256)]
        public string PosterUrl { get; set; }

        public string Regulation { get; set; }
    }
}