using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class ContactInfo
    {
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string QQ { get; set; }
        public string EMailAddress { get; set; }
        public string Visibility { get; set; }
        public string Room { get; set; }
        public virtual Student Student { get; set; }
    }
}