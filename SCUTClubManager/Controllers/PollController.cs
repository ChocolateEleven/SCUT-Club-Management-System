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

        public ActionResult Index()
        {
            return RedirectToAction("List",new {page_number=1});
        }

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
        public ActionResult Create(Poll poll, string[] test)
        {
            //var ab = this.Request.Form[test0];
            ViewBag.AuthorUserName = User.Identity.Name;


            if (test == null || test.Length < 2)
            {
                ModelState.AddModelError("PollItems", "请至少设置两个选项");
            }

            if (poll.OpenDate != null && poll.CloseDate != null &&
                poll.OpenDate.CompareTo(poll.CloseDate) > 0)
            {
                ModelState.AddModelError("Date","投票结束时间不能早于投票开始时间，请检查后再提交");
            }

            for (int i = 0; i < test.Length; i++)
            {
                PollItem tempItem = new PollItem()
                {
                    Caption = test[i],
                    Count = 0,
                    Poll = poll
                };
                unitOfWork.PollItems.Add(tempItem);
                poll.Items.Add(tempItem);
            }

            if (ModelState.IsValid)
            {
                unitOfWork.Polls.Add(poll);
                unitOfWork.SaveChanges();

                return RedirectToAction("Index");

            }

            //return Json(false);
            return View(poll);
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