using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Linq.Expressions;
using SCUTClubManager.Models;
using SCUTClubManager.DAL;
using SCUTClubManager.BusinessLogic;
using SCUTClubManager.Helpers;

namespace SCUTClubManager.Controllers
{
    public class ClubMemberApplicationController : Controller
    {
        UnitOfWork db = new UnitOfWork();

        //
        // GET: /ClubMemberApplication/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(int club_id, int branch_filter = 0, int role_filter = 0, int page_number = 1,
            string order = "Date", string pass_filter = "", string search = "", string search_option = "ClubName")
        {
            if (!User.IsInRole("社联") && !ScmRoleProvider.HasMembershipIn(club_id))
            {
                // TODO: 将用户重定向到另一个页面。
                return RedirectToAction("Index", "Home");
            }

            ClubMember membership = ScmRoleProvider.GetRoleInClub(club_id);
            IQueryable<ClubApplication> applications =
                db.ClubApplications.Include(t => t.Applicant).Include(t => t.Inclinations).ToList().Where(t => t.ClubId == club_id);
            Club club = db.Clubs.Include(t => t.Branches).Include(t => t.MajorInfo).Find(club_id);

            ViewBag.BranchName = "";
            ViewBag.ClubName = club.MajorInfo.Name;
            ViewBag.RoleName = "";

            // 搜索选项下拉框
            List<KeyValuePair<string, string>> search_option_list = new List<KeyValuePair<string, string>>();
            search_option_list.Add(new KeyValuePair<string, string>("姓名", "Name"));
            search_option_list.Add(new KeyValuePair<string, string>("用户名", "UserName"));
            ViewBag.SearchOptions = new SelectList(search_option_list, "Value", "Key", search_option);

            // 部门选项下拉框
            List<KeyValuePair<string, int>> branch_list = new List<KeyValuePair<string, int>>();
            foreach (var branch in club.Branches)
            {
                branch_list.Add(new KeyValuePair<string, int>(branch.BranchName, branch.Id));

                if (branch.Id == branch_filter)
                {
                    ViewBag.BranchName = branch.BranchName;
                }
            }
            if (membership != null)
            {
                branch_list.Add(new KeyValuePair<string, int>("本部门", membership.BranchId));
            }
            branch_list.Add(new KeyValuePair<string, int>("全部", 0));
            ViewBag.BranchFilters = new SelectList(branch_list, "Value", "Key", branch_filter);

            // 角色选项下拉框
            List<KeyValuePair<string, int>> role_list = new List<KeyValuePair<string, int>>();
            int member_role = ScmRoleProvider.GetRoleIdByName("会员");
            int executor_role = ScmRoleProvider.GetRoleIdByName("干事");
            role_list.Add(new KeyValuePair<string, int>("会员", member_role));
            role_list.Add(new KeyValuePair<string, int>("干事", executor_role));
            foreach (var role in db.ClubRoles.ToList())
            {
                if (role.Name == "干事" || role.Name == "会员")
                {
                    role_list.Add(new KeyValuePair<string, int>(role.Name, role.Id));

                    if (role.Id == role_filter)
                    {
                        ViewBag.RoleName = role.Name;
                    }
                }
            }
            ViewBag.RoleList = new SelectList(available_roles, "Value", "Key");
            role_list.Add(new KeyValuePair<string, int>("全部", 0));
            ViewBag.RoleFilters = new SelectList(role_list, "Value", "Key", role_filter);

            ViewBag.ClubId = club_id;
            ViewBag.CurrentOrder = order;
            ViewBag.DateOrderOpt = order == "Date" ? "DateDesc" : "Date";
            ViewBag.ApplicantNameOrderOpt = order == "Applicant.Name" ? "Applicant.NameDesc" : "Applicant.Name";
            ViewBag.PassOrderOpt = order == "Status" ? "StatusDesc" : "Status";
            ViewBag.Search = search;
            ViewBag.PassFilter = pass_filter;
            ViewBag.SearchOption = search_option;
            ViewBag.TypeFilter = type_filter;

            Expression<Func<Application, bool>> filter = null;
            if (!String.IsNullOrWhiteSpace(search) && !String.IsNullOrWhiteSpace(search_option))
            {
                switch (search_option)
                {
                    case "ClubName":
                        filter = s => s.Club.MajorInfo.Name.Contains(search);
                        break;
                    case "ApplicantName":
                        filter = s => s.Applicant.Name.Contains(search);
                        break;
                }
            }

            applications = QueryProcessor.FilterApplication(applications, pass_filter, type_filter, club_id);

            var club_list = QueryProcessor.Query<Application>(applications, filter: filter,
                order_by: order, page_number: page_number, items_per_page: 20);

            return View(club_list);
        }

        //
        // GET: /ClubMemberApplication/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ClubMemberApplication/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /ClubMemberApplication/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /ClubMemberApplication/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ClubMemberApplication/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ClubMemberApplication/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ClubMemberApplication/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
