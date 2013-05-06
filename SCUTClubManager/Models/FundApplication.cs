using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCUTClubManager.Models
{
    public class FundApplication : Application
    {
        public Decimal Quantity { get; set; }

        [System.Web.Script.Serialization.ScriptIgnore]
        public virtual Event Event { get; set; }
    }
}