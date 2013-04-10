using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCUTClubManager.Models;
using SCUTClubManager.DAL;

namespace SCUTClubManager.Controllers
{ 
    public class ForumController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /Forum/

        public ViewResult Index()
        {
            var threads = unitOfWork.Threads.ToList();
            return View(threads.ToList());
        }

        //
        // GET: /Forum/Details/5

        public ViewResult Details(int id)
        {
            Thread thread = unitOfWork.Threads.Find(id);
            return View(thread);
        }

        //
        // GET: /Forum/Create

        public ActionResult Create()
        {
            ViewBag.AuthorUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name");
            return View();
        } 

        //
        // POST: /Forum/Create

        [HttpPost]
        public ActionResult Create(Thread thread)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Threads.Add(thread);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.AuthorUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", thread.AuthorUserName);
            return View(thread);
        }
        
        //
        // GET: /Forum/Edit/5
 
        public ActionResult Edit(int id)
        {
            Thread thread = unitOfWork.Threads.Find(id);
            ViewBag.AuthorUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", thread.AuthorUserName);
            return View(thread);
        }

        //
        // POST: /Forum/Edit/5

        [HttpPost]
        public ActionResult Edit(Thread thread)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Threads.Update(thread);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Name", thread.AuthorUserName);
            return View(thread);
        }

        //
        // GET: /Forum/Delete/5
 
        public ActionResult Delete(int id)
        {
            Thread thread = unitOfWork.Threads.Find(id);
            return View(thread);
        }

        //
        // POST: /Forum/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Thread thread = unitOfWork.Threads.Find(id);
            unitOfWork.Threads.Delete(thread);
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