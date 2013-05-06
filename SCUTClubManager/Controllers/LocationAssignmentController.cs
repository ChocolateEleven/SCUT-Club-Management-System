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
    [Authorize]
    public class LocationAssignmentController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /LocationAssignmnet/
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }


        public ViewResult List(int page_number = 1, string search = "", string search_option = "Club", string order = "Date")
        {
            List<KeyValuePair<string, string>> select_list = new List<KeyValuePair<string, string>>();
            select_list.Add(new KeyValuePair<string, string>("申请人", "Applicant"));
            select_list.Add(new KeyValuePair<string, string>("申请社团", "Club"));
            ViewBag.SearchOptions = new SelectList(select_list, "Value", "Key", "Applicant");
            ViewBag.Search = search;
            ViewBag.DateOrderOpt = order == "Date" ? "DateDesc" : "Date";

            var location_assignment = unitOfWork.LocationAssignments.ToList();

            if (!String.IsNullOrWhiteSpace(search))
            {
                switch (search_option)
                {
                    case "Applicant":
                        location_assignment = location_assignment.Where(s => s.Applicant.Name.Contains(search));
                        break;
                    case "Club":
                        location_assignment = location_assignment.Where(s => s.Club.MajorInfo.Name.Contains(search));
                        break;
                    default:
                        break;
                }

            }
            var list = QueryProcessor.Query(location_assignment, order_by: order, page_number: page_number, items_per_page: 2);
            return View(list);
        }

        //
        // GET: /LocationAssignmnet/Details/5

        public ViewResult Details(int id)
        {
            LocationAssignment locationassignment = unitOfWork.LocationAssignments.Find(id);
            return View(locationassignment);
        }

        //
        // GET: /LocationAssignmnet/Create

        public ActionResult Create()
        {
            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id");
            ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name");
            return View();
        } 

        //
        // POST: /LocationAssignmnet/Create

        [HttpPost]
        public ActionResult Create(LocationAssignment locationassignment)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.LocationAssignments.Add(locationassignment);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id", locationassignment.ClubId);
            ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", locationassignment.ApplicantUserName);
            return View(locationassignment);
        }
        
        //
        // GET: /LocationAssignmnet/Edit/5
 
        public ActionResult Edit(int id)
        {
            LocationAssignment locationassignment = unitOfWork.LocationAssignments.Find(id);
            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id", locationassignment.ClubId);
            ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", locationassignment.ApplicantUserName);
            return View(locationassignment);
        }

        //
        // POST: /LocationAssignmnet/Edit/5

        [HttpPost]
        public ActionResult Edit(LocationAssignment locationassignment)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.LocationAssignments.Update(locationassignment);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClubId = new SelectList(unitOfWork.Clubs.ToList(), "Id", "Id", locationassignment.ClubId);
            ViewBag.ApplicantUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", locationassignment.ApplicantUserName);
            return View(locationassignment);
        }

        //
        // GET: /LocationAssignmnet/Delete/5
 
        public ActionResult Delete(int id)
        {
            LocationAssignment locationassignment = unitOfWork.LocationAssignments.Find(id);
            return View(locationassignment);
        }

        //
        // POST: /LocationAssignmnet/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            LocationAssignment locationassignment = unitOfWork.LocationAssignments.Find(id);
            unitOfWork.LocationAssignments.Delete(locationassignment);
            unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}