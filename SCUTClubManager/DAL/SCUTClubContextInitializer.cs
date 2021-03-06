﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.Entity;
using SCUTClubManager.Models;
using System.Web.Security;
using SCUTClubManager.BusinessLogic;
using SCUTClubManager.Helpers;

namespace SCUTClubManager.DAL
{
    public class SCUTClubContextInitializer : DropCreateDatabaseAlways<SCUTClubContext>
    {
        protected override void Seed(SCUTClubContext context)
        {
            base.Seed(context);

            #region Ids
            var ids = new List<IdentityForTPC>
            {
                new IdentityForTPC
                {
                    BaseName = IdentityForTPC.APPLICATION,
                    Identity = 15
                },
                new IdentityForTPC
                {
                    BaseName = IdentityForTPC.ASSET_BASE,
                    Identity = 20
                },
                new IdentityForTPC
                {
                    BaseName = IdentityForTPC.ROLE_BASE,
                    Identity = 7
                }
            };
            #endregion

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
                    Name = "社联",
                    Password = PasswordProcessor.ProcessWithMD5("Admin"),
                    RoleId = 2
                },

                new Student { 
                    UserName = "000000001",
                    Password = PasswordProcessor.ProcessWithMD5("123456"),
                    RoleId = 1,
                    Name = "张一",
                    Gender = Student.MALE,
                    Birthday = new DateTime(1990,01,01),
                    Department = "软件学院",
                    Major = "软件工程",
                    Grade = "09级",
                    Degree = Student.BACHELOR,
                    PoliticalId = "t",
                    ContactInfo = new ContactInfo
                    {
                        UserName = "000000001",
                        QQ = "10086",
                        Phone = "12580",
                        Room = "C1-101",
                        Visibility = "a"
                    }
                },

                new Student { 
                    UserName = "000000002",
                    Password = PasswordProcessor.ProcessWithMD5("123456"),
                    RoleId = 1,
                    Name = "张二",
                    Gender = Student.MALE,
                    Birthday = new DateTime(1990,01,01),
                    Department = "计算机学院",
                    Major = "计算机软件",
                    Grade = "10级",
                    Degree = Student.BACHELOR,
                    PoliticalId = "t",
                    ContactInfo = new ContactInfo
                    {
                        UserName = "000000002",
                        QQ = "10086",
                        Phone = "12580",
                        Room = "C1-101",
                        Visibility = "c"
                    }
                },

                new Student { 
                    UserName = "000000003",
                    Password = PasswordProcessor.ProcessWithMD5("123456"),
                    RoleId = 1,
                    Name = "张三",
                    Gender = Student.MALE,
                    Birthday = new DateTime(1990,01,01),
                    Department = "生物学院",
                    Major = "生物工程",
                    Grade = "11级",
                    Degree = Student.BACHELOR,
                    PoliticalId = "t",
                    ContactInfo = new ContactInfo
                    {
                        UserName = "000000003",
                        QQ = "10086",
                        Phone = "12580",
                        Room = "C1-101",
                        Visibility = "c"
                    }
                },

                new Student { 
                    UserName = "000000004",
                    Password = PasswordProcessor.ProcessWithMD5("123456"),
                    RoleId = 1,
                    Name = "张四",
                    Gender = Student.MALE,
                    Birthday = new DateTime(1990,01,01),
                    Department = "生物学院",
                    Major = "生物工程",
                    Grade = "12级",
                    Degree = Student.BACHELOR,
                    PoliticalId = "t",
                    ContactInfo = new ContactInfo
                    {
                        UserName = "000000004",
                        QQ = "10086",
                        Phone = "12580",
                        Room = "C1-101",
                        Visibility = "c"
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
                    IsMultiSelectable = true,
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
                    MemberCount = 1,
                    NewMemberCount = 1
                },
                new Club
                {
                    Level = 3,
                    Fund = 1000,
                    FoundDate = DateTime.Parse("2009-10-20"),
                    MajorInfo = major_infos[2],
                    SubInfo = sub_infos[2],
                    MemberCount = 3,
                    NewMemberCount = 1
                },
                new Club
                {
                    Level = 0,
                    Fund = 33,
                    FoundDate = DateTime.Parse("2012-11-11"),
                    MajorInfo = major_infos[3],
                    SubInfo = sub_infos[3],
                    MemberCount = 1,
                    NewMemberCount = 1
                }
            };
            #endregion

            #region ClubBranches
            List<ClubBranch> branches = new List<ClubBranch>
            {
                new ClubBranch
                {
                    Id = 1,
                    Club = clubs[0],
                    MemberCount = 2,
                    NewMemberCount = 0,
                    BranchName = "会员部"
                },
                new ClubBranch
                {
                    Id = 2,
                    Club = clubs[0],
                    MemberCount = 0,
                    NewMemberCount = 0,
                    BranchName = "信息部"
                },
                new ClubBranch
                {
                    Id = 3,
                    Club = clubs[1],
                    MemberCount = 1,
                    NewMemberCount = 1,
                    BranchName = "会员部"
                },
                new ClubBranch
                {
                    Id = 4,
                    Club = clubs[2],
                    MemberCount = 1,
                    NewMemberCount = 0,
                    BranchName = "会员部"
                },
                new ClubBranch
                {
                    Id = 5,
                    Club = clubs[2],
                    MemberCount = 0,
                    NewMemberCount = 0,
                    BranchName = "公交部"
                },
                new ClubBranch
                {
                    Id = 6,
                    Club = clubs[2],
                    MemberCount = 2,
                    NewMemberCount = 1,
                    BranchName = "宣传部"
                },
                new ClubBranch
                {
                    Id = 7,
                    Club = clubs[3],
                    MemberCount = 1,
                    NewMemberCount = 1,
                    BranchName = "会员部"
                },
                new ClubBranch
                {
                    Id = 8,
                    Club = clubs[3],
                    MemberCount = 0,
                    NewMemberCount = 0,
                    BranchName = "文化部"
                },
                new ClubBranch
                {
                    Id = 9,
                    Club = clubs[3],
                    MemberCount = 0,
                    NewMemberCount = 0,
                    BranchName = "天文部"
                },
                new ClubBranch
                {
                    Id = 10,
                    Club = clubs[3],
                    MemberCount = 0,
                    NewMemberCount = 0,
                    BranchName = "考古部"
                },
                new ClubBranch
                {
                    Id = 11,
                    Club = clubs[1],
                    MemberCount = 0,
                    NewMemberCount = 0,
                    BranchName = "爆破部"
                },
                new ClubBranch
                {
                    Id = 12,
                    Club = clubs[1],
                    MemberCount = 0,
                    NewMemberCount = 0,
                    BranchName = "XX部"
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
                    Branch = branches[0],
                    ClubId = 1,
                    ClubRoleId = 4,
                    JoinDate = DateTime.Now,
                    UserName = "000000001"
                },
                new ClubMember
                {
                    Branch = branches[0],
                    ClubId = 1,
                    ClubRoleId =4,
                    JoinDate = DateTime.Now,
                    UserName = "000000002"
                },
                new ClubMember
                {
                    Branch = branches[2],
                    ClubId = 2,
                    ClubRoleId = 5,
                    JoinDate = DateTime.Now.AddDays(20),
                    UserName = "000000003"
                },
                new ClubMember
                {
                    Branch = branches[3],
                    ClubId = 3,
                    ClubRoleId = 5,
                    JoinDate = DateTime.Parse("2009-10-20"),
                    UserName = "000000004"
                },
                new ClubMember
                {
                    Branch = branches[5],
                    ClubId = 3,
                    ClubRoleId = 6,
                    JoinDate = DateTime.Parse("2010-3-3"),
                    UserName = "000000003"
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
                    Applicants = new List<ClubRegisterApplicant>(),
                    Branches = new List<BranchModification>
                    {
                        new BranchCreation
                        {
                            OrigBranch = null,
                            BranchName = "会员部",
                            ApplicationId = 1
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
                    }
                }
            };

            club_register_applications[0].Applicants.Add(
                new ClubRegisterApplicant
                {
                    Applicant = students[1] as Student,
                    IsMainApplicant = true,
                    Description = new ClubRegisterApplicantDescription
                    {
                        Description = "I have two tables Application_User and Application_User_Access." +
                        " Application_User_Access table is having a foreign key constraint with Application_User " +
                        "table.When I delete a record in Application_User table, I receive The DELETE statement conflicted "
                    }
                });

            club_register_applications[0].Applicants.Add(
                new ClubRegisterApplicant
                {
                    Applicant = students[2] as Student,
                    IsMainApplicant = false,
                });
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

            #region BranchModifications
            BranchModification branch_modification = new BranchCreation
            {
                BranchName = "New Branch",
                ApplicationId = 3
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
                        branch_modification
                    }
                }
            };
            #endregion

            #region Threads
            List<Thread> threads = new List<Thread>
            {
                new Thread
                {
                    Id = 1,
                    Title = "华南理工大学社团管理系统",
                    Author = students[1],
                    AuthorUserName = students[1].UserName,
                    PostDate = new DateTime(2002,03,04),
                    LatestReplyDate = new DateTime(2002,03,06),
                    Replies = new List<Reply>
                    {
                        new Reply
                        {
                            Id = 1,
                            ThreadId = 1,
                            Content = "灌水灌水",
                            Author = students[1],
                            AuthorUserName = students[1].UserName,
                            Date = new DateTime(2002,03,04),
                            Number = 1
                        },

                        new Reply
                        {
                            Id = 2,
                            ThreadId = 1,
                            Content = "内容内容内容",
                            Author = students[2],
                            AuthorUserName = students[2].UserName,
                            Date = new DateTime(2002,03,05),
                            Number = 2
                        },

                        
                        new Reply
                        {
                            Id = 3,
                            ThreadId = 1,
                            Content = "首先这个和空气中的污染物、阳光的角度都有关。城市早上空气差，这个说法一般来说是没错的，不管是夜间由于冷却作用导致的污染物富集，还是早高峰时段的汽车尾气排放都将导致空气中污染物浓度的上升。空气中颗粒物或者气体污染物影响大气能见度的方式千差万别，比如城市覆盖的棕红色的烟雾就是由于氮氧化物含量较高造成的。在太阳开始照耀大地以后，空气的活动开始，扩散作用加剧，这时候污染物浓度就会开始降低。",
                            Author = students[3],
                            AuthorUserName = students[3].UserName,
                            Date = new DateTime(2002,03,05),
                            Number = 3
                        },

                        new Reply
                        {
                            Id = 4,
                            ThreadId = 1,
                            Content = ",.,.,.,.,..,",
                            Author = students[3],
                            AuthorUserName = students[3].UserName,
                            Date = new DateTime(2002,03,06),
                            Number = 4
                        },
                    }
                },

                new Thread
                {
                    Id = 2,
                    Title = "今天天气不错",
                    Author = students[2],
                    AuthorUserName = students[2].UserName,
                    PostDate = new DateTime(2012,03,04),
                    LatestReplyDate = new DateTime(2002,03,09),
                    Replies = new List<Reply>
                    {
                        new Reply
                        {
                            Id = 1,
                            ThreadId = 2,
                            Content = "灌水灌水",
                            Author = students[2],
                            AuthorUserName = students[2].UserName,
                            Date = new DateTime(2012,03,04),
                            Number = 1
                        },

                        new Reply
                        {
                            Id = 2,
                            ThreadId = 2,
                            Content = "内容内容内容",
                            Author = students[2],
                            AuthorUserName = students[2].UserName,
                            Date = new DateTime(2002,03,05),
                            Number = 2
                        },

                        
                        new Reply
                        {
                            Id = 3,
                            ThreadId = 2,
                            Content = "灌水灌水灌水灌水灌水灌水灌水灌水灌水灌水灌水灌水灌水灌水灌水灌水灌水灌水灌水灌水灌水灌水灌水灌水",
                            Author = students[3],
                            AuthorUserName = students[3].UserName,
                            Date = new DateTime(2002,03,05),
                            Number = 3
                        },

                        new Reply
                        {
                            Id = 4,
                            ThreadId = 2,
                            Content = "text() - Sets or returns the text content of selected elements \n"+
                                        "html() - Sets or returns the content of selected elements (including HTML markup) \n"+
                                        "val() - Sets or returns the value of form fields \n",
                            Author = students[3],
                            AuthorUserName = students[3].UserName,
                            Date = new DateTime(2002,03,09),
                            Number = 4
                        },
                    }
                },

                new Thread
                {
                    Id = 2,
                    Title = "google",
                    Author = students[1],
                    AuthorUserName = students[1].UserName,
                    LatestReplyDate = new DateTime(2013,03,04),
                    PostDate = new DateTime(2013,03,04),
                    Replies = new List<Reply>
                    {
                        new Reply
                        {
                            Id = 1,
                            ThreadId = 2,
                            Content = "此楼只有楼主一个人",
                            Author = students[1],
                            AuthorUserName = students[1].UserName,
                            Date = new DateTime(2013,03,04),
                            Number = 1
                        }
                    }
                }

            };
            #endregion

            #region Time
            List<Time> times = new List<Time>
            {
                new Time
                {
                    TimeName = "上午1、2节课",
                    StartTime = TimeSpan.FromHours(8.8333),
                    EndTime = TimeSpan.FromHours(10.4167)
                },
                new Time
                {
                    TimeName = "上午3、4节课",
                    StartTime = TimeSpan.FromHours(10.4167),
                    EndTime = TimeSpan.FromHours(12.1667)
                },
                new Time
                {
                    TimeName = "中午",
                    StartTime = TimeSpan.FromHours(12.1667),
                    EndTime = TimeSpan.FromHours(14.0)
                },
                new Time
                {
                    TimeName = "下午5、6节课",
                    StartTime = TimeSpan.FromHours(14.0),
                    EndTime = TimeSpan.FromHours(15.5833)
                },
                new Time
                {
                    TimeName = "下午7、8、9节课",
                    StartTime = TimeSpan.FromHours(15.5833),
                    EndTime = TimeSpan.FromHours(18.1667)
                },
                new Time
                {
                    TimeName = "傍晚",
                    StartTime = TimeSpan.FromHours(18.1667),
                    EndTime = TimeSpan.FromHours(19.0)
                },
                new Time
                {
                    TimeName = "晚上10、11、12节课",
                    StartTime = TimeSpan.FromHours(19.0),
                    EndTime = TimeSpan.FromHours(21.4167)
                }
            };
            #endregion

            #region Asset
            List<Asset> assets = new List<Asset>
            {
                new Asset
                {
                    Id = 1,
                    Count = 10,
                    Name = "帐篷"
                },

                new Asset
                {
                    Id = 2,
                    Count = 20,
                    Name = "长桌"
                },

                new Asset
                {
                    Id = 3,
                    Count = 30,
                    Name = "排插"
                },

                new Asset
                {
                    Id = 4,
                    Count = 0,
                    Name = "饮水机"
                }

            };
            #endregion

            #region AssetAssignment
            List<AssetAssignment> asset_assignments = new List<AssetAssignment> 
            {
                new AssetAssignment
                {
                    Date = new DateTime(2013,4,11),
                    Club = clubs[0],
                    Times = new List<Time>{times[0],times[1],times[2]},
                    Applicant = students[1] as Student,
                    AssignedAssets = new List<AssignedAsset>
                    {
                        new AssignedAsset
                        {
                            Id = 11,
                            Count = 4,
                            Asset = assets[0]
                        },
                        new AssignedAsset
                        {
                            Id = 12,
                            Count = 6,
                            Asset = assets[1]
                        },
                        new AssignedAsset
                        {
                            Id = 13,
                            Count = 4,
                            Asset = assets[2]
                        }
                    }
                },
                new AssetAssignment
                {
                    Date = new DateTime(2013,4,12),
                    Club = clubs[1],
                    Times = new List<Time>{times[0],times[1],times[2]},
                    Applicant = students[1] as Student,
                    AssignedAssets = new List<AssignedAsset>
                    {
                        new AssignedAsset
                        {
                            Id = 14,
                            Count = 2,
                            Asset = assets[0]
                        },
                        new AssignedAsset
                        {
                            Id = 15,
                            Count = 30,
                            Asset = assets[2]
                        },
                        new AssignedAsset
                        {
                            Id = 16,
                            Count = 3,
                            Asset = assets[1]
                        }
                    }
                },
                new AssetAssignment
                {
                    Date = new DateTime(2014,4,13),
                    Club = clubs[0],
                    Times = new List<Time>{times[0],times[1],times[2]},
                    Applicant = students[2] as Student,
                    AssignedAssets = new List<AssignedAsset>
                    {
                        new AssignedAsset
                        {
                            Id = 17,
                            Count = 2,
                            Asset = assets[0]
                        },
                        new AssignedAsset
                        {
                            Id = 18,
                            Count = 5,
                            Asset = assets[2]
                        },
                        new AssignedAsset
                        {
                            Id = 19,
                            Count = 1,
                            Asset = assets[1]
                        }
                    }
                }
            };
            #endregion

            #region AssetApplication
            List<AssetApplication> asset_applications = new List<AssetApplication> 
            {
                new AssetApplication
                {
                    Id = 4,
                    Times = new List<Time>{times[0]},
                    Status = "n",
                    Applicant = students[1] as Student,
                    Date = new DateTime(2013,04,11),
                    ApplicatedDate = DateTime.Now,
                    Club = clubs[0],
                    RejectReason = null,
                    ApplicatedAssets = new List<ApplicatedAsset>()
                    {
                        new ApplicatedAsset
                        {
                            Id = 5,
                            Count = 5,
                            Asset = assets[0]
                        },

                        new ApplicatedAsset
                        {
                            Id = 6,
                            Count = 4,
                            Asset = assets[2]
                        }
                    }
                },
                new AssetApplication
                {
                    Id = 5,
                    Times = new List<Time>{times[1]},
                    Club = clubs[1],
                    Status = "f",
                    Applicant = students[3] as Student,
                    Date = new DateTime(2013,04,12),
                    ApplicatedDate = DateTime.Now,
                    RejectReason = null,
                    ApplicatedAssets = new List<ApplicatedAsset>()
                    {
                        new ApplicatedAsset
                        {
                            Id = 7,
                            Count = 3,
                            Asset = assets[0]
                        },

                        new ApplicatedAsset
                        {
                            Id = 8,
                            Count = 5,
                            Asset = assets[2]
                        }
                    }
                }
                ,
                new AssetApplication
                {
                    Id = 6,
                    Times = new List<Time>{times[2]},
                    Club = clubs[1],
                    Status = "p",
                    Applicant = students[2] as Student,
                    Date = new DateTime(2013,04,10),
                    ApplicatedDate = DateTime.Now,
                    RejectReason = null,
                    ApplicatedAssets = new List<ApplicatedAsset>
                    {
                        new ApplicatedAsset
                        {
                            Id = 9,
                            Count = 2,
                            Asset = assets[0]
                        },

                        new ApplicatedAsset
                        {
                            Id = 10,
                            Count = 3,
                            Asset = assets[2]
                        }
                    }
                }
            };
            #endregion

            #region Location
            List<Location> locations = new List<Location> 
            {
                new Location
                {
                    Name = "A1101",
                    UnAvailableTimes = new List<LocationUnavailableTime>
                    {
                        new LocationUnavailableTime
                        {
                            Time = times[0],
                            WeekDayId = 1
                        },
                        new LocationUnavailableTime
                        {
                            Time = times[5],
                            WeekDayId = 5
                        }
                    }
                },
                new Location
                {
                    Name = "A1102",
                    UnAvailableTimes = new List<LocationUnavailableTime>
                    {
                        new LocationUnavailableTime
                        {
                            Time = times[2],
                            WeekDayId = 3
                        },
                        new LocationUnavailableTime
                        {
                            Time = times[4],
                            WeekDayId = 2
                        },
                        new LocationUnavailableTime
                        {
                            Time = times[3],
                            WeekDayId = 2
                        },
                        new LocationUnavailableTime
                        {
                            Time = times[4],
                            WeekDayId = 4
                        }
                    }
                },
                new Location
                {
                    Name = "A1103",
                    UnAvailableTimes = new List<LocationUnavailableTime>
                    {
                        new LocationUnavailableTime
                        {
                            Time = times[1],
                            WeekDayId = 2
                        },
                        new LocationUnavailableTime
                        {
                            Time = times[3],
                            WeekDayId = 4
                        },
                        new LocationUnavailableTime
                        {
                            Time = times[1],
                            WeekDayId = 3
                        }
                    }
                }
            };
            #endregion

            #region LocationApplication
            List<LocationApplication> location_applications = new List<LocationApplication> 
            {
                new LocationApplication
                {
                    Id = 7,
                    Status = "n",
                    Club = clubs[0],
                    Applicant = students[3] as Student,
                    Date = new DateTime(2013,1,3),
                    ApplicatedDate = DateTime.Now,
                    Times = new List<Time>{times[0],times[1],times[2]},
                    Locations = new List<Location>
                    {
                        locations[0],locations[1]
                    }
                },
                new LocationApplication
                {
                    Id = 8,
                    Status = "f",
                    Club = clubs[1],
                    Applicant = students[1] as Student,
                    Date = new DateTime(2013,1,4),
                    ApplicatedDate = DateTime.Now,
                    Times = new List<Time>{times[0],times[1],times[2]},
                    Locations = new List<Location>
                    {
                        locations[2],locations[1],locations[0]
                    }
                },
                new LocationApplication
                {
                    Id = 9,
                    Status = "p",
                    Club = clubs[2],
                    Applicant = students[2] as Student,
                    Date = new DateTime(2013,1,5),
                    ApplicatedDate = DateTime.Now,
                    Times = new List<Time>{times[0],times[1],times[2]},
                    Locations = new List<Location>
                    {
                        locations[0]
                    }
                }
            };
            #endregion

            #region LocationAssignment
            List<LocationAssignment> location_assignments = new List<LocationAssignment>
            {
                new LocationAssignment
                {
                    Date = new DateTime(2013,4,17),
                    Club = clubs[0],
                    Times = new List<Time>{times[0],times[1],times[2]},
                    Locations = new List<Location>{locations[0],locations[1]},
                    Applicant = students[1] as Student
                },
                 new LocationAssignment
                {
                    Date = new DateTime(2013,4,18),
                    Club = clubs[2],
                    Times = new List<Time>{times[0],times[1],times[2]},
                    Locations = new List<Location>{locations[0],locations[1],locations[2]},
                    Applicant = students[1] as Student
                },
                 new LocationAssignment
                {
                    Date = new DateTime(2013,4,10),
                    Club = clubs[1],
                    Times = new List<Time>{times[0],times[1],times[2]},
                    Locations = new List<Location>{locations[1],locations[2]},
                    Applicant = students[1] as Student
                }
            };
            #endregion

            #region ClubApplications
            var club_applications = new List<ClubApplication>
            {
                new ClubApplication
                {
                    Id = 10,
                    Date = DateTime.Now,
                    ApplicantUserName = "000000004",
                    ClubId = 4,
                    Details = new ClubApplicationDetails
                    {
                        Reason = "Because I wanna join."
                    },
                    Status = Application.NOT_VERIFIED,
                    RoleId = 4,
                    RejectReason = null,
                    IsFlexible = true,
                    Inclinations = new List<ClubApplicationInclination>
                    {
                        new ClubApplicationInclination
                        {
                            Branch = branches[7],
                            Order = 1,
                            Status = Application.FAILED
                        },
                        new ClubApplicationInclination
                        {
                            Branch = branches[8],
                            Order = 2,
                            Status = Application.FAILED
                        }
                    }
                },
                new ClubApplication
                {
                    Id = 11,
                    Date = DateTime.Now,
                    ApplicantUserName = "000000002",
                    ClubId = 4,
                    Details = new ClubApplicationDetails
                    {
                        Reason = "Whatever."
                    },
                    Status = Application.NOT_VERIFIED,
                    RoleId = 4,
                    RejectReason = null,
                    IsFlexible = false,
                    Inclinations = new List<ClubApplicationInclination>
                    {
                        new ClubApplicationInclination
                        {
                            Branch = branches[9],
                            Order = 1,
                            Status = Application.NOT_VERIFIED
                        }
                    }
                },
                new ClubApplication
                {
                    Id = 12,
                    Date = DateTime.Now,
                    ApplicantUserName = "000000003",
                    ClubId = 4,
                    Details = new ClubApplicationDetails
                    {
                        Reason = "Need a reason?"
                    },
                    Status = Application.NOT_VERIFIED,
                    RoleId = 3,
                    RejectReason = null,
                    IsFlexible = true,
                }
            };
            #endregion

            #region FundDetails
            var fund_details = new List<FundDetails>() 
            {
                new FundDetails
                {
                    Date = new DateTime(2013,02,02),
                    Applicant = students[2] as Student,
                    Quantity = 100,
                    Club = clubs[0]
                },
                new FundDetails
                {
                    Date = new DateTime(2013,04,11),
                    Applicant = students[2] as Student,
                    Quantity = 200,
                    Club = clubs[0]
                },
                new FundDetails
                {
                    Date = new DateTime(2013,04,12),
                    Applicant = students[2] as Student,
                    Quantity = 300,
                    Club = clubs[0]
                },
                new FundDetails
                {
                    Date = new DateTime(2013,04,13),
                    Applicant = students[2] as Student,
                    Quantity = 400,
                    Club = clubs[0]
                }
            };
            #endregion

            #region Events
            var events = new List<Event>
            {
                new Event
                {
                    Date = DateTime.Now,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(1),
                    ChiefEventOrganizer = students[1] as Student,
                    Club = clubs[3],
                    PlanUrl = "1.txt",
                    PosterUrl = "",
                    Title = "涙の物語",
                    Status = Application.PASSED,
                    Description = new EventDescription
                    {
                        Description = "Sounds like a plan."
                    },
                    Organizers = new List<Student>
                    {
                        students[1] as Student,
                        students[2] as Student
                    },
                    SubEvents = new List<SubEvent>
                    {
                        new SubEvent
                        {
                            Date = DateTime.Now,
                            StartTime = DateTime.Now.TimeOfDay,
                            EndTime = DateTime.Now.TimeOfDay,
                            Description = new SubEventDescription
                            {
                                Description = "SubEvent1 of Event 涙の物語"
                            },
                            Title = "涙の物語1",
                            LocationApplications = new List<LocationApplication>
                            {
                                location_applications[0],
                                location_applications[1]
                            },
                            AssetApplications = new List<AssetApplication>
                            {
                                asset_applications[0],
                                asset_applications[1]
                            },
                            Times = new List<Time>
                            {
                                times[0],
                                times[1]
                            }
                        }
                    },
                    FundApplication = new FundApplication
                    {
                        Id = 13,
                        Applicant = students[1] as Student,
                        Date = DateTime.Now,
                        Club = clubs[3],
                        Quantity = 100,
                        Status = Application.PASSED
                    }
                },
                new Event
                {
                    Date = DateTime.Now.AddDays(5),
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(1),
                    ChiefEventOrganizer = students[1] as Student,
                    Club = clubs[3],
                    PlanUrl = "1.docx",
                    PosterUrl = "",
                    Title = "BioShock",
                    Status = Application.NOT_VERIFIED,
                    Description = new EventDescription
                    {
                        Description = "All beauty is sad opera."
                    },
                    Organizers = new List<Student>
                    {
                        students[1] as Student,
                        students[2] as Student
                    },
                    SubEvents = new List<SubEvent>
                    {
                        new SubEvent
                        {
                            Date = DateTime.Now,
                            StartTime = DateTime.Now.TimeOfDay,
                            EndTime = DateTime.Now.TimeOfDay,
                            Description = new SubEventDescription
                            {
                                Description = "SubEvent1 of Event BioShock"
                            },
                            Title = "BioShock1",
                            LocationApplications = new List<LocationApplication>
                            {
                                location_applications[2]
                            },
                            AssetApplications = new List<AssetApplication>
                            {
                                asset_applications[2]
                            },
                            Times = new List<Time>
                            {
                                times[0],
                                times[1]
                            }
                        }
                    },
                    FundApplication = new FundApplication
                    {
                        Id = 14,
                        Applicant = students[1] as Student,
                        Date = DateTime.Now.AddDays(5),
                        Club = clubs[3],
                        Quantity = 103,
                        Status = Application.NOT_VERIFIED
                    }
                }
            };
            #endregion

            ids.ForEach(s => context.Identities.Add(s));
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
            threads.ForEach(s => context.Threads.Add(s));
            times.ForEach(s => context.Times.Add(s));
            assets.ForEach(s => context.Assets.Add(s));
            club_register_applications.ForEach(s => context.Applications.Add(s));
            club_unregister_applications.ForEach(s => context.Applications.Add(s));
            club_modification_applications.ForEach(s => context.Applications.Add(s));
            club_applications.ForEach(s => context.Applications.Add(s));

            asset_assignments.ForEach(s => context.AssetAssignments.Add(s));
            asset_applications.ForEach(s => context.Applications.Add(s));
            locations.ForEach(s => context.Locations.Add(s));
            location_applications.ForEach(s => context.Applications.Add(s));
            location_assignments.ForEach(s => context.LocationAssignments.Add(s));
            fund_details.ForEach(s => context.FundDetailses.Add(s));

            events.ForEach(s => context.Events.Add(s));

            context.SaveChanges();

            string poster_path = HttpContext.Current.Server.MapPath(ConfigurationManager.ClubSplashPanelFolder);
            string temp_path = HttpContext.Current.Server.MapPath(ConfigurationManager.TemporaryFilesFolder);

            if (System.IO.Directory.Exists(poster_path))
            {
                System.IO.Directory.Delete(poster_path, true);
                System.IO.Directory.CreateDirectory(poster_path);
            }

            if (System.IO.Directory.Exists(temp_path))
            {
                System.IO.Directory.Delete(temp_path, true);
                System.IO.Directory.CreateDirectory(temp_path);
            }
        }
    }
}