using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ClubUnregisterApplication : Application
    {
        [MaxLength(400)]
        public string Reason { get; set; }
    }
}