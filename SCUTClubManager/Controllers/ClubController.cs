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
using System.IO;

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
                return RedirectToAction("List", new { page_number = 1, order = "", search = "", search_option = "" });
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
        public ActionResult List(int page_number, string order = "", string search = "")
        {
            var clubs = db.Clubs;
            string[] includes = {"MajorInfo"};

            if (String.IsNullOrEmpty(order))
            {
                order = "MajorInfo.Name";
            }

            ViewBag.Search = search;
            ViewBag.CurrentOrder = order;
            ViewBag.NameOrderOpt = order == "MajorInfo.Name" ? "MajorInfo.NameDesc" : "MajorInfo.Name";
            ViewBag.LevelOrderOpt = order == "Level" ? "LevelDesc" : "Level";
            ViewBag.FoundDateOrderOpt = order == "FoundDate" ? "FoundDateDesc" : "FoundDate";
            ViewBag.MemberCountOrderOpt = order == "MemberCount" ? "MemberCountDesc" : "MemberCount";

            Expression<Func<Club, bool>> filter = null;
            if (!String.IsNullOrWhiteSpace(search))
            {
                filter = s => s.MajorInfo.Name.Contains(search);
            }

            var club_list = QueryProcessor.Query<Club>(clubs.ToList(), filter, order, includes, page_number, 4);

            return View(club_list);
        }

        //
        // GET: /Club/Details/5
        [Authorize]
        public ViewResult Details(int id, int page_number = 1, string order = "", string search = "")
        {
            Club club = db.Clubs.Find(id);

            ViewBag.PageNumber = page_number;
            ViewBag.Order = order;
            ViewBag.Search = search;

            return View(club);
        }

        [Authorize]
        public ActionResult Introduction(int id, int page_number = 1, string order = "", string search = "")
        {
            Club club = db.Clubs.Include(t => t.MajorInfo).Include(t => t.SubInfo).Find(id);
            ViewBag.IsMember = ScmRoleProvider.HasMembershipIn(id);
            ViewBag.PageNumber = page_number;
            ViewBag.Order = order;
            ViewBag.Search = search;

            return View(club);
        }

        [Authorize]
        public ActionResult Manage(int id, int page_number = 1, string order = "", string search = "")
        {
            ClubMember membership = ScmRoleProvider.GetRoleInClub(id);

            ViewBag.PageNumber = page_number;
            ViewBag.Order = order;
            ViewBag.Search = search;

            if (membership == null)
            {
                return RedirectToAction("Introduction");
            }
            else
            {
                return View(membership);
            }
        }

        //
        // POST: /Club/Delete/5

        [HttpPost]
        [Authorize(Roles = "社联")]
        public ActionResult Delete(int id)
        {            
            Club club = db.Clubs.Find(id);
            db.Clubs.Delete(club);
            db.SaveChanges();

            return Json(new { idToDelete = id, success = true });
        }

        [HttpPost]
        [Authorize]
        public ActionResult UploadPoster(HttpPostedFileBase poster, int club_id)
        {
            if (poster != null && poster.ContentLength > 0)
            {
                Club club = db.Clubs.Include(t => t.SubInfo).Find(club_id);
                string file_name;
                string path;

                if (String.IsNullOrWhiteSpace(club.SubInfo.PosterUrl))
                {
                    string guid = Guid.NewGuid().ToString();
                    string extension = "";

                    if (Path.HasExtension(poster.FileName))
                    {
                        extension = Path.GetExtension(poster.FileName);
                    }

                    file_name = guid + extension;
                }
                else
                {
                    file_name = club.SubInfo.PosterUrl;
                }

                path = Path.Combine(Server.MapPath(ConfigurationManager.ClubSplashPanelFolder), file_name);
                poster.SaveAs(path);

                return Json(new { success = false, msg = "上传成功" });
            }
            else
            {
                return Json(new { success = false, msg = "上传失败" });
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}