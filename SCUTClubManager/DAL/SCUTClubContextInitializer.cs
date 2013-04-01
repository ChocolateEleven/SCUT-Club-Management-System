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
                }


            };

            context.RoleBases.Add(role);

            students.ForEach(s => context.Users.Add(s));
            context.SaveChanges();
        }
    }
}