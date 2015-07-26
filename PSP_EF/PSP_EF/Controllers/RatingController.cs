using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PSP_EF.DAL;
using PSP_EF.Models;

namespace PSP_EF.Controllers
{
    public class RatingController : Controller
    {
        private PetDbContext db = new PetDbContext();
        int serviceId = 0;
        String ServiceName = "";
        int ProviderId = 0;


        //
        // GET: /Rating/

        public ActionResult Index()
        {
            var ratings = db.Ratings.Include(r=>r.Transaction);
            return View(ratings.ToList());
        }

        //
        // GET: /Rating/Details/5

        public ActionResult Details(int id = 0)
        {
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        //
        // GET: /Rating/Create

        public ActionResult Create()
        {
            ViewBag.TransactionId = new SelectList(db.Transactions, "TransactionId", "Status");
            return View();
        }

        //
        // POST: /Rating/Create

        [HttpPost]
        public ActionResult Create(Rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Ratings.Add(rating);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TransactionId = new SelectList(db.Transactions, "TransactionId", "Status", rating.TransactionId);
            return View(rating);
        }

        //
        // GET: /Rating/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            ViewBag.TransactionId = new SelectList(db.Transactions, "TransactionId", "Status", rating.TransactionId);
            return View(rating);
        }

        //
        // POST: /Rating/Edit/5

        [HttpPost]
        public ActionResult Edit(Rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TransactionId = new SelectList(db.Transactions, "TransactionId", "Status", rating.TransactionId);
            return View(rating);
        }

        //
        // GET: /Rating/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        //
        // POST: /Rating/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Rating rating = db.Ratings.Find(id);
            db.Ratings.Remove(rating);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        public ActionResult CustomerRating(int transId = 0)
        {
            //Transaction trans = new Transaction();
            //trans.CustomerId = 1; //value from session
            var transaction = from p in db.Transactions //where (p.Pet_Category == searchText)
                              select p;

            transaction = transaction.Where(s => s.TransactionId.Equals(transId));
            
            foreach (var p in transaction)
            {
                serviceId = p.ServiceId;
            }

            var service = from s in db.Services
                          select s;
            service = service.Where(s => s.ServiceId.Equals(serviceId));

            foreach (var s in service)
            {
                ServiceName = s.ServiceName;
                ProviderId = s.ProviderId;
            }

            var customer = from c in db.Customers
                           select c;
            customer = customer.Where(c => c.CustomerId.Equals(ProviderId));
            String ProviderName = "";
            foreach (var c in customer)
            {
                ProviderName = c.FirstName + " " + c.LastName;
            }
            Session["transId"] = transId;
            ViewBag.ServiceName = ServiceName;
            ViewBag.ProviderName = ProviderName;
            List<SelectListItem> Ratelist = new List<SelectListItem>();
            Ratelist.Add(new SelectListItem
        {
            Selected = true,
            Text = "1",
            Value = "1"
        });

            Ratelist.Add(new SelectListItem
        {
            Text = "2",
            Value = "2"
        });

            Ratelist.Add(new SelectListItem
        {
            Text = "3",
            Value = "3"
        });

            Ratelist.Add(new SelectListItem
        {
            Text = "4",
            Value = "4"
        });

            Ratelist.Add(new SelectListItem
        {
            Text = "5",
            Value = "5"
        });

            ViewBag.RateList = Ratelist;

            return View("CustomerRating");

        }

        [HttpPost]
        public ActionResult CustomerRating(Rating cModel)
        {

            Customer currentUser = (Customer)Session["currentUser"];
            int customerId = currentUser.CustomerId;
            int tId = Convert.ToInt16(Session["transId"]);
            cModel.TransactionId = tId;
            var transaction = from p in db.Transactions //where (p.Pet_Category == searchText)
                              select p;
            transaction = transaction.Where(s => s.TransactionId.Equals(tId));
            int CustomerId = 0;
            foreach (var p in transaction)
            {
                CustomerId = p.CustomerId;
            }
            if (CustomerId == customerId)
                cModel.Type = true;
            else
                cModel.Type = false;

            db.Ratings.Add(cModel);

            Transaction thistrans = db.Transactions.Find(cModel.TransactionId);
            thistrans.Status = "Rated";
            db.SaveChanges();

            ViewBag.Message = "Thank you for your rating.";

            return RedirectToAction("Index", "Home");
        }


        public ActionResult RateHome(Pet cPet, decimal fromPrice = -12, decimal toPrice = -12)
        {
            Customer currentUser = (Customer)Session["currentUser"];
            int customerId = currentUser.CustomerId;
            
            // get list of all pets which is going to be sold by their owners.
            var custranslist = (from trans in db.Transactions
                              join customer in db.Customers on trans.CustomerId equals customer.CustomerId
                              join service in db.Services on trans.ServiceId equals service.ServiceId
                              where trans.CustomerId == customerId
                              where trans.Status == "Finished"
                              select new TransList
                              {
                                    service_name = service.ServiceName,
                                    trans_id = trans.TransactionId 
                              }).Distinct();

            var protranslist = (from trans in db.Transactions
                              join service in db.Services on trans.ServiceId equals service.ServiceId
                              where service.ProviderId == customerId
                              where trans.Status == "Finished"
                              select new TransList
                              {
                              service_name = service.ServiceName,
                              trans_id = trans.TransactionId
                              }).Distinct();

              

            var wholetranslist = new WholeTransList();
            wholetranslist.CustomerTranslist = (IEnumerable<TransList>)custranslist;
            wholetranslist.ProviderTranslist = (IEnumerable<TransList>)protranslist;


            


            //List<SearchPetResult> test = petForSale.ToList();
            //ViewData["petForSale"] = petForSale.ToList();
            //SearchPetResult petForSaleModel = (SearchPetResult)petForSale;
            return View(new List<WholeTransList>(){wholetranslist});
        }
    }
}