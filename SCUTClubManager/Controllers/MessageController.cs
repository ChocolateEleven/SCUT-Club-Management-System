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
    public class MessageController : Controller
    {
        //private SCUTClubContext db = new SCUTClubContext();
        private UnitOfWork unitOfWork = new UnitOfWork();


        //
        // GET: /Message/

        public ViewResult Index()
        {
           // var messages = db.Messages.Include(m => m.Sender).Include(m => m.Receiver);
            var messages = unitOfWork.Messages;
            var list = messages.ToList();
            
             
            return View(messages.ToList());
        }

        //
        // GET: /Message/Details/5

        public ViewResult Details(int id)
        {
            //Message message = db.Messages.Find(id);
            Message message = unitOfWork.Messages.Find(id);
            return View(message);
        }

        //
        // GET: /Message/Create

        public ActionResult Create()
        {
            //ViewBag.SenderId = new SelectList(db.Users, "UserName", "Password");
            //ViewBag.ReceiverId = new SelectList(db.Users, "UserName", "Password");

            ViewBag.SenderId = new SelectList(unitOfWork.Users.ToList(), "UserName", "Password");
            ViewBag.ReceiverId = new SelectList(unitOfWork.Users.ToList(), "UserName", "Password");
            return View();
        } 

        //
        // POST: /Message/Create

        [HttpPost]
        public ActionResult Create(Message message)
        {
            if (ModelState.IsValid)
            {
               // db.Messages.Add(message);
               // db.SaveChanges();
                unitOfWork.Messages.Add(message);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.SenderId = new SelectList(unitOfWork.Users.ToList(), "UserName", "Password", message.SenderId);
            ViewBag.ReceiverId = new SelectList(unitOfWork.Users.ToList(), "UserName", "Password", message.ReceiverId);
            return View(message);
        }
        
        //
        // GET: /Message/Edit/5
 
        public ActionResult Edit(int id)
        {
            Message message = unitOfWork.Messages.Find(id);
            ViewBag.SenderId = new SelectList(unitOfWork.Users.ToList(), "UserName", "Password", message.SenderId);
            ViewBag.ReceiverId = new SelectList(unitOfWork.Users.ToList(), "UserName", "Password", message.ReceiverId);
            return View(message);
        }

        //
        // POST: /Message/Edit/5

        [HttpPost]
        public ActionResult Edit(Message message)
        {
            if (ModelState.IsValid)
            {
               // db.Entry(message).State = EntityState.Modified;
               // db.SaveChanges();

                unitOfWork.Messages.Update(message);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SenderId = new SelectList(unitOfWork.Users.ToList(), "UserName", "Password", message.SenderId);
            ViewBag.ReceiverId = new SelectList(unitOfWork.Users.ToList(), "UserName", "Password", message.ReceiverId);
            return View(message);
        }

        //
        // GET: /Message/Delete/5
 
        public ActionResult Delete(int id)
        {
            Message message = unitOfWork.Messages.Find(id);
            return View(message);
        }

        //
        // POST: /Message/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = unitOfWork.Messages.Find(id);
            unitOfWork.Messages.Delete(message);
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