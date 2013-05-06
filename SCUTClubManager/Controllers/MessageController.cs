using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SCUTClubManager.Models;
using SCUTClubManager.DAL;
using SCUTClubManager.BusinessLogic;

namespace SCUTClubManager.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        //private SCUTClubContext db = new SCUTClubContext();
        private UnitOfWork unitOfWork = new UnitOfWork();


        //
        // GET: /Message/

        public ViewResult Index(int page_number = 1, string search = "", string search_option = "Title", string order = "Date")
        {
           // var messages = db.Messages.Include(m => m.Sender).Include(m => m.Receiver);

            List<KeyValuePair<string, string>> select_list = new List<KeyValuePair<string, string>>();
            select_list.Add(new KeyValuePair<string, string>("标题", "Title"));
            select_list.Add(new KeyValuePair<string, string>("标题", "SenderUserName"));
            ViewBag.SearchOptions = new SelectList(select_list, "Value", "Key", "Title");
            ViewBag.Search = search;
            ViewBag.DateOrderOpt = order == "Date" ? "DateDesc" : "Date";

            var messages_list = unitOfWork.Messages.ToList();

            if (!String.IsNullOrWhiteSpace(search))
            {
                switch (search_option)
                {
                    case "SenderUserName":
                        messages_list = messages_list.Where(s => s.Sender.Name.Contains(search));
                        break;
                    case "Title":
                        messages_list = messages_list.Where(s => s.Title.Contains(search));
                        break;
                    default:
                        break;
                }

            }
            var list = QueryProcessor.Query(messages_list, order_by: order, page_number: page_number, items_per_page: 2);
           

            return View(list);
        }

        //[HttpPost]
        //public ActionResult Index(string receivers)
        //{
        //    return View();
        //}

        //
        // GET: /Message/Details/5

        public ViewResult Details(int id)
        {
            //Message message = db.Messages.Find(id);
            Message message = unitOfWork.Messages.Find(id);
            message.ReadMark = true;
            ViewBag.sender = message.Sender;
            ViewBag.receiver = message.Receiver;
            return View(message);
        }

        //
        // GET: /Message/Create

        public ActionResult Create()
        {
            //ViewBag.SenderId = new SelectList(db.Users, "UserName", "Password");
            //ViewBag.ReceiverId = new SelectList(db.Users, "UserName", "Password");

           // ViewBag.SenderId = new SelectList(unitOfWork.Users.ToList(), "UserName", "UserName");

           // var sender = QueryProcessor.Query<User>(collection: unitOfWork.Users.ToList());
            ViewBag.SenderId = new SelectList(unitOfWork.Users.ToList(), "UserName", "UserName");

            //var receiver = QueryProcessor.Query<User>(collection: unitOfWork.Users.ToList());
            ViewBag.ReceiverId = new SelectList(unitOfWork.Users.ToList(), "UserName", "UserName");
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

                message.Sender = unitOfWork.Users.ToList().Where(t => t.UserName == User.Identity.Name).Single();
                message.Receiver = unitOfWork.Users.Find(message.ReceiverId);
                message.Date = DateTime.Now;
                message.ReadMark = false;
                unitOfWork.Messages.Add(message);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.SenderId = new SelectList(unitOfWork.Users.ToList(), "UserName", "UserName", message.SenderId);
            ViewBag.ReceiverId = new SelectList(unitOfWork.Users.ToList(), "UserName", "UserName", message.ReceiverId);
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
 
        //public ActionResult Delete(int id)
        //{
        //    Message message = unitOfWork.Messages.Find(id);
        //    return View(message);
        //}

        //
        // POST: /Message/Delete/5

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Message message = unitOfWork.Messages.Find(id);
            unitOfWork.Messages.Delete(message);
            unitOfWork.SaveChanges();
            return Json(new { idToDelete = id, success = true });
            //return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}