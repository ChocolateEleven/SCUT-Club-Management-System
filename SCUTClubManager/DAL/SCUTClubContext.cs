using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using SCUTClubManager.Models;

namespace SCUTClubManager.DAL
{
    public class SCUTClubContext : DbContext
    {
        public DbSet<ClubInfoDetails> ClubInfoDetailses { get; set; }
        public DbSet<ClubInfo> ClubInfos { get; set; }
        public DbSet<ClubInfoModificationApplication> ClubInfoModificationApplications { get; set; }
        public DbSet<BranchModification> BranchModifications { get; set; }
        public DbSet<ClubBranch> ClubBranches { get; set; }
        public DbSet<ClubApplicationInclination> ClubApplicationInclinations { get; set; }
        public DbSet<ClubApplication> ClubApplications { get; set; }
        public DbSet<ClubApplicationDetails> ClubApplicationDetailses { get; set; }
        public DbSet<ClubRole> ClubRoles { get; set; }
        public DbSet<ClubRegisterApplication> ClubRegisterApplications { get; set; }
        public DbSet<ClubRegisterApplicant> ClubRegisterApplicants { get; set; }
        public DbSet<ClubRegisterApplicantDescription> ClubRegisterApplicantDescriptions { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ClubUnregisterApplication> ClubUnregisterApplications { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<FundDetails> FundDetailses { get; set; }
        public DbSet<ClubMember> ClubMembers { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollItem> PollItems { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<MessageContent> MessageContents { get; set; }
        public DbSet<EventOrganizer> EventOrganizers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventDescription> EventDescriptions { get; set; }
        public DbSet<LocationAssignment> LocationAssignments { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationAvailableTime> LocationAvailableTimes { get; set; }
        public DbSet<LocationApplication> LocationApplications { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<AssetAssignment> AssetAssignments { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetApplication> AssetApplications { get; set; }
        public DbSet<SubEvent> SubEvents { get; set; }
        public DbSet<SubEventDescription> SubEventDescriptions { get; set; }
        public DbSet<FundApplication> FundApplications { get; set; }
        public DbSet<UserPoll> UserPolls { get; set; }
        public DbSet<ApplicationRejectReason> ApplicationRejectReasons { get; set; }

    }
}