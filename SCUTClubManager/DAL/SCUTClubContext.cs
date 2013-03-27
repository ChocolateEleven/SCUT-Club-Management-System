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
        public DbSet<ClubInfoDetails> ClubInfoDetailses { get; set; }
        public DbSet<ClubInfo> ClubInfos { get; set; }
        public DbSet<BranchModification> BranchModifications { get; set; }
        public DbSet<ClubBranch> ClubBranches { get; set; }
        public DbSet<ClubApplicationInclination> ClubApplicationInclinations { get; set; }
        public DbSet<ClubApplicationDetails> ClubApplicationDetailses { get; set; }
        public DbSet<ClubRole> ClubRoles { get; set; }
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
        public DbSet<EventOrganizer> EventOrganizers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventDescription> EventDescriptions { get; set; }
        public DbSet<LocationAssignment> LocationAssignments { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationAvailableTime> LocationAvailableTimes { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<AssetAssignment> AssetAssignments { get; set; }
        public DbSet<Asset> Assets { get; set; }
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

            // 表之间的关系。
            model_builder.Entity<ClubInfo>().HasRequired(t => t.Details).WithRequiredPrincipal(t => t.Info);
            model_builder.Entity<Student>().HasRequired(t => t.ContactInfo).WithRequiredPrincipal(t => t.Student);
            model_builder.Entity<Application>().HasOptional(t => t.RejectReason).WithRequired(t => t.Application);
            model_builder.Entity<SubEvent>().HasRequired(t => t.Description).WithRequiredPrincipal(t => t.SubEvent);
            model_builder.Entity<Event>().HasRequired(t => t.Description).WithRequiredPrincipal(t => t.Event);
            model_builder.Entity<SubEvent>().HasOptional(t => t.FundApplication).WithOptionalPrincipal(t => t.SubEvent);
            model_builder.Entity<ClubApplication>().HasRequired(t => t.Details).WithRequiredPrincipal(t => t.Application);
            model_builder.Entity<Message>().HasRequired(t => t.Content).WithRequiredPrincipal(t => t.Message);
        }
    }
}