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

            #region Ids
            var ids = new List<IdentityForTPC>
            {
                new IdentityForTPC
                {
                    BaseName = "Application",
                    Identity = 7
                },
                new IdentityForTPC
                {
                    BaseName = "AssetBase",
                    Identity = 20
                },
                new IdentityForTPC
                {
                    BaseName = "RoleBase",
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
                    Name = "张二",
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
                    Name = "张三",
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
                    Name = "张四",
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
                    Applicants = new List<ClubRegisterApplicant>(),
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
            BranchModification branch_modification = new BranchDeletion
            {
                BranchName = branches[1].BranchName,
                OrigBranch = branches[1],
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
                    TimeName = "上午1、2节课"
                },
                new Time
                {
                    TimeName = "上午3、4节课"
                },
                new Time
                {
                    TimeName = "中午"
                },
                new Time
                {
                    TimeName = "下午5、6节课"
                },
                new Time
                {
                    TimeName = "下午7、8、9节课"
                },
                new Time
                {
                    TimeName = "傍晚"
                },
                new Time
                {
                    TimeName = "晚上10、11、12节课"
                },
                new Time
                {
                    TimeName = "上午1、2、3、4节课"
                },
                new Time
                {
                    TimeName = "下午5、6、7、8、9节课"
                },
                new Time
                {
                    TimeName = "白天（上午、中午、下午）"
                },
                new Time
                {
                    TimeName = "全天"
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
            List<AssetAssignment> assetAssignments = new List<AssetAssignment> 
            {
                new AssetAssignment
                {
                    Date = new DateTime(2013,4,11),
                    Club = clubs[0],
                    Time = times[0],
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
                    Date = new DateTime(2013,3,19),
                    Club = clubs[1],
                    Time = times[5],
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
                    Date = new DateTime(2014,5,17),
                    Club = clubs[0],
                    Time = times[9],
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
            List<AssetApplication> assetApplications = new List<AssetApplication> 
            {
                new AssetApplication
                {
                    Id = 4,
                    Time = times[0],
                    Status = "n",
                    Applicant = students[1] as Student,
                    Date = new DateTime(2013,04,12),
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
                    Time = times[1],
                    Club = clubs[1],
                    Status = "f",
                    Applicant = students[3] as Student,
                    Date = new DateTime(2013,04,15),
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
                    Time = times[5],
                    Club = clubs[1],
                    Status = "p",
                    Applicant = students[0] as Student,
                    Date = new DateTime(2013,04,10),
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
            //club_register_applications.ForEach(s => context.Applications.Add(s));
            club_unregister_applications.ForEach(s => context.Applications.Add(s));
            //club_modification_applications.ForEach(s => context.Applications.Add(s));

            int counter = Application.counter;

            assetAssignments.ForEach(s => context.AssetAssignments.Add(s));
            assetApplications.ForEach(s => context.Applications.Add(s));

            context.SaveChanges();
        }
    }
}