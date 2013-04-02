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

            UserRole role = new UserRole
            {
                Id = 1,
                Name = "Student"
            };

            bool a = false; a = Membership.Provider is BusinessLogic.ScmMembershipProvider;

            var students = new List<User>
            {
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
                    Password = "123456",
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
                    Password = "123456",
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

            polls.ForEach(s => context.Polls.Add(s));

            messages.ForEach(s => context.Messages.Add(s));
            context.RoleBases.Add(role);

            students.ForEach(s => context.Users.Add(s));

            context.SaveChanges();
        }
    }
}