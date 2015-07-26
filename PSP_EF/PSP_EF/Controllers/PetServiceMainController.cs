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
    public class PetServiceMainController : Controller
    {
        private PetDbContext db = new PetDbContext();

        //
        // GET: /PetServiceMain/

        public ActionResult Index()
        {
            var services = db.Services.Include(s => s.Customer);
            return View(services.ToList());
        }

        //
        // GET: /PetServiceMain/Details/5

        public ActionResult Details(int id = 0)
        {
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        //
        // GET: /PetServiceMain/Create

        public ActionResult Create()
        {
            ViewBag.ProviderId = new SelectList(db.Customers, "CustomerId", "Username");
            return View();
        }

        //
        // POST: /PetServiceMain/Create

        [HttpPost]
        public ActionResult Create(Service service)
        {
            if (ModelState.IsValid)
            {
                db.Services.Add(service);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProviderId = new SelectList(db.Customers, "CustomerId", "Username", service.ProviderId);
            return View(service);
        }

        //
        // GET: /PetServiceMain/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProviderId = new SelectList(db.Customers, "CustomerId", "Username", service.ProviderId);
            return View(service);
        }

        //
        // POST: /PetServiceMain/Edit/5

        [HttpPost]
        public ActionResult Edit(Service service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProviderId = new SelectList(db.Customers, "CustomerId", "Username", service.ProviderId);
            return View(service);
        }

        //
        // GET: /PetServiceMain/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        //
        // POST: /PetServiceMain/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Service service = db.Services.Find(id);
            db.Services.Remove(service);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult PostService()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PostService(Service cService)
        {
            //@ViewBag.Message = cPet.Description;
            //var cityId = Request.Form["citylist"];
            Customer currentUser = (Customer)Session["currentUser"];
            int customerId = currentUser.CustomerId;

            cService.ProviderId = customerId;

            db.Services.Add(cService);
            db.SaveChanges();

            ViewBag.Message = "Your service has been posted. Thank you for using our service.";

            return View();
        }

        public ActionResult SearchPetService()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchPetServiceResult(Service cService, decimal fromPrice = -12, decimal toPrice = -12)
        {
            // get list of all valid services
            var theService = (from customer in db.Customers
                              join service in db.Services on customer.CustomerId equals service.ProviderId
                              where service.ServiceName.Equals(cService.ServiceName)
                              where !(from trans in db.Transactions select trans.ServiceId).Contains(service.ServiceId)
                              select new SearchServiceResult
                              {
                                  service_id = service.ServiceId,
                                  service_provider_id = service.ProviderId,
                                  service_provider_name = customer.FirstName + " " + customer.LastName,
                                  service_type = service.ServiceName,
                                  service_desc = service.Description,
                                  service_price = (decimal)service.Price
                              }).Distinct();

            // Check whether user input a minimum value or maximum value for price
            // if the user only input the minimum price, then only minimum price is set
            // if the user only input the maximum price, then only the maximum price is set
            // if the user input both minimum & maximum price, then the price range is set
            if (!(fromPrice == -12))
            {
                theService = theService.Where(x => x.service_price >= fromPrice);
            }

            if (!(toPrice == -12))
            {
                theService = theService.Where(x => x.service_price <= toPrice);
            }
            //List<SearchPetResult> test = petForSale.ToList();
            //ViewData["petForSale"] = petForSale.ToList();
            //SearchPetResult petForSaleModel = (SearchPetResult)petForSale;
            return View(theService.AsEnumerable());
        }

        public ActionResult HireProcess(int serviceId = 0)
        {
            //Service theService = db.Services.Find(serviceId);
            var cUser = (Customer)Session["currentUser"];
            Transaction trans = new Transaction();

            trans.CustomerId = cUser.CustomerId; 
            trans.ServiceId = serviceId; 
            trans.Date = System.DateTime.Now;
            trans.Status = "Finished";

            //Pet pet = db.Pet.Find(id);

            if (ModelState.IsValid)
            {
                db.Transactions.Add(trans);
                db.SaveChanges();
                //return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult CancelService()
        {
            var cUser = (Customer)Session["currentUser"];
            int customerId = cUser.CustomerId;

            var theService = (from service in db.Services
                              join trans in db.Transactions on service.ServiceId equals trans.ServiceId
                              join customer in db.Customers on service.ProviderId equals customer.CustomerId
                              where trans.CustomerId.Equals(customerId)
                              && ( service.ServiceName.Contains("Walking") 
                              || service.ServiceName.Contains("Sitting")  
                              || service.ServiceName.Contains("Saloon")
                              || service.ServiceName.Contains("Washing"))
                              select new SearchServiceResult
                              {
                                  service_id = service.ServiceId,
                                  service_provider_id = service.ProviderId,
                                  service_provider_name = customer.FirstName + " " + customer.LastName,
                                  service_type = service.ServiceName,
                                  service_desc = service.Description,
                                  service_price = (decimal)service.Price
                              }).Distinct();

            /*var theService = (from trans in db.Transactions
                              join service in db.Services on trans.ServiceId equals service.ServiceId
                              join customer in db.Customers on customer.customerId equals service.providerId 
                              where trans.CustomerId.Equals(customerId)
                              where service.ServiceName.Contains("Walking")
                              where service.ServiceName.Contains("Sitting")
                              where service.ServiceName.Contains("Saloon")
                              where service.ServiceName.Contains("Washing")
                              select new SearchServiceResult
                              {
                                  service_id = service.ServiceId,
                                  service_provider_id = service.ProviderId,
                                  service_provider_name = customer.FirstName + " " + customer.LastName,
                                  service_type = service.ServiceName,
                                  service_desc = service.Description,
                                  service_price = (decimal)service.Price
                              }).Distinct();*/

            return View(theService.AsEnumerable());
        }

        public ActionResult CancelServiceProcess(int serviceId = 0)
        {
            var remove = (from trans in db.Transactions
                          where trans.ServiceId == serviceId
                          select trans).FirstOrDefault();

            if (remove != null)
            {
                db.Transactions.Remove(remove);
                db.SaveChanges();
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}