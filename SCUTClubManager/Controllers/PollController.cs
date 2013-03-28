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
    public class PollController : Controller
    {
        private SCUTClubContext db = new SCUTClubContext();
       // private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /Poll/

        public ViewResult Index()
        {
            var polls = db.Polls.Include(p => p.Author);
           // var polls = unitOfWork.Polls;
            return View(polls.ToList());
        }

        //
        // GET: /Poll/Details/5

        public ViewResult Details(int id)
        {
            Poll poll = db.Polls.Find(id);
            return View(poll);
        }

        //
        // GET: /Poll/Create

        public ActionResult Create()
        {
            ViewBag.AuthorUserName = new SelectList(db.Users, "UserName", "Password");
            return View();
        } 

        //
        // POST: /Poll/Create

        [HttpPost]
        public ActionResult Create(Poll poll)
        {
            if (ModelState.IsValid)
            {
                db.Polls.Add(poll);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.AuthorUserName = new SelectList(db.Users, "UserName", "Password", poll.AuthorUserName);
            return View(poll);
        }
        
        //
        // GET: /Poll/Edit/5
 
        public ActionResult Edit(int id)
        {
            Poll poll = db.Polls.Find(id);
            ViewBag.AuthorUserName = new SelectList(db.Users, "UserName", "Password", poll.AuthorUserName);
            return View(poll);
        }

        //
        // POST: /Poll/Edit/5

        [HttpPost]
        public ActionResult Edit(Poll poll)
        {
            if (ModelState.IsValid)
            {
                db.Entry(poll).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorUserName = new SelectList(db.Users, "UserName", "Password", poll.AuthorUserName);
            return View(poll);
        }

        //
        // GET: /Poll/Delete/5
 
        public ActionResult Delete(int id)
        {
            Poll poll = db.Polls.Find(id);
            return View(poll);
        }

        //
        // POST: /Poll/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Poll poll = db.Polls.Find(id);
            db.Polls.Remove(poll);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}