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
    public class ForumController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /Forum/
        public ActionResult Index()
        {
            return RedirectToAction("List", new { page_number = 1 });
        }

        public ViewResult List(int page_number = 1, string search = "", string search_option = "Title", string order = "LatestReplyDateDesc")
        {

            List<KeyValuePair<string, string>> select_list = new List<KeyValuePair<string, string>>();
            select_list.Add(new KeyValuePair<string, string>("标题", "Title"));
            select_list.Add(new KeyValuePair<string, string>("作者", "Author"));
            ViewBag.SearchOptions = new SelectList(select_list, "Value", "Key", "Title");
            ViewBag.Search = search;
            ViewBag.CurrentOrder = order;
            ViewBag.PostDateOrderOpt = order == "PostDate" ? "PostDateDesc" : "PostDate";
            ViewBag.LatestReplyDateOrderOpt = order == "LatestReplyDate" ? "LatestReplyDateDesc" : "LatestReplyDate";
        

            var threads = unitOfWork.Threads.ToList();

            if (!String.IsNullOrWhiteSpace(search))
            {
                switch (search_option)
                {
                    case "Title":
                        threads = threads.Where(s => s.Title.Contains(search));
                        break;
                    case "Author":
                        threads = threads.Where(s => (s.Author is Student && (s.Author as Student).Name.Contains(search)) || (search == "社联" && !(s.Author is Student)));
                        break;
                    default:
                        break;
                }
            }

            var list = QueryProcessor.Query(threads, page_number: page_number, items_per_page: 10, order_by: order);
            return View(list);
        }

        //
        // GET: /Forum/Details/5

        public ViewResult Details(int id)
        {
            Thread thread = unitOfWork.Threads.Find(id);
            return View(thread);
        }

        [HttpPost]
        public ActionResult Details(int id, string reply)
        {
            Thread thread = unitOfWork.Threads.Find(id);

            if (String.IsNullOrWhiteSpace(reply))
            {
                ModelState.AddModelError("replyError", "请输入内容");
                return Json(new { success = false, msg = "请输入内容" });
            }
            if (ModelState.IsValid)
            {
                Reply tempReply = new Reply
                {
                    Content = reply,
                    AuthorUserName = User.Identity.Name,
                    Date = DateTime.Now,
                    Number = thread.Replies.Count+1,
                    Thread = thread
                };
                unitOfWork.Replies.Add(tempReply);
                thread.Replies.Add(tempReply);
                unitOfWork.SaveChanges();
            }
            return Json(new { success = true, msg = "" });
        }

        //
        // GET: /Forum/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Forum/Create

        [HttpPost]
        public ActionResult Create(Thread thread,string threadContent)
        {
            if (String.IsNullOrWhiteSpace(threadContent))
            {
                ModelState.AddModelError("threadContent","请输入内容");
            }
            if (ModelState.IsValid)
            {
                thread.PostDate = DateTime.Now;
                thread.LatestReplyDate = DateTime.Now;
                thread.AuthorUserName = User.Identity.Name;
                Reply tempReply = new Reply{ 
                        Thread = thread,
                        Number = 1,
                        Date = DateTime.Now,
                        AuthorUserName = User.Identity.Name,
                        Content = threadContent};
                unitOfWork.Replies.Add(tempReply);
                thread.Replies.Add(tempReply);
                unitOfWork.Threads.Add(thread);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }

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

        //public ActionResult Delete(int id)
        //{
        //    Thread thread = unitOfWork.Threads.Find(id);
        //    return View(thread);
        //}

        
        // POST: /Forum/Delete/5

        [HttpPost]
        public ActionResult Delete(int id) 
        {             
            Thread thread = unitOfWork.Threads.Find(id);
            unitOfWork.Threads.Delete(thread);
            unitOfWork.SaveChanges();
            //return RedirectToAction("Index");
            return Json(new { idToDelete = id, success = true });
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}