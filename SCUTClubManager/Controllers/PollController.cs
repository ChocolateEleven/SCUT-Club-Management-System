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
    public class PollController : Controller
    {
        //private SCUTClubContext db = new SCUTClubContext();
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /Poll/

        public ViewResult List(int page_number)
        {
            // var polls = db.Polls.Include(p => p.Author);
            var polls = QueryProcessor.Query(unitOfWork.Polls.ToList(), order_by: "Title", page_number: page_number, items_per_page: 2);
            return View(polls);
        }

        //
        // GET: /Poll/Details/5

        public ViewResult Details(int id)
        {
            Poll poll = unitOfWork.Polls.Find(id);
            return View(poll);
        }

        //
        // GET: /Poll/Create

        public ActionResult Create()
        {
            ViewBag.AuthorUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "UserName");

            return View();
        } 

        //
        // POST: /Poll/Create

        [HttpPost]
        public ActionResult Create(string[] items,Poll poll)
        {
            //if (ModelState.IsValid)
            //{
            //    unitOfWork.Polls.Add(poll);
            //    unitOfWork.SaveChanges();
            //    return Json("1");
            //    //return RedirectToAction("Index");  
            //}

            //var i = ViewBag.items;

            //ViewBag.AuthorUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Password", poll.AuthorUserName);
            //return View(poll);
            return View();
        }
        
        //
        // GET: /Poll/Edit/5
 
        public ActionResult Edit(int id)
        {
            Poll poll = unitOfWork.Polls.Find(id);
            ViewBag.AuthorUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Password", poll.AuthorUserName);
            return View(poll);
        }

        //
        // POST: /Poll/Edit/5

        [HttpPost]
        public ActionResult Edit(Poll poll)
        {
            if (ModelState.IsValid)
            {
               // db.Entry(poll).State = EntityState.Modified;
               // db.SaveChanges();
                unitOfWork.Polls.Update(poll);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorUserName = new SelectList(unitOfWork.Users.ToList(), "UserName", "Password", poll.AuthorUserName);
            return View(poll);
        }

        //
        // GET: /Poll/Delete/5
 
        public ActionResult Delete(int id)
        {
            Poll poll = unitOfWork.Polls.Find(id);
            return View(poll);
        }

        //
        // POST: /Poll/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Poll poll = unitOfWork.Polls.Find(id);
            unitOfWork.Polls.Delete(poll);
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