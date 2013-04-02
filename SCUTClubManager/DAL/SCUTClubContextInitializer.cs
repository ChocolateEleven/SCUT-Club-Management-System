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
                    SenderId = "000000004",
                    ReceiverId = "000000002",
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

            messages.ForEach(s => context.Messages.Add(s));
            context.RoleBases.Add(role);
            students.ForEach(s => context.Users.Add(s));

            context.SaveChanges();
        }
    }
}