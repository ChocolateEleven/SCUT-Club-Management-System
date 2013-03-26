using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class ClubInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Principle { get; set; }
        public string Purpose { get; set; }
        public string Range { get; set; }
        public string Instructor { get; set; }
        public string Address { get; set; }
        public string PosterUrl { get; set; }
        public ClubInfoDetails Details { get; set; }
    }
}