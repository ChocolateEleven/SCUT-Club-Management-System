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
    public class FundController : Controller
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
            select_list.Add(new KeyValuePair<string, string>("负责人", "Applicant"));
            ViewBag.SearchOptions = new SelectList(select_list, "Value", "Key", "Title");
            ViewBag.Search = search; 
            ViewBag.CurrentOrder = order;
            ViewBag.DateOrderOpt = order == "Date" ? "DateDesc" : "Date";

            var fund_details = unitOfWork.FundDetailses.ToList();

            if (!String.IsNullOrWhiteSpace(search))
            {
                switch (search_option)
                {
                    case "Club":
                        fund_details = fund_details.Where(s => s.Club.MajorInfo.Name.Contains(search));
                        break;
                    case "Applicant":
                        fund_details = fund_details.Where(s => (s.Applicant is Student && (s.Applicant as Student).Name.Contains(search)) || (search == "社联" && !(s.Applicant is Student)));
                        break;
                    default:
                        break;
                }
            }

            var list = QueryProcessor.Query(fund_details, order_by: order, page_number: page_number, items_per_page: 2);

            return View(list);
        }
        //
        // GET: /Fund/Details/5

        public ViewResult Details(int id)
        {
            FundDetails funddetails = unitOfWork.FundDetailses.Find(id);
            return View(funddetails);
        }

        //
        // GET: /Fund/Create

        //public ActionResult Create()
        //{
        //    ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name");
        //    ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id");
        //    return View();
        //} 

        //
        // POST: /Fund/Create

        public ActionResult Add(int application_id)
        {
            FundApplication application = unitOfWork.FundApplications.Find(application_id);
            FundDetails details = new FundDetails { 
                Date = DateTime.Today,
                Applicant = application.Applicant,
                Quantity = application.Quantity, 
                Purpose = application.Purpose,
                Club = application.Club
            };
            unitOfWork.FundDetailses.Add(details);
            unitOfWork.SaveChanges();
            return RedirectToAction("List", "FundApplication");
        }

        public ActionResult SetRejectReason(int application_id)
        {
            ViewBag.application_id = application_id;
            ViewBag.Reason = "";
            return View();
        }

        [HttpPost]
        [ActionName("SetRejectReason")]
        public ActionResult SetRejectReason2(ApplicationRejectReason Reason, int application_id)
        {
            if (Reason != null)
            { 
                FundApplication fund_application = unitOfWork.FundApplications.Find(application_id);
                fund_application.RejectReason = Reason;
                unitOfWork.FundApplications.Update(fund_application);
                unitOfWork.SaveChanges();
            }
            return RedirectToAction("List", "FundApplication");
        }


        //
        // GET: /Fund/Edit/5
 
        public ActionResult Edit(int id)
        {
            FundDetails funddetails = unitOfWork.FundDetailses.Find(id);
            ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", funddetails.ApplicantUserName);
            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id", funddetails.ClubId);
            return View(funddetails);
        }

        //
        // POST: /Fund/Edit/5

        [HttpPost]
        public ActionResult Edit(FundDetails funddetails)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.FundDetailses.Update(funddetails);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", funddetails.ApplicantUserName);
            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id", funddetails.ClubId);
            return View(funddetails);
        }

        //
        // GET: /Fund/Delete/5
 
        //public ActionResult Delete(int id)
        //{
        //    FundDetails funddetails = unitOfWork.FundDetailses.Find(id);
        //    return View(funddetails);
        //}

        //
        // POST: /Fund/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{            
        //    FundDetails funddetails = unitOfWork.FundDetailses.Find(id);
        //    unitOfWork.FundDetailses.Delete(funddetails);
        //    unitOfWork.SaveChanges();
        //    return RedirectToAction("Index");
        //}


        [HttpPost]
        public ActionResult Delete(int id)
        {
            FundDetails fund = unitOfWork.FundDetailses.Find(id) as FundDetails;
            unitOfWork.FundDetailses.Delete(fund);
            unitOfWork.SaveChanges();
            return Json(new { idToDelete = id, success = true }); 
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}