using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class Poll
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public string AuthorUserName { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public bool IsMultiSelectable { get; set; }
        public User Author { get; set; }
        public IEnumerable<PollItem> Items { get; set; }
    }
}