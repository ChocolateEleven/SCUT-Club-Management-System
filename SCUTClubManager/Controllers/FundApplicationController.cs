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

namespace SCUTClubManager.Controllers
{ 
    public class FundApplicationController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ViewResult List(int page_number = 1, string search = "", string search_option = "Club", string order = "Date")
        {
            List<KeyValuePair<string, string>> select_list = new List<KeyValuePair<string, string>>();
            select_list.Add(new KeyValuePair<string, string>("申请社团", "Club"));
            select_list.Add(new KeyValuePair<string, string>("申请人", "Applicant"));
            ViewBag.SearchOptions = new SelectList(select_list, "Value", "Key", "Title");
            ViewBag.Search = search;
            ViewBag.CurrentOrder = order;
            ViewBag.DateOrderOpt = order == "Date" ? "DateDesc" : "Date";

            var fundApplications = unitOfWork.FundApplications.ToList();

            if (!String.IsNullOrWhiteSpace(search))
            {
                switch (search_option)
                {
                    case "Club":
                        fundApplications = fundApplications.Where(s => s.Club.MajorInfo.Name.Contains(search));
                        break;
                    case "Applicant":
                        fundApplications = fundApplications.Where(s => (s.Applicant is Student && (s.Applicant as Student).Name.Contains(search)) || (search == "社联" && !(s.Applicant is Student)));
                        break;
                    default:
                        break;
                }
            }

            var list = QueryProcessor.Query(fundApplications, order_by: order, page_number: page_number, items_per_page: 2);
          
            return View(list);
        }

        //
        // GET: /FundApplication/Details/5

        public ViewResult Details(int id)
        {
            FundApplication fundapplication = unitOfWork.Applications.Find(id) as FundApplication;
            return View(fundapplication);
        }

        //
        // GET: /FundApplication/Create

        public ActionResult Create()
        {
            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id");
             ViewBag.Id = new SelectList(unitOfWork.ApplicationRejectReasons.ToList(), "ApplicationId", "Reason");
            return View();
        } 

        //
        // POST: /FundApplication/Create

        [HttpPost]
        public ActionResult Create(FundApplication fundapplication)
        {
            if (ModelState.IsValid)
            {
                fundapplication.Status = "n";
                fundapplication.Date = DateTime.Today;
                fundapplication.ApplicantUserName = User.Identity.Name;
                unitOfWork.Applications.Add(fundapplication);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id", fundapplication.ClubId);
            ViewBag.Id = new SelectList(unitOfWork.ApplicationRejectReasons.ToList(), "ApplicationId", "Reason", fundapplication.Id);
            return View(fundapplication);
        }
        
        //
        // GET: /FundApplication/Edit/5
 
        public ActionResult Edit(int id)
        {
            FundApplication fundapplication = unitOfWork.Applications.Find(id) as FundApplication;
            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id", fundapplication.ClubId);
            ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", fundapplication.ApplicantUserName);
            ViewBag.Id = new SelectList(unitOfWork.ApplicationRejectReasons.ToList(), "ApplicationId", "Reason", fundapplication.Id);
            return View(fundapplication);
        }

        //
        // POST: /FundApplication/Edit/5

        [HttpPost]
        public ActionResult Edit(FundApplication fundapplication)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.FundApplications.Update(fundapplication);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id", fundapplication.ClubId);
            ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", fundapplication.ApplicantUserName);
            ViewBag.Id = new SelectList(unitOfWork.ApplicationRejectReasons.ToList(), "ApplicationId", "Reason", fundapplication.Id);
            return View(fundapplication);
        }

        //
        // GET: /FundApplication/Delete/5
 
        //public ActionResult Delete(int id)
        //{
        //    FundApplication fundapplication = unitOfWork.Applications.Find(id) as FundApplication;
        //    return View(fundapplication);
        //}

        //
        // POST: /FundApplication/Delete/5

        public ActionResult Delete(int id)
        {
            FundApplication fund_application = unitOfWork.FundApplications.Find(id) as FundApplication;
            unitOfWork.Applications.Delete(fund_application);
            unitOfWork.SaveChanges();
            return Json(new { idToDelete = id, success = true }); 
        }


        public ActionResult Verify(int id, bool is_passed, string reject_reason)
        {
            var fund_application = unitOfWork.FundApplications.Find(id);
            if (is_passed)
            {
                fund_application.Status = "p";
                unitOfWork.SaveChanges();
                return RedirectToAction("Add", "Fund", new { application_id = fund_application.Id });
            }
            fund_application.Status = "f";
            fund_application.RejectReason = new ApplicationRejectReason { Reason = reject_reason };
            unitOfWork.SaveChanges();
            return RedirectToAction("List");
        }


        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}