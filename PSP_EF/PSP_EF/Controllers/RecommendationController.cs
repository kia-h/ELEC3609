using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PSP_EF.Models;
using PSP_EF.DAL;

namespace PSP_EF.Controllers
{
    public class RecommendationController : Controller
    {
        private PetDbContext db = new PetDbContext();

        //
        // GET: /Recommendation/

        public ActionResult Index()
        {
            var recommendations = db.Recommendations.Include(r => r.Customer);
            return View(recommendations.ToList());
        }

        //
        // GET: /Recommendation/Details/5

        public ActionResult Details(int RecId = 0)
        {
            Recommendation recommendation = db.Recommendations.Find(RecId);
            RecommendationDetail recommendationdetail = new RecommendationDetail();
            recommendationdetail.recommendation = recommendation;
            var customerA = db.Customers.Find(recommendation.CustomerAid);
            var customerB = db.Customers.Find(recommendation.CustomerBid);
            recommendationdetail.Name_CustomerA = customerA.LastName;
            recommendationdetail.Name_CustomerB = customerB.LastName;
            recommendation.Isread = true;
            if (recommendation == null)
            {
                return HttpNotFound();
            }
            return View(recommendationdetail);
        }

        //
        // GET: /Recommendation/Create

        public ActionResult Create()
        {
            ViewBag.CustomerCid = new SelectList(db.Customers, "CustomerId", "Username");
            return View();
        }

        //
        // POST: /Recommendation/Create

        [HttpPost]
        public ActionResult Create(Recommendation recommendation)
        {
            if (ModelState.IsValid)
            {
                db.Recommendations.Add(recommendation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerCid = new SelectList(db.Customers, "CustomerId", "Username", recommendation.CustomerCid);
            return View(recommendation);
        }

        //
        // GET: /Recommendation/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Recommendation recommendation = db.Recommendations.Find(id);
            if (recommendation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerCid = new SelectList(db.Customers, "CustomerId", "Username", recommendation.CustomerCid);
            return View(recommendation);
        }

        //
        // POST: /Recommendation/Edit/5

        [HttpPost]
        public ActionResult Edit(Recommendation recommendation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recommendation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerCid = new SelectList(db.Customers, "CustomerId", "Username", recommendation.CustomerCid);
            return View(recommendation);
        }

        //
        // GET: /Recommendation/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Recommendation recommendation = db.Recommendations.Find(id);
            if (recommendation == null)
            {
                return HttpNotFound();
            }
            return View(recommendation);
        }

        //
        // POST: /Recommendation/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Recommendation recommendation = db.Recommendations.Find(id);
            db.Recommendations.Remove(recommendation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult RecommendationList() 
        {
            Customer currentUser = (Customer)Session["currentUser"];
            int customerId = currentUser.CustomerId;

            var thiscustomer = from p in db.Recommendations
                               select p;
            thiscustomer = thiscustomer.Where(p => p.CustomerCid.Equals(customerId));
            thiscustomer = thiscustomer.Where(p => p.Isread.Equals(false));

            return View(thiscustomer.ToList());
        }

        public ActionResult Recommending()
        { 
            Customer currentUser = (Customer)Session["currentUser"];
            int customerId = currentUser.CustomerId;


            return View();
        }

        [HttpPost]
        public ActionResult Recommending(Recommendation cModel)
        {
            Customer currentUser = (Customer)Session["currentUser"];
            int customerId = currentUser.CustomerId;
            cModel.CustomerAid = customerId;
            cModel.Isread = false;

            //cModel.CustomerBid
                //cModel.CustomerCid

            Customer checkB = db.Customers.Find(cModel.CustomerBid);
            Customer checkC = db.Customers.Find(cModel.CustomerCid);

            if(checkB == null){
                ViewBag.Message1 = "No such service provider";
                return View();
            }

            if (checkC == null)
            {
                ViewBag.Message2 = "No such user";
                return View();
            }

            db.Recommendations.Add(cModel);
            db.SaveChanges();

            ViewBag.Message = "Thank you for your recommendation.";

            return RedirectToAction("Index", "Home");
        }
    }
}