using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;
using SCUTClubManager.Models;

namespace SCUTClubManager.DAL
{
    public class SCUTClubContext : DbContext
    {
        public DbSet<ClubSubInfo> ClubSubInfos { get; set; }
        public DbSet<ClubMajorInfo> ClubMajorInfos { get; set; }
        public DbSet<BranchModification> BranchModifications { get; set; }
        public DbSet<ClubBranch> ClubBranches { get; set; }
        public DbSet<ClubApplicationInclination> ClubApplicationInclinations { get; set; }
        public DbSet<ClubApplicationDetails> ClubApplicationDetailses { get; set; }
        public DbSet<RoleBase> RoleBases { get; set; }
        public DbSet<ClubRegisterApplicant> ClubRegisterApplicants { get; set; }
        public DbSet<ClubRegisterApplicantDescription> ClubRegisterApplicantDescriptions { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<FundDetails> FundDetailses { get; set; }
        public DbSet<ClubMember> ClubMembers { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollItem> PollItems { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<MessageContent> MessageContents { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventDescription> EventDescriptions { get; set; }
        public DbSet<LocationAssignment> LocationAssignments { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationUnavailableTime> LocationAvailableTimes { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<AssetAssignment> AssetAssignments { get; set; }
        public DbSet<AssetBase> Assets { get; set; }
        public DbSet<SubEvent> SubEvents { get; set; }
        public DbSet<SubEventDescription> SubEventDescriptions { get; set; }
        public DbSet<UserPoll> UserPolls { get; set; }
        public DbSet<ApplicationRejectReason> ApplicationRejectReasons { get; set; }
        public DbSet<IdentityForTPC> Identities { get; set; }
        public DbSet<EventRejectReason> EventRejectReasons { get; set; }

        public SCUTClubContext() : base("DefaultConnection") { }

        protected override void OnModelCreating(DbModelBuilder model_builder)
        {
            model_builder.Conventions.Remove<PluralizingTableNameConvention>();

            // 继承处理。
            // TPT
            model_builder.Entity<Student>().ToTable("Student");

            // TPC
            // Application
            model_builder.Entity<ClubUnregisterApplication>().Map(m =>
                {
                    m.MapInheritedProperties();
                    m.ToTable("ClubUnregisterApplication");
                });
            model_builder.Entity<AssetApplication>().Map(m =>
                {
                    m.MapInheritedProperties();
                    m.ToTable("AssetApplication");
                });
            model_builder.Entity<LocationApplication>().Map(m =>
                {
                    m.MapInheritedProperties();
                    m.ToTable("LocationApplication");
                });
            model_builder.Entity<FundApplication>().Map(m =>
                {
                    m.MapInheritedProperties();
                    m.ToTable("FundApplication");
                });
            model_builder.Entity<ClubApplication>().Map(m =>
                {
                    m.MapInheritedProperties();
                    m.ToTable("ClubApplication");
                });
            model_builder.Entity<ClubInfoModificationApplication>().Map(m =>
                {
                    m.MapInheritedProperties();
                    m.ToTable("ClubInfoModificationApplication");
                });
            model_builder.Entity<ClubRegisterApplication>().Map(m =>
                {
                    m.MapInheritedProperties();
                    m.ToTable("ClubRegisterApplication");
                });

            model_builder.Entity<Application>()
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // AssetBase
            model_builder.Entity<Asset>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Asset");
            });
            model_builder.Entity<ApplicatedAsset>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("ApplicatedAsset");
            });
            model_builder.Entity<AssignedAsset>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("AssignedAsset");
            });

            model_builder.Entity<AssetBase>()
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            
            // RoleBase
            model_builder.Entity<UserRole>().Map(m =>
                {
                    m.MapInheritedProperties();
                    m.ToTable("UserRole");
                });
            model_builder.Entity<ClubRole>().Map(m =>
                {
                    m.MapInheritedProperties();
                    m.ToTable("ClubRole");
                });

            model_builder.Entity<RoleBase>()
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            model_builder.Entity<User>().Property(t => t.UserName).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            model_builder.Entity<AssetApplication>().Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Id生成器的主键无需自动生成
            model_builder.Entity<IdentityForTPC>().Property(t => t.BaseName).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            model_builder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // 表之间的关系。
            model_builder.Entity<ClubMember>().HasRequired(t => t.Branch).WithMany(t => t.Members);
            model_builder.Entity<Student>().HasRequired(t => t.ContactInfo).WithRequiredPrincipal().WillCascadeOnDelete(true);
            model_builder.Entity<Application>().HasOptional(t => t.RejectReason).WithRequired().WillCascadeOnDelete(true);
            model_builder.Entity<SubEvent>().HasRequired(t => t.Description).WithRequiredPrincipal().WillCascadeOnDelete(true);
            model_builder.Entity<SubEvent>().HasMany(t => t.LocationApplications).WithOptional(t => t.SubEvent).WillCascadeOnDelete(false);
            model_builder.Entity<SubEvent>().HasMany(t => t.AssetApplications).WithOptional(t => t.SubEvent).WillCascadeOnDelete(false);
            model_builder.Entity<ClubApplication>().HasRequired(t => t.Details).WithRequiredPrincipal().WillCascadeOnDelete(true);
            model_builder.Entity<Student>().HasMany(t => t.Events).WithMany(t => t.Organizers).Map(
                m =>
                {
                    m.MapLeftKey("StudentId");
                    m.MapRightKey("EventId");
                    m.ToTable("EventOrganizer");
                }
            );
            model_builder.Entity<ClubRegisterApplication>().HasMany(t => t.Branches).WithRequired().HasForeignKey(t => t.ApplicationId).WillCascadeOnDelete(true);
            model_builder.Entity<ClubRegisterApplication>().HasMany(t => t.Applicants).WithRequired(t => t.Application).WillCascadeOnDelete(true);
            model_builder.Entity<ClubInfoModificationApplication>().HasMany(t => t.ModificationBranches).WithRequired().
                HasForeignKey(t => t.ApplicationId).WillCascadeOnDelete(true);
            model_builder.Entity<ClubRegisterApplicant>().HasRequired(t => t.Description).WithRequiredPrincipal().WillCascadeOnDelete(true);
            model_builder.Entity<Event>().HasRequired(t => t.Description).WithRequiredPrincipal().WillCascadeOnDelete(true);
            model_builder.Entity<Event>().HasMany(t => t.SubEvents).WithRequired(t => t.Event).WillCascadeOnDelete(true);
            model_builder.Entity<Event>().HasOptional(t => t.FundApplication).WithOptionalPrincipal(t => t.Event).Map(m => m.MapKey("EventId")).WillCascadeOnDelete(false);
            model_builder.Entity<Event>().HasOptional(t => t.RejectReason).WithRequired().WillCascadeOnDelete(true);
            model_builder.Entity<Message>().HasRequired(t => t.Content).WithRequiredPrincipal().WillCascadeOnDelete(true);
            model_builder.Entity<User>().HasMany(t => t.SentMessages).WithRequired(t => t.Sender);
            model_builder.Entity<User>().HasMany(t => t.ReceivedMessages).WithOptional(t => t.Receiver).WillCascadeOnDelete(true);
            model_builder.Entity<User>().HasMany(t => t.Polls).WithRequired(t => t.Author).WillCascadeOnDelete(true);
            model_builder.Entity<User>().HasMany(t => t.Threads).WithOptional(t => t.Author).WillCascadeOnDelete(false);
            model_builder.Entity<User>().HasMany(t => t.Replies).WithOptional(t => t.Author).WillCascadeOnDelete(false);
            model_builder.Entity<Club>().HasMany(t => t.Branches).WithRequired(t => t.Club).WillCascadeOnDelete(true);
            model_builder.Entity<Club>().HasMany(t => t.Members).WithRequired(t => t.Club).WillCascadeOnDelete(true);
            model_builder.Entity<Club>().HasRequired(t => t.MajorInfo).WithRequiredDependent().WillCascadeOnDelete(true);
            model_builder.Entity<Club>().HasRequired(t => t.SubInfo).WithRequiredDependent().WillCascadeOnDelete(true);
            model_builder.Entity<Club>().HasMany(t => t.Events).WithRequired(t => t.Club).WillCascadeOnDelete(true);
            model_builder.Entity<Club>().HasMany(t => t.Applications).WithOptional(t => t.Club).WillCascadeOnDelete(true);
            model_builder.Entity<Club>().HasMany(t => t.AssetAssignments).WithRequired(t => t.Club).WillCascadeOnDelete(true);
            model_builder.Entity<Club>().HasMany(t => t.LocationAssignments).WithRequired(t => t.Club).WillCascadeOnDelete(true);
            
            //model_builder.Entity<ClubRegisterApplication>().HasRequired(t => t.MajorInfo).WithRequiredDependent();
            //model_builder.Entity<ClubRegisterApplication>().HasRequired(t => t.SubInfo).WithRequiredDependent();

            model_builder.Entity<Poll>().HasMany(t => t.Items).WithRequired(t => t.Poll).WillCascadeOnDelete(true);
            
            model_builder.Entity<ClubInfoModificationApplication>().HasMany(t => t.ModificationBranches).WithOptional().WillCascadeOnDelete(true);
            model_builder.Entity<Thread>().HasMany(t => t.Replies).WithRequired(t => t.Thread).WillCascadeOnDelete(true);
            //model_builder.Entity<Time>().HasMany(t => t.AssetApplications).WithRequired(t => t.Time);

            model_builder.Entity<AssetApplication>().HasMany(t => t.ApplicatedAssets).WithRequired(t => t.AssetApplication).WillCascadeOnDelete(true);
            //model_builder.Entity<AssetApplication>().HasOptional(t => t.Assignment).WithRequired(t => t.AssetApplication);
            model_builder.Entity<AssetAssignment>().HasMany(t => t.AssignedAssets).WithRequired(t => t.AssetAssignment).WillCascadeOnDelete(true);

            model_builder.Entity<Student>().HasMany(t => t.MemberShips).WithRequired(t => t.Student).WillCascadeOnDelete(true);
            model_builder.Entity<Student>().HasMany(t => t.Applications).WithRequired(t => t.Applicant).WillCascadeOnDelete(true);
            model_builder.Entity<Student>().HasMany(t => t.LocationAssignments).WithRequired(t => t.Applicant).WillCascadeOnDelete(true);
            model_builder.Entity<Student>().HasMany(t => t.AssetAssignments).WithRequired(t => t.Applicant).WillCascadeOnDelete(true);

            model_builder.Entity<ClubApplication>().HasMany(t => t.Inclinations).WithRequired(t => t.Application).WillCascadeOnDelete(true);

            model_builder.Entity<Location>().HasMany(t => t.UnAvailableTimes).WithRequired(t => t.Location).WillCascadeOnDelete(true);
            model_builder.Entity<Asset>().HasMany(t => t.Applications).WithRequired(t => t.Asset).WillCascadeOnDelete(true);
            model_builder.Entity<Asset>().HasMany(t => t.Assignments).WithRequired(t => t.Asset).WillCascadeOnDelete(true);

            model_builder.Entity<AssetAssignment>().HasMany(t => t.Times).WithMany();
            model_builder.Entity<LocationApplication>().HasMany(t => t.Times).WithMany();
            //model_builder.Entity<LocationApplication>().HasOptional(t => t.Assignment).WithRequired(t => t.LocationApplication);
            model_builder.Entity<SubEvent>().HasMany(t => t.Times).WithMany();
            model_builder.Entity<LocationAssignment>().HasMany(t => t.Times).WithMany();
            model_builder.Entity<AssetApplication>().HasMany(t => t.Times).WithMany();
        }
        
        public DbSet<LocationApplication> LocationApplications { get; set; }

        public DbSet<Student> Students { get; set; }
    }
} 
