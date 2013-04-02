using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SCUTClubManager.Models;
using SCUTClubManager.DAL;
using SCUTClubManager.BusinessLogic;
using SCUTClubManager.Helpers;
using System.Linq.Expressions;

namespace SCUTClubManager.Controllers
{ 
    public class ClubController : Controller
    {
        private UnitOfWork db = new UnitOfWork();

        //
        // GET: /Club/
        [Authorize]
        public ActionResult Index()
        {
            bool is_recruit_enabled = ConfigurationManager.IsRecruitEnabled;

            ViewBag.EnableRecruitText = is_recruit_enabled ? "关闭社团招新" : "开放社团招新";
            ViewBag.EnableRecruitValue = !is_recruit_enabled;

            if (!Roles.Provider.IsUserInRole(User.Identity.Name, "社联"))
            {
                RedirectToAction("List", new { page_number = 1, order = "", search = "", search_option = "" });
            }

            return View();
        }

        [Authorize(Roles = "社联")]
        [HttpPost]
        public ActionResult EnableRecruit(bool is_enabling)
        {
            ConfigurationManager.IsRecruitEnabled = is_enabling;
            bool result = !ConfigurationManager.IsRecruitEnabled;

            return Json(new { text = ConfigurationManager.IsRecruitEnabled ? "关闭社团招新" : "开放社团招新", value = result });
        }

        [Authorize]
        public ActionResult List(int page_number, string order, string search)
        {
            var clubs = db.Clubs;
            string[] includes = {"MajorInfo"};
            ViewBag.Search = search;

            Expression<Func<Club, bool>> filter = null;
            if (!String.IsNullOrWhiteSpace(search))
            {
                filter = s => s.MajorInfo.Name.Contains(search);
            }

            var club_list = QueryProcessor.Query<Club>(clubs.ToList(), filter,
                s =>
                {
                    switch (order)
                    {
                        // 名字降序
                        case "NameDesc":
                            ViewBag.NameOrderOpt = "";
                            return s.OrderByDescending(a => a.MajorInfo.Name);

                        // 社团等级升序
                        case "Level":
                            ViewBag.LevelOrderOpt = "LevelDesc";
                            return s.OrderBy(a => a.Level);

                        // 社团等级降序
                        case "LevelDesc":
                            ViewBag.LevelOrderOpt = "Level";
                            return s.OrderByDescending(a => a.Level);

                        // 成立日期升序
                        case "FoundDate":
                            ViewBag.FoundDateOpt = "FoundDateDesc";
                            return s.OrderBy(a => a.FoundDate);

                        // 成立日期降序
                        case "FoundDateDesc":
                            ViewBag.FoundDateOpt = "FoundDate";
                            return s.OrderByDescending(a => a.FoundDate);

                        // 总人数升序
                        case "MemberCount":
                            ViewBag.MemberCountOpt = "MemberCountDesc";
                            return s.OrderBy(a => a.MemberCount);

                        // 总人数降序
                        case "MemberCountDesc":
                            ViewBag.MemberCountOpt = "MemberCount";
                            return s.OrderByDescending(a => a.MemberCount);

                        // 名字升序（默认）
                        default:
                            ViewBag.NameOrderOpt = "NameDesc";
                            return s.OrderBy(a => a.MajorInfo.Name);
                    }
                }, includes, page_number, 20);

            return View(club_list);
        }

        //
        // GET: /Club/Details/5
        [Authorize]
        public ViewResult Details(int id)
        {
            Club club = db.Clubs.Find(id);
            return View(club);
        }

        //
        // GET: /Club/Delete/5

        [Authorize(Roles = "社联")]
        public ActionResult Delete(int id)
        {
            Club club = db.Clubs.Find(id);

            return View(club);
        }

        [Authorize]
        public ActionResult Introduction(int id)
        {
            Club club = db.Clubs.Find(id);
            ViewBag["IsMember"] = ScmRoleProvider.HasMembershipIn(id);

            return View(club);
        }

        [Authorize]
        public ActionResult Manage(int id)
        {
            ClubMember membership = ScmRoleProvider.GetRoleInClub(id);

            if (membership == null)
            {
                // TODO: 转向到错误提示页面。
                return RedirectToAction("Index");
            }
            else
            {
                return View(membership);
            }
        }

        //
        // POST: /Club/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "社联")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Club club = db.Clubs.Find(id);
            db.Clubs.Delete(club);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}