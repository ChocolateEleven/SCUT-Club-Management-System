using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class Thread
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<Reply> Replies { get; set; }
        public User Author { get; set; }
    }
}