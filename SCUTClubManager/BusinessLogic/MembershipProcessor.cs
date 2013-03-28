using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCUTClubManager.DAL;
using SCUTClubManager.Models;

namespace SCUTClubManager.BusinessLogic
{
    public class MembershipProcessor
    {
        private UnitOfWork context;

        public MembershipProcessor(UnitOfWork unitOfWork = null)
        {
            if (unitOfWork == null)
            {
                context = new UnitOfWork();
            }
            else
            {
                context = unitOfWork;
            }
        }

        public User ValidateUser(string user_name, string password)
        {
            var user = context.Users.Find(user_name);

            if (user != null && user.Password == password)
            {
                return user;
            }

            return null;
        }
    }
}