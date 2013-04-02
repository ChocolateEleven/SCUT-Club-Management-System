﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using SCUTClubManager.Models;
using SCUTClubManager.DAL;
using SCUTClubManager.BusinessLogic;

namespace SCUTClubManager.Controllers
{
    public class ClubApplicationController : Controller
    {
        UnitOfWork db = new UnitOfWork();

        //
        // GET: /ClubApplication/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult List(int page_number, string pass_filter, string search, string search_option, string type_filter)
        {
            var applications = db.Applications.ToList();

            switch (type_filter)
            {
                case "Register":
                    applications.Where(t => t is ClubRegisterApplication);
                    break;

                case "Unregister":
                    applications.Where(t => t is ClubUnregisterApplication);
                    break;

                case "InfoModify":
                    applications.Where(t => t is ClubInfoModificationApplication);
                    break;

                default:
                    applications.Where(t => t is ClubRegisterApplication || t is ClubUnregisterApplication || t is ClubInfoModificationApplication);
                    break;
            }
            
            ViewBag.Search = search;
            ViewBag.PassFilter = pass_filter;
            ViewBag.SearchOption = search_option;

            Expression<Func<Application, bool>> filter = null;
            if (!String.IsNullOrWhiteSpace(search) && !String.IsNullOrWhiteSpace(search_option))
            {
                switch (search_option)
                {
                    case "ClubName":
                        filter = s => s.Club.MajorInfo.Name.Contains(search);
                        break;
                    case "ApplicantName":
                        filter = s => s.Applicant.FullName.Contains(search);
                        break;
                }
            }

            if (!String.IsNullOrWhiteSpace(pass_filter))
            {
                if (pass_filter == "Passed")
                {
                    applications = applications.Where(s => s.Status == "p");
                }
                else if (pass_filter == "Failed")
                {
                    applications = applications.Where(s => s.Status == "f");
                }
                else if (pass_filter == "NotVerified")
                {
                    applications = applications.Where(s => s.Status == "n");
                }
            }

            var club_list = QueryProcessor.Query<Application>(applications, filter: filter,
                order_by: s => s.OrderBy(r => r.Date), page_number: page_number, items_per_page: 20);

            return View(club_list);
        }

        //
        // GET: /ClubApplication/Details/5

        [Authorize]
        public ActionResult Details(int id)
        {
            return View(db.Applications.Find(id));
        }

        //
        // GET: /ClubApplication/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /ClubApplication/Create

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
        // GET: /ClubApplication/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ClubApplication/Edit/5

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
        // GET: /ClubApplication/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ClubApplication/Delete/5

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
    }
}