using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PSP_EF.Models;
using PSP_EF.DAL;
using PSP_EF.BusinessService;

namespace PSP_EF.Controllers
{
    public class PetClassManagerController : Controller
    {
        private PetClassLogic _petClassLogic = new PetClassLogic();
        private PetLogic _petLogic = new PetLogic();

        private PetDbContext petDb = new PetDbContext();

        //
        // GET: /PetClassManager/

        public ActionResult Index()
        {
            var classes = _petClassLogic.GetAllClasses(false);
            return View(classes.ToList());

        }

        //public ActionResult Browse(string petClass)
        //{
        //    // Retrieve Genre and its Associated Albums from database
        //    var petClassModel = _petClassLogic.GetPetClassDetails(petClass);
        //    return View(petClassModel);
        //}

        //
        // GET: /PetClassManager/Details/5

        public ActionResult Details(int id = 0)
        {
            var petClass = _petClassLogic.GetPetClassById(id);
            if (petClass == null)
            {
                return HttpNotFound();
            }
            return View(petClass);
        }

        public ActionResult AddToClass(int id)
        {
            var addedPetId = _petLogic.GetPetById(id);
            var petOwnerId = _petLogic.GetPetById(id).CustomerId;
            //var classToBeAdded = _petClassLogic.GetPetClassById(classId);
            
            var abc = Request.Form["PetClassId"];
            ViewBag.m = abc;
            return View("testing");
            //_petClassLogic.BookThisClassForMyPet(abc,addedPetId,petOwnerId);
        }


        //public bool BookThisClassForMyPet(PetClass target, int petId, int ownerId)
        //public ActionResult AddToCart(int id)
        //{

        //    // Retrieve the album from the database
        //    var addedAlbum = storeDB.Albums
        //        .Single(album => album.AlbumId == id);

        //    // Add it to the shopping cart
        //    var cart = ShoppingCart.GetCart(this.HttpContext);

        //    cart.AddToCart(addedAlbum);

        //    // Go back to the main store page for more shopping
        //    return RedirectToAction("Index");
        //}


        //
        // GET: /PetClassManager/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PetClassManager/Create

        [HttpPost]
        public ActionResult Create(PetClass petClass)
        {
            if (ModelState.IsValid)
            {
                _petClassLogic.AddPetClass(petClass);
                return RedirectToAction("Index");
            }

            return View(petClass);
        }

        //
        // GET: /PetClassManager/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var petClass = _petClassLogic.GetPetClassById(id);
            if (petClass == null)
            {
                return HttpNotFound();
            }
            return View(petClass);
        }

        //
        // POST: /PetClassManager/Edit/5

        [HttpPost]
        public ActionResult Edit(PetClass petClass)
        {
            if (ModelState.IsValid)
            {
                _petClassLogic.UpdatePetClass(petClass);
                return RedirectToAction("Index");
            }
            return View(petClass);
        }

        //
        // GET: /PetClassManager/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var petClass = _petClassLogic.GetPetClassById(id);
            if (petClass == null)
            {
                return HttpNotFound();
            }
            return View(petClass);
        }

        //
        // POST: /PetClassManager/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var petClass = _petClassLogic.GetPetClassById(id);

            _petClassLogic.DeletePetClass(id);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _petClassLogic.Dispose();
            base.Dispose(disposing);
        }
    }
}