using lab_task.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace lab_task.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        [HttpGet]
        public ActionResult List()
        {
            Lab_TaskEntities db = new Lab_TaskEntities();
            var data = db.News.ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult List(String CategorySearch, String DateSearch)
        {
            Lab_TaskEntities db = new Lab_TaskEntities();
            var data = db.News.ToList();

            if (!String.IsNullOrEmpty(CategorySearch) && !String.IsNullOrEmpty(DateSearch))
            {
                //Lab_TaskEntities db = new Lab_TaskEntities();
                data = (from u in db.News where ( u.Category.Contains(CategorySearch) && u.PublishDate.Contains(DateSearch) ) select u).ToList();

            }

            else if (!String.IsNullOrEmpty(CategorySearch))
            {
                //Lab_TaskEntities db = new Lab_TaskEntities();
                data = (from u in db.News where u.Category.Contains(CategorySearch) select u).ToList();

            }

            else if (!String.IsNullOrEmpty(DateSearch))
            {
                //Lab_TaskEntities db = new Lab_TaskEntities();
                data = (from u in db.News where (u.PublishDate.Contains(DateSearch)) select u).ToList();
            }

            else if (String.IsNullOrEmpty(CategorySearch) && String.IsNullOrEmpty(DateSearch))
            {
                ViewBag.error = "Please search something!!";
            }

            if (data.Count() == 0)
            {
                ViewBag.error = "Nothing match";
            }
            return View(data);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(News b)
        {
            if (ModelState.IsValid)
            {
                Lab_TaskEntities db = new Lab_TaskEntities();
                db.News.Add(b);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(b);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Lab_TaskEntities db = new Lab_TaskEntities();
            var data = (from u in db.News where u.Id == id select u).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(News edit_news)
        {
            if (ModelState.IsValid)
            {
                Lab_TaskEntities db = new Lab_TaskEntities();
                var data = (from u in db.News where u.Id == edit_news.Id select u).FirstOrDefault();
                db.Entry(data).CurrentValues.SetValues(edit_news);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            Lab_TaskEntities db = new Lab_TaskEntities();
            var data = (from u in db.News where u.Id == Id select u).FirstOrDefault();
            db.News.Remove(data);
            db.SaveChanges();
            return RedirectToAction("List");
           
        }

        public ActionResult Search(string CategorySearch)
        {
            Lab_TaskEntities db = new Lab_TaskEntities();
            var data = (from u in db.News where u.Description.Contains("Napoli") select u).ToList();

            
            return RedirectToAction("List", "News", new { data });
        }
    }
}