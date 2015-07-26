using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PSP_EF.DAL;
using PSP_EF.Models;
using System.Data.Entity.Validation;

namespace PSP_EF.Controllers
{
    public class CustomerAccountController : Controller
    {
        private PetDbContext db = new PetDbContext();

        //
        // GET: /CustomerAccount/

        public ActionResult Index()
        {
            var customers = db.Customers.Include(c=>c.City);
            return View(customers.ToList());
        }

        //
        // GET: /CustomerAccount/Details/5

        public ActionResult Details(int id = 0)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //
        // GET: /CustomerAccount/Create

        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name");
            return View();
        }

        //
        // POST: /CustomerAccount/Create

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name", customer.CityId);
            return View(customer);
        }

        //
        // GET: /CustomerAccount/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name", customer.CityId);
            return View(customer);
        }

        //
        // POST: /CustomerAccount/Edit/5

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name", customer.CityId);
            return View(customer);
        }

        //
        // GET: /CustomerAccount/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        //
        // POST: /CustomerAccount/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET route for: /CustomerAccount/Register
        public ActionResult Register()
        {
            ViewBag.citylist = getCities();

            return View("Register");
        }

        // POST route for: /CustomerAccount/Register
        [HttpPost]
        public ActionResult Register(Customer cModel)
        {
            var cityId = Request.Form["citylist"];
            ViewBag.citylist = getCities();

            ViewBag.Message = "Thank you for registering. ";

            //check whether the email is already exist or not
            var customer = from p in db.Customers
                           select p;
            customer = customer.Where(s => s.Email.Equals(cModel.Email));
            if (customer.Count() != 0)
            {
                ViewBag.Message = null;
                ViewBag.ErrorMessage = "Your email address is already exists. Please choose another email address.";
                return View();
            }
            else
            {
                cModel.JoinedDate = DateTime.Now;
                cModel.LastUpdate = DateTime.Now;
                cModel.CityId = Convert.ToInt16(cityId);
                db.Customers.Add(cModel);
                db.SaveChanges();
                Session["currentUser"] = cModel;
                return View("Dashboard");
            }
        }

        //
        // GET route for: /CustomerAccount/Login
        // Redirect user to login page for the first time
        public ActionResult Login()
        {
            return View("Login");
        }

        
        // POST route for: /CustomerAccount/Login
        // Redirect user to appropriate page after login
        [HttpPost]
        public ActionResult Login(Customer cModel)
        {
            //check whether the customer is in the database and using the correct email & password
            var customer = from p in db.Customers
                          select p;

            customer = customer.Where(s => s.Email.Equals(cModel.Email));
            customer = customer.Where(s => s.Password.Equals(cModel.Password));

            //validating the customer email and password
            if (customer.Count() == 0 || cModel.Email == "" || cModel.Password == "")
            {
                ViewBag.Message = "Wrong email or password";
                return View("Login");
            }
            else
            {
                //foreach (var c in customer)
                //{
                Session["currentUser"] = customer.First();
                //}
                return View("Dashboard");
            }
        }

        // GET route for: /CustomerAccount/EditDetails
        public ActionResult EditDetails()
        {
            var cModel = (Customer)Session["currentUser"];

            Customer customer = db.Customers.Find(cModel.CustomerId);

            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name", customer.CityId);
            //ViewBag.citylist = getCities();
            if (cModel.DateOfBirth != null)
            {
                DateTime date1 = (DateTime)cModel.DateOfBirth;
                ViewBag.dateValue = date1.ToString("MM/dd/yyyy");
            }
            //cModel.DateOfBirth = cModel.DateOfBirth.ToString("MM/dd/YYYY");
            //var model = new Customer();
            //model.Description = "Text Area Content";
            return View(customer);
        }

        // POST route for: /CustomerAccount/EditDetails
        // Save the updated data

        // POST route for: /CustomerAccount/EditDetails
        // Save the updated data
        [HttpPost]
        public ActionResult EditDetails(Customer cModel)
        {
            //cModel.Description = 
            var currentUser = (Customer)Session["currentUser"];
            //var cityId = Request.Form["citylist"];
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name", cModel.CityId);                        
            cModel.LastUpdate = DateTime.Now;
            cModel.Email = currentUser.Email;
            cModel.Password = currentUser.Password;
            cModel.JoinedDate = currentUser.JoinedDate;

            //cModel.LastUpdate = DateTime.Now;
            //cModel.CityId = Convert.ToInt16(cityId);
            if (ModelState.IsValid)
            {
                db.Entry(cModel).State = EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.Message = "Your details has been updated.";
            Session["currentUser"] = cModel;
            return View();
        }
        /*
        [HttpPost]
        public ActionResult EditDetails(Customer cModel)
        {
            //cModel.Description = 
            var currentUser = (Customer)Session["currentUser"];
            //var cityId = Request.Form["citylist"];
            
            ViewBag.Message = "Your details has been updated.";
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name", cModel.CityId);
            cModel.LastUpdate = DateTime.Now;
            cModel.Email = currentUser.Email;
            cModel.Password = currentUser.Password;
            cModel.JoinedDate = currentUser.JoinedDate;
            //check whether the email is already exist or not
            //var customer = from p in db.Customers
            //               select p;
            //customer = customer.Where(s => s.Email.Equals(currentUser.Email));

            //cModel.LastUpdate = DateTime.Now;
            //cModel.CityId = Convert.ToInt16(cityId);
            if (ModelState.IsValid)
            {
                db.Entry(cModel).State = EntityState.Modified;
                db.SaveChanges();
            }
            
            Session["currentUser"] = cModel;
            return View();
        }
        */
        

        // GET route for: /CustomerAccount/Dashboard
        public ActionResult Dashboard()
        {
            //ViewData["viewCurrentUser"] = Session["currentUser"];
            return View();
        }

        // GET route for: /CustomerAccount/Logout
        public ActionResult Logout()
        {
            //ViewData["viewCurrentUser"] = Session["currentUser"];
            Session["currentUser"] = null;
            return View("Login");
        }

        public ActionResult Unsubscribe()
        {
            //ViewData["viewCurrentUser"] = Session["currentUser"];
            Customer current = (Customer)Session["currentUser"];
            Customer customer = db.Customers.Find(current.CustomerId);
            db.Customers.Remove(customer);
            db.SaveChanges();
            Session["currentUser"] = null;
            return View();
        }


        //method to populate city's dropdownbox
        public List<SelectListItem> getCities()
        {
            var cities = from p in db.Cities
                         select p;

            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var c in cities)
            {
                list.Add(new SelectListItem
                {
                    Text = c.Name,
                    Value = c.CityId.ToString(),
                    Selected = (c.CityId == 1)
                });
            }

            return list;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}