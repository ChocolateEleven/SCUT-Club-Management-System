using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCUTClubManager.Models;
using SCUTClubManager.DAL;
using SCUTClubManager.BusinessLogic;

namespace SCUTClubManager.BusinessLogic
{
    public class RoleHelper
    {
        public static UnitOfWork unitOfWork = new UnitOfWork();


        //参数 用户名、用户职位
        //返回 用户所对应职位的所在社团
        public static List<Club> GetRoleClub(string user_name, string role)
        {
            List<Club> club_list = new List<Club>();
             Student student = unitOfWork.Students.Include(t => t.MemberShips.Select(s => s.ClubRole)).Find(user_name);

             if (student != null)
             {
                 var memberships = student.MemberShips;
                var president_memberships = memberships.Where(t => t.ClubRole.Name == role).ToList();

                if (president_memberships != null && president_memberships.Count > 0)
                {
                    foreach (var president_membership in president_memberships)
                    {
                        club_list.Add(president_membership.Club);
                    }
                }
             }

             return club_list;
        }
    };
}