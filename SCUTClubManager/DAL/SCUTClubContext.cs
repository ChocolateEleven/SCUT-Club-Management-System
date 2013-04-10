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
        public DbSet<LocationAvailableTime> LocationAvailableTimes { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<AssetAssignment> AssetAssignments { get; set; }
        public DbSet<AssetBase> Assets { get; set; }
        public DbSet<SubEvent> SubEvents { get; set; }
        public DbSet<SubEventDescription> SubEventDescriptions { get; set; }
        public DbSet<UserPoll> UserPolls { get; set; }
        public DbSet<ApplicationRejectReason> ApplicationRejectReasons { get; set; }

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

            // 表之间的关系。
            model_builder.Entity<Student>().HasRequired(t => t.ContactInfo).WithRequiredPrincipal();
            model_builder.Entity<Application>().HasOptional(t => t.RejectReason).WithRequired();
            model_builder.Entity<SubEvent>().HasRequired(t => t.Description).WithRequiredPrincipal();
            model_builder.Entity<SubEvent>().HasOptional(t => t.FundApplication).WithOptionalPrincipal(t => t.SubEvent);
            model_builder.Entity<ClubApplication>().HasRequired(t => t.Details).WithRequiredPrincipal();
            model_builder.Entity<Student>().HasMany(t => t.Events).WithMany(t => t.Organizers).Map(
                m =>
                {
                    m.MapLeftKey("StudentId");
                    m.MapRightKey("EventId");
                    m.ToTable("EventOrganizer");
                }
            );
            model_builder.Entity<ClubRegisterApplication>().HasMany(t => t.Branches).WithRequired().HasForeignKey(t => t.ApplicationId);
            model_builder.Entity<ClubInfoModificationApplication>().HasMany(t => t.ModificationBranches).WithRequired().HasForeignKey(t => t.ApplicationId);
            model_builder.Entity<ClubRegisterApplicant>().HasRequired(t => t.Description).WithRequiredPrincipal();
            model_builder.Entity<Event>().HasRequired(t => t.Description).WithRequiredPrincipal();
            model_builder.Entity<FundApplication>().HasOptional(t => t.SubEvent).WithOptionalDependent(t => t.FundApplication).Map(m => m.MapKey("SubEventId"));
            model_builder.Entity<Message>().HasRequired(t => t.Content).WithRequiredPrincipal();
            model_builder.Entity<User>().HasMany(t => t.SentMessages).WithRequired(t => t.Sender);
            model_builder.Entity<User>().HasMany(t => t.ReceivedMessages).WithOptional(t => t.Receiver);

            model_builder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            model_builder.Entity<Club>().HasMany(t => t.Branches).WithRequired(t => t.Club).WillCascadeOnDelete(true);
            model_builder.Entity<Club>().HasMany(t => t.Members).WithRequired(t => t.Club).WillCascadeOnDelete(true);
            model_builder.Entity<Club>().HasRequired(t => t.MajorInfo).WithRequiredDependent().WillCascadeOnDelete(true);
            model_builder.Entity<Club>().HasRequired(t => t.SubInfo).WithRequiredDependent().WillCascadeOnDelete(true);
            model_builder.Entity<Club>().HasMany(t => t.Events).WithRequired(t => t.Club).WillCascadeOnDelete(true);
            model_builder.Entity<Club>().HasMany(t => t.Applications).WithOptional(t => t.Club).WillCascadeOnDelete(true);
            model_builder.Entity<Club>().HasMany(t => t.AssetAssignments).WithRequired(t => t.Club).WillCascadeOnDelete(true);
            model_builder.Entity<Club>().HasMany(t => t.LocationAssignments).WithRequired(t => t.Club).WillCascadeOnDelete(true);

            model_builder.Entity<ClubRegisterApplication>().HasRequired(t => t.MajorInfo).WithRequiredDependent();
            model_builder.Entity<ClubRegisterApplication>().HasRequired(t => t.SubInfo).WithRequiredDependent();

            model_builder.Entity<Poll>().HasMany(t => t.Items).WithRequired(t => t.Poll).WillCascadeOnDelete(true);

            model_builder.Entity<ClubInfoModificationApplication>().HasMany(t => t.ModificationBranches).WithOptional().WillCascadeOnDelete(true);
        }
    }
}