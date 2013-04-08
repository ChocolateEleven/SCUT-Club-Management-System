using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SCUTClubManager.Models;
using System.Web.Security;
using SCUTClubManager.BusinessLogic;

namespace SCUTClubManager.DAL
{
    public class SCUTClubContextInitializer : DropCreateDatabaseAlways<SCUTClubContext>
    {
        protected override void Seed(SCUTClubContext context)
        {
            base.Seed(context);

            #region UserRoles
            List<UserRole> roles = new List<UserRole>
            {
                new UserRole
                {
                    Id = 1,
                    Name = "学生"
                },
                new UserRole
                {
                    Id = 2,
                    Name = "社联"
                }
            };
            #endregion

            #region Users(including students)
            var students = new List<User>
            {
                new User
                {
                    UserName = "Admin",
                    Password = PasswordProcessor.ProcessWithMD5("Admin"),
                    RoleId = 2
                },

                new Student { 
                    UserName = "000000001",
                    Password = PasswordProcessor.ProcessWithMD5("123456"),
                    RoleId = 1,
                    FirstName = "一",
                    LastName = "张",
                    Gender = "M",
                    Birthday = new DateTime(1990,01,01),
                    Department = "软件学院",
                    Major = "软件工程",
                    Grade = "3",
                    Degree = "b",
                    PoliticalId = "t",
                    ContactInfo = new ContactInfo
                    {
                        UserName = "000000001",
                        QQ = "10086",
                        Phone = "12580",
                        Room = "C1-101",
                        Visibility = "s"
                    }
                },

                new Student { 
                    UserName = "000000002",
                    Password = PasswordProcessor.ProcessWithMD5("123456"),
                    RoleId = 1,
                    FirstName = "二",
                    LastName = "张",
                    Gender = "M",
                    Birthday = new DateTime(1990,01,01),
                    Department = "计算机学院",
                    Major = "计算机软件",
                    Grade = "3",
                    Degree = "b",
                    PoliticalId = "t",
                    ContactInfo = new ContactInfo
                    {
                        UserName = "000000002",
                        QQ = "10086",
                        Phone = "12580",
                        Room = "C1-101",
                        Visibility = "s"
                    }
                },

                new Student { 
                    UserName = "000000003",
                    Password = PasswordProcessor.ProcessWithMD5("123456"),
                    RoleId = 1,
                    FirstName = "三",
                    LastName = "张",
                    Gender = "M",
                    Birthday = new DateTime(1990,01,01),
                    Department = "生物学院",
                    Major = "生物工程",
                    Grade = "3",
                    Degree = "b",
                    PoliticalId = "t",
                    ContactInfo = new ContactInfo
                    {
                        UserName = "000000003",
                        QQ = "10086",
                        Phone = "12580",
                        Room = "C1-101",
                        Visibility = "s"
                    }
                },

                new Student { 
                    UserName = "000000004",
                    Password = PasswordProcessor.ProcessWithMD5("123456"),
                    RoleId = 1,
                    FirstName = "四",
                    LastName = "张",
                    Gender = "M",
                    Birthday = new DateTime(1990,01,01),
                    Department = "生物学院",
                    Major = "生物工程",
                    Grade = "3",
                    Degree = "b",
                    PoliticalId = "t",
                    ContactInfo = new ContactInfo
                    {
                        UserName = "000000004",
                        QQ = "10086",
                        Phone = "12580",
                        Room = "C1-101",
                        Visibility = "s"
                    }
                }


            };
            #endregion

            #region Messages
            var messages = new List<Message>
            {
                new Message{
                    SenderId = students[3].UserName,
                    ReceiverId = students[1].UserName,
                    Title = "test",
                    ReadMark = true,
                    Date = new DateTime(2009,09,09),
                   Sender = students[3],
                   Receiver = students[1],
                   Content = new MessageContent{
                       Content = "testtest"
                   }
                }
            };
            #endregion

            #region Polls
            var polls = new List<Poll> 
            { 
                new Poll{
                    Title = "testPoll",
                    Question = "Poll Question",
                    Author = students[1],
                    AuthorUserName = students[1].UserName,
                    OpenDate = new DateTime(2013,01,01),
                    CloseDate = new DateTime(2013,04,01),
                    IsMultiSelectable = false,
                    Items = new List<PollItem>
                    {
                        new PollItem{
                            PollId = 1,
                            Caption = "A",
                            Count = 10
                        },
                        new PollItem{
                            PollId = 2,
                            Caption = "B",
                            Count = 170
                        },
                        new PollItem{
                            PollId = 3,
                            Caption = "C",
                            Count = 50
                        },
                        new PollItem{
                            PollId = 4,
                            Caption = "D",
                            Count = 20
                        }
                    }
                   
                },

                new Poll{
                    Title = "testPoll2",
                    Question = "Poll Question2",
                    Author = students[2],
                    AuthorUserName = students[1].UserName,
                    OpenDate = new DateTime(2013,04,01),
                    CloseDate = new DateTime(2013,04,22),
                    IsMultiSelectable = false,
                    Items = new List<PollItem>
                    {
                        new PollItem{
                            PollId = 1,
                            Caption = "A",
                            Count = 0
                        },
                        new PollItem{
                            PollId = 2,
                            Caption = "B",
                            Count = 760
                        },
                        new PollItem{
                            PollId = 3,
                            Caption = "C",
                            Count = 540
                        },
                        new PollItem{
                            PollId = 4,
                            Caption = "D",
                            Count = 230
                        }
                    }
                   
                },

                new Poll{
                    Title = "testPoll3",
                    Question = "Poll Question3",
                    Author = students[2],
                    AuthorUserName = students[1].UserName,
                    OpenDate = new DateTime(2013,05,10),
                    CloseDate = new DateTime(2013,05,30),
                    IsMultiSelectable = false,
                    Items = new List<PollItem>
                    {
                        new PollItem{
                            PollId = 1,
                            Caption = "A",
                            Count = 60
                        },
                        new PollItem{
                            PollId = 2,
                            Caption = "B",
                            Count = 370
                        },
                        new PollItem{
                            PollId = 3,
                            Caption = "C",
                            Count = 5
                        },
                        new PollItem{
                            PollId = 4,
                            Caption = "D",
                            Count = 30
                        }
                    }
                   
                }
            };
            #endregion

            #region ClubMajorInfos
            List<ClubMajorInfo> major_infos = new List<ClubMajorInfo>
            {
                new ClubMajorInfo
                {
                    Instructor = "萨麦尔",
                    Name = "Ira"
                },
                new ClubMajorInfo
                {
                    Instructor = "利维坦",
                    Name = "Invidia"
                },
                new ClubMajorInfo
                {
                    Instructor = "路西法",
                    Name = "Superbia"
                },
                new ClubMajorInfo
                {
                    Instructor = "阿斯莫德",
                    Name = "Luxuria"
                }
            };
            #endregion

            #region ClubSubInfos
            List<ClubSubInfo> sub_infos = new List<ClubSubInfo>
            {
                new ClubSubInfo
                {
                    Address = "Nevaeh",
                    PosterUrl = "Ira.png",
                    Principle = "Saligia",
                    Purpose = "...",
                    Range = "Ultimate",
                    Regulation = "Nothing"
                },
                new ClubSubInfo
                {
                    Address = "Esidarap",
                    PosterUrl = "Invidia.png",
                    Principle = "Saligia",
                    Purpose = "...",
                    Range = "Ultimate",
                    Regulation = "Nothing"
                },
                new ClubSubInfo
                {
                    Address = "Anavrin",
                    PosterUrl = "Superbia.png",
                    Principle = "Saligia",
                    Purpose = "...",
                    Range = "Ultimate",
                    Regulation = "Nothing"
                },
                new ClubSubInfo
                {
                    Address = "Muisyle",
                    PosterUrl = "Luxuria.png",
                    Principle = "Saligia",
                    Purpose = "...",
                    Range = "Ultimate",
                    Regulation = "Nothing"
                }
            };
            #endregion

            #region Clubs
            List<Club> clubs = new List<Club>
            {
                new Club
                {
                    Level = 1,
                    Fund = 100,
                    FoundDate = DateTime.Now,
                    MajorInfo = major_infos[0],
                    SubInfo = sub_infos[0],
                    MemberCount = 2
                },
                new Club
                {
                    Level = 1,
                    Fund = 20,
                    FoundDate = DateTime.Now.AddDays(10),
                    MajorInfo = major_infos[1],
                    SubInfo = sub_infos[1],
                    MemberCount = 1
                },
                new Club
                {
                    Level = 3,
                    Fund = 1000,
                    FoundDate = DateTime.Parse("2009-10-20"),
                    MajorInfo = major_infos[2],
                    SubInfo = sub_infos[2],
                    MemberCount = 3
                },
                new Club
                {
                    Level = 0,
                    Fund = 33,
                    FoundDate = DateTime.Parse("2012-11-11"),
                    MajorInfo = major_infos[3],
                    SubInfo = sub_infos[3],
                    MemberCount = 1
                }
            };
            #endregion

            #region ClubBranches
            List<ClubBranch> branches = new List<ClubBranch>
            {
                new ClubBranch
                {
                    Club = clubs[0],
                    MemberCount = 2,
                    NewMemberCount = 0,
                    BranchName = "会员部"
                },
                new ClubBranch
                {
                    Club = clubs[0],
                    MemberCount = 0,
                    NewMemberCount = 0,
                    BranchName = "信息部"
                },
                new ClubBranch
                {
                    Club = clubs[1],
                    MemberCount = 1,
                    NewMemberCount = 1,
                    BranchName = "会员部"
                },
                new ClubBranch
                {
                    Club = clubs[2],
                    MemberCount = 1,
                    NewMemberCount = 0,
                    BranchName = "会员部"
                },
                new ClubBranch
                {
                    Club = clubs[2],
                    MemberCount = 0,
                    NewMemberCount = 0,
                    BranchName = "公交部"
                },
                new ClubBranch
                {
                    Club = clubs[2],
                    MemberCount = 2,
                    NewMemberCount = 1,
                    BranchName = "宣传部"
                },
                new ClubBranch
                {
                    Club = clubs[3],
                    MemberCount = 1,
                    NewMemberCount = 1,
                    BranchName = "会员部"
                }
            };
            #endregion

            #region ClubRoles
            List<ClubRole> club_roles = new List<ClubRole>
            {
                new ClubRole
                {
                    Id = 3,
                    Name = "会员"
                },
                new ClubRole
                {
                    Id = 4,
                    Name = "干事"
                },
                new ClubRole
                {
                    Id = 5,
                    Name = "会长"
                },
                new ClubRole
                {
                    Id = 6,
                    Name = "部长"
                }
            };
            #endregion

            #region ClubMembers
            List<ClubMember> club_members = new List<ClubMember>
            {
                new ClubMember
                {
                    BranchId = 1,
                    ClubId = 1,
                    ClubRoleId = 3,
                    JoinDate = DateTime.Now,
                    UserName = "000000001"
                },
                new ClubMember
                {
                    BranchId = 1,
                    ClubId = 1,
                    ClubRoleId = 5,
                    JoinDate = DateTime.Now,
                    UserName = "000000002"
                },
                new ClubMember
                {
                    BranchId = 3,
                    ClubId = 2,
                    ClubRoleId = 5,
                    JoinDate = DateTime.Now.AddDays(20),
                    UserName = "000000003"
                },
                new ClubMember
                {
                    BranchId = 4,
                    ClubId = 3,
                    ClubRoleId = 5,
                    JoinDate = DateTime.Parse("2009-10-20"),
                    UserName = "000000004"
                },
                new ClubMember
                {
                    BranchId = 6,
                    ClubId = 3,
                    ClubRoleId = 6,
                    JoinDate = DateTime.Parse("2010-3-3"),
                    UserName = "000000003"
                },
                new ClubMember
                {
                    BranchId = 6,
                    ClubId = 3,
                    ClubRoleId = 4,
                    JoinDate = DateTime.Now,
                    UserName = "000000001"
                },
                new ClubMember
                {
                    BranchId = 7,
                    ClubId = 4,
                    ClubRoleId = 5,
                    JoinDate = DateTime.Parse("2012-11-11"),
                    UserName = "000000001"
                }
            };
            #endregion

            #region ClubRegisterApplications
            List<ClubRegisterApplication> club_register_applications = new List<ClubRegisterApplication>
            {
                new ClubRegisterApplication
                {
                    Id = 1,
                    Date = DateTime.Now,
                    ClubId = null,
                    Status = "n",
                    Applicant = students[1] as Student,
                    RejectReason = null,
                    Applicants = new List<ClubRegisterApplicant>
                    {
                        new ClubRegisterApplicant
                        {
                            Applicant = students[1] as Student,
                            IsMainApplicant = true,
                            Description = new ClubRegisterApplicantDescription
                            {
                                Description = "4yklnfadkgbglkanfgjngklnhk"
                            }
                        },
                        new ClubRegisterApplicant
                        {
                            Applicant = students[2] as Student,
                            IsMainApplicant = false,
                            Description = new ClubRegisterApplicantDescription
                            {
                                Description = "jkhdsfmdfrrr"
                            }
                        }
                    },
                    Branches = new List<BranchModification>
                    {
                        new BranchCreation
                        {
                            BranchId = null,
                            BranchName = "会员部"
                        }
                    },
                    MajorInfo = new ClubMajorInfo
                    {
                        Name = "新社团",
                        Instructor = "Nameless"
                    },
                    SubInfo = new ClubSubInfo
                    {
                        Principle = "毁灭",
                        Purpose = "破坏",
                        Range = "无限",
                        Address = "地球",
                        PosterUrl = "N.png",
                        Regulation = "None"
                    },
                    Material = "Materials/N.doc"
                }
            };
            #endregion

            #region ClubUnRegisterApplication
            List<ClubUnregisterApplication> club_unregister_applications = new List<ClubUnregisterApplication>
            {
                new ClubUnregisterApplication
                {
                    Id = 2,
                    Date = DateTime.Now,
                    Club = clubs[0],
                    Status = "n",
                    Applicant = students[1] as Student,
                    RejectReason = null,
                    Reason = "I don't care."
                }
            };
            #endregion

            #region ClubInfoModificationApplications
            List<ClubInfoModificationApplication> club_modification_applications = new List<ClubInfoModificationApplication>
            {
                new ClubInfoModificationApplication
                {
                    Id = 3,
                    Date = DateTime.Now,
                    Club = clubs[0],
                    Status = "n",
                    Applicant = students[1] as Student,
                    RejectReason = null,
                    MajorInfo = null,
                    SubInfo = null,
                    ModificationBranches = new List<BranchModification>
                    {
                        new BranchDeletion
                        {
                            OrigBranch = branches[1],
                            ApplicationId = 3
                        }
                    }
                }
            };
            #endregion

            major_infos.ForEach(s => context.ClubMajorInfos.Add(s));
            sub_infos.ForEach(s => context.ClubSubInfos.Add(s));
            club_roles.ForEach(s => context.RoleBases.Add(s));
            clubs.ForEach(s => context.Clubs.Add(s));
            branches.ForEach(s => context.ClubBranches.Add(s));
            club_members.ForEach(s => context.ClubMembers.Add(s));
            polls.ForEach(s => context.Polls.Add(s));
            roles.ForEach(s => context.RoleBases.Add(s));
            students.ForEach(s => context.Users.Add(s));
            messages.ForEach(s => context.Messages.Add(s));
            club_register_applications.ForEach(s => context.Applications.Add(s));
            club_unregister_applications.ForEach(s => context.Applications.Add(s));
            club_modification_applications.ForEach(s => context.Applications.Add(s));

            context.SaveChanges();
        }
    }
}