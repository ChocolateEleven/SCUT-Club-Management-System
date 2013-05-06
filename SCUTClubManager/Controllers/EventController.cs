using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Linq.Expressions;
using System.Threading;
using SCUTClubManager.BusinessLogic;
using SCUTClubManager.DAL;
using SCUTClubManager.Models;
using SCUTClubManager.Helpers;
using SCUTClubManager.Models.View_Models;
using PagedList;
using Newtonsoft.Json;

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
        [HttpPost]
        public ActionResult Verify(int id, bool is_passed, string reject_reason, int page_number = 1, string search = "", string search_option = "Title", string order = "Title",
            string pass_filter = "")
        {
            Event e = db.Events.Include(t => t.FundApplication).Include(t => t.SubEvents.Select(s => s.LocationApplications)).
                Include(t => t.SubEvents.Select(s => s.AssetApplications)).ToList().SingleOrDefault(t => t.Id == id && t.Status == Application.NOT_VERIFIED);

            if (e != null)
            {
                LocationHelpers loc_hlp = new LocationHelpers(db);
                AssetHelpers ass_hlp = new AssetHelpers(db);

                foreach (SubEvent sub_event in e.SubEvents)
                {
                    foreach (LocationApplication loc_app in sub_event.LocationApplications)
                    {
                        loc_hlp.VerifyLocationApplication(loc_app, is_passed, false);
                    }

                    foreach (AssetApplication ass_app in sub_event.AssetApplications)
                    {
                        ass_hlp.VerifyAssetApplication(ass_app, is_passed, false);
                    }

                    e.Status = is_passed ? Application.PASSED : Application.FAILED;

                    if (e.FundApplication != null)
                    {
                        e.FundApplication.Status = is_passed ? Application.PASSED : Application.FAILED;
                    }

                    if (!is_passed)
                    {
                        e.RejectReason = new EventRejectReason
                        {
                            Reason = reject_reason
                        };
                    }
                }

                db.SaveChanges();

                return RedirectToAction("Applications", new
                {
                    page_number = page_number,
                    order = order,
                    pass_filter = pass_filter,
                    search = search,
                    search_option = search_option
                });
            }

            return View("InvalidOperationError");
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
            ViewBag.IsPresident = club_id != Application.ALL && ScmRoleProvider.HasMembershipIn(club_id, null, new string[] { "会长" });

            if (!ViewBag.IsAdminMode)
            {
                if (pass_filter == "NotVerified" || pass_filter == "Verified" || pass_filter == "Failed"
                    || String.IsNullOrWhiteSpace(pass_filter) || pass_filter == "NotSubmitted" || pass_filter == "All")
                    pass_filter = "Passed";
            }

            if ((pass_filter == "NotSubmitted" || pass_filter == "All") && !ViewBag.IsPresident)
            {
                pass_filter = "";
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
                .Include(t => t.SubEvents.Select(s => s.AssetApplications.Select(f => f.Assignment))).Include(t => t.FundApplication).Find(id);
            string msg;
            bool success = false;
            string operation = "";

            if (e != null)
            {
                msg = e.Title;

                // 作为负责人取消活动的时候，若申请未审批则直接删除该申请，若申请已通过但未结束则将活动的状态修改为已取消。
                if (ScmMembershipProvider.IsMe(e.ChiefEventOrganizerId))
                {
                    if (e.Status == Application.NOT_VERIFIED || e.Status == Application.NOT_SUBMITTED)
                    {
                        // 由于会和Club与Application之间的级联删除产生冲突，因此SubEvent和Application之间的级联删除只能反人类地手动了。。。
                        foreach (SubEvent sub_event in e.SubEvents)
                        {
                            List<LocationApplication> loc_apps = sub_event.LocationApplications.ToList();
                            loc_apps.ForEach(t => db.LocationApplications.Delete(t));

                            List<AssetApplication> ass_apps = sub_event.AssetApplications.ToList();
                            ass_apps.ForEach(t => db.AssetApplications.Delete(t));
                        }

                        if (e.FundApplication != null)
                        {
                            db.FundApplications.Delete(e.FundApplication);
                        }

                        db.Events.Delete(e);
                        msg += "的申请已撤回";
                        success = true;
                        operation = "undo";
                    }
                    else if (e.Status == Application.PASSED && e.EndDate >= DateTime.Now)
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
                    if (e.Status == Application.PASSED && e.EndDate >= DateTime.Now)
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

        [Authorize]
        public ActionResult Details(int id, int page_number = 1, string search = "", string search_option = "Title", string order = "Title",
            string pass_filter = "")
        {
            Event e = db.Events.Include(t => t.ChiefEventOrganizer).Include(t => t.Club.MajorInfo).Include(t => t.RejectReason).Find(id);

            if (e != null)
            {
                ClubMember membership = ScmRoleProvider.GetRoleInClub(e.ClubId);

                if (membership != null || User.IsInRole("社联") || e.Status != Application.NOT_VERIFIED && e.Status != Application.FAILED)
                {
                    if (e.Status != Application.NOT_SUBMITTED || ScmRoleProvider.IsOrganizerOf(e.Id)
                        && membership.ClubRoleId != ScmRoleProvider.GetRoleIdByName("会长"))
                    {
                        ViewBag.HasAccessToCriticalSections = HasAccessToCriticalDetails(e);
                        ViewBag.CurrentOrder = order;
                        ViewBag.PageNumber = page_number;
                        ViewBag.Search = search;
                        ViewBag.SearchOption = search_option;
                        ViewBag.PassFilter = pass_filter;
                        ViewBag.PublicKey = HtmlHelpersExtensions.GenerateKeyPairFor("/Event/Details");

                        return View(e);
                    }
                    else
                    {
                        return RedirectToAction("Edit", new
                        {
                            id = id,
                            page_number = page_number,
                            search = search,
                            search_option = search_option,
                            order = order,
                            pass_filter = pass_filter
                        });
                    }
                }
            }

            return View("EventNotFoundError");
        }

        //
        // GET: /Event/Create

        [Authorize]
        public ActionResult Create(int club_id)
        {
            // 只有会长才能创建新活动。
            if (ScmRoleProvider.HasMembershipIn(club_id, null, new string[] { "会长" }))
            {
                ViewBag.ClubId = club_id;

                return View();
            }

            return View("InvalidOperationError");
        }

        //
        // POST: /Event/Create

        [Authorize]
        [HttpPost]
        public ActionResult Create(Event e)
        {
            // 只有会长才能创建新活动。
            if (ScmRoleProvider.HasMembershipIn(e.ClubId, null, new string[] { "会长" }))
            {
                if (ModelState.IsValid)
                {
                    e.Status = Application.NOT_SUBMITTED;

                    db.Events.Add(e);
                    db.SaveChanges();

                    return Json(new { success = true, msg = e.Title + "创建成功", url = "List?pass_filter=NotSubmitted&club_id=" + e.ClubId });
                }

                return Json(new { success = false, msg = "创建活动失败" });
            }

            return Json(new { success = false, msg = "非法操作！您没有创建新活动的权限" });
        }

        [Authorize]
        public ActionResult SubEvents(int id, string public_key, bool is_edit = false)
        {
            Event e = db.Events.Include(t => t.SubEvents.Select(s => s.LocationApplications.Select(f => f.Locations)))
                .Include(t => t.SubEvents.Select(s => s.AssetApplications.Select(f => f.ApplicatedAssets.Select(p => p.Asset))))
                .Include(t => t.SubEvents.Select(s => s.LocationApplications.Select(f => f.Times))).Include(t => t.Description)
                .Include(t => t.SubEvents.Select(s => s.AssetApplications.Select(f => f.Times))).Find(id);

            if (e != null)
            {
                if (HtmlHelpersExtensions.IsLegalAccessFrom("/Event/Details", public_key) && !is_edit)
                {
                    return View(e);
                }
                else if (HtmlHelpersExtensions.IsLegalAccessFrom("/Event/Edit", public_key))
                {
                    SelectList list = new SelectList(db.Times.ToList().ToList(), "Id", "TimeName");
                    ViewBag.Times = list;

                    var settings = new JsonSerializerSettings()
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.All
                    };

                    string json = JsonConvert.SerializeObject(e.SubEvents, settings);

                    return View("EditSubEvents", (object)json);
                }
                else
                {
                    return RedirectToAction("Details", new { id = id });
                }
            }

            return View("EventNotFoundError");
        }

        [Authorize]
        public ActionResult Organizers(int id, string public_key, bool is_edit = false)
        {
            Event e = db.Events.Include(t => t.Organizers).Include(t => t.ChiefEventOrganizer).Find(id);

            if (e != null)
            {
                if (HasAccessToCriticalDetails(e))
                {
                    if (HtmlHelpersExtensions.IsLegalAccessFrom("/Event/Details", public_key) && !is_edit)
                    {
                        return View(e);
                    }
                    else if (HtmlHelpersExtensions.IsLegalAccessFrom("/Event/Edit", public_key))
                    {
                        return View("EditOrganizers", e);
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = id });
                    }
                }
                else
                {
                    return View("PermissionDeniedError");
                }
            }

            return View("EventNotFoundError");
        }

        [Authorize]
        public ActionResult Funds(int id, string public_key, bool is_edit = false)
        {
            Event e = db.Events.Include(t => t.Club).Include(t => t.FundApplication).Find(id);

            if (e != null)
            {
                if (HasAccessToCriticalDetails(e))
                {
                    if (HtmlHelpersExtensions.IsLegalAccessFrom("/Event/Details", public_key) && !is_edit)
                    {
                        return View(e);
                    }
                    else if (HtmlHelpersExtensions.IsLegalAccessFrom("/Event/Edit", public_key))
                    {
                        return View("EditFunds", e);
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = id });
                    }
                }
                else
                {
                    return View("PermissionDeniedError");
                }
            }

            return View("EventNotFoundError");
        }

        [Authorize]
        public ActionResult EventDescription(int id, string public_key, bool is_edit = false)
        {
            Event e = db.Events.Include(t => t.Description).Find(id);

            if (e != null)
            {
                if (HtmlHelpersExtensions.IsLegalAccessFrom("/Event/Details", public_key) && !is_edit)
                {
                    return View(e);
                }
                else if (HtmlHelpersExtensions.IsLegalAccessFrom("/Event/Edit", public_key))
                {
                    return View("EditEventDescription", e);
                }
                else
                {
                    return RedirectToAction("Details", new { id = id });
                }
            }

            return View("EventNotFoundError");
        }

        [Authorize]
        public ActionResult DownloadPlan(int id)
        {
            Event e = db.Events.Find(id);

            if (e != null)
            {
                if (HasAccessToCriticalDetails(e))
                {
                    string path = Path.Combine(Server.MapPath(ConfigurationManager.EventPlanFolder), e.PlanUrl);

                    if (System.IO.File.Exists(path))
                    {
                        return File(path, MimeMapping.GetMimeMapping(e.PlanUrl));
                    }
                    else
                    {
                        return View("FileNotFoundError");
                    }
                }
                else
                {
                    return View("PermissionDeniedError");
                }
            }

            return View("EventNotFoundError");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Submit(int id)
        {
            Event e = db.Events
                .Include(t => t.FundApplication)
                .Include(t => t.SubEvents.Select(s => s.AssetApplications))
                .Include(t => t.SubEvents.Select(s => s.LocationApplications))
                .Find(id);

            if (e != null)
            {
                if (ScmMembershipProvider.IsMe(e.ChiefEventOrganizerId))
                {
                    if (e.Status == Application.NOT_SUBMITTED)
                    {
                        e.Status = Application.NOT_VERIFIED;
                        e.Organizers.Add(e.ChiefEventOrganizer);
                        e.Date = DateTime.Now;

                        if (e.FundApplication != null)
                        {
                            e.FundApplication.Date = e.Date.Value;
                            e.FundApplication.Status = Application.NOT_VERIFIED;
                        }

                        foreach (var sub_event in e.SubEvents)
                        {
                            foreach (var asset_app in sub_event.AssetApplications)
                            {
                                asset_app.Status = Application.NOT_VERIFIED;
                                asset_app.Date = e.Date.Value;
                            }

                            foreach (var loc_app in sub_event.LocationApplications)
                            {
                                loc_app.Status = Application.NOT_VERIFIED;
                                loc_app.Date = e.Date.Value;
                            }
                        }

                        db.SaveChanges();

                        string return_url = "Details?id=" + id;

                        return Json(new { success = true, msg = "提交成功", url = return_url });
                    }

                    return Json(new { success = false, msg = "非法操作，提交失败" });
                }

                return Json(new { success = false, msg = "您没有执行该操作的权限，提交失败" });
            }

            return Json(new { success = false, msg = "活动已被删除，提交失败" });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Quit(int id)
        {
            Event e = db.Events.Include(t => t.Organizers).Find(id);

            if (e != null)
            {
                if (ScmRoleProvider.IsOrganizerOf(id))
                {
                    if (ScmMembershipProvider.IsMe(e.ChiefEventOrganizerId))
                        e.ChiefEventOrganizerId = null;

                    Student student = e.Organizers.FirstOrDefault(t => ScmMembershipProvider.IsMe(t.UserName));

                    if (student != null)
                        e.Organizers.Remove(student);

                    db.SaveChanges();

                    return Json(new { success = true, msg = "成功退出" + e.Title + "的组织小组", url = "List?club_id=" + e.ClubId });
                }

                return Json(new { success = false, msg = "非法操作，提交失败" });
            }

            return Json(new { success = false, msg = "活动已被删除，提交失败" });
        }

        //
        // GET: /Event/Edit/5

        [Authorize]
        public ActionResult Edit(int id, int page_number = 1, string search = "", string search_option = "Title", string order = "Title", string pass_filter = "")
        {
            Event e = db.Events.Include(t => t.ChiefEventOrganizer).Include(t => t.Club.MajorInfo).Include(t => t.Organizers)
                .Include(t => t.Club).Find(id);
            
            if (e != null)
            {
                object route_values = new
                        {
                            id = id,
                            page_number = page_number,
                            search = search,
                            search_option = search_option,
                            order = order,
                            pass_filter = pass_filter
                        };

                ClubMember membership = ScmRoleProvider.GetRoleInClub(e.ClubId);

                bool is_member = membership != null;
                bool is_chief = ScmMembershipProvider.IsMe(e.ChiefEventOrganizerId);
                bool is_president = membership.ClubRoleId == ScmRoleProvider.GetRoleIdByName("会长");
                bool is_organizer = e.Organizers.Any(t => ScmMembershipProvider.IsMe(t.UserName)) || is_chief;
                bool is_admin = User.IsInRole("社联");

                ViewBag.HasAccessToCriticalSections = HasAccessToCriticalDetails(e);
                ViewBag.CurrentOrder = order;
                ViewBag.PageNumber = page_number;
                ViewBag.Search = search;
                ViewBag.SearchOption = search_option;
                ViewBag.PassFilter = pass_filter;
                ViewBag.IsPresident = is_president;
                ViewBag.IsChief = is_chief;
                ViewBag.IsOrganizer = is_organizer;

                if (e.Status == Application.NOT_SUBMITTED)
                {
                    if (is_chief || is_president)
                    {
                        ViewBag.PublicKey = HtmlHelpersExtensions.GenerateKeyPairFor("/Event/Edit");

                        return View(e);
                    }
                    else if (is_organizer)
                    {
                        return RedirectToAction("Details", route_values);
                    }
                    else
                    {
                        return View("PermissionDeniedError");
                    }
                }
                else if (e.Status == Application.NOT_VERIFIED || e.Status == Application.FAILED)
                {
                    if (is_member || is_admin)
                    {
                        return RedirectToAction("Details", route_values);
                    }
                    else
                    {
                        return View("PermissionDeniedError");
                    }
                }
                else
                {
                    return RedirectToAction("Details", route_values);
                }
            }
            else
            {
                return View("EventNotFoundError");
            }
        }

        //
        // POST: /Event/Edit/5

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Event e, HttpPostedFileBase PosterUrl, HttpPostedFileBase PlanUrl, string[] event_organizers, bool has_modified_sub_events = false, bool has_modified_organizers = false)
        {
            if (ModelState.IsValid)
            {
                EventHelpers event_helper = new EventHelpers(db);
                AssetHelpers asset_helper = new AssetHelpers(db);
                LocationHelpers location_helper = new LocationHelpers(db);

                Event modifying_event = db.Events
                    .Include(t => t.Description)
                    .Include(t => t.FundApplication)
                    .Include(t => t.Organizers)
                    .Include(t => t.SubEvents.Select(s => s.AssetApplications.Select(f => f.ApplicatedAssets)))
                    .Include(t => t.SubEvents.Select(s => s.Description.Description))
                    .Include(t => t.SubEvents.Select(s => s.LocationApplications.Select(f => f.Locations)))
                    .Include(t => t.SubEvents.Select(s => s.Times))
                    .Find(e.Id);

                if (ScmMembershipProvider.IsMe(modifying_event.ChiefEventOrganizerId))
                {
                    if (modifying_event != null)
                    {
                        if (e.Description != null)
                        {
                            if (modifying_event.Description == null)
                                modifying_event.Description = new EventDescription();

                            modifying_event.Description.Description = e.Description.Description;
                        }

                        modifying_event.EndDate = e.EndDate;
                        modifying_event.StartDate = e.StartDate;

                        if (e.FundApplication != null)
                        {
                            if (e.FundApplication.Quantity > 0)
                            {
                                if (modifying_event.FundApplication == null)
                                    modifying_event.FundApplication = new FundApplication
                                    {
                                        Id = db.GenerateIdFor(IdentityForTPC.APPLICATION),
                                        ApplicantUserName = e.ChiefEventOrganizerId,
                                        ClubId = modifying_event.ClubId,
                                        Date = DateTime.Now,
                                        Status = Application.NOT_SUBMITTED
                                    };

                                modifying_event.FundApplication.Quantity = e.FundApplication.Quantity;
                            }
                            else
                            {
                                if (modifying_event.FundApplication != null)
                                    db.FundApplications.Delete(modifying_event.FundApplication);
                            }
                        }

                        if (has_modified_organizers)
                        {
                            e.Organizers = new List<Student>();

                            if (event_organizers != null)
                            {
                                foreach (string organizer in event_organizers)
                                {
                                    Student student = db.Students.Find(organizer);

                                    if (student != null)
                                    {
                                        e.Organizers.Add(student);
                                    }
                                }
                            }

                            if (e.Organizers != null)
                            {
                                // 检察是否有不存在的组织者
                                if (e.Organizers.Any(t => db.Students.ToList().All(s => s.UserName != t.UserName)))
                                {
                                    ModelState.AddModelError("Organizers", "非法用户");
                                    return Json(new { success = false, msg = "存在非法用户，保存失败" });
                                }

                                // 检察是否存在重复用户
                                IDictionary<string, int> organizer_occurance_counter = new Dictionary<string, int>();
                                foreach (var organizer in e.Organizers)
                                {
                                    if (!organizer_occurance_counter.ContainsKey(organizer.UserName))
                                        organizer_occurance_counter.Add(new KeyValuePair<string, int>(organizer.UserName, 0));

                                    organizer_occurance_counter[organizer.UserName]++;
                                }
                                foreach (var organizer_occurance in organizer_occurance_counter)
                                {
                                    if (organizer_occurance.Value > 1)
                                    {
                                        e.Organizers.Remove(e.Organizers.Single(t => t.UserName == organizer_occurance.Key));
                                    }
                                }

                                if (modifying_event.Organizers == null)
                                    modifying_event.Organizers = new List<Student>();

                                // 更新组织者列表
                                var deleting_organizers = modifying_event.Organizers.Where(t => e.Organizers.All(s => s.UserName != t.UserName));

                                foreach (var deleting_organizer in deleting_organizers)
                                {
                                    modifying_event.Organizers.Remove(deleting_organizer);
                                }

                                foreach (var organizer in e.Organizers)
                                {
                                    if (modifying_event.Organizers.All(t => t.UserName != organizer.UserName))
                                    {
                                        modifying_event.Organizers.Add(organizer);
                                    }
                                }
                            }
                            else
                            {
                                if (modifying_event.Organizers != null)
                                    modifying_event.Organizers.Clear();
                            }
                        }

                        string plan_url = HtmlHelpersExtensions.RenameAndSaveFile(PlanUrl, ConfigurationManager.EventPlanFolder);
                        string poster_url = HtmlHelpersExtensions.RenameAndSaveFile(PosterUrl, ConfigurationManager.EventPosterFolder);

                        if (!String.IsNullOrWhiteSpace(plan_url))
                            modifying_event.PlanUrl = plan_url;
                        if (!String.IsNullOrWhiteSpace(poster_url))
                            modifying_event.PosterUrl = poster_url;

                        modifying_event.Title = e.Title;

                        if (has_modified_sub_events)
                        {
                            // 删除已被删除的子活动
                            if (modifying_event.SubEvents != null)
                            {
                                IEnumerable<SubEvent> deleting_sub_events = null;

                                if (e.SubEvents == null)
                                {
                                    deleting_sub_events = modifying_event.SubEvents.ToList();
                                }
                                else
                                {
                                    deleting_sub_events = modifying_event.SubEvents.Where(t => e.SubEvents.All(s => s.Id != t.Id));
                                }

                                foreach (var deleting_sub_event in deleting_sub_events)
                                {
                                    event_helper.DeleteSubEvent(deleting_sub_event, modifying_event.SubEvents, true);
                                }
                            }

                            if (e.SubEvents != null)
                            {
                                foreach (SubEvent sub_event in e.SubEvents)
                                {
                                    // 合并重复物资
                                    if (sub_event.AssetApplications != null)
                                    {
                                        foreach (AssetApplication application in sub_event.AssetApplications)
                                        {
                                            asset_helper.JoinApplicatedAssets(application);
                                        }
                                    }

                                    // 合并重复场地
                                    if (sub_event.LocationApplications != null)
                                    {
                                        foreach (LocationApplication application in sub_event.LocationApplications)
                                        {
                                            location_helper.JoinLocations(application);
                                        }
                                    }

                                    sub_event.Times = db.Times.ToList().ToList().Where(t => t.IsCoveredBy(sub_event.StartTime, sub_event.EndTime)).ToList();

                                    // 添加子活动
                                    if (sub_event.Id == 0)
                                    {
                                        sub_event.Event = modifying_event;

                                        if (sub_event.AssetApplications != null)
                                        {
                                            foreach (var asset_app in sub_event.AssetApplications)
                                            {
                                                event_helper.NewAssetApplication(asset_app, sub_event);
                                            }
                                        }

                                        if (sub_event.LocationApplications != null)
                                        {
                                            foreach (var location_app in sub_event.LocationApplications)
                                            {
                                                event_helper.NewLocationApplication(location_app, sub_event);
                                            }
                                        }

                                        modifying_event.SubEvents.Add(sub_event);
                                    }
                                    else // 修改原有子活动
                                    {
                                        SubEvent modifying_sub_event = modifying_event.SubEvents.FirstOrDefault(t => t.Id == sub_event.Id);

                                        modifying_sub_event.Date = sub_event.Date;
                                        modifying_sub_event.EndTime = sub_event.EndTime;
                                        modifying_sub_event.StartTime = sub_event.StartTime;
                                        modifying_sub_event.Title = sub_event.Title;

                                        event_helper.UpdateTimes(modifying_sub_event.Times, sub_event.Times);

                                        // 创建物资申请
                                        if (modifying_sub_event.AssetApplications == null || modifying_sub_event.AssetApplications.Count == 0)
                                        {
                                            if (sub_event.AssetApplications != null)
                                            {
                                                foreach (var asset_app in sub_event.AssetApplications)
                                                {
                                                    event_helper.NewAssetApplication(asset_app, modifying_sub_event);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // 删除物资申请
                                            if (sub_event.AssetApplications == null)
                                            {
                                                foreach (var application in modifying_sub_event.AssetApplications.ToList())
                                                {
                                                    db.AssetApplications.Delete(application);
                                                }
                                            }
                                            else // 修改物资申请
                                            {
                                                foreach (var new_application in sub_event.AssetApplications)
                                                {
                                                    foreach (var orig_application in modifying_sub_event.AssetApplications)
                                                    {
                                                        event_helper.UpdateAssetApplication(orig_application, new_application);
                                                    }
                                                }
                                            }
                                        }

                                        // 创建场地申请
                                        if (modifying_sub_event.LocationApplications == null || modifying_sub_event.LocationApplications.Count == 0)
                                        {
                                            if (sub_event.LocationApplications != null)
                                            {
                                                foreach (var location_app in sub_event.LocationApplications)
                                                {
                                                    event_helper.NewLocationApplication(location_app, modifying_sub_event);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // 删除场地申请
                                            if (sub_event.LocationApplications == null)
                                            {
                                                foreach (var application in modifying_sub_event.LocationApplications.ToList())
                                                {
                                                    db.LocationApplications.Delete(application);
                                                }

                                            }
                                            else // 修改场地申请
                                            {
                                                foreach (var new_application in sub_event.LocationApplications)
                                                {
                                                    foreach (var orig_application in modifying_sub_event.LocationApplications)
                                                    {
                                                        event_helper.UpdateLocationApplication(orig_application, new_application);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (ScmRoleProvider.HasMembershipIn(modifying_event.ClubId, null, new string[] { "会长" }))
                {
                    if (modifying_event != null)
                        modifying_event.ChiefEventOrganizerId = e.ChiefEventOrganizerId;
                }

                db.SaveChanges();

                return Json(new { success = true, msg = "保存成功" });
            }

            return Json(new { success = false, msg = "保存失败" });
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

        [Authorize]
        public ActionResult GetAvailableLocations(DateTime date, TimeSpan start_time, TimeSpan end_time)
        {
            LocationHelpers helper = new LocationHelpers(db);
            IEnumerable<Location> locations = helper.GetAvailableLocations(date, start_time, end_time).ToList();
            AvailableLocation[] locations_for_view = new AvailableLocation[locations.Count()];

            for (int i = 0; i < locations_for_view.Length; ++i)
            {
                locations_for_view[i] = new AvailableLocation
                {
                    Id = locations.ElementAt(i).Id,
                    Name = locations.ElementAt(i).Name
                };
            }

            return Json(locations_for_view, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetAvailableAssets(DateTime date, TimeSpan start_time, TimeSpan end_time)
        {
            AssetHelpers helper = new AssetHelpers(db);
            IEnumerable<Asset> assets = helper.GetAvailableAssets(date, start_time, end_time);
            AvailableAsset[] assets_for_view = new AvailableAsset[assets.Count()];

            for (int i = 0; i < assets_for_view.Length; ++i)
            {
                assets_for_view[i] = new AvailableAsset
                {
                    Id = assets.ElementAt(i).Id,
                    Name = assets.ElementAt(i).Name,
                    Count = assets.ElementAt(i).Count
                };
            }

            return Json(assets_for_view, JsonRequestBehavior.AllowGet);
        }

        private bool HasAccessToCriticalDetails(Event e)
        {
            return User.IsInRole("社联") ||
                            ScmRoleProvider.HasMembershipIn(e.ClubId, null, new string[] { "会长" }) || ScmRoleProvider.IsOrganizerOf(e.Id);
        }

        private IEnumerable<Event> FilterEvents(IQueryable<Event> collection, int page_number = 1,
            string order = "Title", string search = "", string search_option = "", string pass_filter = "", int club_id = Application.ALL, string user_name = "")
        {
            IPagedList<Event> list = null;

            ViewBag.CurrentOrder = order;
            ViewBag.TitleOrderOpt = order == "Title" ? "TitleDesc" : "Title";
            ViewBag.ClubNameOrderOpt = order == "Club.MajorInfo.Name" ? "Club.MajorInfo.NameDesc" : "Club.MajorInfo.Name";
            ViewBag.OrganizerOrderOpt = order == "ChiefEventOrganizer.Name" ? "ChiefEventOrganizer.NameDesc" : "ChiefEventOrganizer.Name";
            ViewBag.ScoreOrderOpt = order == "Score" ? "ScoreDesc" : "Score";
            ViewBag.DateOrderOpt = order == "Date" ? "DateDesc" : "Date";
            ViewBag.StatusOrderOpt = order == "Status" ? "StatusDesc" : "Status";
            ViewBag.StartDateOrderOpt = order == "StartDate" ? "StartDateDesc" : "StartDate";
            ViewBag.EndDateOrderOpt = order == "EndDate" ? "EndDateDesc" : "EndDate";
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

                if (!String.IsNullOrWhiteSpace(user_name))
                {
                    collection = collection.Where(t => t.Organizers.Any(s => s.UserName == user_name));
                }

                switch (pass_filter)
                {
                    case "Passed":
                        collection = collection.Where(s => s.Status == Application.PASSED || s.Status == Application.CANCELED || s.Status == Application.TERMINATED);
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
                        collection = collection.Where(s => s.Status == Application.PASSED &&
                            s.StartDate > DateTime.Now);
                        break;

                    case "Active":
                        collection = collection.Where(s => s.Status == Application.PASSED && s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now);
                        break;

                    case "Finished":
                        collection = collection.Where(s => s.Status == Application.PASSED && s.EndDate < DateTime.Now);
                        break;

                    case "Canceled":
                        collection = collection.Where(s => s.Status == Application.CANCELED);
                        break;

                    case "Terminated":
                        collection = collection.Where(s => s.Status == Application.TERMINATED);
                        break;

                    case "NotSubmitted":
                        collection = collection.Where(s => s.Status == Application.NOT_SUBMITTED);
                        break;

                    case "All":
                        break;

                    default:
                        collection = collection.Where(s => s.Status != Application.NOT_SUBMITTED);
                        break;
                }

                string[] includes = { "Club.MajorInfo", "ChiefEventOrganizer", "SubEvents" };
                list = QueryProcessor.Query(collection, null, order, includes, page_number, 20);
            }

            return list;
        }
    }
}
