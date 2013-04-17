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

            ClubMember membership = ScmRoleProvider.GetRoleInClub(club_id);
            IQueryable<ClubMember> members =
                db.ClubMembers.Include(t => t.Student).Include(t => t.Branch).Include(t => t.ClubRole).ToList();
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
            List<KeyValuePair<string, int>> available_roles = new List<KeyValuePair<string, int>>();
            foreach (var role in db.ClubRoles.ToList())
            {
                available_roles.Add(new KeyValuePair<string, int>(role.Name, role.Id));

                if ((ViewBag.BranchName == "会员部" || ViewBag.BranchName == "") || (role.Name != "会长" && role.Name != "会员"))
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
            ViewBag.ClubName = club.MajorInfo.Name;
            ViewBag.CurrentOrder = order;
            ViewBag.NameOrderOpt = order == "Student.Name" ? "Student.NameDesc" : "Student.Name";
            ViewBag.UserNameOrderOpt = order == "UserName" ? "UserNameDesc" : "UserName";
            ViewBag.JoinDateOrderOpt = order == "JoinDate" ? "JoinDateDesc" : "JoinDate";
            ViewBag.Search = search;
            ViewBag.BranchFilter = branch_filter;
            ViewBag.SearchOption = search_option;
            ViewBag.RoleFilter = role_filter;

            members = FilterMembers(members, club_id, branch_filter, role_filter, search, search_option);

            var member_list = QueryProcessor.Query<ClubMember>(members, filter: null,
                order_by: order, page_number: page_number, items_per_page: 20);

            return View(member_list);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateMembers(int role_id, int[] member_ids, bool all_in, int club_id,
            int branch_filter, int role_filter, string search, string search_option)
        {
            string operation = "update";

            if (member_ids != null && member_ids.Length > 0)
            {
                ClubRole role = db.ClubRoles.Find(role_id);

                if (role != null)
                {
                    string message;
                    ClubBranch member_branch = db.ClubBranches.ToList().SingleOrDefault(t => t.ClubId == club_id && t.BranchName == "会员部");
                    bool branch_changed = (role.Name == "会长" || role.Name == "会员") && member_branch != null;

                    if (all_in)
                    {
                        var members = FilterMembers(db.ClubMembers.Include(t => t.Branch).ToList(), club_id, branch_filter, role_filter, search, search_option);

                        foreach (var member in members)
                        {
                            member.ClubRoleId = role_id;

                            if (branch_changed)
                            {
                                member.Branch.MemberCount--;
                                member_branch.MemberCount++;

                                if (member.IsNewMember)
                                {
                                    member.Branch.NewMemberCount--;
                                    member_branch.NewMemberCount++;
                                }

                                member.BranchId = member_branch.Id;
                            }
                        }

                        message = "已将当前搜索/过滤条件下所有成员的角色改变为" + role.Name;
                    }
                    else
                    {
                        foreach (int member_id in member_ids)
                        {
                            ClubMember member = db.ClubMembers.Find(member_id);

                            if (member != null)
                            {
                                member.ClubRoleId = role_id;
                            }

                            if (branch_changed)
                            {
                                member.Branch.MemberCount--;
                                member_branch.MemberCount++;

                                if (member.IsNewMember)
                                {
                                    member.Branch.NewMemberCount--;
                                    member_branch.NewMemberCount++;
                                }

                                member.BranchId = member_branch.Id;
                            }
                        }

                        message = "已将选中成员的角色改变为" + role.Name;
                    }

                    db.SaveChanges();

                    return Json(new { success = false, msg = message, role = role.Name, operation = operation });
                }

                return Json(new { success = false, msg = "该角色不存在", operation = operation });
            }

            return Json(new { success = false, msg = "没有选择任何成员", operation = operation });
        }

        [Authorize]
        [HttpPost]
        public ActionResult RemoveMembers(int[] member_ids, bool all_in, int club_id,
            int branch_filter, int role_filter, string search, string search_option)
        {
            string operation = "delete";

            if (member_ids != null && member_ids.Length > 0)
            {
                string message;
                Club club = db.Clubs.Find(club_id);

                if (all_in)
                {
                    var members = FilterMembers(db.ClubMembers.Include(t => t.Branch).ToList(), club_id, branch_filter, role_filter, search, search_option);

                    foreach (var member in members)
                    {

                        member.Branch.MemberCount--;
                        club.MemberCount--;

                        if (member.IsNewMember)
                        {
                            member.Branch.NewMemberCount--;
                            club.NewMemberCount--;
                        }

                        db.ClubMembers.Delete(member);
                    }

                    message = "已将当前搜索/过滤条件下所有成员移除出本社团";
                }
                else
                {
                    foreach (int member_id in member_ids)
                    {
                        ClubMember member = db.ClubMembers.Find(member_id);

                        if (member != null)
                        {
                            member.Branch.MemberCount--;
                            club.MemberCount--;

                            if (member.IsNewMember)
                            {
                                member.Branch.NewMemberCount--;
                                club.NewMemberCount--;
                            }

                            db.ClubMembers.Delete(member);
                        }
                    }
                }
                
                message = "已将选中成员移除出本社团";

                db.SaveChanges();

                return Json(new { success = false, msg = message, operation = operation });
            }

            return Json(new { success = false, msg = "没有选择任何成员", operation = operation });
        }

        private IQueryable<ClubMember> FilterMembers(IQueryable<ClubMember> collection, int club_id,
            int branch_filter, int role_filter, string search, string search_option)
        {
            IQueryable<ClubMember> members = collection.Where(t => t.ClubId == club_id);
            if (branch_filter != 0)
            {
                members = members.Where(t => t.BranchId == branch_filter);
            }
            if (role_filter != 0)
            {
                members = members.Where(t => t.ClubRoleId == role_filter);
            }

            if (!String.IsNullOrWhiteSpace(search) && !String.IsNullOrWhiteSpace(search_option))
            {
                switch (search_option)
                {
                    case "Name":
                        members = members.Where(t => t.Student.Name.Contains(search));
                        break;
                    case "UserName":
                        members = members.Where(t => t.UserName.Contains(search));
                        break;
                }
            }

            return members;
        }
    }
}
