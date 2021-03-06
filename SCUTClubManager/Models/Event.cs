﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCUTClubManager.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        [Display(Name = "活动名")]
        public string Title { get; set; }

        //[Required]
        public int ClubId { get; set; }

        //[Required]
        [Display(Name = "负责人")]
        public string ChiefEventOrganizerId { get; set; }

        [Range(0,100)]
        public int? Score { get; set; }

        [MaxLength(256)]
        public string PosterUrl { get; set; }

        [MaxLength(256)]
        public string PlanUrl { get; set; }

        [MaxLength(1)]
        public string Status { get; set; }

        // 这里的Date为活动的申请日期
        public DateTime? Date { get; set; }

        // 这里的StartDate为活动的开始日期
        [Display(Name = "开始日期")]
        public DateTime StartDate { get; set; }

        // 这里的EndDate为活动的结束日期
        [Display(Name = "结束日期")]
        public DateTime EndDate { get; set; }

        [System.Web.Script.Serialization.ScriptIgnore]
        public virtual Club Club { get; set; }
        public virtual EventDescription Description { get; set; }

        [System.Web.Script.Serialization.ScriptIgnore]
        public virtual ICollection<Student> Organizers { get; set; }

        public virtual ICollection<SubEvent> SubEvents { get; set; }

        public virtual FundApplication FundApplication { get; set; }

        [System.Web.Script.Serialization.ScriptIgnore]
        [ForeignKey("ChiefEventOrganizerId")]
        public virtual Student ChiefEventOrganizer { get; set; }

        public virtual EventRejectReason RejectReason { get; set; }
    }
}