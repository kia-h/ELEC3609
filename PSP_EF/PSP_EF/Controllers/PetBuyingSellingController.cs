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
    public class PetBuyingSellingController : Controller
    {
        public PetDbContext db = new PetDbContext();

        //
        // GET: /PetBuyingSelling/

        public ActionResult Index()
        {
            var pets = db.Pets.Include(p => p.Customer);
            return View(pets.ToList());
        }

        //
        // GET: /PetBuyingSelling/Details/5

        public ActionResult Details(int id = 0)
        {
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        //
        // GET: /PetBuyingSelling/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName");
            return View();
        }

        //
        // POST: /PetBuyingSelling/Create

        [HttpPost]
        public ActionResult Create(Pet pet)
        {
            if (ModelState.IsValid)
            {
                db.Pets.Add(pet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", pet.CustomerId);
            return View(pet);
        }

        //
        // GET: /PetBuyingSelling/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", pet.CustomerId);
            return View(pet);
        }

        //
        // POST: /PetBuyingSelling/Edit/5

        [HttpPost]
        public ActionResult Edit(Pet pet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName", pet.CustomerId);
            return View(pet);
        }

        //
        // GET: /PetBuyingSelling/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        //
        // POST: /PetBuyingSelling/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Pet pet = db.Pets.Find(id);
            db.Pets.Remove(pet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //POST: /PetBuyingSelling/Search/
        /*[HttpPost]
        public ActionResult Search(String searchText, decimal fromPrice = -1234, decimal toPrice = -1234)
        {

            //if (fromPrice == null)
            //  fromPrice = 54;
            ViewBag.Message = fromPrice;
            //return RedirectToAction("Search") ;

            var petList = from p in db.Pets //where (p.Pet_Category == searchText)
                          select p;


            //decimal? convertedFromAmount = fromPrice;
            //decimal? convertedToAmount = toPrice;

            if (!String.IsNullOrEmpty(searchText))
            {
                petList = petList.Where(s => s.PetCategory.Contains(searchText));
            }

            if (!(fromPrice == -1234))
            {
                petList = petList.Where(x => x.Price >= fromPrice);// && p.Price == convertedFromAmount
            }

            if (!(toPrice == -1234))
            {
                petList = petList.Where(x => x.Price <= toPrice);// && p.Price == convertedFromAmount
            }

            /*if (!String.IsNullOrEmpty(searchText))
            {
                petList = petList.Where(s => s.Pet_Category.Contains(searchText));
            }

            if (fromPrice.)
            {
                petList = petList.Where(x => x.Price == fromPrice);
            }*/
            //return View(petList.ToList());
        //}
        /*
        public ActionResult Buy(int id = 0)
        {
            Transaction trans = new Transaction();

            var cUser = (Customer)Session["currentUser"];
            int customerId = cUser.CustomerId;

            trans.CustomerId = customerId; // hard coded
            trans.ServiceId = 1; // hard coded
            trans.Date = System.DateTime.Now;
            trans.Status = "Customer submit the offer";

            //Pet pet = db.Pet.Find(id);

            if (ModelState.IsValid)
            {
                db.Transactions.Add(trans);
                db.SaveChanges();
                //return RedirectToAction("Index");
            }

            /*
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);*/
            //return View();
        //}

        // GET:/PetBuyingSelling/Buying/
        // get the initial page for buying a pet
        public ActionResult Buying()
        {
            return View();
        }

        // POST:/PetBuyingSelling/SearchResult/
        // Show the search result and option to buy
        public ActionResult SearchResult(Pet cPet, decimal fromPrice = -12, decimal toPrice = -12)
        {
            // get list of all pets which is going to be sold by their owners.
            var petForSale = (from thePets in db.Pets
                              join theOwner in db.Customers on thePets.CustomerId equals theOwner.CustomerId
                              join service in db.Services on thePets.PetId equals service.PetId
                              where service.ServiceName.StartsWith("Pet Selling by ")
                              where thePets.PetCategory.Equals(cPet.PetCategory)
                              where !thePets.TransactionId.HasValue
                              select new SearchPetResult
                              {
                                  service_id = service.ServiceId,
                                  pet_owner_name = theOwner.FirstName + " " + theOwner.LastName,
                                  pet_category = thePets.PetCategory,
                                  pet_desc = thePets.Description,
                                  pet_price = (decimal)thePets.Price
                              }).Distinct();


            // Check whether user input a minimum value or maximum value for price
            // if the user only input the minimum price, then only minimum price is set
            // if the user only input the maximum price, then only the maximum price is set
            // if the user input both minimum & maximum price, then the price range is set
            if (!(fromPrice == -12))
            {
                petForSale = petForSale.Where(x => x.pet_price >= fromPrice);
            }

            if (!(toPrice == -12))
            {
                petForSale = petForSale.Where(x => x.pet_price <= toPrice);
            }
            //List<SearchPetResult> test = petForSale.ToList();
            //ViewData["petForSale"] = petForSale.ToList();
            //SearchPetResult petForSaleModel = (SearchPetResult)petForSale;
            return View(petForSale.AsEnumerable());
        }

        // GET: /PetBuyingSelling/Selling/
        // get the initial page for selling a pet
        public ActionResult Selling()
        {
            return View();
        }

        // POST: /PetBuyingSelling/Selling/
        // get the initial page for selling
        [HttpPost, ValidateInput(false)]
        public ActionResult Selling(Pet cPet)
        {
            //@ViewBag.Message = cPet.Description;
            //var cityId = Request.Form["citylist"];
            Customer currentUser = (Customer)Session["currentUser"];
            int customerId = currentUser.CustomerId;

            cPet.CustomerId = customerId;
            if (cPet.Description == null)
            {
                cPet.Description = "";
            }
            db.Pets.Add(cPet);
            db.SaveChanges();

            Service newService = new Service();
            newService.ProviderId = customerId;
            newService.PetId = cPet.PetId;
            newService.ServiceName = "Pet Selling by " + currentUser.FirstName + " " + currentUser.LastName;
            newService.Description = "Ordinary pet selling transaction";
            newService.Price = cPet.Price;
            db.Services.Add(newService);
            db.SaveChanges();

            ViewBag.Message = "Your pet info has been posted. Thank you for using our service.";

            return View();
        }

        public ActionResult BuyProcess(int serviceId = 0)
        {
            int petId = 0;
            var cUser = (Customer)Session["currentUser"];
            Transaction trans = new Transaction();

            trans.CustomerId = cUser.CustomerId;
            trans.ServiceId = serviceId;
            trans.Date = System.DateTime.Now;
            trans.Status = "Finished";

            if (ModelState.IsValid)
            {
                db.Transactions.Add(trans);
                db.SaveChanges();
                //return RedirectToAction("Index");
            }

            /* Service theService = db.Services.Find(serviceId);
             db.Services.Remove(theService);
             db.SaveChanges();*/

            var getPetId = (from service in db.Services
                            where service.ServiceId.Equals(serviceId)
                            select new { service.PetId });

            foreach (var c in getPetId)
            {
                petId = (int)c.PetId;
            }


            Pet thePet = new Pet();
            thePet = db.Pets.Find(petId);
            thePet.TransactionId = trans.TransactionId;

            if (ModelState.IsValid)
            {
                db.Entry(thePet).State = EntityState.Modified;
                db.SaveChanges();
            }

            /*
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);*/
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}