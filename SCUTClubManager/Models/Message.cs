using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReciverId { get; set; }
        public string Title { get; set; }
        public bool ReadMark { get; set; }
        public DateTime Date { get; set; }
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
        public virtual MessageContent Content { get; set; }
    }
}