using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCUTClubManager.DAL;
using SCUTClubManager.Models;
using System.Web.Security;

namespace SCUTClubManager.BusinessLogic
{
    public class ScmMembershipProvider : MembershipProvider
    {
        private UnitOfWork context = new UnitOfWork();

        public override bool ValidateUser(string user_name, string password)
        {
            if (string.IsNullOrEmpty(password.Trim()) || string.IsNullOrEmpty(user_name.Trim()))
                return false;

            string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(password.Trim(), "md5");

            return context.Users.ToList().Any(user => (user.UserName == user_name.Trim()) && (user.Password == hash));
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string user_name, string old_password, string new_password)
        {
            if (!ValidateUser(user_name, old_password) || string.IsNullOrEmpty(new_password.Trim()))
                return false;

            User user = context.Users.Find(user_name);
            string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(new_password.Trim(), "md5");

            user.Password = hash;

            context.SaveChanges();

            return true;
        }

        public void CreateUser(string user_name, string password, string role_id)
        {
            var users = context.Users;
            UserRole role = context.UserRoles.Find(role_id);
            User user;

            if (role != null && !users.ToList().Any(t => t.UserName == user_name))
            {
                if (role.Name == "Student")
                {
                    user = new Student
                    {
                        UserName = user_name,
                        Password = FormsAuthentication.HashPasswordForStoringInConfigFile(password.Trim(), "md5"),
                        Role = role
                    };
                }
                else
                {
                user = new User
                    {
                        UserName = user_name,
                        Password = FormsAuthentication.HashPasswordForStoringInConfigFile(password.Trim(), "md5"),
                        Role = role
                    };
                }

                context.SaveChanges();
            }
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }
    }
}