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
    public class ClubApplicationController : Controller
    {
        UnitOfWork db = new UnitOfWork();

        //
        // GET: /ClubApplication/
        
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        [Authorize]
        public ActionResult List(int club_id = 0, int page_number = 1, string order = "Date", string pass_filter = "", string search = "", 
            string search_option = "ClubName", string type_filter = "ClubTransaction")
        {
            if (!User.IsInRole("社联") && !ScmRoleProvider.HasMembershipIn(club_id))
            {
                // TODO: 将用户重定向到另一个页面。
                return RedirectToAction("Index", "Home");
            }

            IQueryable<Application> applications = 
                db.Applications.Include(s => s.Club).Include(s => s.Club.MajorInfo).Include(s => s.Applicant).ToList() as IQueryable<Application>;

            // 下拉框
            List<KeyValuePair<string, string>> select_list = new List<KeyValuePair<string, string>>();
            select_list.Add(new KeyValuePair<string, string>("社团名称", "ClubName"));
            select_list.Add(new KeyValuePair<string, string>("申请人名", "ApplicantName"));

            ViewBag.SearchOptions = new SelectList(select_list, "Value", "Key", search_option);
            
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
        // GET: /ClubApplication/Details/5

        [Authorize]
        public ActionResult Details(int id, int club_id = 0, int page_number = 1, string order = "Date", string pass_filter = "", string search = "",
            string search_option = "", string type_filter = "")
        {
            Application application = db.Applications.Include(s => s.Applicant).Include(s => s.Club).Find(id);

            ViewBag.ClubId = club_id;
            ViewBag.CurrentOrder = order;
            ViewBag.PageNumber = page_number;
            ViewBag.Search = search;
            ViewBag.PassFilter = pass_filter;
            ViewBag.SearchOption = search_option;
            ViewBag.TypeFilter = type_filter;

            if (application != null)
            {
                if (User.IsInRole("社联") || (application is ClubRegisterApplication &&
                    (application as ClubRegisterApplication).Applicants.Any(s => s.ApplicantUserName == User.Identity.Name)) ||
                    ScmRoleProvider.HasMembershipIn(application.ClubId.Value))
                {
                    if (application is ClubRegisterApplication)
                    {
                        return View("RegisterApplicationDetails", application as ClubRegisterApplication);
                    }
                    else if (application is ClubUnregisterApplication)
                    {
                        return View("UnregisterApplicationDetails", application as ClubUnregisterApplication);
                    }
                    else if (application is ClubInfoModificationApplication)
                    {
                        return View("InfoModificationApplicationDetails", application as ClubInfoModificationApplication);
                    }
                }
                else
                {
                    return View("PermissionDeniedError");
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "社联")]
        public ActionResult Verify(int id, bool is_passed, string reject_reason, int club_id = 0, int page_number = 1, string order = "Date", string pass_filter = "", string search = "",
            string search_option = "", string type_filter = "")
        {
            Application application = db.Applications.Find(id);

            if (application != null && application.Status == Application.NOT_VERIFIED)
            {
                // 更改申请状态
                application.Status = is_passed ? Application.PASSED : Application.FAILED;

                // 使申请生效
                if (is_passed)
                {
                    if (application is ClubRegisterApplication)
                    {
                        ClubRole president = null;
                        ClubRole member = null;

                        foreach (var role in db.ClubRoles.ToList())
                        {
                            if (role.Name == "会长")
                            {
                                president = role;
                            }
                            else if (role.Name == "会员")
                            {
                                member = role;
                            }
                        }

                        ClubRegisterApplication register_application = application as ClubRegisterApplication;
                        int applicant_count = register_application.Applicants.Count();

                        ClubBranch member_branch = new ClubBranch
                        {
                            BranchName = "会员部",
                            MemberCount = applicant_count,
                            NewMemberCount = applicant_count,
                            Members = new List<ClubMember>()
                        };

                        ClubMajorInfo major_info = new ClubMajorInfo
                        {
                            Name = register_application.MajorInfo.Name,
                            Instructor = register_application.MajorInfo.Instructor
                        };

                        ClubSubInfo sub_info = new ClubSubInfo
                        {
                            Principle = register_application.SubInfo.Principle,
                            Purpose = register_application.SubInfo.Purpose,
                            Range = register_application.SubInfo.Range,
                            Address = register_application.SubInfo.Address,
                            PosterUrl = register_application.SubInfo.PosterUrl,
                            Regulation = register_application.SubInfo.Regulation
                        };

                        Club registering_club = new Club
                        {
                            Level = 1,
                            Fund = 0,
                            FoundDate = DateTime.Now,
                            MemberCount = applicant_count,
                            NewMemberCount = applicant_count,
                            MajorInfo = major_info,
                            SubInfo = sub_info,
                            Branches = new List<ClubBranch>
                        {
                            member_branch
                        },
                            Applications = new List<Application>
                        {
                            register_application
                        }
                        };

                        if (register_application.Applicants != null)
                        {
                            foreach (var applicant in register_application.Applicants)
                            {
                                member_branch.Members.Add(new ClubMember
                                {
                                    JoinDate = DateTime.Now,
                                    Student = applicant.Applicant,
                                    Club = registering_club,
                                    ClubRole = applicant.IsMainApplicant ? president : member
                                });
                            }
                        }
                        else
                        {
                            // 不可能出现没有申请人的社团申请
                            throw new ArgumentException("No applicant in this club register application.");
                        }

                        if (register_application.Branches != null)
                        {
                            foreach (var created_branch in register_application.Branches)
                            {
                                registering_club.Branches.Add(new ClubBranch
                                {
                                    BranchName = created_branch.BranchName,
                                    MemberCount = 0,
                                    NewMemberCount = 0
                                });
                            }
                        }

                        db.Clubs.Add(registering_club);
                    }
                    else if (application is ClubInfoModificationApplication)
                    {
                        ClubInfoModificationApplication info_modification_application = application as ClubInfoModificationApplication;
                        Club modifying_club = info_modification_application.Club;

                        if (modifying_club != null)
                        {
                            if (info_modification_application.MajorInfo != null)
                            {
                                modifying_club.MajorInfo.Name = info_modification_application.MajorInfo.Name;
                                modifying_club.MajorInfo.Instructor = info_modification_application.MajorInfo.Instructor;
                            }

                            if (info_modification_application.SubInfo != null)
                            {
                                modifying_club.SubInfo.Address = info_modification_application.SubInfo.Address;
                                modifying_club.SubInfo.PosterUrl = info_modification_application.SubInfo.PosterUrl;
                                modifying_club.SubInfo.Principle = info_modification_application.SubInfo.Principle;
                                modifying_club.SubInfo.Purpose = info_modification_application.SubInfo.Purpose;
                                modifying_club.SubInfo.Range = info_modification_application.SubInfo.Range;
                                modifying_club.SubInfo.Regulation = info_modification_application.SubInfo.Regulation;
                            }

                            if (info_modification_application.ModificationBranches != null)
                            {
                                foreach (var branch_modification in info_modification_application.ModificationBranches)
                                {
                                    if (branch_modification is BranchCreation)
                                    {
                                        modifying_club.Branches.Add(new ClubBranch
                                        {
                                            BranchName = branch_modification.BranchName,
                                            MemberCount = 0,
                                            NewMemberCount = 0,
                                            Club = modifying_club
                                        });
                                    }
                                    else if (branch_modification is BranchDeletion)
                                    {
                                        var orig_branch = branch_modification.OrigBranch;
                                        ClubBranch receiving_branch = null;

                                        // 优先选用会员部作为收容部门，若会员部不存在则使用第一个找到的部门。
                                        receiving_branch = modifying_club.Branches.First(s => s.BranchName == "会员部");
                                        if (receiving_branch == null)
                                        {
                                            receiving_branch = modifying_club.Branches.First();
                                        }

                                        // 还是没有部门？没救了
                                        if (receiving_branch == null)
                                        {
                                            throw new NullReferenceException("Damn! There is no branch in this stupid club!!!");
                                        }

                                        foreach (var branch_member in branch_modification.OrigBranch.Members)
                                        {
                                            receiving_branch.Members.Add(branch_member);
                                        }

                                        receiving_branch.MemberCount += orig_branch.MemberCount;
                                        receiving_branch.NewMemberCount += orig_branch.NewMemberCount;

                                        modifying_club.Branches.Remove(branch_modification.OrigBranch);
                                        db.ClubBranches.Delete(branch_modification.OrigBranch);
                                    }
                                    else if (branch_modification is BranchUpdate)
                                    {
                                        branch_modification.OrigBranch.BranchName = branch_modification.BranchName;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // 修改一个不存在的社团。。。
                            throw new ArgumentException("The club being modified does not exist.");
                        }
                    }
                    else if (application is ClubUnregisterApplication)
                    {
                        ClubUnregisterApplication unregister_application = application as ClubUnregisterApplication;

                        db.Clubs.Include(t => t.Applications).Delete(unregister_application.Club);
                    }
                    else
                    {
                        // 只有3种社团事务申请。
                        throw new ArgumentException("Invalid club transaction application!");
                    }
                }
                else
                {
                    application.RejectReason = new ApplicationRejectReason
                    {
                        Reason = reject_reason
                    };

                    // 清除遗产
                    if (application is ClubRegisterApplication)
                    {
                        ClubRegisterApplication register_application = application as ClubRegisterApplication;
                        string path = Path.Combine(Server.MapPath(ConfigurationManager.ClubSplashPanelFolder), register_application.SubInfo.PosterUrl);

                        if (!String.IsNullOrWhiteSpace(register_application.SubInfo.PosterUrl) && System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                    else if (application is ClubUnregisterApplication)
                    {
                        Club club = db.Clubs.Include(t => t.SubInfo).Find(application.ClubId);
                        string path = Path.Combine(Server.MapPath(ConfigurationManager.ClubSplashPanelFolder), club.SubInfo.PosterUrl);

                        if (!String.IsNullOrWhiteSpace(club.SubInfo.PosterUrl) && System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                }

                db.SaveChanges();

                return RedirectToAction("List", new
                {
                    club_id = club_id,
                    page_number = page_number,
                    order = order,
                    pass_filter = pass_filter,
                    search = search,
                    search_option = search_option,
                    type_filter = type_filter
                });
            }

            return View("InvalidOperationError");
        }

        //
        // GET: /ClubApplication/Create

        [Authorize]
        public ActionResult ApplyNewClub()
        {
            return View();
        } 

        //
        // POST: /ClubApplication/Create

        [HttpPost]
        [Authorize]
        public ActionResult ApplyNewClub(ClubRegisterApplication register_application, BranchCreation[] new_branches,
            HttpPostedFileBase poster, string[] test)
        {
            if (new_branches != null)
            {
                register_application.Branches = new List<BranchModification>();

                foreach (var branch in new_branches)
                {
                    register_application.Branches.Add(branch);
                }
            }

            if (ModelState.IsValid)
            {
                int id = db.GenerateIdFor(IdentityForTPC.APPLICATION);

                if (poster != null && poster.ContentLength > 0)
                {
                    string guid = Guid.NewGuid().ToString();
                    string extension = "";

                    if (Path.HasExtension(poster.FileName))
                    {
                        extension = Path.GetExtension(poster.FileName);
                    }

                    string file_name = guid + extension;
                    string path = Path.Combine(Server.MapPath(ConfigurationManager.ClubSplashPanelFolder), file_name);

                    poster.SaveAs(path);
                    register_application.SubInfo.PosterUrl = file_name;
                }

                register_application.ApplicantUserName = User.Identity.Name;
                register_application.Date = DateTime.Now;
                register_application.Status = Application.NOT_VERIFIED;
                register_application.Id = id;

                db.Applications.Add(register_application);
                db.SaveChanges();

                return Json(new { success = true, msg = "成功提交申请", url = "List" });
            }

            return Json(new { success = false, msg = "提交失败" });
        }

        [HttpPost]
        [Authorize]
        public ActionResult ApplyNewClubAddApplicant(string user_name)
        {
            if (db.Students.ToList().Any(t => t.UserName == user_name) && ModelState.IsValid)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, msg = "该用户不存在或该用户不是学生" });
            }
        }

        [Authorize]
        public ActionResult ApplyModifyClubInfo(int id)
        {
            var club = db.Clubs.Include(t => t.MajorInfo).Include(t => t.SubInfo).Include(t => t.Branches).Find(id);

            if (club != null)
            {
                return View(club);
            }
            else
            {
                return View("ClubNotFoundError");
            }
        }
        
        [Authorize]
        [HttpPost]
        public ActionResult ApplyModifyClubInfo(Club modified_club, int[] deleted_branch_ids)
        {
            if (ModelState.IsValid)
            {
                Club club = db.Clubs.Include(t => t.Branches).Find(modified_club.Id);

                ClubInfoModificationApplication application = new ClubInfoModificationApplication();
                int id = db.GenerateIdFor(IdentityForTPC.APPLICATION);
                bool has_changed = false;

                application.Id = id;
                application.Status = Application.NOT_VERIFIED;
                application.RejectReason = null;
                application.Date = DateTime.Now;
                application.ClubId = modified_club.Id;
                application.ApplicantUserName = User.Identity.Name;

                if (modified_club.MajorInfo != null && modified_club.MajorInfo != club.MajorInfo)
                {
                    application.MajorInfo = modified_club.MajorInfo;
                    has_changed = true;
                }

                if (modified_club.SubInfo != null && modified_club.SubInfo != club.SubInfo)
                {
                    application.SubInfo = modified_club.SubInfo;
                    application.SubInfo.PosterUrl = club.SubInfo.PosterUrl;
                    has_changed = true;
                }

                application.ModificationBranches = new List<BranchModification>();

                if (modified_club.Branches != null)
                {
                    foreach (var modified_branch in modified_club.Branches)
                    {
                        if (modified_branch.Id == 0)
                        {
                            application.ModificationBranches.Add(new BranchCreation
                            {
                                BranchName = modified_branch.BranchName
                            });
                            has_changed = true;
                        }
                        else
                        {
                            ClubBranch orig_branch = club.Branches.First(t => t.Id == modified_branch.Id);

                            if (orig_branch.BranchName != modified_branch.BranchName)
                            {
                                application.ModificationBranches.Add(new BranchUpdate
                                {
                                    BranchName = modified_branch.BranchName,
                                    OrigBranch = orig_branch
                                });
                                has_changed = true;
                            }
                        }
                    }
                }

                if (deleted_branch_ids != null)
                {
                    foreach (int deleted_branch_id in deleted_branch_ids)
                    {
                        application.ModificationBranches.Add(new BranchDeletion
                        {
                            BranchId = deleted_branch_id
                        });
                    }
                    has_changed = true;
                }

                if (has_changed)
                {
                    db.Applications.Add(application);
                    db.SaveChanges();

                    return Json(new { success = true, msg = "成功提交申请", url = "/ClubApplication/Details?id=" + application.Id });
                }
                else
                {
                    return Json(new { success = false, msg = "没有做出任何修改，提交失败" });
                }
            }

            return Json(new { success = false, msg = "提交失败" });
        }

        [Authorize]
        public ActionResult ApplyUnregisterClub(int id)
        {
            var club = db.Clubs.Include(t => t.MajorInfo).Include(t => t.SubInfo).Include(t => t.Branches).Find(id);

            if (club != null)
            {
                return View(club);
            }
            else
            {
                return View("ClubNotFoundError");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult ApplyUnregisterClub(ClubUnregisterApplication application)
        {
            if (ModelState.IsValid)
            {
                int id = db.GenerateIdFor(IdentityForTPC.APPLICATION);

                application.ApplicantUserName = User.Identity.Name;
                application.Date = DateTime.Now;
                application.Status = Application.NOT_VERIFIED;
                application.Id = id;

                db.Applications.Add(application);
                db.SaveChanges();

                return Json(new { success = true, msg = "成功提交申请", url = "/ClubApplication/Details?id=" + application.Id });
            }

            return Json(new { success = false, msg = "提交失败" });
        }
    }
}
