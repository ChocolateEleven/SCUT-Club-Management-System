﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SCUTClubManager.Models
{
    public class SubEvent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        [MaxLength(40)]
        public string Title { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int TimeId { get; set; }
        public virtual SubEventDescription Description { get; set; }
        public virtual Time Time { get; set; }
        public virtual Event Event { get; set; }
        public virtual IEnumerable<LocationApplication> LocationApplications { get; set; }
        public virtual IEnumerable<AssetApplication> AssetApplications { get; set; }
        public virtual FundApplication FundApplication { get; set; }
    }
}