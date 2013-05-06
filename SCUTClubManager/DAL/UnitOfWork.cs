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

        public int GenerateIdFor(string base_class_name)
        {
            var id_generators = context.Identities;

            if (id_generators.Any(t => t.BaseName == base_class_name))
            {
                var id_generator = id_generators.Find(base_class_name);
                int id = id_generator.Identity;
                id_generator.Identity++;

                this.SaveChanges();

                return id;
            }

            throw new ArgumentException("There is no generators named " + base_class_name + ".");
        }

        private IRepository<ApplicatedAsset> applicatedAssets;
        public IRepository<ApplicatedAsset> ApplicatedAssets
        {
            get
            {
                if (this.applicatedAssets == null)
                {
                    this.applicatedAssets = new Repository<ApplicatedAsset>(context);
                }

                return applicatedAssets;
            }
        }

        private IRepository<ClubInfoModificationApplication> clubInfoModificationApplications;
        public IRepository<ClubInfoModificationApplication> ClubInfoModificationApplications
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

        private IRepository<BranchModification> branchModifications;
        public IRepository<BranchModification> BranchModifications
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

        private IRepository<ClubBranch> clubBranches;
        public IRepository<ClubBranch> ClubBranches
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

        private IRepository<ClubApplicationInclination> clubApplicationInclinations;
        public IRepository<ClubApplicationInclination> ClubApplicationInclinations
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

        private IRepository<ClubApplication> clubApplications;
        public IRepository<ClubApplication> ClubApplications
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

        private IRepository<ClubApplicationDetails> clubApplicationDetails;
        public IRepository<ClubApplicationDetails> ClubApplicationDetails
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

        private IRepository<ClubRole> clubRoles;
        public IRepository<ClubRole> ClubRoles
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

        private IRepository<UserRole> userRoles;
        public IRepository<UserRole> UserRoles
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

        private IRepository<ClubRegisterApplication> clubRegisterApplications;
        public IRepository<ClubRegisterApplication> ClubRegisterApplications
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

        private IRepository<ClubRegisterApplicant> clubRegisterApplicants;
        public IRepository<ClubRegisterApplicant> ClubRegisterApplicants
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


        private IRepository<ClubRegisterApplicantDescription> clubRegisterApplicantDescription;
        public IRepository<ClubRegisterApplicantDescription> ClubRegisterApplicantDescription
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

        private IRepository<Application> applications;
        public IRepository<Application> Applications
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


        private IRepository<ClubUnregisterApplication> clubUnregisterApplications;
        public IRepository<ClubUnregisterApplication> ClubUnregisterApplications
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


        private IRepository<Club> clubs;
        public IRepository<Club> Clubs
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


        private IRepository<FundDetails> fundDetailses;
        public IRepository<FundDetails> FundDetailses
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



        private IRepository<ClubMember> clubMembers;
        public IRepository<ClubMember> ClubMembers
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


        private IRepository<Poll> polls;
        public IRepository<Poll> Polls
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


        private IRepository<PollItem> pollItems;
        public IRepository<PollItem> PollItems
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


        private IRepository<Message> messages;
        public IRepository<Message> Messages
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


        private IRepository<User> users;
        public IRepository<User> Users
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


        private IRepository<ContactInfo> contactInfos;
        public IRepository<ContactInfo> ContactInfos
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


        private IRepository<Student> students;
        public IRepository<Student> Students
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


        private IRepository<Thread> threads;
        public IRepository<Thread> Threads
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


        private IRepository<Reply> replies;
        public IRepository<Reply> Replies
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


        private IRepository<MessageContent> messageContents;
        public IRepository<MessageContent> MessageContents
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


        private IRepository<Event> events;
        public IRepository<Event> Events
        {
            get
            {
                if (this.events == null)
                {
                    this.events = new Repository<Event>(context);
                }
                return events;
            }
        }


        private IRepository<EventDescription> eventDescriptions;
        public IRepository<EventDescription> EventDescriptions
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


        private IRepository<Location> locations;
        public IRepository<Location> Locations
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


        private IRepository<LocationAssignment> locationAssignments;
        public IRepository<LocationAssignment> LocationAssignments
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



        private IRepository<LocationUnavailableTime> locationUnAvailableTimes;
        public IRepository<LocationUnavailableTime> LocationUnAvailableTimes
        {
            get
            {
                if (this.locationUnAvailableTimes == null)
                {
                    this.locationUnAvailableTimes = new Repository<LocationUnavailableTime>(context);
                }
                return locationUnAvailableTimes;
            }
        }


        private IRepository<LocationApplication> locationApplications;
        public IRepository<LocationApplication> LocationApplications
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


        private IRepository<Time> times;
        public IRepository<Time> Times
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


        private IRepository<AssetAssignment> assetAssignments;
        public IRepository<AssetAssignment> AssetAssignments
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


        private IRepository<Asset> assets;
        public IRepository<Asset> Assets
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


        private IRepository<AssetApplication> assetApplications;
        public IRepository<AssetApplication> AssetApplications
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


        private IRepository<SubEvent> subEvents;
        public IRepository<SubEvent> SubEvents
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


        private IRepository<SubEventDescription> subEventDescriptions;
        public IRepository<SubEventDescription> SubEventDescriptions
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


        private IRepository<FundApplication> fundApplications;
        public IRepository<FundApplication> FundApplications
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


        private IRepository<UserPoll> userPolls;
        public IRepository<UserPoll> UserPolls
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


        private IRepository<ApplicationRejectReason> applicationRejectReasons;
        public IRepository<ApplicationRejectReason> ApplicationRejectReasons
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

        private IRepository<RoleBase> roleBases;
        public IRepository<RoleBase> RoleBases
        {
            get
            {
                if (this.roleBases == null)
                {
                    this.roleBases = new Repository<RoleBase>(context);
                }
                return roleBases;
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