using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class PollItem
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string Caption { get; set; }
        public int Count { get; set; }
        public virtual Poll Poll { get; set; }
    }
}