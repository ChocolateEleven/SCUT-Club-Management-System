﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCUTClubManager.Models
{
    public class UserPoll
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PollId { get; set; }
        public virtual User User { get; set; }
        public virtual Poll Poll { get; set; }
    }
}