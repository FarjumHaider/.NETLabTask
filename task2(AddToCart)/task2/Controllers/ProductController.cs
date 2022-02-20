using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using task2.Models.Database;

using System.Web.Script.Serialization;

namespace task2.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        [HttpGet]
        public ActionResult list()
        {
            //Session.Clear();
            Lab_Task_2Entities db = new Lab_Task_2Entities();
            var data = db.Products.ToList();
            return View(data);
        }

        [HttpGet]


        public ActionResult ListCart(int Id)
        {
            Lab_Task_2Entities db = new Lab_Task_2Entities();

            var data = (from u in db.Products where u.Id == Id select u).FirstOrDefault();


            List<Product> p = new List<Product>();


            if (Session["key"] != null)
            {

                var c = Session["key"].ToString();
                p = new JavaScriptSerializer().Deserialize<List<Product>>(c);

            }
            data.Qty = 1;
            p.Add(data);
            
            //string json = new JavaScriptSerializer().Serialize(p);
            Session["key"] = data;

            return RedirectToAction("List");
            
        }

        [HttpGet]
        public ActionResult Cart()
        {
            var v = Session["key"].ToString();
            var d = new JavaScriptSerializer().Deserialize<List<Product>>(v);
            //Session["key"] = d;

            return View(d);
        }

        public ActionResult Order(int? Total_price)
        {
            Lab_Task_2Entities db = new Lab_Task_2Entities();

            Order O = new Order();
            O.Total_price = 10;
            O.Status = "Pending";
            db.Orders.Add(O);
            db.SaveChanges();

            var v = Session["key"].ToString();
            var d = new JavaScriptSerializer().Deserialize<List<Product>>(v);

            foreach (Product item in d)
            {
                OrderDetail od = new OrderDetail();
                od.O_id = O.Id;
                od.P_id = item.Id;
                db.OrderDetails.Add(od);
                db.SaveChanges();
            }

            Session.Remove("Key");
            return RedirectToAction("List");
        }
    }
}