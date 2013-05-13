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
    [Authorize]
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

        public ViewResult List(int page_number = 1, string search = "", string search_option = "Title", string order = "OpenDate")
        {
            List<KeyValuePair<string, string>> select_list = new List<KeyValuePair<string,string>>();
            select_list.Add(new KeyValuePair<string,string>("标题", "Title"));
            select_list.Add(new KeyValuePair<string,string>("作者", "Author"));
            ViewBag.SearchOptions = new SelectList(select_list, "Value", "Key", "Title");
            ViewBag.Search = search;
            ViewBag.CurrentOrder = order;
            ViewBag.OpenDateOrderOpt = order == "OpenDate" ? "OpenDateDesc" : "OpenDate";
            ViewBag.CloseDateOrderOpt = order == "CloseDate" ? "CloseDateDesc" : "CloseDate";
            ViewBag.TitleOrderOpt = order == "Title" ? "TitleDesc" : "Title";
            // var polls = db.Polls.Include(p => p.Author);
            var polls = unitOfWork.Polls.ToList();

            if (!String.IsNullOrWhiteSpace(search))
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

            var list = QueryProcessor.Query(polls, order_by: order, page_number: page_number, items_per_page: 10);
            return View(list);
        }


        public ViewResult Details(int id)
        {
            Poll poll = unitOfWork.Polls.Find(id);
            return View(poll);
        }

        //
        // GET: /Poll/Details/5

        public ActionResult Vote(int id)
        {
            var user_polls = unitOfWork.UserPolls.ToList();
            user_polls = from s in user_polls
                         where s.PollId == id && s.UserName == User.Identity.Name
                             select s;

            int count = user_polls.Count();

            var poll = unitOfWork.Polls.Find(id);

            if (user_polls != null && user_polls.Count() != 0 || poll.CloseDate.CompareTo(DateTime.Now) <= 0)
            {
                return RedirectToAction("Details", new { id = id } );
            }

            return View(poll);
        }


        [HttpPost]
        public ActionResult Vote(int id, int[] selectItem)
        {
            Poll poll = unitOfWork.Polls.Find(id);

            var user_polls = unitOfWork.UserPolls.ToList();
            user_polls = from s in user_polls
                         where s.PollId == id && s.UserName == User.Identity.Name
                         select s;

            int count = user_polls.Count();

            if (user_polls != null && user_polls.Count() != 0)
            {
                return Json(new { success = false, msg = "不能重复投票" });
            }

            if (poll.CloseDate.CompareTo(DateTime.Now) <= 0)
            {
                return Json(new { success = false, msg = "投票已结束" });
            }

            if (selectItem != null)
            {
                foreach (int itemId in selectItem)
                {
                    unitOfWork.PollItems.Find(itemId).Count++;
                }
                unitOfWork.UserPolls.Add(new UserPoll
                {
                    UserName = User.Identity.Name,
                    PollId = id
                });
                unitOfWork.SaveChanges();
                return Json(new { success = true, msg = "投票成功" });
            }

            return Json(new { success = false, msg = "请选择后再提交" });
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
        public ActionResult Create(Poll poll, string[] item)
        {
            //var ab = this.Request.Form[test0];
            ViewBag.AuthorUserName = User.Identity.Name;


            

            if (poll.OpenDate != null && poll.CloseDate != null &&
                poll.OpenDate.CompareTo(poll.CloseDate) > 0)
            {
                ModelState.AddModelError("Date","投票结束时间不能早于投票开始时间，请检查后再提交");
            }

            if (item == null || item.Length < 2)
            {
                ModelState.AddModelError("PollItemNumber", "请至少设置两个选项");
            }
            if (item != null)
            {
                if (poll.Items != null)
                {
                    poll.Items.Clear();
                }
                for (int i = 0; i < item.Length; i++)
                {
                    if (String.IsNullOrWhiteSpace(item[i]))
                    {
                        ModelState.AddModelError("PollItem" + i, "请输入内容");
                        item[i] = " ";
                    } 
                    PollItem tempItem = new PollItem()
                    {
                        Id = i,
                        Caption = item[i],
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
 
        //public ActionResult Delete(int id)
        //{
        //    Poll poll = unitOfWork.Polls.Find(id);
        //    return View(poll);
        //}

        //
        // POST: /Poll/Delete/5

        [HttpPost]
        public ActionResult Delete(int id) 
        {               
            Poll poll = unitOfWork.Polls.Find(id);
            unitOfWork.Polls.Delete(poll);
            unitOfWork.SaveChanges();
            return Json(new { idToDelete = id, success = true });
        }

        protected override void Dispose(bool disposing)
        { 
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}