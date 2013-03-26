using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class Reply
    {
        public int Id { get; set; }
        public int ThreadId { get; set; }
        public string Content { get; set; }
        public string AuthorUserName { get; set; }
        public DateTime Date { get; set; }
        public int Number { get; set; }
        public Thread Thread { get; set; }
        public User Author { get; set; }
    }
}