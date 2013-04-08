using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using SCUTClubManager.Models;
using SCUTClubManager.DAL;
using SCUTClubManager.BusinessLogic;

namespace SCUTClubManager.Controllers
{
    public class ClubApplicationController : Controller
    {
        UnitOfWork db = new UnitOfWork();

        //
        // GET: /ClubApplication/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult List(int club_id = 0, int page_number = 1, string order = "Date", string pass_filter = "", string search = "", string search_option = "", string type_filter = "")
        {
            if (!User.IsInRole("社联") && !ScmRoleProvider.HasMembershipIn(club_id))
            {
                // TODO: 将用户重定向到另一个页面。
                return RedirectToAction("Index", "Home");
            }

            IQueryable<Application> applications = db.Applications.Include(s => s.Club).Include(s => s.Club.MajorInfo).Include(s => s.Applicant).ToList() as IQueryable<Application>;

            ViewBag.ClubId = club_id;
            ViewBag.CurrentOrder = order;
            ViewBag.DateOrderOpt = order == "Date" ? "DateDesc" : "Date";
            ViewBag.Search = search;
            ViewBag.PassFilter = pass_filter;
            ViewBag.SearchOption = search_option;
            ViewBag.TypeFilter = type_filter;

            Expression<Func<Application, bool>> filter = null;
            if (!String.IsNullOrWhiteSpace(search) && !String.IsNullOrWhiteSpace(search_option))
            {
                switch (search_option)
                {
                    case "ClubName":
                        filter = s => s.Club.MajorInfo.Name.Contains(search);
                        break;
                    case "ApplicantName":
                        filter = s => s.Applicant.FullName.Contains(search);
                        break;
                }
            }

            applications = QueryProcessor.FilterApplication(applications, pass_filter, type_filter);

            var club_list = QueryProcessor.Query<Application>(applications, filter: filter,
                order_by: order, page_number: page_number, items_per_page: 20);          

            return View(club_list);
        }

        //
        // GET: /ClubApplication/Details/5

        [Authorize]
        public ActionResult Details(int id)
        {
            return View(db.Applications.Find(id));
        }

        //
        // GET: /ClubApplication/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /ClubApplication/Create

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
        // GET: /ClubApplication/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ClubApplication/Edit/5

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
        // GET: /ClubApplication/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ClubApplication/Delete/5

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
    }
}
