using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class ApplicationRejectReason
    {
        [Key]
        public int ApplicationId { get; set; }

        [MaxLength(200)]
        public string Reason { get; set; }

        public virtual Application Application { get; set; }
    }
}