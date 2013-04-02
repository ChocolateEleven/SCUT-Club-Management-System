using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCUTClubManager.Models;

namespace SCUTClubManager.DAL
{
    public class UnitOfWork : IDisposable
    {
        private SCUTClubContext context = new SCUTClubContext();

        private Repository<ClubInfoModificationApplication> clubInfoModificationApplications;
        public Repository<ClubInfoModificationApplication> ClubInfoModificationApplications
        {
            get
            {
                if (this.clubInfoModificationApplications == null)
                {
                    this.clubInfoModificationApplications = new Repository<ClubInfoModificationApplication>(context);
                }
                return clubInfoModificationApplications;
            }
        }

        private Repository<BranchModification> branchModifications;
        public Repository<BranchModification> BranchModifications
        {
            get 
            {
                if (this.branchModifications == null)
                {
                    this.branchModifications = new Repository<BranchModification>(context);
                }
                return branchModifications;
            }
        }

        private Repository<ClubBranch> clubBranches;
        public Repository<ClubBranch> ClubBranches
        {
            get
            {
                if (this.clubBranches == null)
                {
                    this.clubBranches = new Repository<ClubBranch>(context);
                }
                return clubBranches;
            }
        }

        private Repository<ClubApplicationInclination> clubApplicationInclinations;
        public Repository<ClubApplicationInclination> ClubApplicationInclinations
        {
            get 
            {
                if (this.clubApplicationInclinations == null)
                {
                    this.clubApplicationInclinations = new Repository<ClubApplicationInclination>(context);
                }
                return clubApplicationInclinations;
            }
        }

        private Repository<ClubApplication> clubApplications;
        public Repository<ClubApplication> ClubApplications
        {
            get
            {
                if (this.clubApplications == null)
                {
                    this.clubApplications = new Repository<ClubApplication>(context);
                }
                return clubApplications;
            }
        }

        private Repository<ClubApplicationDetails> clubApplicationDetails;
        public Repository<ClubApplicationDetails> ClubApplicationDetails
        {
            get
            {
                if (this.clubApplicationDetails == null)
                {
                    this.clubApplicationDetails = new Repository<ClubApplicationDetails>(context);
                }
                return clubApplicationDetails;
            }
        }

        private Repository<ClubRole> clubRoles;
        public Repository<ClubRole> ClubRoles
        {
            get
            {
                if (this.clubRoles == null)
                {
                    this.clubRoles = new Repository<ClubRole>(context);
                }
                return clubRoles;
            }
        }

        private Repository<UserRole> userRoles;
        public Repository<UserRole> UserRoles
        {
            get
            {
                if (this.userRoles == null)
                {
                    this.userRoles = new Repository<UserRole>(context);
                }
                return userRoles;
            }
        }

        private Repository<ClubRegisterApplication> clubRegisterApplications;
        public Repository<ClubRegisterApplication> ClubRegisterApplications
        {
            get
            {
                if(this.clubRegisterApplications == null)
                {
                    this.clubRegisterApplications = new Repository<ClubRegisterApplication>(context);
                }
                return clubRegisterApplications;
            }
        }

        private Repository<ClubRegisterApplicant> clubRegisterApplicants;
        public Repository<ClubRegisterApplicant> ClubRegisterApplicants
        {
            get
            {
                if (this.clubRegisterApplicants == null)
                {
                    this.clubRegisterApplicants = new Repository<ClubRegisterApplicant>(context);
                }
                return clubRegisterApplicants;
            }
        }


        private Repository<ClubRegisterApplicantDescription> clubRegisterApplicantDescription;
        public Repository<ClubRegisterApplicantDescription> ClubRegisterApplicantDescription
        {
            get
            {
                if (this.clubRegisterApplicantDescription == null)
                {
                    this.clubRegisterApplicantDescription = new Repository<ClubRegisterApplicantDescription>(context);
                }
                return clubRegisterApplicantDescription;
            }
        }

        private Repository<Application> applications;
        public Repository<Application> Applications
        {
            get
            {
                if (this.applications == null)
                {
                    this.applications = new Repository<Application>(context);
                }
                return applications;
            }
        }


        private Repository<ClubUnregisterApplication> clubUnregisterApplications;
        public Repository<ClubUnregisterApplication> ClubUnregisterApplications
        {
            get
            {
                if (this.clubUnregisterApplications == null)
                {
                    this.clubUnregisterApplications = new Repository<ClubUnregisterApplication>(context);
                }
                return clubUnregisterApplications;
            }
        }


        private Repository<Club> clubs;
        public Repository<Club> Clubs
        {
            get
            {
                if (this.clubs == null)
                {
                    this.clubs = new Repository<Club>(context);
                }
                return clubs;
            }
        }


        private Repository<FundDetails> fundDetailses;
        public Repository<FundDetails> FundDetailses
        {
            get
            {
                if (this.fundDetailses == null)
                {
                    this.fundDetailses = new Repository<FundDetails>(context);
                }
                return fundDetailses;
            }
        }



        private Repository<ClubMember> clubMembers;
        public Repository<ClubMember> ClubMembers
        {
            get
            {
                if (this.clubMembers == null)
                {
                    this.clubMembers = new Repository<ClubMember>(context);
                }
                return clubMembers;
            }
        }


        private Repository<Poll> polls;
        public Repository<Poll> Polls
        {
            get
            {
                if (this.polls == null)
                {
                    this.polls = new Repository<Poll>(context);
                }
                return polls;
            }
        }


        private Repository<PollItem> pollItems;
        public Repository<PollItem> PollItems
        {
            get
            {
                if (this.pollItems == null)
                { 
                    this.pollItems = new Repository<PollItem>(context);
                }
                return pollItems;
            }
        }


        private Repository<Message> messages;
        public Repository<Message> Messages
        {
            get
            {
                if (this.messages == null)
                {
                    this.messages = new Repository<Message>(context);
                }
                return messages;
            }
        }


        private Repository<User> users;
        public Repository<User> Users
        {
            get
            {
                if (this.users == null)
                {
                    this.users = new Repository<User>(context);
                }
                return users;
            }
        }


        private Repository<ContactInfo> contactInfos;
        public Repository<ContactInfo> ContactInfos
        {
            get
            {
                if (this.contactInfos == null)
                {
                    this.contactInfos = new Repository<ContactInfo>(context);
                }
                return contactInfos;
            }
        }


        private Repository<Student> students;
        public Repository<Student> Students
        {
            get
            {
                if (this.students == null)
                {
                    this.students = new Repository<Student>(context);
                }
                return students;
            }
        }


        private Repository<Thread> threads;
        public Repository<Thread> Threads
        {
            get
            {
                if (this.threads == null)
                {
                    this.threads = new Repository<Thread>(context);
                }
                return threads;
            }
        }


        private Repository<Reply> replies;
        public Repository<Reply> Replies
        {
            get
            {
                if (this.replies == null)
                {
                    this.replies = new Repository<Reply>(context);
                }
                return replies;
            }
        }


        private Repository<MessageContent> messageContents;
        public Repository<MessageContent> MessageContents
        {
            get
            {
                if (this.messageContents == null)
                {
                    this.messageContents = new Repository<MessageContent>(context);
                }
                return messageContents;
            }
        }



        //private Repository<EventOrganizer> eventOrganizers;
        //public Repository<EventOrganizer> EventOrganizers
        //{
        //    get
        //    {
        //        if (this.eventOrganizers == null)
        //        {
        //            this.eventOrganizers = new Repository<EventOrganizer>(context);
        //        }
        //        return eventOrganizers;
        //    }
        //}


        private Repository<EventArgs> events;
        public Repository<EventArgs> Events
        {
            get
            {
                if (this.events == null)
                {
                    this.events = new Repository<EventArgs>(context);
                }
                return events;
            }
        }


        private Repository<EventDescription> eventDescriptions;
        public Repository<EventDescription> EventDescriptions
        {
            get
            {
                if (this.eventDescriptions == null)
                {
                    this.eventDescriptions = new Repository<EventDescription>(context);
                }
                return eventDescriptions;
            }
        }


        private Repository<Location> locations;
        public Repository<Location> Locations
        {
            get
            {
                if (this.locations == null)
                {
                    this.locations = new Repository<Location>(context);
                }
                return locations;
            }
        }


        private Repository<LocationAssignment> locationAssignments;
        public Repository<LocationAssignment> LocationAssignments
        {
            get
            {
                if (this.locationAssignments == null)
                {
                    this.locationAssignments = new Repository<LocationAssignment>(context);
                }
                return locationAssignments;
            }
        }



        private Repository<LocationAvailableTime> locationAvailableTimes;
        public Repository<LocationAvailableTime> LocationAvailableTimes
        {
            get
            {
                if (this.locationAvailableTimes == null)
                {
                    this.locationAvailableTimes = new Repository<LocationAvailableTime>(context);
                }
                return locationAvailableTimes;
            }
        }


        private Repository<LocationApplication> locationApplications;
        public Repository<LocationApplication> LocationApplications
        {
            get
            {
                if (this.locationApplications == null)
                {
                    this.locationApplications = new Repository<LocationApplication>(context);
                }
                return locationApplications;
            }
        }


        private Repository<Time> times;
        public Repository<Time> Times
        {
            get
            {
                if (this.times == null)
                {
                    this.times = new Repository<Time>(context);
                }
                return times;
            }
        }


        private Repository<AssetAssignment> assetAssignments;
        public Repository<AssetAssignment> AssetAssignments
        {
            get
            {
                if (this.assetAssignments == null)
                {
                    this.assetAssignments = new Repository<AssetAssignment>(context);
                }
                return assetAssignments;
            }
        }


        private Repository<Asset> assets;
        public Repository<Asset> Assets
        {
            get
            {
                if (this.assets == null)
                {
                    this.assets = new Repository<Asset>(context);
                }
                return assets;
            }
        }


        private Repository<AssetApplication> assetApplications;
        public Repository<AssetApplication> AssetApplications
        {
            get
            {
                if (this.assetApplications == null)
                {
                    this.assetApplications = new Repository<AssetApplication>(context);
                }
                return assetApplications;
            }
        }


        private Repository<SubEvent> subEvents;
        public Repository<SubEvent> SubEvents
        {
            get
            {
                if (this.subEvents == null)
                {
                    this.subEvents = new Repository<SubEvent>(context);
                }
                return subEvents;
            }
        }


        private Repository<SubEventDescription> subEventDescriptions;
        public Repository<SubEventDescription> SubEventDescriptions
        {
            get
            {
                if (this.subEventDescriptions == null)
                {
                    this.subEventDescriptions = new Repository<SubEventDescription>(context);
                }
                return subEventDescriptions;
            }
        }


        private Repository<FundApplication> fundApplications;
        public Repository<FundApplication> FundApplications
        {
            get
            {
                if (this.fundApplications == null)
                {
                    this.fundApplications = new Repository<FundApplication>(context);
                }
                return fundApplications;
            }
        }


        private Repository<UserPoll> userPolls;
        public Repository<UserPoll> UserPolls
        {
            get
            {
                if (this.userPolls == null)
                {
                    this.userPolls = new Repository<UserPoll>(context);
                }
                return userPolls;
            }
        }


        private Repository<ApplicationRejectReason> applicationRejectReasons;
        public Repository<ApplicationRejectReason> ApplicationRejectReasons
        {
            get
            {
                if (this.applicationRejectReasons == null)
                {
                    this.applicationRejectReasons = new Repository<ApplicationRejectReason>(context);
                }
                return applicationRejectReasons;
            }
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}