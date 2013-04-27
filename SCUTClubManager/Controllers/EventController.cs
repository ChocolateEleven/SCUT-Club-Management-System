﻿using System;
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

        public ActionResult Scoring(int page_number = 1, string search = "", string search_option = "EventName")
        {
            return null;
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

        private IEnumerable<Event> FilterEvents(IEnumerable<Event> collection, int page_number = 1,
            string order = "Title", string search = "", string search_option = "Title", string pass_filter = Application.NOT_VERIFIED)
        {
            return null;
        }

    }
}