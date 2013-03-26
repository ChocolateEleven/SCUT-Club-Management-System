using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCUTClubManager.Models;

namespace SCUTClubManager.DAL
{
    public class UnitOfWork
    {
        public SCUTClubContext Context { get; set; }
        public Repository<ClubInfoDetails> ClubInfoDetailses { get; set; }
        public Repository<ClubInfo> ClubInfos { get; set; }
        public Repository<ClubInfoModificationApplication> ClubInfoModificationApplications { get; set; }
        public Repository<BranchModification> BranchModifications { get; set; }
        public Repository<ClubBranch> ClubBranches { get; set; }
        public Repository<ClubApplicationInclination> ClubApplicationInclinations { get; set; }
        public Repository<ClubApplication> ClubAplications { get; set; }
        public Repository<ClubApplicationDetails> ClubApplicationDetails;
        public Repository<ClubRole> ClubRoles { get; set; }
        public Repository<ClubRegisterApplication> ClubRegisterAplications;
        public Repository<ClubRegisterApplicant> ClubRegisterApplicant { get; set; }
        public Repository<ClubRegisterApplicantDescription> ClubRegisterApplicantDescription { get; set; }
        public Repository<Application> Applications { get; set; }
        public Repository<ClubUnregisterApplication> ClubUnregisterApplications { get; set; }
        public Repository<Club> Clubs { get; set; }
        public Repository<FundDetails> FundDetailses { get; set; }
        public Repository<ClubMember> ClubMembers { get; set; }
        public Repository<Poll> Polls { get; set; }
        public Repository<PollItem> PollItems { get; set; }
        public Repository<Message> Messages { get; set; }
        public Repository<User> Users { get; set; }
        public Repository<ContactInfo> ContactInfos { get; set; }
        public Repository<Student> Students { get; set; }
        public Repository<Thread> Threads { get; set; }
        public Repository<Reply> Replies { get; set; }
        public Repository<MessageContent> MessageContents { get; set; }
        public Repository<EventOrganizer> EventOrganizers { get; set; }
        public Repository<EventArgs> Events { get; set; }
        public Repository<EventDescription> EventDescriptions { get; set; }
        public Repository<Location> Locations { get; set; }
        public Repository<LocationAssignment> LocationAssignments { get; set; }
        public Repository<LocationAvailableTime> LocationAvailableTimes { get; set; }
        public Repository<LocationApplication> LocationApplications { get; set; }
        public Repository<Time> Times { get; set; }
        public Repository<AssetAssignment> AssetAssignments { get; set; }
        public Repository<Asset> Assets { get; set; }
        public Repository<AssetApplication> AssetApplications { get; set; }
        public Repository<SubEvent> SubEvents { get; set; }
        public Repository<SubEventDescription> SubEventDescriptions { get; set; }
        public Repository<FundApplication> FundApplications { get; set; }
        public Repository<UserPoll> UserPolls { get; set; }
        public Repository<ApplicationRejectReason> ApplicationRejectReasons { get; set; }
    }
}