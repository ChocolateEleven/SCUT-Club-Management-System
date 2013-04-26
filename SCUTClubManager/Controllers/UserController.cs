using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using SCUTClubManager.Models;
using SCUTClubManager.BusinessLogic;
using SCUTClubManager.DAL;
using SCUTClubManager.Helpers;

namespace SCUTClubManager.Controllers
{
    public class UserController : Controller
    {
        private UnitOfWork db = new UnitOfWork();

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult List(int role_filter = 1, int page_number = 1,
            string order = "UserName", string search = "", string search_option = "Name")
        {
            UserRole student_role = db.UserRoles.ToList().Single(t => t.Name == "学生");
            UserRole admin_role = db.UserRoles.ToList().Single(t => t.Name == "社联");

            ViewBag.CurrentOrder = order;
            ViewBag.NameOrderOpt = order == "Name" ? "NameDesc" : "Name";
            ViewBag.UserNameOrderOpt = order == "UserName" ? "UserNameDesc" : "UserName";
            ViewBag.DepartmentOrderOpt = order == "Department" ? "DepartmentDesc" : "Department";
            ViewBag.GradeOrderOpt = order == "Grade" ? "GradeDesc" : "Grade";
            ViewBag.Search = search;
            ViewBag.SearchOption = search_option;
            ViewBag.RoleFilter = role_filter;
            ViewBag.ProgressProfileInterval = ConfigurationManager.ProgressProfileInterval;

            IEnumerable<User> users = FilterUsers(role_filter, search, search_option);
            users = QueryProcessor.Query(users, null, order, null, page_number, 20);

            if (users != null)
            {
                return View(users);
            }
            else
            {
                return View("InvalidFilterError");
            }
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /User/Add

        public ActionResult Add()
        {
            return View();
        } 

        //
        // POST: /User/Add

        [Authorize(Roles = "社联")]
        [HttpPost]
        public ActionResult Add(User user, bool is_admin)
        {
            string operation = "add";

            if (ModelState.IsValid)
            {
                int role_id;

                if (is_admin)
                {
                    role_id = ScmRoleProvider.GetRoleIdByName("社联");
                    user = new User
                    {
                        UserName = user.UserName,
                        Password = user.Password,
                        RoleId = role_id,
                        Name = "社联"
                    };
                }
                else
                {
                    role_id = ScmRoleProvider.GetRoleIdByName("学生");
                    user = new Student
                    {
                        UserName = user.UserName,
                        Password = user.Password,
                        RoleId = role_id,
                        Name = user.Name
                    };
                }

                ScmMembershipProvider.AddUser(user);

                return Json(new { success = true, msg = "添加成功", operation = operation });
            }

            return Json(new { success = false, msg = "参数错误", operation = operation });
        }

        [Authorize(Roles = "社联")]
        [HttpPost]
        public ActionResult AddRange(HttpPostedFileBase file, int role_filter, string search, string search_option, string order)
        {
            Session["HasStarted"] = false;

            if (file != null && file.ContentLength > 0)
            {
                // 开始记录处理进度
                Session["HasStarted"] = true;
                Session["Progress"] = 0.0f;

                string guid = Guid.NewGuid().ToString();
                string extension = "";

                if (Path.HasExtension(file.FileName))
                {
                    extension = Path.GetExtension(file.FileName);
                }

                string file_name = guid + extension;
                string path = Path.Combine(Server.MapPath(ConfigurationManager.TemporaryFilesFolder), file_name);

                file.SaveAs(path);

                using (ExcelProcessor excel = new ExcelProcessor(ConfigurationManager.MaxRangeForRangeAdding, path))
                {
                    excel.Init();

                    while (!excel.IsEnd())
                    {
                        IEnumerable<User> users = excel.FetchMoreUsers();

                        foreach (User user in users)
                        {
                            ScmMembershipProvider.AddUser(user, false);
                        }

                        ScmMembershipProvider.SaveChanges();

                        Session["Progress"] = (float)excel.RowsRead / excel.TotalRows;
                    }
                }

                string return_url = "List?role_filter=" + role_filter + "&search=" + search +
                "&search_option=" + search_option + "&order=" + order;

                System.IO.File.Delete(path);

                return Json(new { success = true, msg = "添加成功", url = return_url });
            }

            return Json(new { success = false, msg = "上传失败" });
        }

        public ActionResult CurrentProgress()
        {
            bool has_started = Session["HasStarted"] == null ? false : (bool)Session["HasStarted"];
            float progress = Session["Progress"] == null ? 0.0f : (float)Session["Progress"];
            
            return Json(new { has_started = has_started, progress = progress }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /User/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /User/Edit/5

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
        // GET: /User/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /User/Delete/5

        [Authorize(Roles = "社联")]
        [HttpPost]
        public ActionResult Delete(string[] user_names, bool all_in,
            int role_filter, string search, string search_option, string order)
        {
            string operation = "delete";
            UserRole student_role = db.UserRoles.ToList().Single(t => t.Name == "学生");

            if (user_names != null && user_names.Length > 0)
            {
                string message;
                IQueryable<User> users = null;

                if (all_in)
                {
                    users = FilterUsers(role_filter, search, search_option);
                    message = "已将当前搜索/过滤条件下的所有用户删除";
                }
                else
                {
                    users = db.Users.Include(t => t.Replies).Include(t => t.Threads).ToList().Where(t => user_names.Any(s => s == t.UserName));
                    message = "已将选中的用户删除";
                }

                IEnumerable<ClubMember> memberships = null;

                if (role_filter == student_role.Id)
                {
                    memberships = db.ClubMembers.Include(t => t.Branch).Include(t => t.Club).ToList().Where(t => users.Any(s => s.UserName == t.UserName)).ToList();
                }

                foreach (var user in users)
                {
                    if (!ScmMembershipProvider.IsMe(user.UserName))
                    {
                        if (memberships != null)
                        {
                            var memberships_for_this_user = memberships.Where(t => t.UserName == user.UserName);

                            if (memberships_for_this_user != null)
                            {
                                foreach (var membership_for_this_user in memberships_for_this_user)
                                {
                                    membership_for_this_user.Club.MemberCount--;
                                    membership_for_this_user.Branch.MemberCount--;

                                    if (membership_for_this_user.IsNewMember)
                                    {
                                        membership_for_this_user.Club.NewMemberCount--;
                                        membership_for_this_user.Branch.NewMemberCount--;
                                    }
                                }
                            }
                        }

                        user.Threads.Clear();
                        user.Replies.Clear();
                        db.Users.Delete(user);
                    }
                }

                db.SaveChanges();

                string return_url = "List?role_filter=" + role_filter + "&search=" + search +
                "&search_option=" + search_option + "&order=" + order;

                if (!all_in)
                    return Json(new { success = true, msg = message, operation = operation });
                else
                    return Json(new { success = true, msg = message, operation = operation, url = return_url });
            }

            return Json(new { success = false, msg = "没有选择任何用户", operation = operation });
        }

        private IQueryable<User> FilterUsers(int role_filter, string search, string search_option)
        {
            UserRole student_role = db.UserRoles.ToList().Single(t => t.Name == "学生");
            UserRole admin_role = db.UserRoles.ToList().Single(t => t.Name == "社联");
            IQueryable<User> users = db.Users.ToList();

            if (role_filter == student_role.Id)
            {
                ViewBag.RoleName = "学生";

                users = users.Where(t => t is Student);

                if (!String.IsNullOrWhiteSpace(search) && !String.IsNullOrWhiteSpace(search_option))
                {
                    switch (search_option)
                    {
                        case "Name":
                            users = users.Where(s => s.Name.Contains(search));
                            break;
                        case "UserName":
                            users = users.Where(s => s.UserName.Contains(search));
                            break;
                        case "Department":
                            users = users.Where(s => (s as Student).Department.Contains(search));
                            break;
                    }
                }
            }
            else if (role_filter == admin_role.Id)
            {
                ViewBag.RoleName = "管理员";

                users = users.Where(t => !(t is Student));

                if (!String.IsNullOrWhiteSpace(search) && !String.IsNullOrWhiteSpace(search_option))
                {
                    switch (search_option)
                    {
                        case "Name":
                            users = users.Where(s => s.Name.Contains(search));
                            break;
                        case "UserName":
                            users = users.Where(s => s.UserName.Contains(search));
                            break;
                    }
                }
            }
            else
            {
                return null;
            }

            return users;
        }
    }
}
