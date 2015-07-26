using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PSP_EF.Models;
using PSP_EF.DAL;
using System.Data.Entity.Validation;

namespace PSP_EF.Controllers
{
    public class AdoptionController : Controller
    {
        private PetDbContext db = new PetDbContext();

        //
        // GET: /Adoption/

        public ActionResult Index()
        {
            return View(db.Adoptions.ToList());
        }

        //
        // GET: /Adoption/Details/5

        public ActionResult Details(int id = 0)
        {
            Adoption adoption = db.Adoptions.Find(id);
            if (adoption == null)
            {
                return HttpNotFound();
            }
            return View(adoption);
        }

        //
        // GET: /Adoption/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Adoption/Create

        [HttpPost]
        public ActionResult Create(Adoption adoption)
        {
            if (ModelState.IsValid)
            {
                db.Adoptions.Add(adoption);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adoption);
        }

        //
        // GET: /Adoption/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Adoption adoption = db.Adoptions.Find(id);
            if (adoption == null)
            {
                return HttpNotFound();
            }
            return View(adoption);
        }

        //
        // POST: /Adoption/Edit/5

        [HttpPost]
        public ActionResult Edit(Adoption adoption)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adoption).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adoption);
        }

        //
        // GET: /Adoption/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Adoption adoption = db.Adoptions.Find(id);
            if (adoption == null)
            {
                return HttpNotFound();
            }
            return View(adoption);
        }

        //
        // POST: /Adoption/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Adoption adoption = db.Adoptions.Find(id);
            db.Adoptions.Remove(adoption);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult PostInfo()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PostInfo(Pet cPet)
        {
            //@ViewBag.Message = cPet.Description;
            //var cityId = Request.Form["citylist"];
            Customer currentUser = (Customer)Session["currentUser"];
            int customerId = currentUser.CustomerId;


            cPet.CustomerId = customerId;
            cPet.IsForAdoption = true;
            db.Pets.Add(cPet);
            db.SaveChanges();
            ViewBag.Message = "Your pet info has been posted. Thank you.";

            return View();
        }


        public ActionResult Adoptlist()
        {
            var adoptlist = from a in db.Pets //where (p.Pet_Category == searchText)
                            where a.AdoptionId == null
                            where a.IsForAdoption == true
                            select a;


            return View(adoptlist.ToList());
        }

        public ActionResult Adopting(int petid = 0)
        {
            Customer currentUser = (Customer)Session["currentUser"];
            int customerId = currentUser.CustomerId;



            Adoption newadoption = new Adoption();
            newadoption.AdoptDate = DateTime.Now;
            newadoption.CustomerId = customerId;
            newadoption.status = "Pending";
            db.Adoptions.Add(newadoption);
            

            Pet thispet = db.Pets.Find(petid);
            thispet.AdoptionId = newadoption.AdoptionId;
            db.SaveChanges();

            return View(newadoption);
        }

    }
}