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
using PSP_EF.Models.ViewModels;

namespace PSP_EF.Controllers
{
    public class PetController : Controller
    {

        private PetLogic _logic;
        private PetDbContext db;
        private PetClassLogic _classLogic;
        //
        // GET: /Pet/

        public ActionResult Index()
        {
         
            
            _logic = new PetLogic();
            var customers = _logic.GetAllCustomers();
            var pets = _logic.GetAllPets(true);
            PetsViewModel model = new PetsViewModel();
           
            model.Pets = pets;
            model.Customers = customers;
            return View("Index",model);

        }

        //
        // GET: /Pet/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    var _classes = db.PetClass;

        //    //ViewBag.PetClassId = new SelectList(db.PetClass, "PetClassId", "Name");

        //    Pet pet = _logic.GetPetById(id);
        //    //Pet pet = db.Pets.Find(id);
        //    if (pet == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(pet);
        //}

        //
        // GET: /Pet/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Username");
            return View();
        }

        //
        // POST: /Pet/Create
        [HttpPost]
        public ActionResult Create(Pet pet)
        {
            if (ModelState.IsValid)
            {
                _logic = new PetLogic();
                _logic.AddPet(pet);
                //db.Pets.Add(pet);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Username", pet.CustomerId);
            return View(pet);
        }              

        //
        // GET: /Pet/Edit/5

        public ActionResult Edit(int id = 0)
        {
            _logic = new PetLogic();
            Pet pet = _logic.GetPetById(id);
            //Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Username", pet.CustomerId);
            return View(pet);
        }

        //
        // POST: /Pet/Edit/5

        [HttpPost]
        public ActionResult Edit(Pet pet)
        {
            if (ModelState.IsValid)
            {
                _logic = new PetLogic();
                _logic.UpdatePet(pet);
                //db.Entry(pet).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "Username", pet.CustomerId);
            return View(pet);
        }

        //
        // GET: /Pet/Delete/5

        public ActionResult Delete(int id = 0)
        {
            _logic = new PetLogic();
            Pet pet = _logic.GetPetById(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        //
        // POST: /Pet/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _logic = new PetLogic();
            //Pet pet = _logic.GetPetById(id);
            _logic.DeletePet(id);
            //db.Pets.Remove(pet);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        ///Russelll section crude Update :
        public string UpdateData(string id, string value, string columnPosition)
        {
            if (Request.IsAjaxRequest())
            {
                if (columnPosition == "4" )
                {
                    string str_classId = RetriveIDFromThis(value);
                    _classLogic = new PetClassLogic();
                    if (_classLogic.BookThisClassForMyPet(Convert.ToInt32(str_classId), Convert.ToInt32(id)))
                        return value;
                    else return "Internal problem, plz try later";
                }

                try
                {
                    _logic = new PetLogic();
                    return _logic.UpdatePetAjaxService(Convert.ToInt32(id), value, Convert.ToInt32(columnPosition));
                }
                catch (Exception exp)
                {
                    return exp.Message;
                }
            }
            //SomeOne trying to hack, Implement Somemlogic here :
            return null;
        }

        public ActionResult GetAvailableClassesList()
        {
            
            _classLogic = new PetClassLogic();
            var classes = _classLogic.GetAllClasses(true).ToList();
            Dictionary<string, string> result = new Dictionary<string, string>();
           
            foreach (var cl in classes)
            {
                result.Add(string.Format("{0}-{1}", cl.PetClassId, cl.Name), string.Format("{0}-{1}", cl.PetClassId, cl.Name));
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public int AddData(string customerId, string category, string price,  string description)
        {
            customerId = RetriveIDFromThis(customerId);
            _logic = new PetLogic();
            PetLogic _pLogic = new PetLogic(_logic._petDb);
            try
            {
                Customer owner = _logic.GetCustomerById(Convert.ToInt32(customerId));

               Pet  newpet = new Pet()
                {
                    Customer = owner,
                    CustomerId = owner.CustomerId,
                    PetCategory = category,
                    Description = description,
                    Price =Convert.ToDecimal(price)
                    
                };

               var addedPetId = _pLogic.CreatePet(newpet);
               if (addedPetId > 0)
                {
                    return addedPetId;
                }
                else
                {
                    Response.Write("Some error happened in our dataBase, Refresh the page and try again");
                    Response.StatusCode = 500;
                    Response.End();
                    return -1;
                }
            }
            catch (FormatException)
            {

                Response.Write("Please check your inputs.");
                Response.StatusCode = 404;
                Response.End();
                return -1;
            }
        }

        public string DeleteData(int id)
        {
            try
            {
                PetLogic _petLogic = new PetLogic();

                if (_petLogic.DeletePet(id) ) return "ok";
                else return "The Given Pet Not Found";
            }
            catch (Exception exp)
            {
                return exp.GetType().Equals(typeof(ArgumentNullException)) ? "The Given Pet Not Found" : exp.Message;
            }
        }

        private string RetriveIDFromThis(string src)
        {
           string[] arry = src.Split('-');
           return arry[0];
        }
            
        protected override void Dispose(bool disposing)
        {
            if (db != null) db.Dispose();
            base.Dispose(disposing);
        }
    }
}