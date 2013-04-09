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

        public ViewResult List(int page_number = 1, string search = "", string search_option = "Title")
        {
            List<KeyValuePair<string, string>> select_list = new List<KeyValuePair<string,string>>();
            select_list.Add(new KeyValuePair<string,string>("标题", "Title"));
            select_list.Add(new KeyValuePair<string,string>("作者", "Author"));
            ViewBag.SearchOptions = new SelectList(select_list, "Value", "Key", "Title");
            // var polls = db.Polls.Include(p => p.Author);
            var polls = unitOfWork.Polls.ToList();

            if (!String.IsNullOrEmpty(search))
            {
                switch (search_option)
                {
                    case "Title":
                        polls = polls.Where(s => s.Title.Contains(search));
                        break;
                    case "Author":
                        polls = polls.Where(s => (s.Author is Student && (s.Author as Student).Name.Contains(search)) || (search == "社联" && !(s.Author is Student)));
                        break;
                    default:
                        break;
                }
            }

            var list = QueryProcessor.Query(polls, order_by: "Title", page_number: page_number, items_per_page: 2);
            return View(list);
        }

        //
        // GET: /Poll/Details/5

        public ViewResult Details(int id)
        {
            Poll poll = unitOfWork.Polls.Find(id);
            return View(poll);
        }


        [HttpPost]
        public ActionResult Details(string[] selectItem)
        {

            return View();
        }

        //
        // GET: /Poll/Create

        public ActionResult Create()
        {
            return View();
        }


        //
        // POST: /Poll/Create

        [HttpPost]
        public ActionResult Create(Poll poll, string[] test)
        {
            //var ab = this.Request.Form[test0];
            ViewBag.AuthorUserName = User.Identity.Name;


            

            if (poll.OpenDate != null && poll.CloseDate != null &&
                poll.OpenDate.CompareTo(poll.CloseDate) > 0)
            {
                ModelState.AddModelError("Date","投票结束时间不能早于投票开始时间，请检查后再提交");
            }

            if (test == null || test.Length < 2)
            {
                ModelState.AddModelError("PollItems", "请至少设置两个选项");
            }
            if (test != null)
            {
                if (poll.Items != null)
                {
                    poll.Items.Clear();
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
            }
            poll.AuthorUserName = User.Identity.Name;

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