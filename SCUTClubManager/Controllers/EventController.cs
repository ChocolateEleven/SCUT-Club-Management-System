using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Linq.Expressions;
using System.Threading;
using SCUTClubManager.BusinessLogic;
using SCUTClubManager.DAL;
using SCUTClubManager.Models;
using SCUTClubManager.Helpers;
using PagedList;

namespace SCUTClubManager.Controllers
{
    public class EventController : Controller
    {
        private UnitOfWork db = new UnitOfWork();

        //
        // GET: /Event/

        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole("社联"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("List");
            }
        }

        [Authorize(Roles = "社联")]
        public ActionResult UploadAppTemplate(HttpPostedFileBase application_template)
        {
            if (application_template != null && application_template.ContentLength > 0)
            {
                string name = ConfigurationManager.EventApplicationTemplateFile;
                string extension = "";

                if (Path.HasExtension(application_template.FileName))
                {
                    extension = Path.GetExtension(application_template.FileName);
                }

                string file_name = name + extension;
                string path = Path.Combine(Server.MapPath(ConfigurationManager.TemplateFolder), file_name);

                application_template.SaveAs(path);

                //System.Threading.Thread.Sleep(5000);

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [Authorize]
        public ActionResult List(int page_number = 1, string search = "", string search_option = "Title", string order = "Title",
            string pass_filter = "", int club_id = Application.ALL)
        {
            IEnumerable<Event> events = db.Events.ToList();

            if (club_id != Application.ALL)
            {
                Club club = db.Clubs.Include(t => t.MajorInfo).Find(club_id);
                ViewBag.ClubName = club.MajorInfo.Name;
            }

            ViewBag.IsAdminMode = User.IsInRole("社联") || club_id != Application.ALL && ScmRoleProvider.HasMembershipIn(club_id);

            if (!ViewBag.IsAdminMode)
            {
                if (pass_filter == "NotVerified" || pass_filter == "Verified" || pass_filter == "Failed" || String.IsNullOrWhiteSpace(pass_filter))
                    pass_filter = "Passed";
            }

            events = FilterEvents(events as IQueryable<Event>, page_number, order, search, search_option, pass_filter, club_id);

            return View(events);
        }

        [Authorize(Roles = "社联")]
        public ActionResult Applications(int page_number = 1, string search = "", string search_option = "Title", string order = "Title")
        {
            IEnumerable<Event> events = (IQueryable<Event>)db.Events.ToList();
            events = FilterEvents(events as IQueryable<Event>, page_number, order, search, search_option, "NotVerified");

            return View(events);
        }

        [Authorize(Roles = "社联")]
        public ActionResult Scoring(int page_number = 1, string search = "", string search_option = "Title", string order = "Title")
        {
            IEnumerable<Event> events = db.Events.ToList().Where(t => t.Score == null);
            events = FilterEvents(events as IQueryable<Event>, page_number, order, search, search_option, "Passed");

            return View(events);
        }

        [Authorize(Roles = "社联")]
        [HttpPost]
        public ActionResult Scoring(int[] ids, string[] scores, string search, string search_option, string order)
        {
            if (ids.Length != scores.Length)
                return Json(new { success = false, msg = "活动数和评分数不一致，评分失败" });

            var events = db.Events.ToList().Where(t => ids.Contains(t.Id)).ToList();

            for (int i = 0; i < ids.Length; ++i)
            {
                int id = ids[i];
                string score = scores[i];

                Event e = events.Find(t => t.Id == id);

                if (e != null && !String.IsNullOrWhiteSpace(score))
                {
                    e.Score = Int32.Parse(score);
                }
            }
            db.SaveChanges();

            string return_url = "Scoring?search=" + search + "&search_option=" + search_option + "&order=" + order;

            return Json(new { success = true, msg = "评分成功", url = return_url });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Cancel(int id)
        {
            Event e = db.Events.Include(t => t.SubEvents.Select(s => s.LocationApplications.Select(f => f.Assignment)))
                .Include(t => t.SubEvents.Select(s => s.AssetApplications.Select(f => f.Assignment))).Include(t => t.SubEvents.Select(s => s.FundApplication)).Find(id);
            string msg;
            bool success = false;
            string operation = "";

            if (e != null)
            {
                msg = e.Title;

                // 作为发起人取消活动的时候，若申请未审批则直接删除该申请，若申请已通过但未结束则将活动的状态修改为已取消。
                if (ScmMembershipProvider.IsMe(e.ChiefEventOrganizerId))
                {
                    if (e.Status == Application.NOT_VERIFIED)
                    {
                        // 由于会和Club与Application之间的级联删除产生冲突，因此SubEvent和Application之间的级联删除只能反人类地手动了。。。
                        foreach (SubEvent sub_event in e.SubEvents)
                        {
                            List<LocationApplication> loc_apps = sub_event.LocationApplications.ToList();
                            loc_apps.ForEach(t => db.LocationApplications.Delete(t));

                            List<AssetApplication> ass_apps = sub_event.AssetApplications.ToList();
                            ass_apps.ForEach(t => db.AssetApplications.Delete(t));

                            if (sub_event.FundApplication != null)
                            {
                                db.FundApplications.Delete(sub_event.FundApplication);
                            }
                        }

                        db.Events.Delete(e);
                        msg += "的申请已撤回";
                        success = true;
                        operation = "undo";
                    }
                    else if (e.Status == Application.PASSED && e.SubEvents.Any(t => t.Date.Date >= DateTime.Now.Date))
                    {
                        foreach (SubEvent sub_event in e.SubEvents)
                        {
                            foreach (LocationApplication loc_app in sub_event.LocationApplications)
                            {
                                var ass = loc_app.Assignment;

                                if (ass != null)
                                {
                                    db.LocationAssignments.Delete(ass);
                                }
                            }
                            foreach (AssetApplication ass_app in sub_event.AssetApplications)
                            {
                                var ass = ass_app.Assignment;

                                if (ass != null)
                                {
                                    db.AssetAssignments.Delete(ass);
                                }
                            }
                        }

                        e.Status = Application.CANCELED;
                        msg += "已取消";
                        success = true;
                        operation = "cancel";
                    }
                    else
                    {
                        msg += "无法取消";
                    }
                }
                else if (User.IsInRole("社联")) // 作为管理员终止活动的时候，将申请已通过但未结束的活动的状态修改为已终止
                {
                    if (e.Status == Application.PASSED && e.SubEvents.Any(t => t.Date.Date >= DateTime.Now.Date))
                    {
                        foreach (SubEvent sub_event in e.SubEvents)
                        {
                            foreach (LocationApplication loc_app in sub_event.LocationApplications)
                            {
                                var ass = loc_app.Assignment;

                                if (ass != null)
                                {
                                    db.LocationAssignments.Delete(ass);
                                }
                            }
                            foreach (AssetApplication ass_app in sub_event.AssetApplications)
                            {
                                var ass = ass_app.Assignment;

                                if (ass != null)
                                {
                                    db.AssetAssignments.Delete(ass);
                                }
                            }
                        }

                        e.Status = Application.TERMINATED;
                        msg += "已终止";
                        success = true;
                        operation = "terminate";
                    }
                    else
                    {
                        msg += "无法终止";
                    }
                }
                else
                {
                    msg = "您没有权限取消" + e.Title;
                }

                db.SaveChanges();

            }
            else
            {
                msg = "该活动不存在";
            }

            return Json(new { success = success, msg = msg, operation = operation });
        }

        //
        // GET: /Event/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Event/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Event/Create

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
        // GET: /Event/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Event/Edit/5

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
        // GET: /Event/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Event/Delete/5

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

        private IEnumerable<Event> FilterEvents(IQueryable<Event> collection, int page_number = 1,
            string order = "Title", string search = "", string search_option = "", string pass_filter = "", int club_id = Application.ALL)
        {
            IPagedList<Event> list = null;

            ViewBag.CurrentOrder = order;
            ViewBag.TitleOrderOpt = order == "Title" ? "TitleDesc" : "Title";
            ViewBag.ClubNameOrderOpt = order == "Club.MajorInfo.Name" ? "Club.MajorInfo.NameDesc" : "Club.MajorInfo.Name";
            ViewBag.OrganizerOrderOpt = order == "ChiefEventOrganizer.Name" ? "ChiefEventOrganizer.NameDesc" : "ChiefEventOrganizer.Name";
            ViewBag.ScoreOrderOpt = order == "Score" ? "ScoreDesc" : "Score";
            ViewBag.DateOrderOpt = order == "Date" ? "DateDesc" : "Date";
            ViewBag.StatusOrderOpt = order == "Status" ? "StatusDesc" : "Status";
            ViewBag.PageNumber = page_number;
            ViewBag.Search = search;
            ViewBag.SearchOption = search_option;
            ViewBag.PassFilter = pass_filter;
            ViewBag.ClubId = club_id;

            List<KeyValuePair<string, string>> search_option_list = new List<KeyValuePair<string, string>>();
            search_option_list.Add(new KeyValuePair<string, string>("活动名", "Title"));
            search_option_list.Add(new KeyValuePair<string, string>("举办社团", "ClubName"));
            search_option_list.Add(new KeyValuePair<string, string>("负责人", "Organizer"));

            ViewBag.SearchOptions = new SelectList(search_option_list, "Value", "Key", search_option);

            if (collection != null)
            {
                if (!String.IsNullOrWhiteSpace(search) && !String.IsNullOrWhiteSpace(search_option))
                {
                    switch (search_option)
                    {
                        case "Title":
                            collection = collection.Where(t => t.Title.Contains(search));
                            break;
                        case "ClubName":
                            collection = collection.Where(t => t.Club.MajorInfo.Name.Contains(search));
                            break;
                        case "Organizer":
                            collection = collection.Where(t => t.ChiefEventOrganizer.Name.Contains(search));
                            break;
                    }
                }

                if (club_id != Application.ALL)
                {
                    collection = collection.Where(t => t.ClubId == club_id);
                }

                if (!String.IsNullOrWhiteSpace(pass_filter))
                {
                    switch (pass_filter)
                    {
                        case "Passed":
                            collection = collection.Where(s => s.Status == Application.PASSED);
                            break;

                        case "Failed":
                            collection = collection.Where(s => s.Status == Application.FAILED);
                            break;

                        case "NotVerified":
                            collection = collection.Where(s => s.Status == Application.NOT_VERIFIED);
                            break;

                        case "Verified":
                            collection = collection.Where(s => s.Status == Application.PASSED || s.Status == Application.FAILED);
                            break;

                        case "NotStarted":
                            collection = collection.Where(s => s.Status == Application.PASSED && s.Date.Date > DateTime.Now.Date);
                            break;

                        case "Active":
                            collection = collection.Where(s => s.Status == Application.PASSED && s.SubEvents.Any(t => t.Date.Date == DateTime.Now.Date));
                            break;

                        case "Finished":
                            collection = collection.Where(s => s.Status == Application.PASSED && s.SubEvents.All(t => t.Date.Date < DateTime.Now.Date));
                            break;

                        case "Canceled":
                            collection = collection.Where(s => s.Status == Application.CANCELED);
                            break;
                            
                        case "Terminated":
                            collection = collection.Where(s => s.Status == Application.TERMINATED);
                            break;
                    }
                }

                string[] includes = { "Club.MajorInfo", "ChiefEventOrganizer", "SubEvents" };
                list = QueryProcessor.Query(collection, null, order, includes, page_number, 20);
            }

            return list;
        }

    }
}
