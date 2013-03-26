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
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public MessageContent Content { get; set; }
    }
}