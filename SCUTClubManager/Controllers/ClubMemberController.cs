using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using SCUTClubManager.BusinessLogic;
using SCUTClubManager.Models;
using SCUTClubManager.DAL;

namespace SCUTClubManager.Controllers
{
    public class ClubMemberController : Controller
    {
        private UnitOfWork db = new UnitOfWork();

        //
        // GET: /ClubMember/

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        [Authorize]
        public ActionResult List(int club_id, int page_number = 1, string order = "UserName", string search = "",
            string search_option = "Name", int branch_filter = 0, int role_filter = 0)
        {
            if (!User.IsInRole("社联") && !ScmRoleProvider.HasMembershipIn(club_id))
            {
                // TODO: 将用户重定向到另一个页面。
                return View("PermissionDeniedError");
            }

            IQueryable<ClubMember> members =
                db.ClubMembers.Include(t => t.Student).Include(t => t.Branch).Include(t => t.ClubRole).ToList().Where(t => t.ClubId == club_id) as IQueryable<ClubMember>;
            Club club = db.Clubs.Include(s => s.Branches).Include(s => s.MajorInfo).Find(club_id);

            ViewBag.BranchName = "";
            ViewBag.RoleName = "全部成员";

            // 搜索选项下拉框
            List<KeyValuePair<string, string>> search_option_list = new List<KeyValuePair<string, string>>();
            search_option_list.Add(new KeyValuePair<string, string>("姓名", "Name"));
            search_option_list.Add(new KeyValuePair<string, string>("用户名", "UserName"));
            ViewBag.SearchOptions = new SelectList(search_option_list, "Value", "Key", search_option);

            // 部门选项下拉框
            List<KeyValuePair<string, int>> branch_list = new List<KeyValuePair<string, int>>();
            branch_list.Add(new KeyValuePair<string, int>("全部", 0));
            foreach (var branch in club.Branches)
            {
                branch_list.Add(new KeyValuePair<string, int>(branch.BranchName, branch.Id));

                if (branch.Id == branch_filter)
                {
                    ViewBag.BranchName = branch.BranchName;
                }
            }
            ViewBag.BranchFilters = new SelectList(branch_list, "Value", "Key", branch_filter);

            // 角色选项下拉框
            List<KeyValuePair<string, int>> role_list = new List<KeyValuePair<string, int>>();
            role_list.Add(new KeyValuePair<string, int>("全部", 0));
            foreach (var role in db.ClubRoles.ToList())
            {
                role_list.Add(new KeyValuePair<string, int>(role.Name, role.Id));

                if (role.Id == role_filter)
                {
                    ViewBag.RoleName = role.Name;
                }
            }
            ViewBag.RoleFilters = new SelectList(role_list, "Value", "Key", role_filter);

            ViewBag.ClubId = club_id;
            ViewBag.ClubName = club.MajorInfo.Name;
            ViewBag.CurrentOrder = order;
            ViewBag.NameOrderOpt = order == "Student.Name" ? "Student.NameDesc" : "Student.Name";
            ViewBag.UserNameOrderOpt = order == "UserName" ? "UserNameDesc" : "UserName";
            ViewBag.JoinDateOrderOpt = order == "JoinDate" ? "JoinDateDesc" : "JoinDate";
            ViewBag.Search = search;
            ViewBag.BranchFilter = branch_filter;
            ViewBag.SearchOption = search_option;
            ViewBag.RoleFilter = role_filter;

            Expression<Func<ClubMember, bool>> filter = null;
            if (!String.IsNullOrWhiteSpace(search) && !String.IsNullOrWhiteSpace(search_option))
            {
                switch (search_option)
                {
                    case "Name":
                        filter = s => s.Student.Name.Contains(search);
                        break;
                    case "UserName":
                        filter = s => s.UserName.Contains(search);
                        break;
                }
            }

            if (branch_filter != 0)
            {
                members = members.Where(t => t.BranchId == branch_filter);
            }
            if (role_filter != 0)
            {
                members = members.Where(t => t.ClubRoleId == role_filter);
            }

            var member_list = QueryProcessor.Query<ClubMember>(members, filter: filter,
                order_by: order, page_number: page_number, items_per_page: 20);

            return View(member_list);
        }

        //
        // GET: /ClubMember/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ClubMember/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /ClubMember/Create

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
        // GET: /ClubMember/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ClubMember/Edit/5

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
        // GET: /ClubMember/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ClubMember/Delete/5

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
